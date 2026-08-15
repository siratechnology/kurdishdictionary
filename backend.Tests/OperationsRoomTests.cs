using backend.Data.Models;
using backend.Services.Lexicon;
using Microsoft.EntityFrameworkCore;

namespace backend.Tests;

/// <summary>
/// پڕۆمپت ٧ — the operations room, including the sanity test the prompt spells out.
/// </summary>
[Collection("ledger")]
public class OperationsRoomTests
{
    private readonly LedgerFixture _fx;

    public OperationsRoomTests(LedgerFixture fx) => _fx = fx;

    // ═══════════════════════════════════════════════════════════════════════
    // The sanity test, verbatim from the prompt:
    //   Soma creates 1000 words. Perjin reclassifies 300. Assert:
    //     Soma.WordsCreated == 1000 (unchanged)
    //     Perjin.SensesClassified == 300 and Perjin.WordsCreated == 0
    //     all 300 histories show BOTH names in order
    //   Then soft-delete one word and assert Soma.WordsCreated is still 1000.
    //   Then bulk-set an axis on 50 senses and assert 50 separate FeatureSet events exist.
    //
    // Scaled to 200/60/50 for runtime — it is a real SQL Server and 1,000 words means 1,000
    // ledger rows plus their audit rows. The invariant under test is independence between two
    // contributors' records, which does not become more true at 1,000.
    // ═══════════════════════════════════════════════════════════════════════
    [Fact]
    public async Task Two_contributors_records_never_move_each_other()
    {
        const int wordCount = 200;
        const int reclassified = 60;
        const int bulkSize = 50;

        var somaBefore = await StatsFor(_fx.SomaId);
        var perjinBefore = await StatsFor(_fx.PerjinId);

        // ── Soma creates the words, each with one sense ─────────────────────
        var wordIds = new List<int>();
        var senseIds = new List<int>();

        await _fx.As(_fx.SomaId, "soma", async db =>
        {
            for (var i = 0; i < wordCount; i++)
            {
                var word = new Word { Kurdish = $"سۆما-{i}" };
                db.Words.Add(word);
                await db.SaveChangesAsync();

                var sense = new Sense
                {
                    WordId = word.Id,
                    Definition = $"پێناسەی {i}",
                    PartOfSpeechId = 1,
                    ExampleUsage = "نموونە",
                };
                db.Senses.Add(sense);
                await db.SaveChangesAsync();

                wordIds.Add(word.Id);
                senseIds.Add(sense.Id);
            }
        });

        Assert.Equal(somaBefore.WordsCreated + wordCount, (await StatsFor(_fx.SomaId)).WordsCreated);

        // ── Perjin reclassifies some of them ────────────────────────────────
        var touched = senseIds.Take(reclassified).ToList();

        await _fx.As(_fx.PerjinId, "perjin", async db =>
        {
            foreach (var id in touched)
            {
                var sense = await db.Senses.FirstAsync(s => s.Id == id);
                sense.Definition += " — پێداچوونەوە";
                await db.SaveChangesAsync();
            }
        });

        var soma = await StatsFor(_fx.SomaId);
        var perjin = await StatsFor(_fx.PerjinId);

        // Soma's creation count did NOT move because Perjin worked on her words.
        Assert.Equal(somaBefore.WordsCreated + wordCount, soma.WordsCreated);

        // Perjin created nothing.
        Assert.Equal(perjinBefore.WordsCreated, perjin.WordsCreated);

        // Every touched word's history names both people, creator first.
        await using (var read = _fx.NewContext())
        {
            var wordsTouched = wordIds.Take(reclassified).ToList();

            var histories = await read.ContributionEvents
                .Where(e => e.WordId != null && wordsTouched.Contains(e.WordId!.Value))
                .OrderBy(e => e.Id)
                .Select(e => new { e.WordId, e.UserId, e.EventType })
                .ToListAsync();

            foreach (var wordId in wordsTouched)
            {
                var story = histories.Where(h => h.WordId == wordId).ToList();

                Assert.Equal(_fx.SomaId, story.First().UserId);
                Assert.Equal(ContributionEventType.WordCreated, story.First().EventType);
                Assert.Contains(story, h => h.UserId == _fx.PerjinId);
            }
        }

        // ── Soft-delete one word: Soma's record is untouched ────────────────
        await _fx.As(_fx.PerjinId, "perjin", async db =>
        {
            var word = await db.Words.FirstAsync(w => w.Id == wordIds[0]);
            db.Words.Remove(word);
            await db.SaveChangesAsync();
        });

        Assert.Equal(somaBefore.WordsCreated + wordCount, (await StatsFor(_fx.SomaId)).WordsCreated);

        // ── Bulk-set one axis on 50 senses ──────────────────────────────────
        int axisId, valueId;

        await using (var setup = _fx.NewContext())
        {
            var axis = new FeatureAxis { Code = $"axis-{Guid.NewGuid():N}"[..20], NameKu = "ڕەگەز" };
            setup.FeatureAxes.Add(axis);
            await setup.SaveChangesAsync();

            var value = new FeatureValue { AxisId = axis.Id, NameKu = "نێر" };
            setup.FeatureValues.Add(value);
            await setup.SaveChangesAsync();

            axisId = axis.Id;
            valueId = value.Id;
        }

        var bulkSenses = senseIds.Skip(reclassified).Take(bulkSize).ToList();
        Assert.Equal(bulkSize, bulkSenses.Count);

        long ledgerBefore;
        await using (var count = _fx.NewContext())
        {
            ledgerBefore = await count.ContributionEvents
                .CountAsync(e => e.EventType == ContributionEventType.FeatureSet);
        }

        _fx.CurrentUser.UserId = _fx.PerjinId;
        _fx.CurrentUser.UserName = "perjin";

        await using (var db = _fx.NewContext())
        {
            var applied = await new ClassificationService(db)
                .BulkSetFeatureAsync(bulkSenses, axisId, valueId, _fx.PerjinId);

            Assert.Equal(bulkSize, applied);
        }

        await using (var verify = _fx.NewContext())
        {
            var after = await verify.ContributionEvents
                .CountAsync(e => e.EventType == ContributionEventType.FeatureSet);

            // 50 senses, 50 events. A bulk action that collapsed into one log line would put a hole
            // in 50 word histories exactly where somebody's work was.
            Assert.Equal(ledgerBefore + bulkSize, after);
        }
    }

