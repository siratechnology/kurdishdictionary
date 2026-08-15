using backend.Data;
using backend.Data.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos;

namespace backend.Services.Lexicon;

/// <summary>
/// Moving every sense from one part of speech onto another.
///
/// This exists because adding a بەشی ئاخاوتن is cheap and populating it is not. A new one arrives
/// holding zero words, and the only way to fill it was the station: one sense, one keystroke, two
/// and a half thousand times. So the taxonomy could be corrected and the data could not, and the
/// new part of speech sat at ٠ وشە looking like a mistake.
///
/// It follows <see cref="MergeService"/>'s three rules, because this is the same kind of operation
/// on a bigger unit and the reasons have not changed:
///
///  · Preview before commit. The sense count, the word count, and — the one that actually decides
///    whether to go ahead — how many stored grammatical answers cannot come along.
///  · One event per moved sense. The interceptor walks the change tracker, so a bulk run still
///    writes per-word history. A single log line would put a hole in every affected word's story
///    exactly where its classification changed.
///  · Reversible. Each moved sense carries where it came from, so an undo puts back exactly the
///    rows this run moved and leaves alone anything the target already had.
///
/// What it will NOT do is carry incompatible answers across. Axes belong to a part of speech; a
/// sense moved to a part of speech that never asks about ژمارە cannot keep a ژمارە answer, because
/// no screen would ever show it again and the work queue would count it as coverage it does not
/// have. Those rows are soft-deleted, counted in the preview, and restored by the undo.
/// </summary>
public class PartOfSpeechReassignService
{
    /// <summary>How long the UI keeps offering the undo. The provenance itself is kept forever.</summary>
    public static readonly TimeSpan UndoWindow = TimeSpan.FromDays(30);

    /// <summary>Enough headwords to make a number feel like words. More is a list nobody reads.</summary>
    private const int SampleSize = 8;

    private readonly AppDbContext _db;

    public PartOfSpeechReassignService(AppDbContext db) => _db = db;

    /// <summary>What would happen. Writes nothing.</summary>
    public async Task<PartOfSpeechReassignPreviewDto> PreviewAsync(
        int fromId, int toId, CancellationToken ct = default)
    {
        if (fromId == toId)
            throw new InvalidOperationException("ناتوانرێت بەشێکی ئاخاوتن بخرێتە ناو خۆیەوە.");

        var from = await _db.PartsOfSpeech.AsNoTracking().FirstOrDefaultAsync(p => p.Id == fromId, ct)
            ?? throw new InvalidOperationException("ئەم بەشەی ئاخاوتن نەدۆزرایەوە.");

        var to = await _db.PartsOfSpeech.AsNoTracking().FirstOrDefaultAsync(p => p.Id == toId, ct)
            ?? throw new InvalidOperationException("بەشی ئاخاوتنی مەبەست نەدۆزرایەوە.");

        var senseIds = await _db.Senses.AsNoTracking()
            .Where(s => s.PartOfSpeechId == fromId)
            .Select(s => s.Id)
            .ToListAsync(ct);

        var wordsAffected = await _db.Senses.AsNoTracking()
            .Where(s => s.PartOfSpeechId == fromId)
            .Select(s => s.WordId)
            .Distinct()
            .CountAsync(ct);

        var sample = await _db.Senses.AsNoTracking()
            .Where(s => s.PartOfSpeechId == fromId)
            .OrderBy(s => s.Word.Kurdish)
            .Select(s => s.Word.Kurdish)
            .Distinct()
            .Take(SampleSize)
            .ToListAsync(ct);

        var keptAxisIds = await TargetAxisIdsAsync(toId, ct);

        var dropped = await _db.SenseFeatures.AsNoTracking()
            .Where(f => senseIds.Contains(f.SenseId) && !keptAxisIds.Contains(f.AxisId))
            .Select(f => new { f.Id, f.Axis.NameKu })
            .ToListAsync(ct);

        return new PartOfSpeechReassignPreviewDto
        {
            FromId = fromId,
            FromName = from.NameKu,
            ToId = toId,
            ToName = to.NameKu,
            SensesToMove = senseIds.Count,
            WordsAffected = wordsAffected,
            FeaturesDropped = dropped.Count,
            DroppedAxisNames = dropped.Select(d => d.NameKu).Distinct().OrderBy(n => n).ToList(),
            SampleWords = sample,
            Summary = $"{senseIds.Count} مانا لە «{from.NameKu}» دەچنە «{to.NameKu}»",
        };
    }

