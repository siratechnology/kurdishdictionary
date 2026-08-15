using backend.Data;
using backend.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace LexiconMigration;

/// <summary>
/// Creates the two axes the team asked for — ژمارە and ڕەگەز — with their values, assigns them to
/// the parts of speech that take them, and carries the existing Word.Gender data across.
///
/// The taxonomy is normally the team's to enter through the settings screen; this exists because
/// 736 words already carry a grammatical gender in the old enum column, and that is real work
/// somebody did. Re-typing it by hand would be asking them to do it twice.
///
/// Everything here is idempotent: run it twice and the second run changes nothing.
/// </summary>
public static class ConfigureAxes
{
    public static async Task RunAsync(AppDbContext db, Guid actorId, bool dryRun, CancellationToken ct = default)
    {
        Console.WriteLine(dryRun ? "DRY RUN — nothing will be written.\n" : "Configuring axes…\n");

        // ── ژمارە ───────────────────────────────────────────────────────────
        var number = await UpsertAxis(db, "jimare", "ژمارە",
            "تاک یان کۆ", allowsNotApplicable: false, dryRun, ct);

        var single = await UpsertValue(db, number, "تاک", code: null, order: 1, dryRun, ct);
        var plural = await UpsertValue(db, number, "کۆ", code: TaxonomyCodes.Plural, order: 2, dryRun, ct);

        // ── ڕەگەز ───────────────────────────────────────────────────────────
        // AllowsNotApplicable, because a good many Kurdish nouns simply do not carry gender and
        // "nobody has answered" must stay distinguishable from "the question does not apply".
        var gender = await UpsertAxis(db, "regez", "ڕەگەز",
            "ڕەگەزی ڕێزمانی", allowsNotApplicable: true, dryRun, ct);

        // Names match the old enum's labels exactly, so the backfill below is a straight mapping
        // and a teacher reading a migrated sense sees the word they originally chose.
        var masculine = await UpsertValue(db, gender, "نێر", null, 1, dryRun, ct);
        var feminine = await UpsertValue(db, gender, "مێ", null, 2, dryRun, ct);
        var neuter = await UpsertValue(db, gender, "بێلایەن", null, 3, dryRun, ct);
        var common = await UpsertValue(db, gender, "دوولایەن", null, 4, dryRun, ct);

        if (dryRun)
        {
            Console.WriteLine("\nDry run — nothing written.");
            return;
        }

        // ── Which parts of speech ask for them ──────────────────────────────
        var noun = await db.PartsOfSpeech.FirstAsync(p => p.Code == "noun", ct);
        var pronoun = await db.PartsOfSpeech.FirstAsync(p => p.Code == "pronoun", ct);

        // Not marked required. Turning them on as required would put every one of the 2,329 noun
        // senses into ڕیزی کار the instant the axis exists — the team can raise the bar from the
        // settings screen once they have worked through some of it.
        await Assign(db, noun.Id, number.Id, ct);
        await Assign(db, noun.Id, gender.Id, ct);
        await Assign(db, pronoun.Id, number.Id, ct);

        await db.SaveChangesAsync(ct);

        // ── Carry the old gender column onto the senses ─────────────────────
        var map = new Dictionary<GrammaticalGender, int>
        {
            [GrammaticalGender.Masculine] = masculine.Id,
            [GrammaticalGender.Feminine] = feminine.Id,
            [GrammaticalGender.Neuter] = neuter.Id,
            [GrammaticalGender.Common] = common.Id,
            // None is deliberately absent: it is the enum's default and means "nobody said",
            // not "genderless". Treating it as an answer would fabricate 2,290 judgements.
        };

        var existing = (await db.SenseFeatures
            .Where(f => f.AxisId == gender.Id)
            .Select(f => f.SenseId)
            .ToListAsync(ct))
            .ToHashSet();

        var senses = await db.Senses
            .Where(s => s.PartOfSpeechId == noun.Id && map.Keys.Contains(s.Word.Gender))
            .Select(s => new { s.Id, s.Word.Gender })
            .ToListAsync(ct);

        var added = 0;
        foreach (var sense in senses)
        {
            if (existing.Contains(sense.Id)) continue;

            db.SenseFeatures.Add(new SenseFeature
            {
                SenseId = sense.Id,
                AxisId = gender.Id,
                ValueId = map[sense.Gender],
            });

            if (++added % 500 == 0) await db.SaveChangesAsync(ct);
        }

        await db.SaveChangesAsync(ct);

        Console.WriteLine();
        Console.WriteLine("── configured ──────────────────────────────────────────");
        Console.WriteLine($"  ژمارە  : تاک · کۆ");
        Console.WriteLine($"  ڕەگەز  : نێر · مێ · بێلایەن · دوولایەن");
        Console.WriteLine($"  assigned to ناو and جێناو (optional, not required)");
        Console.WriteLine($"  gender carried onto {added:N0} senses from the old Word.Gender column");
        Console.WriteLine();
        Console.WriteLine("The station will now ask for these. Raise them to «required» from ڕێکخستن");
        Console.WriteLine("when the team is ready for every noun to need an answer.");
    }

    private static async Task<FeatureAxis> UpsertAxis(
        AppDbContext db, string code, string name, string description,
        bool allowsNotApplicable, bool dryRun, CancellationToken ct)
    {
        var existing = await db.FeatureAxes.FirstOrDefaultAsync(a => a.Code == code, ct);
        if (existing is not null)
        {
            Console.WriteLine($"  axis «{name}» already exists");
            return existing;
        }

        var axis = new FeatureAxis
        {
            Code = code,
            NameKu = name,
            Description = description,
            AllowsNotApplicable = allowsNotApplicable,
            SortOrder = await db.FeatureAxes.CountAsync(ct) + 1,
        };

        Console.WriteLine($"  axis «{name}»");
        if (dryRun) return axis;

        db.FeatureAxes.Add(axis);
        await db.SaveChangesAsync(ct);
        return axis;
    }

    private static async Task<FeatureValue> UpsertValue(
        AppDbContext db, FeatureAxis axis, string name, string? code, int order,
        bool dryRun, CancellationToken ct)
    {
        if (axis.Id != 0)
        {
            var existing = await db.FeatureValues
                .FirstOrDefaultAsync(v => v.AxisId == axis.Id && v.NameKu == name, ct);

            if (existing is not null) return existing;
        }

        var value = new FeatureValue
        {
            AxisId = axis.Id,
            NameKu = name,
            Code = code,
            SortOrder = order,
        };

        Console.WriteLine($"    value «{name}»{(code is null ? "" : $"  [{code}]")}");
        if (dryRun) return value;

        db.FeatureValues.Add(value);
        await db.SaveChangesAsync(ct);
        return value;
    }

    private static async Task Assign(AppDbContext db, int partOfSpeechId, int axisId, CancellationToken ct)
    {
        var exists = await db.PartOfSpeechAxes
            .AnyAsync(a => a.PartOfSpeechId == partOfSpeechId && a.AxisId == axisId, ct);

        if (exists) return;

        db.PartOfSpeechAxes.Add(new PartOfSpeechAxis
        {
            PartOfSpeechId = partOfSpeechId,
            AxisId = axisId,
            IsRequired = false,
            SortOrder = await db.PartOfSpeechAxes.CountAsync(a => a.PartOfSpeechId == partOfSpeechId, ct) + 1,
        });
    }

    /// <summary>Mirrors backend.Services.Lexicon.TaxonomyCodes so the tool does not reference it.</summary>
    private const string TaxonomyCodesPlural = "plural";
    private static class TaxonomyCodes
    {
        public const string Plural = TaxonomyCodesPlural;
    }
}