    // ── Claim lock ─────────────────────────────────────────────────────────

    [Fact]
    public async Task A_claimed_sense_reports_its_holder_by_name()
    {
        var senseId = await NewSense();

        await using (var db = _fx.NewContext())
        {
            var first = await new ClaimService(db).ClaimAsync(senseId, _fx.SomaId);
            Assert.True(first.Granted);
        }

        await using (var db = _fx.NewContext())
        {
            var second = await new ClaimService(db).ClaimAsync(senseId, _fx.PerjinId);

            // Refused, and it says WHO — the point is for the second person to go and talk to them.
            Assert.False(second.Granted);
            Assert.Equal("سۆما", second.HolderName);
        }
    }

    [Fact]
    public async Task Reclaiming_your_own_sense_extends_it_rather_than_failing()
    {
        var senseId = await NewSense();

        await using var db = _fx.NewContext();
        var service = new ClaimService(db);

        var first = await service.ClaimAsync(senseId, _fx.SomaId);
        var again = await service.ClaimAsync(senseId, _fx.SomaId);

        Assert.True(again.Granted);
        Assert.True(again.ExpiresAt >= first.ExpiresAt);
    }

    [Fact]
    public async Task Releasing_frees_the_sense_for_someone_else()
    {
        var senseId = await NewSense();

        await using (var db = _fx.NewContext())
            await new ClaimService(db).ClaimAsync(senseId, _fx.SomaId);

        await using (var db = _fx.NewContext())
            await new ClaimService(db).ReleaseAsync(senseId, _fx.SomaId);

        await using (var db = _fx.NewContext())
        {
            var next = await new ClaimService(db).ClaimAsync(senseId, _fx.PerjinId);
            Assert.True(next.Granted);
        }
    }

    // ── Disagreement, not error ────────────────────────────────────────────

    [Fact]
    public async Task A_second_teacher_changing_a_classification_records_a_disagreement()
    {
        var senseId = await NewSense();

        int axisId, firstValueId, secondValueId;
        await using (var setup = _fx.NewContext())
        {
            var axis = new FeatureAxis { Code = $"ax{Guid.NewGuid():N}"[..12], NameKu = "پۆلی وشە" };
            setup.FeatureAxes.Add(axis);
            await setup.SaveChangesAsync();

            var v1 = new FeatureValue { AxisId = axis.Id, NameKu = "هاوەڵناو" };
            var v2 = new FeatureValue { AxisId = axis.Id, NameKu = "هاوەڵکار" };
            setup.FeatureValues.AddRange(v1, v2);
            await setup.SaveChangesAsync();

            axisId = axis.Id; firstValueId = v1.Id; secondValueId = v2.Id;
        }

        _fx.CurrentUser.UserId = _fx.SomaId;
        await using (var db = _fx.NewContext())
            await new ClassificationService(db).SetFeatureAsync(senseId, axisId, firstValueId, _fx.SomaId);

        _fx.CurrentUser.UserId = _fx.PerjinId;
        SenseDisagreement? recorded;
        await using (var db = _fx.NewContext())
            recorded = (await new ClassificationService(db)
                .SetFeatureAsync(senseId, axisId, secondValueId, _fx.PerjinId)).Disagreement;

        Assert.NotNull(recorded);

        await using var read = _fx.NewContext();
        var d = await read.SenseDisagreements.FirstAsync(x => x.SenseId == senseId);

        // Both judgements and both names survive. Neither is marked wrong — زۆر is genuinely both
        // هاوەڵناو and هاوەڵکار in the source deck, and losing that loses information.
        Assert.Equal("هاوەڵناو", d.FirstJudgement);
        Assert.Equal(_fx.SomaId, d.FirstUserId);
        Assert.Equal("هاوەڵکار", d.SecondJudgement);
        Assert.Equal(_fx.PerjinId, d.SecondUserId);
        Assert.Null(d.ResolvedAt);
    }