    /// <summary>Does it, in one transaction.</summary>
    public async Task<PartOfSpeechReassignResultDto> ExecuteAsync(
        int fromId, int toId, Guid userId, string reason, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new InvalidOperationException("بەشی ئاخاوتن بڕیارێکی زمانەوانییە — هۆکارەکەی بنووسە.");

        var preview = await PreviewAsync(fromId, toId, ct);

        await using var tx = await _db.Database.BeginTransactionAsync(ct);

        // One stamp for the whole run, taken once. Reading DateTime.UtcNow per row would give each
        // sense a slightly different time and there would be no way to name this run afterwards.
        var stamp = DateTime.UtcNow;

        var senses = await _db.Senses.Where(s => s.PartOfSpeechId == fromId).ToListAsync(ct);
        var senseIds = senses.Select(s => s.Id).ToList();

        var keptAxisIds = await TargetAxisIdsAsync(toId, ct);

        // Cleared BEFORE the senses move, so the ledger reads in the order things actually
        // happened: the answers to questions the old part of speech asked, then the move.
        var orphaned = await _db.SenseFeatures
            .Where(f => senseIds.Contains(f.SenseId) && !keptAxisIds.Contains(f.AxisId))
            .ToListAsync(ct);

        foreach (var feature in orphaned)
        {
            // The cascade flag makes the interceptor write FeatureCleared rather than
            // FeatureChanged — this answer was not overruled, the question stopped being asked.
            feature.ClearedByCascade = true;
            _db.SenseFeatures.Remove(feature);
        }

        foreach (var sense in senses)
        {
            sense.PartOfSpeechId = toId;
            sense.ReassignedFromPartOfSpeechId = fromId;
            sense.ReassignedAt = stamp;
        }

        // One SaveChanges, but the interceptor walks the change tracker entry by entry — so this
        // is still one event per sense and one per cleared answer, which is the point.
        await _db.SaveChangesAsync(ct);

        // The run itself, as one ledger line carrying the reason. IN ADDITION to the per-sense
        // events, not instead of them: those are the history, this is the explanation.
        _db.ContributionEvents.Add(new ContributionEvent
        {
            UserId = userId,
            EventType = ContributionEventType.FeatureChanged,
            EntityType = nameof(PartOfSpeech),
            EntityId = fromId,
            FieldName = "SensesReassigned",
            OldValue = preview.FromName,
            NewValue = preview.ToName,
            Note = reason.Trim(),
            SourceKind = ContributionSourceKind.Human,
        });

        await _db.SaveChangesAsync(ct);
        await tx.CommitAsync(ct);

        return new PartOfSpeechReassignResultDto
        {
            SensesMoved = senses.Count,
            FeaturesDropped = orphaned.Count,
            ReassignedAt = stamp,
        };
    }

    /// <summary>
    /// Runs still inside the undo window, newest first.
    ///
    /// Read out of the senses rather than a log table: the provenance columns already say which
    /// rows moved, where from and when, so grouping them IS the list — and it can never disagree
    /// with what the undo would actually touch, because it is the same rows.
    /// </summary>
    public async Task<List<PartOfSpeechReassignRunDto>> RecentAsync(CancellationToken ct = default)
    {
        var cutoff = DateTime.UtcNow - UndoWindow;

        var runs = await _db.Senses.AsNoTracking()
            .Where(s => s.ReassignedFromPartOfSpeechId != null
                     && s.ReassignedAt != null
                     && s.ReassignedAt >= cutoff)
            .GroupBy(s => new
            {
                FromId = s.ReassignedFromPartOfSpeechId!.Value,
                ToId = s.PartOfSpeechId,
                At = s.ReassignedAt!.Value,
            })
            .Select(g => new
            {
                g.Key.FromId,
                g.Key.ToId,
                g.Key.At,
                Count = g.Count(),
            })
            .ToListAsync(ct);

        if (runs.Count == 0) return new();

        var ids = runs.SelectMany(r => new[] { r.FromId, r.ToId ?? 0 }).Distinct().ToList();

        var names = await _db.PartsOfSpeech.AsNoTracking()
            .Where(p => ids.Contains(p.Id))
            .ToDictionaryAsync(p => p.Id, p => p.NameKu, ct);

        return runs
            .OrderByDescending(r => r.At)
            .Select(r => new PartOfSpeechReassignRunDto
            {
                FromId = r.FromId,
                FromName = names.GetValueOrDefault(r.FromId, "?"),
                ToId = r.ToId ?? 0,
                ToName = r.ToId is { } to ? names.GetValueOrDefault(to, "?") : "?",
                At = r.At,
                SenseCount = r.Count,
            })
            .ToList();
    }

