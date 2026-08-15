using backend.Data;
using backend.Data.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos;

namespace backend.Services.Lexicon;

/// <summary>
/// Managing the taxonomy itself (پڕۆمپت ١١).
///
/// READ THIS BEFORE ADDING A METHOD. The disease in this project is category sprawl: 26 categories
/// became 79 in one month because creating one was easy. If this service makes adding easy for
/// everyone, the same bug is rebuilt in a new table.
///
/// So EDITING and EXTENDING are separate permissions, deliberately:
///   Tier 1 — any admin        : rename · reorder · deactivate/reactivate · edit description
///   Tier 2 — LinguisticOwner  : add/remove a VALUE inside an axis · merge values · conditional rules
///   Tier 3 — Admin + a reason : add/remove an AXIS · add/remove a PART OF SPEECH · top-level domain
///
/// Tier 3 sits with Admin rather than with the linguistic owner because it must feel deliberate: a
/// part of speech is a linguistic claim, not a data-entry convenience, and the seven are closed by
/// default. The mandatory reason is stored on the ledger event, so opening that list is explained
/// forever, to whoever asks next year.
/// </summary>
public class TaxonomyAdminService
{
    private readonly AppDbContext _db;

    public TaxonomyAdminService(AppDbContext db) => _db = db;

    // ═══════════════════════════════════════════════════════════════════════
    // Tier 1 — rename, reorder, deactivate. What the team needs TODAY:
    // ئاوەڵناو→هاوەڵناو, هەوەڵکار→هاوەڵکار, داڕێژڕاو→داڕێژراو.
    // ═══════════════════════════════════════════════════════════════════════

    /// <summary>
    /// Renames a value IN PLACE.
    ///
    /// This is an UPDATE and must never become delete-and-insert. The row's id is pointed at by
    /// every SenseFeature that uses it and by every ContributionEvent that ever mentioned it;
    /// re-creating the row would break all of them silently, and a teacher's history would start
    /// referring to a value that no longer exists.
    /// </summary>
    public async Task RenameValueAsync(int valueId, string newName, CancellationToken ct = default)
    {
        var value = await _db.FeatureValues.FirstAsync(v => v.Id == valueId, ct);

        var trimmed = (newName ?? "").Trim();
        if (trimmed.Length == 0)
            throw new InvalidOperationException("ناوی نوێ ناتوانێت بەتاڵ بێت.");

        // Two values on one axis may not share a name.
        var clash = await _db.FeatureValues
            .AnyAsync(v => v.AxisId == value.AxisId && v.Id != valueId && v.NameKu == trimmed, ct);

        if (clash)
            throw new InvalidOperationException($"«{trimmed}» پێشتر لەم تەوەرەدا هەیە.");

        value.NameKu = trimmed;
        await _db.SaveChangesAsync(ct);
    }

    public async Task RenameAxisAsync(int axisId, string newName, string? description, CancellationToken ct = default)
    {
        var axis = await _db.FeatureAxes.FirstAsync(a => a.Id == axisId, ct);

        var trimmed = (newName ?? "").Trim();
        if (trimmed.Length == 0)
            throw new InvalidOperationException("ناوی نوێ ناتوانێت بەتاڵ بێت.");

        axis.NameKu = trimmed;
        if (description is not null) axis.Description = description.Trim();

        await _db.SaveChangesAsync(ct);
    }

    public async Task RenamePartOfSpeechAsync(int id, string newName, string? description, CancellationToken ct = default)
    {
        var pos = await _db.PartsOfSpeech.FirstAsync(p => p.Id == id, ct);

        var trimmed = (newName ?? "").Trim();
        if (trimmed.Length == 0)
            throw new InvalidOperationException("ناوی نوێ ناتوانێت بەتاڵ بێت.");

        // Code is deliberately NOT touched. Rules key on it (see TaxonomyCodes), and a rename that
        // moved the code would silently stop those rules firing.
        pos.NameKu = trimmed;
        if (description is not null) pos.Description = description.Trim();

        await _db.SaveChangesAsync(ct);
    }

