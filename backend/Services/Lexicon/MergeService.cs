using backend.Data;
using backend.Data.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos;

namespace backend.Services.Lexicon;

/// <summary>
/// Merging one taxonomy value into another (پڕۆمپت ١١).
///
/// This is the operation that repairs the 79 categories, and the settings area is not worth
/// shipping without it. «٤٨٤ مانا لە سیفەت دەچنە هاوەڵناو» is the sentence the whole feature exists
/// to make true.
///
/// Three rules it will not bend on:
///
///  · Preview before commit. The exact count moves, and any row that would end up holding the
///    target value twice on one axis is listed by name — a silent unique-constraint failure
///    halfway through would leave the taxonomy in a state nobody asked for.
///  · One event per moved row. A merge that collapses into a single log line puts a hole in every
///    affected word's history exactly where somebody's classification used to be.
///  · Reversible for 30 days. The source id is recorded on each moved row, so an undo can put them
///    back rather than guessing.
/// </summary>
public class MergeService
{
    /// <summary>How long a merge can be undone. After this the source id is still there, but the UI stops offering it.</summary>
    public static readonly TimeSpan UndoWindow = TimeSpan.FromDays(30);

    private readonly AppDbContext _db;

    public MergeService(AppDbContext db) => _db = db;

    /// <summary>
    /// What would happen. Changes nothing.
    /// </summary>
    public async Task<MergePreviewDto> PreviewAsync(int sourceValueId, int targetValueId, CancellationToken ct = default)
    {
        if (sourceValueId == targetValueId)
            throw new InvalidOperationException("ناتوانرێت نرخێک بخرێتە ناو خۆیەوە.");

        var source = await _db.FeatureValues.Include(v => v.Axis).FirstAsync(v => v.Id == sourceValueId, ct);
        var target = await _db.FeatureValues.Include(v => v.Axis).FirstAsync(v => v.Id == targetValueId, ct);

        if (source.AxisId != target.AxisId)
            throw new InvalidOperationException(
                $"«{source.NameKu}» لە تەوەری «{source.Axis.NameKu}»ە بەڵام «{target.NameKu}» لە «{target.Axis.NameKu}» — تەنها نرخی هەمان تەوەر تێکەڵ دەبن.");

        var moving = await _db.SenseFeatures
            .Where(f => f.ValueId == sourceValueId)
            .Select(f => f.SenseId)
            .ToListAsync(ct);

        // Senses that already hold the target on this axis. UNIQUE(SenseId, AxisId) means these
        // cannot simply be repointed — one of the two rows has to go, and that is a decision, not
        // a detail to discover mid-transaction.
        var conflicting = await _db.SenseFeatures
            .Where(f => f.ValueId == targetValueId && moving.Contains(f.SenseId))
            .Select(f => new MergeConflictDto
            {
                SenseId = f.SenseId,
                Word = f.Sense.Word.Kurdish,
                Definition = f.Sense.Definition,
            })
            .ToListAsync(ct);

        var dependentRules = await _db.PartOfSpeechAxes.CountAsync(a => a.RequiresValueId == sourceValueId, ct);

        return new MergePreviewDto
        {
            SourceValueId = sourceValueId,
            SourceName = source.NameKu,
            TargetValueId = targetValueId,
            TargetName = target.NameKu,
            AxisName = source.Axis.NameKu,
            RowsToMove = moving.Count,
            Conflicts = conflicting,
            DependentRuleCount = dependentRules,
            Summary = $"{moving.Count - conflicting.Count} مانا لە «{source.NameKu}» دەچنە «{target.NameKu}»",
        };
    }

    /// <summary>
    /// Does it, in one transaction.
    /// </summary>
    public async Task<int> ExecuteAsync(
        int sourceValueId, int targetValueId, Guid userId, string? reason, CancellationToken ct = default)
    {
        var preview = await PreviewAsync(sourceValueId, targetValueId, ct);

        await using var tx = await _db.Database.BeginTransactionAsync(ct);

        var rows = await _db.SenseFeatures
            .Where(f => f.ValueId == sourceValueId)
            .ToListAsync(ct);

        var conflictSenseIds = preview.Conflicts.Select(c => c.SenseId).ToHashSet();

        var moved = 0;
        foreach (var row in rows)
        {
            if (conflictSenseIds.Contains(row.SenseId))
            {
                // The sense already holds the target. Repointing would violate
                // UNIQUE(SenseId, AxisId), so the duplicate is retired instead — the answer it
                // carried is the one being merged away, and the sense keeps the target it already had.
                _db.SenseFeatures.Remove(row);
                continue;
            }

            row.ValueId = targetValueId;

            // Recorded on the row so an undo can put it back. Thirty days is a policy in the UI;
            // the fact itself is kept indefinitely, because throwing it away would make a merge
            // permanent the moment the window closed.
            row.Note = $"merged-from:{sourceValueId}";
            moved++;
        }

        // One SaveChanges, but the interceptor walks the change tracker row by row — so this is
        // still one FeatureChanged event per sense, which is the point.
        await _db.SaveChangesAsync(ct);

        var source = await _db.FeatureValues.FirstAsync(v => v.Id == sourceValueId, ct);
        source.IsActive = false;
        source.MergedIntoValueId = targetValueId;
        source.MergedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync(ct);

        // The merge itself, as one ledger line carrying the reason. This is IN ADDITION to the
        // per-row events, not instead of them: the per-row events are the history, this is the
        // explanation.
        _db.ContributionEvents.Add(new ContributionEvent
        {
            UserId = userId,
            EventType = ContributionEventType.FeatureChanged,
            EntityType = nameof(FeatureValue),
            EntityId = sourceValueId,
            FieldName = "MergedInto",
            OldValue = preview.SourceName,
            NewValue = preview.TargetName,
            Note = reason,
            SourceKind = ContributionSourceKind.Human,
        });

        await _db.SaveChangesAsync(ct);
        await tx.CommitAsync(ct);

        return moved;
    }

    /// <summary>
    /// Puts a merge back. Only offered inside the undo window, and only for rows this merge moved —
    /// a sense that was set to the target independently since then is left alone.
    /// </summary>
    public async Task<int> UndoAsync(int sourceValueId, Guid userId, CancellationToken ct = default)
    {
        var source = await _db.FeatureValues.FirstAsync(v => v.Id == sourceValueId, ct);

        if (source.MergedIntoValueId is not { } targetId || source.MergedAt is not { } mergedAt)
            throw new InvalidOperationException("ئەم نرخە تێکەڵ نەکراوە.");

        if (DateTime.UtcNow - mergedAt > UndoWindow)
            throw new InvalidOperationException(
                $"ماوەی گەڕاندنەوە ({UndoWindow.TotalDays:0} ڕۆژ) بەسەرچووە.");

        var marker = $"merged-from:{sourceValueId}";

        var rows = await _db.SenseFeatures
            .Where(f => f.ValueId == targetId && f.Note == marker)
            .ToListAsync(ct);

        foreach (var row in rows)
        {
            row.ValueId = sourceValueId;
            row.Note = null;
        }

        source.IsActive = true;
        source.MergedIntoValueId = null;
        source.MergedAt = null;

        await _db.SaveChangesAsync(ct);
        return rows.Count;
    }
}
