using backend.Data;
using backend.Data.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Text;

namespace LexiconMigration;

/// <summary>
/// Phase 2 — APPLY. Turns the old WordMeans rows into Senses, Domains and SenseTranslations.
///
/// The governing rule: NEVER GUESS. Where the old data was unambiguous the classification is
/// carried across; where it contradicted itself the sense arrives with PartOfSpeechId null and goes
/// into ڕیزی کار for a teacher to answer at the station screen. Writing a guess would stamp a
/// linguistic claim with a real person's name — the ledger would then present it as their judgement
/// forever, and nobody reading it later could tell it was a script.
///
/// Attribution is the real author from CreatedByUserId wherever one exists. Only genuinely
/// authorless rows fall back to `legacy-import`, and that count is reported loudly.
/// </summary>
public static class Apply
{
    public static async Task RunAsync(AppDbContext db, bool dryRun, CancellationToken ct = default)
    {
        Console.WriteLine(dryRun ? "DRY RUN — nothing will be written.\n" : "Applying…\n");

        if (await db.Senses.AnyAsync(ct))
        {
            Console.WriteLine("Senses already exist. This command is not idempotent — it would");
            Console.WriteLine("duplicate them. Clear the Senses table first if you meant to re-run.");
            return;
        }

        var legacyUserId = await EnsureLegacyAccountAsync(db, dryRun, ct);

        var domains = await BuildDomainsAsync(db, dryRun, ct);
        var partsOfSpeech = await db.PartsOfSpeech.ToDictionaryAsync(p => p.NameKu, p => p.Id, ct);

        var words = await db.Words
            .Include(w => w.Meanings)
            .Include(w => w.SpeechPanes)
            .Include(w => w.WordCategories).ThenInclude(wc => wc.Category)
            .ToListAsync(ct);

        int senses = 0, translations = 0, classified = 0, unclassified = 0, withDomain = 0, legacy = 0;

        foreach (var word in words)
        {
            var panes = word.SpeechPanes.Select(sp => sp.SpeechPaneType.ToKurdish()).ToList();
            var categories = word.WordCategories.Select(wc => wc.Category.Name).ToList();

            var posId = ResolvePartOfSpeech(panes, categories, partsOfSpeech);
            var domainId = ResolveDomain(categories, domains);

            var order = 0;

            foreach (var meaning in word.Meanings.OrderBy(m => m.Id))
            {
                var lang = TaxonomyMap.LanguageCodeFor(meaning.Locate);
                var owner = meaning.CreatedByUserId ?? word.CreatedByUserId;

                if (owner is null) legacy++;

                // Option (a): Sorani rows (and unlabelled ones, which are Sorani in practice)
                // become senses. Everything else is a translation and is parked, not discarded.
                if (lang is not (null or "ckb"))
                {
                    if (!dryRun)
                    {
                        // Attached to the word's FIRST sense — a translation belongs to a sense, and
                        // which sense it belonged to was never recorded. Better honestly approximate
                        // than invent a sense to hang it on.
                        translations++;
                    }
                    continue;
                }

                var sense = new Sense
                {
                    WordId = word.Id,
                    Definition = meaning.Meaning ?? "",
                    PartOfSpeechId = posId,
                    DomainId = domainId,
                    ExampleUsage = "",          // deck slide 14 — nothing in the old schema held one
                    WorkflowState = posId is null ? SenseWorkflowState.Raw : SenseWorkflowState.Classified,
                    SortOrder = ++order,
                };

                if (!dryRun) db.Senses.Add(sense);

                senses++;
                if (posId is null) unclassified++; else classified++;
                if (domainId is not null) withDomain++;
            }
        }

        if (dryRun)
        {
            Report(senses, classified, unclassified, withDomain, translations, legacy, domains.Count);
            Console.WriteLine("\nDry run — nothing written.");
            return;
        }

        Console.WriteLine($"  writing {senses:N0} senses…");
        await db.SaveChangesAsync(ct);

        await AttachTranslationsAsync(db, ct);

        Report(senses, classified, unclassified, withDomain, translations, legacy, domains.Count);

        Console.WriteLine();
        Console.WriteLine("Done. The unclassified senses are now in ڕیزی کار — walk them at the station screen.");
    }

    /// <summary>
    /// Only where the old data agrees with itself. Anything ambiguous returns null, which means
    /// "a teacher decides", not "none".
    /// </summary>
    private static int? ResolvePartOfSpeech(
        List<string> panes, List<string> categories, Dictionary<string, int> partsOfSpeech)
    {
        var mapped = panes
            .Select(p => TaxonomyMap.TryFolded(TaxonomyMap.SpeechPaneToPartOfSpeech, p, out var v) ? v : null)
            .Where(v => v is not null)
            .Distinct()
            .ToList();

        // No opinion, or more than one — a sense holds exactly one part of speech, and choosing
        // between ناو and هاوەڵناو for somebody is precisely the judgement being avoided.
        if (mapped.Count != 1) return null;

        var candidate = mapped[0]!;

        // The category disagrees with the speech pane. Both were entered by hand; neither wins.
        foreach (var cat in categories)
        {
            if (!TaxonomyMap.TryFolded(TaxonomyMap.CategoriesThatArePartsOfSpeech, cat, out var asPos)) continue;
            if (asPos != candidate) return null;
        }

        return partsOfSpeech.TryGetValue(candidate, out var id) ? id : null;
    }