    public async Task ReorderValuesAsync(IReadOnlyList<int> orderedIds, CancellationToken ct = default)
    {
        var values = await _db.FeatureValues
            .Where(v => orderedIds.Contains(v.Id))
            .ToListAsync(ct);

        for (var i = 0; i < orderedIds.Count; i++)
        {
            var value = values.FirstOrDefault(v => v.Id == orderedIds[i]);
            if (value is not null) value.SortOrder = i + 1;
        }

        await _db.SaveChangesAsync(ct);
    }

    /// <summary>
    /// Deactivating stops a value being OFFERED. Existing data still renders it and history stays
    /// intact — which is the difference between retiring a term and rewriting the past.
    /// </summary>
    public async Task SetValueActiveAsync(int valueId, bool isActive, CancellationToken ct = default)
    {
        var value = await _db.FeatureValues.FirstAsync(v => v.Id == valueId, ct);
        value.IsActive = isActive;
        await _db.SaveChangesAsync(ct);
    }

    /// <summary>
    /// Removes a value entirely from the settings area. Permitted ONLY at usage count zero.
    ///
    /// Two rules meet here and it is worth being precise about the result:
    ///   · پڕۆمپت ١١ — deletion is allowed only when nothing points at the value.
    ///   · پڕۆمپت ٢  — nothing in the lexicon is ever hard-deleted, because a row the ledger points
    ///                 at cannot vanish without turning somebody's history into a gap.
    ///
    /// So this is a SOFT delete, and the difference from Deactivate is what the team sees:
    ///   Deactivate → stops being offered, still listed in the collapsed section, still renders in
    ///                existing data. The right answer for a term being retired.
    ///   Delete     → gone from the settings area altogether. Only ever reachable at usage zero,
    ///                so nothing on screen changes for anyone, and the row survives for the ledger.
    ///
    /// Deleting a value that 400 senses point at would take those senses' answers with it — and
    /// the teachers' record of having answered. Hence the guard rather than a confirm dialog.
    /// </summary>
    public async Task DeleteValueAsync(int valueId, CancellationToken ct = default)
    {
        var usage = await UsageCountAsync(valueId, ct);
        if (usage > 0)
            throw new InvalidOperationException(
                $"ئەم نرخە لە {usage} مانادا بەکارهاتووە — ناتوانرێت بسڕدرێتەوە، تەنها ناچالاک بکرێت.");

        var dependents = await _db.PartOfSpeechAxes.CountAsync(a => a.RequiresValueId == valueId, ct);
        if (dependents > 0)
            throw new InvalidOperationException(
                $"{dependents} ڕێسای مەرجدار پشت بەم نرخە دەبەستن — سەرەتا ئەوان بگۆڕە.");

        var value = await _db.FeatureValues.FirstAsync(v => v.Id == valueId, ct);
        _db.FeatureValues.Remove(value);
        await _db.SaveChangesAsync(ct);
    }

    // ═══════════════════════════════════════════════════════════════════════
    // Usage counts — everywhere.
    //
    // Right now nobody can see that most of the 79 categories hold two or three words each. Once
    // the count is visible, the sprawl becomes self-correcting: an editor looking at a list sorted
    // by usage ascending sees the long tail immediately and starts merging it.
    // ═══════════════════════════════════════════════════════════════════════

    public Task<int> UsageCountAsync(int valueId, CancellationToken ct = default) =>
        _db.SenseFeatures.CountAsync(f => f.ValueId == valueId, ct);

