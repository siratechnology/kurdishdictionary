using backend.Data;
using backend.Data.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos;

namespace backend.Services.Lexicon;

/// <summary>
/// What each teacher contributed, broken down BY DOMAIN.
///
/// The breakdown is by بواری زانستی because that is what goes in the printed acknowledgements —
/// «بەشی پزیشکی: ...», «بەشی ڕووەکناسی: ...». It is a credits list, not a performance report:
/// there is no ranking, no total to be top of, and no rate.
///
/// Every number is a COUNT over ContributionEvent. No counter column exists anywhere to disagree
/// with it, which is what makes a teacher's name in the printed book defensible.
/// </summary>
public class ContributorCreditService
{
    private readonly AppDbContext _db;

    public ContributorCreditService(AppDbContext db) => _db = db;

    public async Task<List<ContributorCreditDto>> GetCreditsAsync(CancellationToken ct = default)
    {
        var users = await _db.Users
            .AsNoTracking()
            .Select(u => new { u.Id, u.UserName, u.FullName })
            .ToListAsync(ct);

        // Word-scoped work, attributed through the word's senses to a domain. Senses whose domain
        // is not set are counted under a null domain rather than dropped — unfiled work is still
        // that person's work.
        var byDomain = await _db.ContributionEvents
            .AsNoTracking()
            .Where(e => e.WordId != null)
            .Join(_db.Senses.IgnoreQueryFilters(),
                  e => e.WordId, s => s.WordId,
                  (e, s) => new { e.UserId, e.EventType, s.DomainId })
            .GroupBy(x => new { x.UserId, x.DomainId })
            .Select(g => new
            {
                g.Key.UserId,
                g.Key.DomainId,
                WordsCreated = g.Count(x => x.EventType == ContributionEventType.WordCreated),
                SensesClassified = g.Count(x => x.EventType == ContributionEventType.SenseClassified ||
                                                x.EventType == ContributionEventType.SenseReclassified),
                FeaturesSet = g.Count(x => x.EventType == ContributionEventType.FeatureSet ||
                                           x.EventType == ContributionEventType.FeatureChanged),
                RelationsAdded = g.Count(x => x.EventType == ContributionEventType.RelationAdded),
                FormsAdded = g.Count(x => x.EventType == ContributionEventType.FormAdded),
            })
            .ToListAsync(ct);

        var domains = await _db.Domains
            .AsNoTracking()
            .Select(d => new { d.Id, d.NameKu })
            .ToDictionaryAsync(d => d.Id, d => d.NameKu, ct);

        var totals = await _db.ContributionStats.AsNoTracking().ToListAsync(ct);

        return users
            .Select(u =>
            {
                var rows = byDomain.Where(d => d.UserId == u.Id).ToList();
                var total = totals.FirstOrDefault(t => t.UserId == u.Id);

                return new ContributorCreditDto
                {
                    UserId = u.Id,
                    UserName = u.UserName ?? "",
                    FullName = u.FullName,
                    WordsCreated = total?.WordsCreated ?? 0,
                    SensesClassified = total?.SensesClassified ?? 0,
                    RelationsAdded = total?.RelationsAdded ?? 0,
                    FormsAdded = total?.FormsAdded ?? 0,
                    FirstContributionAt = total?.FirstContributionAt,
                    LastContributionAt = total?.LastContributionAt,
                    Domains = rows
                        .Where(r => r.WordsCreated + r.SensesClassified + r.FeaturesSet +
                                    r.RelationsAdded + r.FormsAdded > 0)
                        .Select(r => new ContributorDomainCreditDto
                        {
                            DomainId = r.DomainId,
                            Domain = r.DomainId is { } id && domains.TryGetValue(id, out var name)
                                ? name
                                : "بێ بوار",
                            WordsCreated = r.WordsCreated,
                            SensesClassified = r.SensesClassified,
                            FeaturesSet = r.FeaturesSet,
                            RelationsAdded = r.RelationsAdded,
                            FormsAdded = r.FormsAdded,
                        })
                        .OrderByDescending(d => d.SensesClassified + d.WordsCreated)
                        .ToList(),
                };
            })
            // Alphabetical, deliberately. Sorting by output would make this a league table, which
            // is the one thing پڕۆمپت ٧ says it must never become.
            .OrderBy(c => c.FullName ?? c.UserName, StringComparer.Ordinal)
            .ToList();
    }

    /// <summary>
    /// The credits as they would be printed: one paragraph per domain, naming everyone who worked
    /// on it. This is the artefact the whole ledger exists to produce.
    /// </summary>
    public async Task<string> ExportAsync(CancellationToken ct = default)
    {
        var credits = await GetCreditsAsync(ct);
        var sb = new System.Text.StringBuilder();

        sb.AppendLine("# ئامادەکارانی فەرهەنگ");
        sb.AppendLine();

        var byDomain = credits
            .SelectMany(c => c.Domains.Select(d => new { Credit = c, Domain = d }))
            .GroupBy(x => x.Domain.Domain)
            .OrderBy(g => g.Key, StringComparer.Ordinal);

        foreach (var group in byDomain)
        {
            sb.AppendLine($"## بەشی {group.Key}");

            var names = group
                .OrderBy(x => x.Credit.FullName ?? x.Credit.UserName, StringComparer.Ordinal)
                .Select(x => x.Credit.FullName ?? x.Credit.UserName);

            sb.AppendLine(string.Join("، ", names));
            sb.AppendLine();
        }

        var unattributed = credits.Where(c => c.Domains.Count == 0).ToList();
        if (unattributed.Count > 0)
        {
            sb.AppendLine("## بەشداربووانی دیکە");
            sb.AppendLine(string.Join("، ", unattributed.Select(c => c.FullName ?? c.UserName)));
        }

        return sb.ToString();
    }

    /// <summary>
    /// Every event on one word: who, when, old → new, and any note.
    ///
    /// This is the proof that a teacher's work still exists, so it must never be able to show a
    /// gap — which is why soft delete exists and why bulk operations emit one event per row.
    /// </summary>
    public async Task<List<WordHistoryEntryDto>> GetWordHistoryAsync(int wordId, CancellationToken ct = default)
    {
        return await _db.ContributionEvents
            .AsNoTracking()
            .Where(e => e.WordId == wordId)
            .OrderBy(e => e.Id)
            .Join(_db.Users, e => e.UserId, u => u.Id, (e, u) => new WordHistoryEntryDto
            {
                Id = e.Id,
                OccurredAt = e.OccurredAt,
                UserName = u.UserName ?? "",
                FullName = u.FullName,
                EventType = e.EventType.ToString(),
                EntityType = e.EntityType,
                FieldName = e.FieldName,
                OldValue = e.OldValue,
                NewValue = e.NewValue,
                Note = e.Note,
                SourceKind = e.SourceKind.ToString(),
            })
            .ToListAsync(ct);
    }
}