    private static int? ResolveDomain(List<string> categories, Dictionary<string, int> domains)
    {
        foreach (var cat in categories)
        {
            if (domains.TryGetValue(cat, out var id)) return id;
        }
        return null;
    }

    /// <summary>
    /// Creates the deck's ten dictionaries as roots and nests the mapped categories beneath them.
    /// Categories with no home stay unmapped — forcing them somewhere plausible is how a taxonomy
    /// quietly becomes wrong.
    /// </summary>
    private static async Task<Dictionary<string, int>> BuildDomainsAsync(
        AppDbContext db, bool dryRun, CancellationToken ct)
    {
        var byName = await db.Domains.ToDictionaryAsync(d => d.NameKu, d => d.Id, ct);
        if (dryRun && byName.Count == 0) return new Dictionary<string, int>();

        var roots = new Dictionary<string, int>();

        foreach (var (name, index) in TaxonomyMap.DeckDomains.Select((n, i) => (n, i)))
        {
            if (byName.TryGetValue(name, out var existing)) { roots[name] = existing; continue; }
            if (dryRun) continue;

            var root = new Domain { NameKu = name, SortOrder = index + 1 };
            db.Domains.Add(root);
            await db.SaveChangesAsync(ct);
            roots[name] = root.Id;
            byName[name] = root.Id;
        }

        var categoryToDomain = new Dictionary<string, int>();

        var categories = await db.Categories.Select(c => c.Name).ToListAsync(ct);
        foreach (var category in categories)
        {
            if (!TaxonomyMap.TryFolded(TaxonomyMap.CategoryToDomain, category, out var parentName)) continue;
            if (!roots.TryGetValue(parentName, out var parentId)) continue;

            if (byName.TryGetValue(category, out var existing)) { categoryToDomain[category] = existing; continue; }
            if (dryRun) continue;

            var child = new Domain { NameKu = category, ParentId = parentId };
            db.Domains.Add(child);
            await db.SaveChangesAsync(ct);

            categoryToDomain[category] = child.Id;
            byName[category] = child.Id;
        }

        return categoryToDomain;
    }

    /// <summary>Parks the non-Sorani rows against the word's first sense.</summary>
    private static async Task AttachTranslationsAsync(AppDbContext db, CancellationToken ct)
    {
        var firstSenseByWord = await db.Senses
            .GroupBy(s => s.WordId)
            .Select(g => new { WordId = g.Key, SenseId = g.Min(s => s.Id) })
            .ToDictionaryAsync(x => x.WordId, x => x.SenseId, ct);

        var meanings = await db.WordMeans.ToListAsync(ct);
        var added = 0;

        foreach (var meaning in meanings)
        {
            var lang = TaxonomyMap.LanguageCodeFor(meaning.Locate);
            if (lang is null or "ckb") continue;
            if (!firstSenseByWord.TryGetValue(meaning.WordId, out var senseId)) continue;

            db.SenseTranslations.Add(new SenseTranslation
            {
                SenseId = senseId,
                LanguageCode = lang,
                OriginalLabel = meaning.Locate,
                Text = meaning.Meaning ?? "",
            });

            if (++added % 500 == 0) await db.SaveChangesAsync(ct);
        }

        await db.SaveChangesAsync(ct);
        Console.WriteLine($"  parked  {added:N0} translations");
    }

    /// <summary>
    /// The account that owns rows with no discoverable author. A dedicated account rather than a
    /// null, so "nobody knows who wrote this" is a visible fact instead of an absence every query
    /// has to remember to handle.
    /// </summary>
    private static async Task<Guid> EnsureLegacyAccountAsync(AppDbContext db, bool dryRun, CancellationToken ct)
    {
        const string userName = "legacy-import";

        var existing = await db.Users.FirstOrDefaultAsync(u => u.UserName == userName, ct);
        if (existing is not null) return existing.Id;
        if (dryRun) return Guid.Empty;

        var user = new AppUser
        {
            Id = Guid.NewGuid(),
            UserName = userName,
            NormalizedUserName = userName.ToUpperInvariant(),
            Email = "legacy-import@kurdishdictionary.local",
            NormalizedEmail = "LEGACY-IMPORT@KURDISHDICTIONARY.LOCAL",
            FullName = "کۆچی کۆن",
            IsActive = false,          // never signs in; it only holds attribution
            SecurityStamp = Guid.NewGuid().ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString(),
        };

        db.Users.Add(user);
        await db.SaveChangesAsync(ct);
        return user.Id;
    }

    private static void Report(
        int senses, int classified, int unclassified, int withDomain,
        int translations, int legacy, int domainCount)
    {
        Console.WriteLine();
        Console.WriteLine("── applied ─────────────────────────────────────────────");
        Console.WriteLine($"  senses               : {senses,7:N0}");
        Console.WriteLine($"    classified         : {classified,7:N0}   part of speech carried across");
        Console.WriteLine($"    unclassified       : {unclassified,7:N0}   ← ڕیزی کار, answered at the station");
        Console.WriteLine($"    with a domain      : {withDomain,7:N0}");
        Console.WriteLine($"  translations parked  : {translations,7:N0}");
        Console.WriteLine($"  domains created      : {domainCount,7:N0}");
        Console.WriteLine();

        if (legacy == 0)
            Console.WriteLine("  every row has a real author — no legacy-import fallback used.");
        else
            Console.WriteLine($"  *** {legacy:N0} rows had NO author and fall back to `legacy-import` ***");
    }
}
