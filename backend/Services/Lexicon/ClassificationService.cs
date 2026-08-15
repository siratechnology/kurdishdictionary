using backend.Data;
using backend.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services.Lexicon;

/// <summary>
/// The outcome of answering one axis.
///
/// <paramref name="Refusal"/> is not an exception because the only thing that can refuse here is the
/// selection cap, and hitting a cap is an ordinary thing a teacher does — the control says «٣ لە ٣
/// هەڵبژێردراو» and they click a fourth. It deserves a sentence on screen, not a stack trace.
/// </summary>
public sealed record FeatureChange(SenseDisagreement? Disagreement = null, string? Refusal = null)
{
    public static readonly FeatureChange None = new();
}

/// <summary>
/// Setting a sense's features, and everything that has to be true around it.
///
/// Three rules drive this class:
///
///  · A second teacher changing a first teacher's answer records a DISAGREEMENT, not a correction.
///    زۆر is هاوەڵناو and هاوەڵکار in the source deck itself; overwriting silently would throw away
///    the fact that two experts read the word differently, which is data the dictionary wants.
///
///  · The escape hatch never blocks a save. A teacher who cannot save until the taxonomy fits will
///    pick a wrong value, and a wrong value is worse than an honest "this does not apply".
///
///  · How many values an axis holds is <see cref="FeatureAxis.MaxSelections"/> — data, never a
///    branch on a part of speech or an axis name. Nothing in this file knows what ناو is.
/// </summary>
public class ClassificationService
{
    private readonly AppDbContext _db;

    public ClassificationService(AppDbContext db) => _db = db;

    /// <summary>
    /// Records an answer on one axis, following that axis's configured selection count.
    ///
    ///   MaxSelections == 1  → picking a value REPLACES whatever was there. A radio group.
    ///   MaxSelections != 1  → picking a value TOGGLES it, up to the cap. Checkboxes.
    ///
    /// Either way, a value and <see cref="SenseFeature.IsNotApplicable"/> are mutually exclusive:
    /// selecting anything clears the not-applicable flag. They are one control on screen and they
    /// have to be one fact in the database, or a sense ends up simultaneously answered and declared
    /// inapplicable and nothing downstream can decide which it is.
    /// </summary>
    public async Task<FeatureChange> SetFeatureAsync(
        int senseId, int axisId, int valueId, Guid userId, CancellationToken ct = default)
    {
        var axis = await _db.FeatureAxes
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == axisId, ct);

        // MaxSelections is nullable and null means UNLIMITED, so it cannot be coalesced to 1 — that
        // would turn "هەرچەند بێت" into "تەنها یەک" and quietly refuse the second answer. Only an
        // axis that does not exist falls back to single choice, which is the safe default.
        var singleSelect = axis is null || axis.MaxSelections == 1;
        var cap = axis?.MaxSelections;

        var rows = await _db.SenseFeatures
            .Where(f => f.SenseId == senseId && f.AxisId == axisId)
            .ToListAsync(ct);

        var notApplicable = rows.FirstOrDefault(f => f.IsNotApplicable);
        var held = rows.Where(f => f.ValueId is not null).ToList();

        // ── Single choice ───────────────────────────────────────────────────
        if (singleSelect)
        {
            var existing = held.FirstOrDefault() ?? notApplicable;

            if (existing is null)
            {
                _db.SenseFeatures.Add(new SenseFeature { SenseId = senseId, AxisId = axisId, ValueId = valueId });
                await _db.SaveChangesAsync(ct);
                return FeatureChange.None;
            }

            if (existing.ValueId == valueId && !existing.IsNotApplicable)
                return FeatureChange.None;   // agreeing with what is already there is not an event

            var disagreement = await RecordDisagreementIfSomeoneElsesAsync(
                senseId, axisId, existing, valueId, userId, ct);

            // Reused rather than deleted-and-inserted: the row's id is what the ledger points at,
            // so re-creating it would detach this answer from the history of who gave it.
            existing.ValueId = valueId;
            existing.IsNotApplicable = false;
            existing.Note = null;
            existing.IsDeleted = false;

            // Any stray extra rows from a period when this axis allowed several. Clearing them is
            // what makes lowering the cap back to one honest.
            foreach (var extra in held.Where(f => f != existing))
                _db.SenseFeatures.Remove(extra);

            await _db.SaveChangesAsync(ct);
            return new FeatureChange(disagreement);
        }

        // ── Several ─────────────────────────────────────────────────────────
        var already = held.FirstOrDefault(f => f.ValueId == valueId);

        if (already is not null)
        {
            // Toggling off. The caller then re-resolves the tree, which clears exactly this value's
            // descendants and leaves its siblings' subtrees alone.
            _db.SenseFeatures.Remove(already);
            await _db.SaveChangesAsync(ct);
            return FeatureChange.None;
        }

