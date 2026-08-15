using backend.Data;
using backend.Data.Models;
using backend.Services.Lexicon;
using Microsoft.EntityFrameworkCore;

namespace backend.Tests;

/// <summary>
/// ناو is finished, and nothing built on top of the options tree may change it.
///
/// This file was written BEFORE the MinSelections/MaxSelections columns and the unique-index change,
/// and it is the reason those changes are safe: it pins the shape of the noun's configuration — five
/// PARALLEL axes, the fifth carrying four values — and the behaviour that shape produces. Acceptance
/// test ١ is <see cref="Noun_has_five_parallel_axes_the_fifth_carrying_four_values"/>; acceptance
/// test ١٠ is <see cref="After_the_index_change_every_noun_axis_is_still_single_select"/>, which is
/// the same assertions plus the cardinality the migration must have backfilled.
/// </summary>
[Collection("ledger")]
public class NounRegressionTests
{
    private readonly LedgerFixture _fx;

    public NounRegressionTests(LedgerFixture fx) => _fx = fx;

    /// <summary>The fifth axis's four values, from the source deck. Test fixture data, not app data.</summary>
    private static readonly string[] FifthAxisValues = { "گشتی", "تایبەت", "بەرجەستە", "واتای" };

    // ═══════════════════════════════════════════════════════════════════════
    // ١. ناو regression: five axes, fifth with four values, unchanged behaviour and data.
    // ═══════════════════════════════════════════════════════════════════════
    [Fact]
    public async Task Noun_has_five_parallel_axes_the_fifth_carrying_four_values()
    {
        var noun = await BuildNounAsync();
        var senseId = await NewNounSenseAsync(noun.PartOfSpeechId);

        await using var db = _fx.NewContext();
        var station = _fx.Station(db);

        var dto = await station.GetBySenseAsync(senseId, _fx.SomaId);
        Assert.NotNull(dto);

        // Five axes, all of them PARALLEL — every one visible the moment ناو is chosen, none
        // waiting on an answer somewhere else.
        Assert.Equal(5, dto!.Axes.Count);
        Assert.Equal(noun.AxisIds, dto.Axes.Select(a => a.AxisId).ToList());

        var conditional = await db.PartOfSpeechAxes
            .CountAsync(a => a.PartOfSpeechId == noun.PartOfSpeechId && a.RequiresValueId != null);
        Assert.Equal(0, conditional);

        // The fifth carries the four.
        var fifth = dto.Axes[4];
        Assert.Equal(FifthAxisValues, fifth.Values.Select(v => v.Name).ToArray());

        // One answer per axis: answering again REPLACES rather than accumulates.
        await _fx.Classification(db).SetFeatureAsync(senseId, fifth.AxisId, noun.FifthValueIds[0], _fx.SomaId);
        await _fx.Classification(db).SetFeatureAsync(senseId, fifth.AxisId, noun.FifthValueIds[2], _fx.SomaId);

        await using var read = _fx.NewContext();
        var held = await read.SenseFeatures
            .Where(f => f.SenseId == senseId && f.AxisId == fifth.AxisId)
            .Select(f => f.ValueId)
            .ToListAsync();

        Assert.Single(held);
        Assert.Equal(noun.FifthValueIds[2], held[0]);

        // And the form still shows five axes with the answer on the fifth — nothing revealed,
        // nothing retired, because nothing on ناو is conditional.
        var after = await _fx.Station(read).GetBySenseAsync(senseId, _fx.SomaId);
        Assert.Equal(5, after!.Axes.Count);
        Assert.Equal(noun.FifthValueIds[2], after.Axes[4].SelectedValueId);
    }

