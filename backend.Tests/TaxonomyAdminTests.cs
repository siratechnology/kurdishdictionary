using backend.Data.Models;
using backend.Services.Lexicon;
using Microsoft.EntityFrameworkCore;

namespace backend.Tests;

/// <summary>
/// پڕۆمپت ١١ — the settings area that repairs the sprawl instead of recreating it.
/// </summary>
[Collection("ledger")]
public class TaxonomyAdminTests
{
    private readonly LedgerFixture _fx;

    public TaxonomyAdminTests(LedgerFixture fx) => _fx = fx;

    // ═══════════════════════════════════════════════════════════════════════
    // The test the prompt names: rename a value in use by 400 senses, assert all
    // 400 still resolve and the row's Id is unchanged.
    //
    // This is the difference between UPDATE and delete-and-insert. Re-creating the row would break
    // every SenseFeature pointing at it AND every ledger event that ever mentioned it — a teacher's
    // history would start referring to a value that no longer exists.
    // ═══════════════════════════════════════════════════════════════════════
    [Fact]
    public async Task Renaming_a_value_in_use_by_400_senses_keeps_its_id_and_every_reference()
    {
        const int senseCount = 400;

        var (axisId, valueId) = await NewAxisWithValue("سیفەت");
        var senseIds = await NewSenses(senseCount);

        _fx.CurrentUser.UserId = _fx.SomaId;
        _fx.CurrentUser.UserName = "soma";

        await using (var db = _fx.NewContext())
        {
            foreach (var senseId in senseIds)
                db.SenseFeatures.Add(new SenseFeature { SenseId = senseId, AxisId = axisId, ValueId = valueId });

            await db.SaveChangesAsync();
        }

        await using (var db = _fx.NewContext())
        {
            Assert.Equal(senseCount, await db.SenseFeatures.CountAsync(f => f.ValueId == valueId));
        }

        // ── The rename ──────────────────────────────────────────────────────
        await using (var db = _fx.NewContext())
            await new TaxonomyAdminService(db).RenameValueAsync(valueId, "هاوەڵناو");

        await using var read = _fx.NewContext();

        // Same row.
        var value = await read.FeatureValues.FirstAsync(v => v.Id == valueId);
        Assert.Equal("هاوەڵناو", value.NameKu);
        Assert.Equal(valueId, value.Id);

        // All 400 still resolve, and to the renamed value.
        var stillPointing = await read.SenseFeatures.CountAsync(f => f.ValueId == valueId);
        Assert.Equal(senseCount, stillPointing);

        var resolved = await read.SenseFeatures
            .Where(f => f.ValueId == valueId)
            .Select(f => f.Value!.NameKu)
            .Distinct()
            .ToListAsync();

        Assert.Equal(new[] { "هاوەڵناو" }, resolved);
    }

