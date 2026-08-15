using System.Globalization;
using System.Text;
using backend.Data;
using backend.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace LexiconMigration;

/// <summary>
/// Phase 1 — PROPOSE. Reads the lexicon and writes what it would do. Nothing is written to the
/// database; there is no code path from here to a save.
/// </summary>
public static class Propose
{
    public static async Task RunAsync(AppDbContext db, string connection)
    {
        var outDir = Path.Combine(Directory.GetCurrentDirectory(), "migration-review");
        Directory.CreateDirectory(outDir);

        Console.WriteLine($"Reading  : {Redact(connection)}");
        Console.WriteLine($"Writing  : {outDir}");
        Console.WriteLine();

        // IgnoreQueryFilters is wrong here: soft-deleted rows are deleted, and migrating them would
        // resurrect work somebody removed on purpose.
        var words = await db.Words
            .AsNoTracking()
            .Include(w => w.Meanings)
            .Include(w => w.SpeechPanes)
            .Include(w => w.WordCategories).ThenInclude(wc => wc.Category)
            .Include(w => w.CreatedByUser)
            .ToListAsync();

        Console.WriteLine($"Words    : {words.Count:N0}");

        var senses = new List<SenseRow>();
        var translations = new List<TranslationRow>();
        var unknownLabels = new Dictionary<string, int>();

        var legacyRows = 0;

        foreach (var word in words)
        {
            var panes = word.SpeechPanes.Select(sp => sp.SpeechPaneType.ToKurdish()).ToList();
            var categories = word.WordCategories.Select(wc => wc.Category.Name).ToList();

            foreach (var meaning in word.Meanings)
            {
                var lang = TaxonomyMap.LanguageCodeFor(meaning.Locate);

                if (lang is null && !string.IsNullOrWhiteSpace(meaning.Locate))
                {
                    unknownLabels[meaning.Locate!] = unknownLabels.GetValueOrDefault(meaning.Locate!) + 1;
                }

                // Option (a): only Sorani rows — and rows with no label at all, which are Sorani in
                // practice — become senses. Everything else is a translation and is parked.
                var isSense = lang is null or "ckb";

                var owner = meaning.CreatedByUserId ?? word.CreatedByUserId;
                if (owner is null) legacyRows++;

                if (!isSense)
                {
                    translations.Add(new TranslationRow(
                        MeaningId: meaning.Id,
                        WordId: word.Id,
                        Word: word.Kurdish,
                        LanguageCode: lang!,
                        OriginalLabel: meaning.Locate ?? "",
                        Text: meaning.Meaning,
                        OwnerUserId: owner));
                    continue;
                }

                senses.Add(Build(word, meaning, panes, categories, owner));
            }
        }

        // ── Every distinct category, and whether it has a home ──────────────
        var unmapped = new List<UnmappedRow>();
        var allCategories = await db.Categories
            .AsNoTracking()
            .Select(c => new { c.Name, Words = c.WordCategories.Count })
            .ToListAsync();

        foreach (var c in allCategories.OrderByDescending(c => c.Words))
        {
            if (TaxonomyMap.ContainsFolded(TaxonomyMap.CategoriesThatArePartsOfSpeech, c.Name))
            {
                unmapped.Add(new UnmappedRow(c.Name, c.Words, "IS_A_PART_OF_SPEECH",
                    $"«{c.Name}» is a part of speech, not a subject field. It must not become a Domain."));
                continue;
            }

            if (!TaxonomyMap.ContainsFolded(TaxonomyMap.CategoryToDomain, c.Name))
            {
                unmapped.Add(new UnmappedRow(c.Name, c.Words, "NO_HOME_IN_DECK",
                    "No parent among the deck's ten dictionaries. Decide a parent, or retire it."));
            }
        }

        await WriteSenses(Path.Combine(outDir, "senses.csv"), senses);
        await WriteTranslations(Path.Combine(outDir, "translations.csv"), translations);
        await WriteUnmapped(Path.Combine(outDir, "unmapped.csv"), unmapped);

        Report(senses, translations, unmapped, unknownLabels, legacyRows, outDir);
    }