    /// <summary>Every value on an axis with its live usage and the date it was last used.</summary>
    public async Task<List<TaxonomyValueDto>> GetValuesAsync(int axisId, CancellationToken ct = default)
    {
        var values = await _db.FeatureValues
            .IgnoreQueryFilters()
            .Where(v => v.AxisId == axisId && !v.IsDeleted)
            .Select(v => new
            {
                v.Id, v.NameKu, v.Code, v.SortOrder, v.IsActive, v.MergedIntoValueId, v.MergedAt,
                Usage = _db.SenseFeatures.Count(f => f.ValueId == v.Id),
                DependentRules = _db.PartOfSpeechAxes.Count(a => a.RequiresValueId == v.Id),
            })
            .ToListAsync(ct);

        // Last used comes from the ledger, not from a column — a "last used" counter would be one
        // more number able to drift from the events it summarises.
        var ids = values.Select(v => v.Id).ToList();
        var lastUsed = await _db.ContributionEvents
            .Where(e => e.EntityType == nameof(SenseFeature) && e.NewValue != null)
            .GroupBy(e => e.NewValue)
            .Select(g => new { Value = g.Key, When = g.Max(x => x.OccurredAt) })
            .ToDictionaryAsync(x => x.Value!, x => x.When, ct);

        return values
            .Select(v => new TaxonomyValueDto
            {
                Id = v.Id,
                NameKu = v.NameKu,
                Code = v.Code,
                SortOrder = v.SortOrder,
                IsActive = v.IsActive,
                UsageCount = v.Usage,
                DependentRuleCount = v.DependentRules,
                LastUsedAt = lastUsed.GetValueOrDefault(v.Id.ToString()),
                MergedIntoValueId = v.MergedIntoValueId,
                MergedAt = v.MergedAt,
            })
            // Ascending by usage, by default. That is what surfaces the long tail.
            .OrderBy(v => v.UsageCount)
            .ThenBy(v => v.SortOrder)
            .ToList();
    }

    /// <summary>«بەکارنەهاتووەکان» — values nothing points at. The bulk-deactivate candidates.</summary>
    public async Task<List<TaxonomyValueDto>> GetUnusedValuesAsync(CancellationToken ct = default)
    {
        var unused = await _db.FeatureValues
            .Where(v => v.IsActive && !_db.SenseFeatures.Any(f => f.ValueId == v.Id))
            .Select(v => new TaxonomyValueDto
            {
                Id = v.Id,
                NameKu = v.NameKu,
                AxisName = v.Axis.NameKu,
                SortOrder = v.SortOrder,
                IsActive = v.IsActive,
                UsageCount = 0,
                DependentRuleCount = _db.PartOfSpeechAxes.Count(a => a.RequiresValueId == v.Id),
            })
            .ToListAsync(ct);

        return unused;
    }

    public async Task<int> BulkDeactivateAsync(IReadOnlyCollection<int> valueIds, CancellationToken ct = default)
    {
        var values = await _db.FeatureValues.Where(v => valueIds.Contains(v.Id)).ToListAsync(ct);

        // Re-check usage at the moment of the action, not from whatever the screen was showing.
        // Somebody may have used one of these while the list sat open.
        var used = await _db.SenseFeatures
            .Where(f => f.ValueId != null && valueIds.Contains(f.ValueId!.Value))
            .Select(f => f.ValueId!.Value)
            .Distinct()
            .ToListAsync(ct);

        var changed = 0;
        foreach (var value in values.Where(v => !used.Contains(v.Id) && v.IsActive))
        {
            value.IsActive = false;
            changed++;
        }

        await _db.SaveChangesAsync(ct);
        return changed;
    }

    // ═══════════════════════════════════════════════════════════════════════
    // Axis ↔ part-of-speech mapping, and conditional rules. Data, not code.
    // ═══════════════════════════════════════════════════════════════════════

    /// <summary>How many senses a mapping change would affect. Shown BEFORE it commits.</summary>
    public Task<int> CountSensesAffectedByAssignmentAsync(int partOfSpeechId, int axisId, CancellationToken ct = default) =>
        _db.Senses.CountAsync(s => s.PartOfSpeechId == partOfSpeechId &&
                                   s.Features.Any(f => f.AxisId == axisId), ct);