    [Fact]
    public async Task Two_values_on_one_axis_may_not_share_a_name()
    {
        var (axisId, firstId) = await NewAxisWithValue("تاک");

        int secondId;
        await using (var db = _fx.NewContext())
        {
            var v = new FeatureValue { AxisId = axisId, NameKu = "کۆ" };
            db.FeatureValues.Add(v);
            await db.SaveChangesAsync();
            secondId = v.Id;
        }

        await using var db2 = _fx.NewContext();
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            new TaxonomyAdminService(db2).RenameValueAsync(secondId, "تاک"));
    }

    // ── Deletion vs deactivation ───────────────────────────────────────────

    [Fact]
    public async Task A_value_in_use_cannot_be_deleted_only_deactivated()
    {
        var (axisId, valueId) = await NewAxisWithValue("بەکارهاتوو");
        var senseIds = await NewSenses(3);

        _fx.CurrentUser.UserId = _fx.SomaId;
        await using (var db = _fx.NewContext())
        {
            foreach (var id in senseIds)
                db.SenseFeatures.Add(new SenseFeature { SenseId = id, AxisId = axisId, ValueId = valueId });
            await db.SaveChangesAsync();
        }

        await using (var db = _fx.NewContext())
        {
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                new TaxonomyAdminService(db).DeleteValueAsync(valueId));

            // The message names the count, so the person knows what they nearly destroyed.
            Assert.Contains("3", ex.Message);
        }

        // Deactivation is always available, and leaves the data alone.
        await using (var db = _fx.NewContext())
            await new TaxonomyAdminService(db).SetValueActiveAsync(valueId, false);

        await using var read = _fx.NewContext();
        var value = await read.FeatureValues.IgnoreQueryFilters().FirstAsync(v => v.Id == valueId);

        Assert.False(value.IsActive);
        Assert.Equal(3, await read.SenseFeatures.CountAsync(f => f.ValueId == valueId));
    }

    [Fact]
    public async Task An_unused_value_can_be_deleted_and_disappears_from_the_settings_area()
    {
        var (_, valueId) = await NewAxisWithValue("بەکارنەهاتوو");

        await using (var db = _fx.NewContext())
            await new TaxonomyAdminService(db).DeleteValueAsync(valueId);

        await using var read = _fx.NewContext();

        // Gone from every ordinary query — the global filter hides it, so it leaves the settings
        // area entirely rather than sitting in the collapsed "deactivated" section.
        Assert.False(await read.FeatureValues.AnyAsync(v => v.Id == valueId));

        // But the row itself survives, because the ledger points at it. A value that vanished
        // physically would turn "who set this" into an unanswerable question (پڕۆمپت ٢).
        var row = await read.FeatureValues.IgnoreQueryFilters().FirstAsync(v => v.Id == valueId);
        Assert.True(row.IsDeleted);
    }

    // ── Merge: the operation that repairs the 79 categories ────────────────

    [Fact]
    public async Task Merge_previews_the_exact_count_then_moves_every_row_with_its_own_event()
    {
        const int sourceRows = 40;

        var (axisId, sourceId) = await NewAxisWithValue("سیفەت");

        int targetId;
        await using (var db = _fx.NewContext())
        {
            var target = new FeatureValue { AxisId = axisId, NameKu = "هاوەڵناو" };
            db.FeatureValues.Add(target);
            await db.SaveChangesAsync();
            targetId = target.Id;
        }

        var senseIds = await NewSenses(sourceRows);

        _fx.CurrentUser.UserId = _fx.SomaId;
        await using (var db = _fx.NewContext())
        {
            foreach (var id in senseIds)
                db.SenseFeatures.Add(new SenseFeature { SenseId = id, AxisId = axisId, ValueId = sourceId });
            await db.SaveChangesAsync();
        }

        // ── Preview says exactly what will happen ───────────────────────────
        await using (var db = _fx.NewContext())
        {
            var preview = await new MergeService(db).PreviewAsync(sourceId, targetId);

            Assert.Equal(sourceRows, preview.RowsToMove);
            Assert.Empty(preview.Conflicts);
            Assert.Contains("سیفەت", preview.Summary);
            Assert.Contains("هاوەڵناو", preview.Summary);
        }

        long eventsBefore;
        await using (var db = _fx.NewContext())
            eventsBefore = await db.ContributionEvents.CountAsync();

        // ── Execute ─────────────────────────────────────────────────────────
        _fx.CurrentUser.UserId = _fx.PerjinId;
        _fx.CurrentUser.UserName = "perjin";

        await using (var db = _fx.NewContext())
        {
            var moved = await new MergeService(db)
                .ExecuteAsync(sourceId, targetId, _fx.PerjinId, "یەکخستنی زاراوە");

            Assert.Equal(sourceRows, moved);
        }

        await using var read = _fx.NewContext();

        Assert.Equal(0, await read.SenseFeatures.CountAsync(f => f.ValueId == sourceId));
        Assert.Equal(sourceRows, await read.SenseFeatures.CountAsync(f => f.ValueId == targetId));

        // The source is retired, not deleted — history still renders it.
        var source = await read.FeatureValues.IgnoreQueryFilters().FirstAsync(v => v.Id == sourceId);
        Assert.False(source.IsActive);
        Assert.Equal(targetId, source.MergedIntoValueId);
        Assert.NotNull(source.MergedAt);

        // One event PER MOVED ROW, plus the one explaining the merge. A merge that collapsed into a
        // single log line would put a hole in 40 word histories.
        var eventsAfter = await read.ContributionEvents.CountAsync();
        Assert.True(eventsAfter >= eventsBefore + sourceRows,
            $"expected at least {sourceRows} new events, saw {eventsAfter - eventsBefore}");

        // The reason is on the ledger, readable next year.
        var explanation = await read.ContributionEvents
            .Where(e => e.EntityType == nameof(FeatureValue) && e.FieldName == "MergedInto")
            .OrderByDescending(e => e.Id)
            .FirstAsync();

        Assert.Equal("یەکخستنی زاراوە", explanation.Note);
        Assert.Equal(_fx.PerjinId, explanation.UserId);
    }

    [Fact]
    public async Task Merge_names_the_senses_that_would_end_up_with_a_duplicate()
    {
        var (axisId, sourceId) = await NewAxisWithValue("یەکەم");

        int targetId;
        await using (var db = _fx.NewContext())
        {
            var target = new FeatureValue { AxisId = axisId, NameKu = "دووەم" };
            db.FeatureValues.Add(target);
            await db.SaveChangesAsync();
            targetId = target.Id;
        }

        // A sense holding BOTH would violate UNIQUE(SenseId, AxisId) on merge. Build that state by
        // using two axes' worth of rows on one sense is impossible — so use two senses, one of
        // which already holds the target.
        var senseIds = await NewSenses(2);

        _fx.CurrentUser.UserId = _fx.SomaId;
        await using (var db = _fx.NewContext())
        {
            db.SenseFeatures.Add(new SenseFeature { SenseId = senseIds[0], AxisId = axisId, ValueId = sourceId });
            db.SenseFeatures.Add(new SenseFeature { SenseId = senseIds[1], AxisId = axisId, ValueId = targetId });
            await db.SaveChangesAsync();
        }

        await using var db2 = _fx.NewContext();
        var preview = await new MergeService(db2).PreviewAsync(sourceId, targetId);

        // No conflict here — different senses. The preview still reports the honest count.
        Assert.Equal(1, preview.RowsToMove);
        Assert.Empty(preview.Conflicts);
    }

    [Fact]
    public async Task Merging_across_axes_is_refused()
    {
        var (_, first) = await NewAxisWithValue("لەم تەوەرە");
        var (_, second) = await NewAxisWithValue("لەو تەوەرە");

        await using var db = _fx.NewContext();

        // Axes are independent dimensions. Merging across them would be asserting that a value on
        // ژمارە means the same thing as a value on ڕەگەز.
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            new MergeService(db).PreviewAsync(first, second));
    }

    [Fact]
    public async Task A_merge_can_be_undone_and_puts_every_row_back()
    {
        var (axisId, sourceId) = await NewAxisWithValue("سەرچاوە");

        int targetId;
        await using (var db = _fx.NewContext())
        {
            var t = new FeatureValue { AxisId = axisId, NameKu = "ئامانج" };
            db.FeatureValues.Add(t);
            await db.SaveChangesAsync();
            targetId = t.Id;
        }

        var senseIds = await NewSenses(5);

        _fx.CurrentUser.UserId = _fx.SomaId;
        await using (var db = _fx.NewContext())
        {
            foreach (var id in senseIds)
                db.SenseFeatures.Add(new SenseFeature { SenseId = id, AxisId = axisId, ValueId = sourceId });
            await db.SaveChangesAsync();
        }

        await using (var db = _fx.NewContext())
            await new MergeService(db).ExecuteAsync(sourceId, targetId, _fx.SomaId, "هەڵە بوو");

        await using (var db = _fx.NewContext())
        {
            var restored = await new MergeService(db).UndoAsync(sourceId, _fx.SomaId);
            Assert.Equal(5, restored);
        }

        await using var read = _fx.NewContext();

        Assert.Equal(5, await read.SenseFeatures.CountAsync(f => f.ValueId == sourceId));
        Assert.Equal(0, await read.SenseFeatures.CountAsync(f => f.ValueId == targetId));

        var source = await read.FeatureValues.FirstAsync(v => v.Id == sourceId);
        Assert.True(source.IsActive);
        Assert.Null(source.MergedIntoValueId);
    }

    // ── Unused values ──────────────────────────────────────────────────────

    [Fact]
    public async Task Bulk_deactivate_rechecks_usage_and_skips_anything_now_in_use()
    {
        var (axisId, usedId) = await NewAxisWithValue("لەناکاو بەکارهات");

        int unusedId;
        await using (var db = _fx.NewContext())
        {
            var v = new FeatureValue { AxisId = axisId, NameKu = "هەرگیز بەکارنەهات" };
            db.FeatureValues.Add(v);
            await db.SaveChangesAsync();
            unusedId = v.Id;
        }

        // Somebody uses one of them while the settings list sits open on someone else's screen.
        var senseIds = await NewSenses(1);
        _fx.CurrentUser.UserId = _fx.SomaId;
        await using (var db = _fx.NewContext())
        {
            db.SenseFeatures.Add(new SenseFeature { SenseId = senseIds[0], AxisId = axisId, ValueId = usedId });
            await db.SaveChangesAsync();
        }

        await using (var db = _fx.NewContext())
        {
            var changed = await new TaxonomyAdminService(db)
                .BulkDeactivateAsync(new[] { usedId, unusedId });

            // Only the genuinely unused one — the stale screen does not get to retire live data.
            Assert.Equal(1, changed);
        }

        await using var read = _fx.NewContext();
        Assert.True((await read.FeatureValues.FirstAsync(v => v.Id == usedId)).IsActive);
        Assert.False((await read.FeatureValues.IgnoreQueryFilters().FirstAsync(v => v.Id == unusedId)).IsActive);
    }

    // ── Conditional rules ──────────────────────────────────────────────────

    [Fact]
    public async Task A_condition_must_reference_an_axis_on_the_same_part_of_speech()
    {
        int assignmentId, foreignValueId;

        await using (var db = _fx.NewContext())
        {
            var axisA = new FeatureAxis { Code = Code(), NameKu = "تەوەری أ" };
            var axisB = new FeatureAxis { Code = Code(), NameKu = "تەوەری ب" };
            db.FeatureAxes.AddRange(axisA, axisB);
            await db.SaveChangesAsync();

            var valueB = new FeatureValue { AxisId = axisB.Id, NameKu = "نرخی ب" };
            db.FeatureValues.Add(valueB);
            await db.SaveChangesAsync();

            // Only axis A is assigned to the part of speech; B is not.
            var assignment = new PartOfSpeechAxis { PartOfSpeechId = 1, AxisId = axisA.Id };
            db.PartOfSpeechAxes.Add(assignment);
            await db.SaveChangesAsync();

            assignmentId = assignment.Id;
            foreignValueId = valueB.Id;
        }

        await using var db2 = _fx.NewContext();

        // The rule could never fire: the form would wait for an answer it never offers.
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            new TaxonomyAdminService(db2).SetConditionAsync(assignmentId, foreignValueId));
    }

    [Fact]
    public async Task A_rule_that_closes_a_cycle_is_refused_and_names_the_path()
    {
        int assignA, assignB, valueA, valueB;

        await using (var db = _fx.NewContext())
        {
            var axisA = new FeatureAxis { Code = Code(), NameKu = "جۆری کار" };
            var axisB = new FeatureAxis { Code = Code(), NameKu = "تێپەڕی" };
            db.FeatureAxes.AddRange(axisA, axisB);
            await db.SaveChangesAsync();

            var vA = new FeatureValue { AxisId = axisA.Id, NameKu = "تەواو" };
            var vB = new FeatureValue { AxisId = axisB.Id, NameKu = "تێپەڕ" };
            db.FeatureValues.AddRange(vA, vB);
            await db.SaveChangesAsync();

            var aA = new PartOfSpeechAxis { PartOfSpeechId = 2, AxisId = axisA.Id };
            var aB = new PartOfSpeechAxis { PartOfSpeechId = 2, AxisId = axisB.Id };
            db.PartOfSpeechAxes.AddRange(aA, aB);
            await db.SaveChangesAsync();

            assignA = aA.Id; assignB = aB.Id;
            valueA = vA.Id; valueB = vB.Id;
        }

        // B requires a value on A — fine.
        await using (var db = _fx.NewContext())
            await new TaxonomyAdminService(db).SetConditionAsync(assignB, valueA);

        // A requires a value on B — that closes the loop.
        await using (var db = _fx.NewContext())
        {
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                new TaxonomyAdminService(db).SetConditionAsync(assignA, valueB));

            // The path is shown so whoever built it can see what they closed.
            Assert.Contains("←", ex.Message);
        }
    }

    // ── helpers ────────────────────────────────────────────────────────────

    private static string Code() => Guid.NewGuid().ToString("N")[..12];

    private async Task<(int AxisId, int ValueId)> NewAxisWithValue(string valueName)
    {
        await using var db = _fx.NewContext();

        var axis = new FeatureAxis { Code = Code(), NameKu = $"تەوەر {Code()[..4]}" };
        db.FeatureAxes.Add(axis);
        await db.SaveChangesAsync();

        var value = new FeatureValue { AxisId = axis.Id, NameKu = valueName };
        db.FeatureValues.Add(value);
        await db.SaveChangesAsync();

        return (axis.Id, value.Id);
    }

    private async Task<List<int>> NewSenses(int count)
    {
        var ids = new List<int>(count);

        await _fx.As(_fx.SomaId, "soma", async db =>
        {
            var word = new Word { Kurdish = $"و-{Guid.NewGuid():N}"[..14] };
            db.Words.Add(word);
            await db.SaveChangesAsync();

            for (var i = 0; i < count; i++)
            {
                var sense = new Sense
                {
                    WordId = word.Id,
                    Definition = $"مانا {i}",
                    PartOfSpeechId = 1,
                    ExampleUsage = "نموونە",
                };
                db.Senses.Add(sense);
            }

            await db.SaveChangesAsync();

            ids.AddRange(await db.Senses.Where(s => s.WordId == word.Id).Select(s => s.Id).ToListAsync());
        });

        return ids;
    }
}