    private static SenseRow Build(
        Word word, WordMeans meaning, List<string> panes, List<string> categories, Guid? owner)
    {
        var reasons = new List<string>();
        string confidence = "HIGH";

        // ── جۆری وشە → PartOfSpeech ────────────────────────────────────────
        var mapped = panes
            .Select(p => TaxonomyMap.TryFolded(TaxonomyMap.SpeechPaneToPartOfSpeech, p, out var v) ? v : null)
            .Where(p => p is not null)
            .Distinct()
            .ToList();

        var unmappedPanes = panes
            .Where(p => !TaxonomyMap.ContainsFolded(TaxonomyMap.SpeechPaneToPartOfSpeech, p))
            .ToList();

        string proposedPos = "";

        if (mapped.Count == 1)
        {
            proposedPos = mapped[0]!;
        }
        else if (mapped.Count > 1)
        {
            // Legitimate linguistic ambiguity — زۆر is both هاوەڵناو and هاوەڵکار in the deck
            // itself. Not an error, but not something to pick for a teacher either.
            confidence = "LOW";
            reasons.Add($"word carries several parts of speech ({string.Join(" / ", mapped)}); a sense may only have one");
        }
        else if (panes.Count == 0)
        {
            confidence = "LOW";
            reasons.Add("word has no جۆری وشە at all");
        }
        else
        {
            confidence = "LOW";
            reasons.Add($"جۆری وشە «{string.Join(", ", unmappedPanes)}» has no equivalent among the seven");
        }

        // ── پۆل → Domain (never PartOfSpeech) ──────────────────────────────
        var domains = new List<string>();
        foreach (var cat in categories)
        {
            if (TaxonomyMap.TryFolded(TaxonomyMap.CategoriesThatArePartsOfSpeech, cat, out var asPos))
            {
                confidence = "LOW";
                reasons.Add($"پۆل «{cat}» is really the part of speech «{asPos}» — routed nowhere, needs a decision");
                continue;
            }

            if (TaxonomyMap.TryFolded(TaxonomyMap.CategoryToDomain, cat, out var parent))
                domains.Add($"{parent} › {cat}");
            else
                reasons.Add($"پۆل «{cat}» has no home among the deck's ten (see unmapped.csv)");
        }

        // ── Do پۆل and جۆری وشە disagree? ──────────────────────────────────
        foreach (var cat in categories)
        {
            if (!TaxonomyMap.TryFolded(TaxonomyMap.CategoriesThatArePartsOfSpeech, cat, out var catAsPos)) continue;
            if (proposedPos.Length == 0 || catAsPos == proposedPos) continue;

            confidence = "LOW";
            reasons.Add($"پۆل says «{catAsPos}», جۆری وشە says «{proposedPos}» — they disagree");
        }

        // ── Axis values: only where mechanically derivable ─────────────────
        var axisValues = new List<string>();
        var genderKu = word.Gender.ToKurdish();
        if (TaxonomyMap.TryFolded(TaxonomyMap.GenderToAxisValue, genderKu, out var axis))
            axisValues.Add(axis);

        if (string.IsNullOrWhiteSpace(meaning.Meaning))
        {
            confidence = "LOW";
            reasons.Add("definition is empty");
        }

        return new SenseRow(
            SenseId: meaning.Id,
            WordId: word.Id,
            Word: word.Kurdish,
            Definition: meaning.Meaning,
            CurrentPol: string.Join(" | ", categories),
            CurrentSpeechPane: string.Join(" | ", panes),
            ProposedPartOfSpeech: proposedPos,
            ProposedDomain: string.Join(" | ", domains),
            ProposedAxisValues: string.Join(" | ", axisValues),
            // Deck slide 14 requires an example on every sense and the current schema has nowhere
            // to have stored one, so every migrated sense starts without it. That is a fact to
            // surface, not a per-row conflict — see the summary.
            ProposedExample: "",
            Confidence: confidence,
            ConflictReason: string.Join(" ; ", reasons),
            OwnerUserId: owner,
            OwnerUserName: word.CreatedByUser?.UserName);
    }

    // ── CSV writing ────────────────────────────────────────────────────────
    // UTF-8 WITH BOM: Excel reads a BOM-less UTF-8 file as the system codepage and renders every
    // Kurdish column as mojibake, which would make the review impossible.
    private static readonly UTF8Encoding Utf8Bom = new(encoderShouldEmitUTF8Identifier: true);

    private static async Task WriteSenses(string path, List<SenseRow> rows)
    {
        var sb = new StringBuilder();
        sb.AppendLine(string.Join(",",
            "sense_id", "word_id", "word", "definition",
            "current_pol", "current_speech_pane",
            "proposed_part_of_speech", "proposed_domain", "proposed_axis_values", "proposed_example",
            "confidence", "conflict_reason",
            "owner_user_id", "owner_user_name",
            // The reviewer writes here. Phase 2 reads these columns, not the proposed_* ones.
            "human_part_of_speech", "human_domain", "human_axis_values", "human_example", "human_note"));

        foreach (var r in rows)
        {
            sb.AppendLine(string.Join(",",
                C(r.SenseId), C(r.WordId), C(r.Word), C(r.Definition),
                C(r.CurrentPol), C(r.CurrentSpeechPane),
                C(r.ProposedPartOfSpeech), C(r.ProposedDomain), C(r.ProposedAxisValues), C(r.ProposedExample),
                C(r.Confidence), C(r.ConflictReason),
                C(r.OwnerUserId?.ToString()), C(r.OwnerUserName),
                "", "", "", "", ""));
        }

        await File.WriteAllTextAsync(path, sb.ToString(), Utf8Bom);
    }