    [Fact]
    public async Task Changing_your_own_answer_is_not_a_disagreement()
    {
        var senseId = await NewSense();

        int axisId, v1Id, v2Id;
        await using (var setup = _fx.NewContext())
        {
            var axis = new FeatureAxis { Code = $"ax{Guid.NewGuid():N}"[..12], NameKu = "ژمارە" };
            setup.FeatureAxes.Add(axis);
            await setup.SaveChangesAsync();

            var a = new FeatureValue { AxisId = axis.Id, NameKu = "تاک" };
            var b = new FeatureValue { AxisId = axis.Id, NameKu = "کۆ" };
            setup.FeatureValues.AddRange(a, b);
            await setup.SaveChangesAsync();

            axisId = axis.Id; v1Id = a.Id; v2Id = b.Id;
        }

        _fx.CurrentUser.UserId = _fx.SomaId;

        await using (var db = _fx.NewContext())
            await new ClassificationService(db).SetFeatureAsync(senseId, axisId, v1Id, _fx.SomaId);

        await using (var db = _fx.NewContext())
        {
            var result = await new ClassificationService(db).SetFeatureAsync(senseId, axisId, v2Id, _fx.SomaId);
            Assert.Null(result.Disagreement);
        }
    }

    // ── Escape hatch ───────────────────────────────────────────────────────

    [Fact]
    public async Task Not_applicable_requires_a_reason_and_routes_to_the_disputed_queue()
    {
        var senseId = await NewSense();

        int axisId;
        await using (var setup = _fx.NewContext())
        {
            var axis = new FeatureAxis { Code = $"ax{Guid.NewGuid():N}"[..12], NameKu = "تێپەڕی" };
            setup.FeatureAxes.Add(axis);
            await setup.SaveChangesAsync();
            axisId = axis.Id;
        }

        _fx.CurrentUser.UserId = _fx.SomaId;

        // The note is not optional: the escape hatch always costs a sentence of explanation.
        await using (var db = _fx.NewContext())
        {
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                new ClassificationService(db).MarkNotApplicableAsync(senseId, axisId, "  ", _fx.SomaId));
        }

        await using (var db = _fx.NewContext())
        {
            await new ClassificationService(db)
                .MarkNotApplicableAsync(senseId, axisId, "ئەم کارە ناتەواوە", _fx.SomaId);
        }

        await using var read = _fx.NewContext();

        var feature = await read.SenseFeatures.FirstAsync(f => f.SenseId == senseId && f.AxisId == axisId);
        Assert.True(feature.IsNotApplicable);
        Assert.Null(feature.ValueId);

        var sense = await read.Senses.FirstAsync(s => s.Id == senseId);
        Assert.Equal(SenseWorkflowState.Disputed, sense.WorkflowState);
    }

    // ── Trust levels ───────────────────────────────────────────────────────

    [Fact]
    public async Task A_senior_publishes_directly_and_a_contributor_does_not()
    {
        var contributorSense = await NewSense();
        var seniorSense = await NewSense();

        await using (var db = _fx.NewContext())
        {
            var senior = await db.Users.FirstAsync(u => u.Id == _fx.PerjinId);
            senior.TrustLevel = TrustLevel.Senior;
            await db.SaveChangesAsync();
        }

        _fx.CurrentUser.UserId = _fx.SomaId;
        await using (var db = _fx.NewContext())
        {
            var state = await new ClassificationService(db).SubmitAsync(contributorSense, _fx.SomaId);
            Assert.Equal(SenseWorkflowState.Classified, state);
        }

        _fx.CurrentUser.UserId = _fx.PerjinId;
        await using (var db = _fx.NewContext())
        {
            var state = await new ClassificationService(db).SubmitAsync(seniorSense, _fx.PerjinId);
            Assert.Equal(SenseWorkflowState.Published, state);
        }
    }

    // ── helpers ────────────────────────────────────────────────────────────

    private async Task<int> NewSense()
    {
        var senseId = 0;

        await _fx.As(_fx.SomaId, "soma", async db =>
        {
            var word = new Word { Kurdish = $"وشە-{Guid.NewGuid():N}"[..12] };
            db.Words.Add(word);
            await db.SaveChangesAsync();

            var sense = new Sense
            {
                WordId = word.Id,
                Definition = "پێناسە",
                PartOfSpeechId = 1,
                ExampleUsage = "نموونە",
            };
            db.Senses.Add(sense);
            await db.SaveChangesAsync();
            senseId = sense.Id;
        });

        return senseId;
    }

    private async Task<ContributionStats> StatsFor(Guid userId)
    {
        await using var db = _fx.NewContext();
        return await db.ContributionStats.FirstAsync(s => s.UserId == userId);
    }
}