    // ═══════════════════════════════════════════════════════════════════════
    // ١٠. ناو regression AFTER the index change: every noun axis is still single-select,
    //     the entry form is unchanged, and no existing sense gained or lost a value.
    // ═══════════════════════════════════════════════════════════════════════
    [Fact]
    public async Task After_the_index_change_every_noun_axis_is_still_single_select()
    {
        var noun = await BuildNounAsync();
        var senseId = await NewNounSenseAsync(noun.PartOfSpeechId);

        // Answer every axis, the way a teacher who finished this word would have left it.
        await using (var db = _fx.NewContext())
        {
            var classification = _fx.Classification(db);
            for (var i = 0; i < noun.AxisIds.Count; i++)
                await classification.SetFeatureAsync(senseId, noun.AxisIds[i], noun.FirstValueIds[i], _fx.SomaId);
        }

        await using var read = _fx.NewContext();

        // The migration backfilled MaxSelections = 1 on every axis that already existed. Anything
        // else and the noun would silently start accepting a second value.
        var axes = await read.FeatureAxes
            .Where(a => noun.AxisIds.Contains(a.Id))
            .Select(a => new { a.Id, a.MinSelections, a.MaxSelections })
            .ToListAsync();

        Assert.Equal(5, axes.Count);
        Assert.All(axes, a => Assert.Equal(1, a.MaxSelections));

        // MinSelections drives the work queue only, so it starts at 0 — nothing below the part of
        // speech may gate a save.
        Assert.All(axes, a => Assert.Equal(0, a.MinSelections));

        // One row per axis, exactly the values that were set. No sense gained or lost a value.
        var held = await read.SenseFeatures
            .Where(f => f.SenseId == senseId)
            .Select(f => new { f.AxisId, f.ValueId })
            .ToListAsync();

        Assert.Equal(5, held.Count);
        Assert.Equal(noun.FirstValueIds.OrderBy(x => x), held.Select(h => h.ValueId!.Value).OrderBy(x => x));

        // The entry form is unchanged: five axes, each single-select, each showing its one answer.
        var dto = await _fx.Station(read).GetBySenseAsync(senseId, _fx.SomaId);

        Assert.Equal(5, dto!.Axes.Count);
        Assert.All(dto.Axes, a => Assert.Equal(1, a.MaxSelections));
        Assert.All(dto.Axes, a => Assert.NotNull(a.SelectedValueId));
        Assert.All(dto.Axes, a => Assert.Single(a.SelectedValueIds));

        // Still parallel: no axis on the noun hangs off another's value.
        Assert.All(dto.Axes, a => Assert.Null(a.ParentValueId));
    }

    // ── the noun's configuration, built the way the team would build it ─────

    private sealed record NounConfig(int PartOfSpeechId, List<int> AxisIds, List<int> FirstValueIds, List<int> FifthValueIds);

    /// <summary>
    /// Five axes on ناو, all unconditional, the fifth carrying the deck's four values.
    ///
    /// Built here rather than seeded because the taxonomy ships EMPTY — the team enters it from
    /// settings. These names are fixture data, not application data, which is why acceptance
    /// test ٩ excludes test fixtures from its grep.
    /// </summary>
    private async Task<NounConfig> BuildNounAsync()
    {
        await using var db = _fx.NewContext();

        // A part-of-speech row of its own rather than the seeded ناو.
        //
        // The tests in this collection share one database, so building onto the seeded row would
        // leave the first test's five axes in place for the second and it would count ten. What is
        // being pinned is the SHAPE of the noun's configuration — five parallel axes, the fifth
        // carrying four values — not which primary key it happens to sit on.
        var partOfSpeech = new PartOfSpeech { Code = Code(), NameKu = "ناو", SortOrder = 1 };
        db.PartsOfSpeech.Add(partOfSpeech);
        await db.SaveChangesAsync();

        var axisIds = new List<int>();
        var firstValueIds = new List<int>();
        var fifthValueIds = new List<int>();

        for (var i = 1; i <= 5; i++)
        {
            var axis = new FeatureAxis { Code = Code(), NameKu = $"تەوەری ناو {i} — {Code()[..4]}", SortOrder = i };
            db.FeatureAxes.Add(axis);
            await db.SaveChangesAsync();

            var names = i == 5 ? FifthAxisValues : new[] { $"أ{i}", $"ب{i}" };
            var order = 1;

            foreach (var name in names)
            {
                var value = new FeatureValue { AxisId = axis.Id, NameKu = name, SortOrder = order++ };
                db.FeatureValues.Add(value);
                await db.SaveChangesAsync();

                if (order == 2) firstValueIds.Add(value.Id);
                if (i == 5) fifthValueIds.Add(value.Id);
            }

            db.PartOfSpeechAxes.Add(new PartOfSpeechAxis
            {
                PartOfSpeechId = partOfSpeech.Id,
                AxisId = axis.Id,
                SortOrder = i,
                RequiresValueId = null,   // parallel: nothing on ناو is conditional
            });
            await db.SaveChangesAsync();

            axisIds.Add(axis.Id);
        }

        return new NounConfig(partOfSpeech.Id, axisIds, firstValueIds, fifthValueIds);
    }

    private static string Code() => Guid.NewGuid().ToString("N")[..12];

    private async Task<int> NewNounSenseAsync(int partOfSpeechId)
    {
        var senseId = 0;

        await _fx.As(_fx.SomaId, "soma", async db =>
        {
            var word = new Word { Kurdish = $"و-{Guid.NewGuid():N}"[..14] };
            db.Words.Add(word);
            await db.SaveChangesAsync();

            var sense = new Sense
            {
                WordId = word.Id,
                Definition = "مانا",
                ExampleUsage = "نموونە",
                PartOfSpeechId = partOfSpeechId,
            };

            db.Senses.Add(sense);
            await db.SaveChangesAsync();

            senseId = sense.Id;
        });

        return senseId;
    }
}