    /// <summary>
    /// Re-parents an axis: "تێپەڕی hangs off جۆری کار = تەواو". Null detaches it back to the first
    /// level. This is DATA — no developer should be needed to wire a new sub-group at any depth.
    ///
    /// The three rules below are what keep the structure a TREE rather than an arbitrary graph. A
    /// graph is expressible in this schema and completely unmanageable in a settings screen: you
    /// could not draw it, and nobody could answer "what does a کار ask for" by looking at it.
    /// </summary>
    public async Task SetConditionAsync(int assignmentId, int? requiresValueId, CancellationToken ct = default)
    {
        var assignment = await _db.PartOfSpeechAxes
            .Include(a => a.Axis)
            .FirstAsync(a => a.Id == assignmentId, ct);

        if (requiresValueId is { } valueId)
        {
            var value = await _db.FeatureValues.FirstOrDefaultAsync(v => v.Id == valueId, ct)
                        ?? throw new InvalidOperationException("نرخەکە نەدۆزرایەوە.");

            // ١. The parent value must live on an axis assigned to the SAME part of speech, or the
            //    group can never open: the entry form would wait for an answer it never offers.
            var sameSpeech = await _db.PartOfSpeechAxes.AnyAsync(
                a => a.PartOfSpeechId == assignment.PartOfSpeechId && a.AxisId == value.AxisId, ct);

            if (!sameSpeech)
                throw new InvalidOperationException(
                    "مەرجەکە دەبێت ئاماژە بە نرخێک بکات لە تەوەرێکی هەمان بەشی ئاخاوتن.");

            if (value.AxisId == assignment.AxisId)
                throw new InvalidOperationException("تەوەرێک ناتوانێت مەرجی خۆی بێت.");

            // ٢ and ٣. The parent chain must terminate at an unconditional axis, and must not run
            //    back through this one.
            await GuardAgainstCycleAsync(assignment, valueId, ct);
        }

        // ٤. One axis has at most one parent value, never two — which is structural rather than
        //    checked: the parent link is a single column, and UNIQUE(PartOfSpeechId, AxisId) means
        //    an axis appears once per part of speech. Assigning a second parent overwrites the
        //    first, so a second parent cannot exist to be found.
        assignment.RequiresValueId = requiresValueId;
        await _db.SaveChangesAsync(ct);
    }

    /// <summary>
    /// Refuses a parent link that would close a loop — C under B, B under A, A under C.
    ///
    /// The entry form resolves the visible set by walking this chain, so a cycle is not a slow
    /// render: it is a form that can never decide what to show. The offending path is named in the
    /// message, because "بازنە دروست دەکات" without the path leaves whoever built it hunting through
    /// a tree for the edge they just closed.
    /// </summary>
    private async Task GuardAgainstCycleAsync(PartOfSpeechAxis assignment, int requiresValueId, CancellationToken ct)
    {
        var assignments = await _db.PartOfSpeechAxes
            .Include(a => a.Axis)
            .Where(a => a.PartOfSpeechId == assignment.PartOfSpeechId)
            .ToListAsync(ct);

        var axisIds = assignments.Select(a => a.AxisId).ToList();

        var axisOfValue = await _db.FeatureValues
            .Where(v => axisIds.Contains(v.AxisId))
            .ToDictionaryAsync(v => v.Id, v => v.AxisId, ct);

        // Walk UP from the proposed parent. Reaching an unconditional axis means the chain
        // terminates and the link is a legal tree edge; reaching this axis again means it does not.
        var path = new List<string> { assignment.Axis.NameKu };
        var seen = new HashSet<int> { assignment.AxisId };

        var currentAxisId = axisOfValue.GetValueOrDefault(requiresValueId);

        while (currentAxisId != 0)
        {
            var link = assignments.FirstOrDefault(a => a.AxisId == currentAxisId);
            path.Add(link?.Axis.NameKu ?? "?");

            if (currentAxisId == assignment.AxisId)
                throw new InvalidOperationException(
                    $"ئەم ڕێسایە بازنەیەک دروست دەکات: {string.Join(" ← ", path)}");

            // A loop that does NOT pass through this axis can only come from data written before
            // this guard existed. Report it the same way rather than walking it forever.
            if (!seen.Add(currentAxisId))
                throw new InvalidOperationException(
                    $"بازنەیەکی پێشوو لە درەختەکەدا هەیە: {string.Join(" ← ", path)}");

            if (link?.RequiresValueId is not { } next) break;   // reached an unconditional axis
            currentAxisId = axisOfValue.GetValueOrDefault(next);
        }
    }
}