        if (cap is { } limit && held.Count >= limit)
        {
            // Say what the limit is rather than ignoring the click. A control that silently does
            // nothing reads as broken, and the teacher clicks it again.
            return new FeatureChange(
                Refusal: $"لەم تەوەرەدا تەنها {limit} بژاردە دەکرێت هەڵبژێردرێت.");
        }

        if (notApplicable is not null)
        {
            // Reuse the not-applicable row as the first selection: one UPDATE instead of a delete
            // and an insert racing each other against the unique index.
            notApplicable.ValueId = valueId;
            notApplicable.IsNotApplicable = false;
            notApplicable.Note = null;
            notApplicable.IsDeleted = false;
        }
        else
        {
            _db.SenseFeatures.Add(new SenseFeature { SenseId = senseId, AxisId = axisId, ValueId = valueId });
        }

        await _db.SaveChangesAsync(ct);
        return FeatureChange.None;
    }

    /// <summary>
    /// «ئەم تەوەرە بۆ ئەم وشە کار ناکات» — the escape hatch.
    ///
    /// Sets the axis to not-applicable, requires a reason, and routes the sense to the کێشەدار
    /// queue, which is the input for revising the source taxonomy. Saving is never blocked by it.
    ///
    /// Exclusivity works in this direction too: marking an axis not-applicable clears EVERY value it
    /// holds, however many the cap allowed.
    /// </summary>
    public async Task MarkNotApplicableAsync(
        int senseId, int axisId, string note, Guid userId, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(note))
            throw new InvalidOperationException("هۆکارێک بنووسە بۆ ئەوەی ئەم تەوەرە کار ناکات.");

        var rows = await _db.SenseFeatures
            .Where(f => f.SenseId == senseId && f.AxisId == axisId)
            .ToListAsync(ct);

        // Reuse one row and retire the rest, rather than deleting all and inserting: the filtered
        // unique index would otherwise have to tolerate an insert and a soft delete of the same key
        // inside one SaveChanges, whose statement order EF is free to choose.
        var keeper = rows.FirstOrDefault();

        if (keeper is null)
        {
            _db.SenseFeatures.Add(new SenseFeature
            {
                SenseId = senseId,
                AxisId = axisId,
                ValueId = null,
                IsNotApplicable = true,
                Note = note.Trim(),
            });
        }
        else
        {
            keeper.ValueId = null;
            keeper.IsNotApplicable = true;
            keeper.Note = note.Trim();
            keeper.IsDeleted = false;

            foreach (var extra in rows.Where(f => f != keeper))
                _db.SenseFeatures.Remove(extra);
        }

        // کێشەدار is not a rejection. It means the taxonomy did not fit, and a pattern across
        // twenty words is exactly what the settings area needs to see.
        var sense = await _db.Senses.FirstAsync(s => s.Id == senseId, ct);
        sense.WorkflowState = SenseWorkflowState.Disputed;

        await _db.SaveChangesAsync(ct);
    }

    /// <summary>
    /// Publishes according to trust level: a Senior goes straight to بڵاوکراو, a Contributor's work
    /// stops at پۆلێنکراو and waits in a Senior's queue.
    ///
    /// The one hard gate in the whole system lives here: no publish without a part of speech. Every
    /// axis and every sub-option below it is optional at every depth, so nothing else may refuse.
    /// </summary>
    public async Task<SenseWorkflowState> SubmitAsync(int senseId, Guid userId, CancellationToken ct = default)
    {
        var sense = await _db.Senses.FirstAsync(s => s.Id == senseId, ct);
        var user = await _db.Users.FirstAsync(u => u.Id == userId, ct);

        // A sense nobody has classified yet stays خام and stays off the public site. It is not an
        // error and it must not be one: a teacher who does not know the part of speech has to be
        // able to save and move on, or they will guess, and a guess looks finished forever.
        if (sense.PartOfSpeechId is null)
        {
            sense.WorkflowState = SenseWorkflowState.Raw;
            await _db.SaveChangesAsync(ct);
            return sense.WorkflowState;
        }

        sense.WorkflowState = user.TrustLevel == TrustLevel.Senior
            ? SenseWorkflowState.Published
            : SenseWorkflowState.Classified;

        await _db.SaveChangesAsync(ct);

        // ~5% of Senior-published senses get a terminology read. Sampled at random, so being drawn
        // says nothing about the person — and it is surfaced as «یەکڕێزی زاراوە», never as review.
        if (sense.WorkflowState == SenseWorkflowState.Published && Random.Shared.NextDouble() < 0.05)
        {
            _db.ConsistencySamples.Add(new ConsistencySample { SenseId = senseId });
            await _db.SaveChangesAsync(ct);
        }

        return sense.WorkflowState;
    }

    /// <summary>
    /// Applies one axis value across a filtered set — the operation that makes an expert fast.
    ///
    /// Every affected sense emits its OWN FeatureSet event. A bulk action must never collapse into
    /// a single log line, or per-word history develops a hole exactly where somebody's work was.
    /// </summary>
    public async Task<int> BulkSetFeatureAsync(
        IReadOnlyCollection<int> senseIds, int axisId, int valueId, Guid userId, CancellationToken ct = default)
    {
        if (senseIds.Count == 0) return 0;

        var axis = await _db.FeatureAxes.AsNoTracking().FirstOrDefaultAsync(a => a.Id == axisId, ct);

        // Null means unlimited, so it must not be coalesced to 1 — see SetFeatureAsync.
        var singleSelect = axis is null || axis.MaxSelections == 1;

        var existing = await _db.SenseFeatures
            .Where(f => senseIds.Contains(f.SenseId) && f.AxisId == axisId)
            .ToListAsync(ct);

        var bySense = existing.GroupBy(f => f.SenseId).ToDictionary(g => g.Key, g => g.ToList());

        foreach (var senseId in senseIds)
        {
            var rows = bySense.GetValueOrDefault(senseId) ?? new List<SenseFeature>();

            if (rows.Any(f => f.ValueId == valueId && !f.IsNotApplicable)) continue;

            if (singleSelect)
            {
                var row = rows.FirstOrDefault();

                if (row is null)
                {
                    _db.SenseFeatures.Add(new SenseFeature { SenseId = senseId, AxisId = axisId, ValueId = valueId });
                    continue;
                }

                row.ValueId = valueId;
                row.IsNotApplicable = false;
                row.Note = null;
                row.IsDeleted = false;

                foreach (var extra in rows.Where(f => f != row))
                    _db.SenseFeatures.Remove(extra);

                continue;
            }

            // Multi-select: ADD the value, leaving whatever else the sense already said. A bulk
            // action that quietly replaced a teacher's other answers would be a data loss disguised
            // as a convenience.
            var cap = axis?.MaxSelections;
            var heldCount = rows.Count(f => f.ValueId is not null);
            if (cap is { } limit && heldCount >= limit) continue;

            var notApplicable = rows.FirstOrDefault(f => f.IsNotApplicable);

            if (notApplicable is not null)
            {
                notApplicable.ValueId = valueId;
                notApplicable.IsNotApplicable = false;
                notApplicable.Note = null;
                notApplicable.IsDeleted = false;
            }
            else
            {
                _db.SenseFeatures.Add(new SenseFeature { SenseId = senseId, AxisId = axisId, ValueId = valueId });
            }
        }

        // One SaveChanges, but the interceptor walks the change tracker entry by entry — so this is
        // still one event per sense, not one event for the batch.
        await _db.SaveChangesAsync(ct);
        return senseIds.Count;
    }

    /// <summary>How many senses a bulk action would touch. Shown before it runs, never after.</summary>
    public Task<int> CountForBulkAsync(int partOfSpeechId, int axisId, CancellationToken ct = default) =>
        _db.Senses.CountAsync(s =>
            s.PartOfSpeechId == partOfSpeechId &&
            !s.Features.Any(f => f.AxisId == axisId), ct);

    private async Task<SenseDisagreement?> RecordDisagreementIfSomeoneElsesAsync(
        int senseId, int axisId, SenseFeature existing, int newValueId, Guid userId, CancellationToken ct)
    {
        // Who set the current answer? The ledger knows; the feature row does not carry an author.
        var previousAuthor = await _db.ContributionEvents
            .Where(e => e.EntityType == nameof(SenseFeature) && e.EntityId == existing.Id)
            .OrderBy(e => e.Id)
            .Select(e => (Guid?)e.UserId)
            .FirstOrDefaultAsync(ct);

        // Changing your own mind is not a disagreement, and neither is filling in a blank.
        if (previousAuthor is null || previousAuthor == userId) return null;
        if (existing.ValueId is null && !existing.IsNotApplicable) return null;

        var oldLabel = existing.IsNotApplicable
            ? "نەگونجاو"
            : await _db.FeatureValues.Where(v => v.Id == existing.ValueId)
                                     .Select(v => v.NameKu).FirstOrDefaultAsync(ct) ?? "?";

        var newLabel = await _db.FeatureValues.Where(v => v.Id == newValueId)
                                              .Select(v => v.NameKu).FirstOrDefaultAsync(ct) ?? "?";

        var disagreement = new SenseDisagreement
        {
            SenseId = senseId,
            AxisId = axisId,
            FirstJudgement = oldLabel,
            FirstUserId = previousAuthor.Value,
            FirstNote = existing.Note,
            SecondJudgement = newLabel,
            SecondUserId = userId,
        };

        _db.SenseDisagreements.Add(disagreement);
        return disagreement;
    }
}