    private static async Task WriteTranslations(string path, List<TranslationRow> rows)
    {
        var sb = new StringBuilder();
        sb.AppendLine(string.Join(",",
            "meaning_id", "word_id", "word", "language_code", "original_label", "text", "owner_user_id"));

        foreach (var r in rows)
        {
            sb.AppendLine(string.Join(",",
                C(r.MeaningId), C(r.WordId), C(r.Word),
                C(r.LanguageCode), C(r.OriginalLabel), C(r.Text), C(r.OwnerUserId?.ToString())));
        }

        await File.WriteAllTextAsync(path, sb.ToString(), Utf8Bom);
    }

    private static async Task WriteUnmapped(string path, List<UnmappedRow> rows)
    {
        var sb = new StringBuilder();
        sb.AppendLine(string.Join(",", "category", "word_count", "reason_code", "reason", "human_decision"));

        foreach (var r in rows)
            sb.AppendLine(string.Join(",", C(r.Category), C(r.WordCount), C(r.ReasonCode), C(r.Reason), ""));

        await File.WriteAllTextAsync(path, sb.ToString(), Utf8Bom);
    }

    /// <summary>RFC 4180 quoting. A definition containing a comma would otherwise shift every column after it.</summary>
    private static string C(object? value)
    {
        var s = value?.ToString() ?? "";
        if (s.Length == 0) return "";

        var needsQuotes = s.Contains(',') || s.Contains('"') || s.Contains('\n') || s.Contains('\r');
        s = s.Replace("\r", " ").Replace("\n", " ");

        return needsQuotes ? $"\"{s.Replace("\"", "\"\"")}\"" : s;
    }

    private static string Redact(string connection) =>
        string.Join(";", connection.Split(';')
            .Where(p => !p.TrimStart().StartsWith("Password", StringComparison.OrdinalIgnoreCase)));

    private static void Report(
        List<SenseRow> senses, List<TranslationRow> translations, List<UnmappedRow> unmapped,
        Dictionary<string, int> unknownLabels, int legacyRows, string outDir)
    {
        var low = senses.Count(s => s.Confidence == "LOW");

        Console.WriteLine();
        Console.WriteLine("── proposal ────────────────────────────────────────────");
        Console.WriteLine($"  senses proposed      : {senses.Count,7:N0}");
        Console.WriteLine($"    confidence HIGH    : {senses.Count - low,7:N0}");
        Console.WriteLine($"    confidence LOW     : {low,7:N0}   ← phase 2 refuses these without a human value");
        Console.WriteLine($"  translations parked  : {translations.Count,7:N0}");
        Console.WriteLine($"  categories unmapped  : {unmapped.Count,7:N0}");
        Console.WriteLine();

        Console.WriteLine("── conflicts, most common first ────────────────────────");
        var byReason = senses
            .Where(s => s.ConflictReason.Length > 0)
            .SelectMany(s => s.ConflictReason.Split(" ; "))
            .GroupBy(r => Generalise(r))
            .OrderByDescending(g => g.Count())
            .Take(10);

        foreach (var g in byReason)
            Console.WriteLine($"  {g.Count(),7:N0}  {g.Key}");

        Console.WriteLine();
        Console.WriteLine("── attribution ─────────────────────────────────────────");
        if (legacyRows == 0)
        {
            Console.WriteLine("  every row has a real author. No legacy-import fallback is needed.");
        }
        else
        {
            // Loudly, as پڕۆمپت ٤ requires.
            Console.WriteLine($"  *** {legacyRows:N0} rows have NO author and would fall back to `legacy-import` ***");
        }

        if (unknownLabels.Count > 0)
        {
            Console.WriteLine();
            Console.WriteLine("── unrecognised language labels (treated as Sorani) ────");
            foreach (var (label, n) in unknownLabels.OrderByDescending(x => x.Value).Take(10))
                Console.WriteLine($"  {n,7:N0}  «{label}»");
        }

        Console.WriteLine();
        Console.WriteLine($"Nothing was written to the database. Review the CSVs in {outDir}");
    }

    /// <summary>Collapses per-row detail so the summary shows kinds of problem, not 3,000 lines.</summary>
    private static string Generalise(string reason)
    {
        var i = reason.IndexOf('«');
        var j = reason.LastIndexOf('»');
        return i >= 0 && j > i ? reason.Remove(i, j - i + 1).Insert(i, "…") : reason;
    }

    private record SenseRow(
        int SenseId, int WordId, string Word, string Definition,
        string CurrentPol, string CurrentSpeechPane,
        string ProposedPartOfSpeech, string ProposedDomain, string ProposedAxisValues, string ProposedExample,
        string Confidence, string ConflictReason,
        Guid? OwnerUserId, string? OwnerUserName);

    private record TranslationRow(
        int MeaningId, int WordId, string Word,
        string LanguageCode, string OriginalLabel, string Text, Guid? OwnerUserId);

    private record UnmappedRow(string Category, int WordCount, string ReasonCode, string Reason);
}