    /// <summary>
    /// Puts one run back.
    ///
    /// Only the senses this run moved, identified by the pair it recorded on each row — a sense
    /// somebody has since classified onto the target by hand has no such marker and is left where
    /// it is. The answers cleared by the move come back too: they were soft-deleted, not destroyed,
    /// precisely so that this is possible.
    /// </summary>
    public async Task<PartOfSpeechReassignResultDto> UndoAsync(
        int fromId, DateTime reassignedAt, Guid userId, CancellationToken ct = default)
    {
        if (DateTime.UtcNow - reassignedAt > UndoWindow)
            throw new InvalidOperationException(
                $"ماوەی گەڕاندنەوە ({UndoWindow.TotalDays:0} ڕۆژ) بەسەرچووە.");

        var senses = await _db.Senses
            .Where(s => s.ReassignedFromPartOfSpeechId == fromId && s.ReassignedAt == reassignedAt)
            .ToListAsync(ct);

        if (senses.Count == 0)
            throw new InvalidOperationException("هیچ مانایەک لەم گواستنەوەیە نەماوە بۆ گەڕاندنەوە.");

        var senseIds = senses.Select(s => s.Id).ToList();

        await using var tx = await _db.Database.BeginTransactionAsync(ct);

        foreach (var sense in senses)
        {
            sense.PartOfSpeechId = fromId;
            sense.ReassignedFromPartOfSpeechId = null;
            sense.ReassignedAt = null;
        }

        // IgnoreQueryFilters, or the soft-deleted rows this is here to resurrect are invisible.
        var cleared = await _db.SenseFeatures
            .IgnoreQueryFilters()
            .Where(f => senseIds.Contains(f.SenseId) && f.IsDeleted && f.DeletedAt >= reassignedAt)
            .ToListAsync(ct);

        foreach (var feature in cleared)
        {
            feature.IsDeleted = false;
            feature.DeletedAt = null;
            feature.DeletedByUserId = null;
        }

        await _db.SaveChangesAsync(ct);

        _db.ContributionEvents.Add(new ContributionEvent
        {
            UserId = userId,
            EventType = ContributionEventType.FeatureChanged,
            EntityType = nameof(PartOfSpeech),
            EntityId = fromId,
            FieldName = "SensesReassignedUndone",
            NewValue = senses.Count.ToString(),
            SourceKind = ContributionSourceKind.Human,
        });

        await _db.SaveChangesAsync(ct);
        await tx.CommitAsync(ct);

        return new PartOfSpeechReassignResultDto
        {
            SensesMoved = senses.Count,
            FeaturesDropped = cleared.Count,
            ReassignedAt = reassignedAt,
        };
    }

    /// <summary>
    /// The axes the target part of speech asks about — every one it declares, conditional or not.
    ///
    /// Conditional axes are included deliberately. Whether the condition is satisfied depends on
    /// answers this sense may be about to lose, and re-deriving that per sense would delete an
    /// answer the entry form is going to ask for again the moment the parent value is re-picked.
    /// The generous rule keeps data; the strict one throws away work.
    /// </summary>
    private async Task<List<int>> TargetAxisIdsAsync(int toId, CancellationToken ct) =>
        await _db.PartOfSpeechAxes.AsNoTracking()
            .Where(a => a.PartOfSpeechId == toId)
            .Select(a => a.AxisId)
            .Distinct()
            .ToListAsync(ct);
}
