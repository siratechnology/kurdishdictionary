using backend.Data;
using backend.Data.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos;

namespace backend.Services.Lexicon;

/// <summary>
/// The settings tree — reading it, and the four things a language teacher does to it.
///
/// The person configuring this is a language teacher, not a developer. They must be able to build
/// کار's full cascade — جۆری کار ← تەواو ← تێپەڕی ← تێپەڕ ← whatever comes next — without ever seeing
/// the words "axis", "conditional" or "requires value", and without a developer, a code change or a
/// migration. That is the whole design brief for this class:
///
///   · <see cref="AddChildAxisAsync"/> is the one button, «بژاردەی زیاتر زیاد بکە», under any value
///     at any depth. It is the ONLY way a sub-group is created, and it is one call.
///   · <see cref="SetSelectionModeAsync"/> is «چەند بژاردە دەکرێت هەڵبژێردرێت؟».
///   · <see cref="ReorderSiblingsAsync"/> is drag-to-reorder.
///   · <see cref="SetAxisActiveAsync"/> is deactivate-rather-than-delete.
///
/// Nothing here branches on a part of speech or an axis name. The tree is data, all the way down.
/// </summary>
public class TaxonomyTreeService
{
    private readonly AppDbContext _db;
    private readonly TaxonomyAdminService _admin;
    private readonly TaxonomyCache _cache;

    /// <summary>
    /// One row per distinct headword, lowest id — the same dedupe the word list, the relation
    /// workspace and the station all use. Declared once here so a count on this screen can never
    /// disagree with the one next to it.
    /// </summary>
    private IQueryable<int> DedupedWordIds() =>
        _db.Words.AsNoTracking().GroupBy(w => w.Kurdish).Select(g => g.Min(w => w.Id));

    public TaxonomyTreeService(AppDbContext db, TaxonomyAdminService admin, TaxonomyCache cache)
    {
        _db = db;
        _admin = admin;
        _cache = cache;
    }

    // ═══════════════════════════════════════════════════════════════════════
    // Reading the tree
    // ═══════════════════════════════════════════════════════════════════════

    /// <summary>
    /// The whole tree for one part of speech, on one screen, with live counts on every node.
    ///
    /// Counts are queried in two round trips for the whole tree rather than one per node: a screen
    /// that issues a query per value would take a second to draw for کار and would get slower every
    /// time somebody added an option, which is exactly the behaviour that stops people using it.
    /// </summary>
    public async Task<TaxonomyTreeDto?> GetTreeAsync(int partOfSpeechId, CancellationToken ct = default)
    {
        var partOfSpeech = await _db.PartsOfSpeech
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == partOfSpeechId, ct);

        if (partOfSpeech is null) return null;

        var assignments = await _db.PartOfSpeechAxes
            .AsNoTracking()
            .Include(a => a.Axis).ThenInclude(x => x.Values)
            .Where(a => a.PartOfSpeechId == partOfSpeechId)
            .ToListAsync(ct);

        var axisIds = assignments.Select(a => a.AxisId).ToList();

        // Counted in distinct WORDS, not senses.
        //
        // These numbers are how the team reads the size of a decision — "484 uses this" is what
        // stops somebody deleting a value — and they were counting senses while every other screen
        // counted words. A word with three senses under کار is one word that is a verb, not three;
        // reporting three made the taxonomy look bigger than the dictionary it describes.
        //
        // Deduped by spelling too, so a duplicate headword no other screen shows cannot inflate a
        // count here either.
        var wordIds = DedupedWordIds();

        var axisUsage = await _db.SenseFeatures
            .Where(f => axisIds.Contains(f.AxisId) && wordIds.Contains(f.Sense.WordId))
            .GroupBy(f => f.AxisId)
            .Select(g => new { AxisId = g.Key, Count = g.Select(f => f.Sense.WordId).Distinct().Count() })
            .ToDictionaryAsync(x => x.AxisId, x => x.Count, ct);

        var valueUsage = await _db.SenseFeatures
            .Where(f => f.ValueId != null && axisIds.Contains(f.AxisId) && wordIds.Contains(f.Sense.WordId))
            .GroupBy(f => f.ValueId!.Value)
            .Select(g => new { ValueId = g.Key, Count = g.Select(f => f.Sense.WordId).Distinct().Count() })
            .ToDictionaryAsync(x => x.ValueId, x => x.Count, ct);

        var nodes = assignments.ToDictionary(
            a => a.AxisId,
            a => new TaxonomyTreeAxisDto
            {
                AssignmentId = a.Id,
                AxisId = a.AxisId,
                NameKu = a.Axis.NameKu,
                PromptKu = a.Axis.PromptKu,
                PromptNeedsReview = a.Axis.PromptNeedsReview,
                Description = a.Axis.Description,
                IsActive = a.Axis.IsActive,
                IsRequired = a.IsRequired,
                AllowsNotApplicable = a.Axis.AllowsNotApplicable,
                MinSelections = a.Axis.MinSelections,
                MaxSelections = a.Axis.MaxSelections,
                SortOrder = a.SortOrder,
                ParentValueId = a.RequiresValueId,
                UsageCount = axisUsage.GetValueOrDefault(a.AxisId),
                Values = a.Axis.Values
                    .Where(v => !v.IsDeleted)
                    .OrderBy(v => v.SortOrder).ThenBy(v => v.Id)
                    .Select(v => new TaxonomyTreeValueDto
                    {
                        ValueId = v.Id,
                        NameKu = v.NameKu,
                        OptionHintKu = v.OptionHintKu,
                        Code = v.Code,
                        SortOrder = v.SortOrder,
                        IsActive = v.IsActive,
                        WordCount = valueUsage.GetValueOrDefault(v.Id),
                    })
                    .ToList(),
            });

        var valueNodes = nodes.Values
            .SelectMany(n => n.Values)
            .ToDictionary(v => v.ValueId);

        var roots = new List<TaxonomyTreeAxisDto>();

        foreach (var node in nodes.Values.OrderBy(n => n.SortOrder).ThenBy(n => n.AxisId))
        {
            if (node.ParentValueId is { } parentId && valueNodes.TryGetValue(parentId, out var parent))
            {
                parent.Children.Add(node);
                continue;
            }

            // Unconditional, or an orphan whose parent value lives outside this part of speech. Both
            // belong at the first level HERE — unlike the entry form, which hides the orphan. The
            // settings screen is where somebody has to be able to see it and fix it.
            roots.Add(node);
        }

        AssignDepths(roots, 0);

        return new TaxonomyTreeDto
        {
            PartOfSpeechId = partOfSpeech.Id,
            PartOfSpeechName = partOfSpeech.NameKu,
            WordCount = await _db.Senses
                .Where(s => s.PartOfSpeechId == partOfSpeechId && DedupedWordIds().Contains(s.WordId))
                .Select(s => s.WordId)
                .Distinct()
                .CountAsync(ct),
            Axes = roots,
            Version = _cache.Version,
        };
    }

    private static void AssignDepths(List<TaxonomyTreeAxisDto> axes, int depth)
    {
        foreach (var axis in axes)
        {
            axis.Depth = depth;

            foreach (var value in axis.Values)
            {
                // Guard against a cycle in legacy data: a settings screen that never finishes
                // drawing is worse than one that draws a tree with a branch missing.
                if (depth > 32) return;
                AssignDepths(value.Children, depth + 1);
            }
        }
    }

    // ═══════════════════════════════════════════════════════════════════════
    // «بژاردەی زیاتر زیاد بکە» — the one button, at any depth
    // ═══════════════════════════════════════════════════════════════════════

    /// <summary>
    /// Creates a new group of options hanging off <paramref name="parentValueId"/>.
    ///
    /// One call because it is one gesture. Underneath it is a FeatureAxis, a PartOfSpeechAxis and a
    /// parent link, and the teacher pressing the button should never learn that — the previous
    /// screen made them create an axis, attach it to a part of speech, then find it again in a flat
    /// list of every value in the system to set its condition, and unsurprisingly nobody did.
    ///
    /// Pass <paramref name="parentValueId"/> as null to add a first-level group instead.
    /// </summary>
    public async Task<int> AddChildAxisAsync(
        int partOfSpeechId, int? parentValueId, string name, CancellationToken ct = default)
    {
        var trimmed = (name ?? "").Trim();
        if (trimmed.Length == 0)
            throw new InvalidOperationException("ناوێک بنووسە.");

        if (parentValueId is { } parentId)
        {
            var parent = await _db.FeatureValues.FirstOrDefaultAsync(v => v.Id == parentId, ct)
                         ?? throw new InvalidOperationException("نرخی سەرەوە نەدۆزرایەوە.");

            var parentIsOnThisPart = await _db.PartOfSpeechAxes.AnyAsync(
                a => a.PartOfSpeechId == partOfSpeechId && a.AxisId == parent.AxisId, ct);

            if (!parentIsOnThisPart)
                throw new InvalidOperationException(
                    "نرخی سەرەوە سەر بەم بەشی ئاخاوتنە نییە.");
        }

        var axis = new FeatureAxis
        {
            NameKu = trimmed,
            Code = Guid.NewGuid().ToString("N")[..8],
            SortOrder = await _db.FeatureAxes.CountAsync(ct) + 1,

            // A brand-new group behaves like every group that already exists: one choice. Widening
            // it is a deliberate act on the node, never something that happens by omission.
            MinSelections = 0,
            MaxSelections = 1,
        };

        _db.FeatureAxes.Add(axis);
        await _db.SaveChangesAsync(ct);

        _db.PartOfSpeechAxes.Add(new PartOfSpeechAxis
        {
            PartOfSpeechId = partOfSpeechId,
            AxisId = axis.Id,
            RequiresValueId = parentValueId,
            IsRequired = false,
            SortOrder = await NextSortOrderAsync(partOfSpeechId, parentValueId, ct),
        });

        await _db.SaveChangesAsync(ct);
        return axis.Id;
    }

    /// <summary>Adds a value to a node. The other half of building a tree.</summary>
    public async Task<int> AddValueAsync(int axisId, string name, string? code, CancellationToken ct = default)
    {
        var trimmed = (name ?? "").Trim();
        if (trimmed.Length == 0)
            throw new InvalidOperationException("ناوێک بنووسە.");

        var clash = await _db.FeatureValues.AnyAsync(v => v.AxisId == axisId && v.NameKu == trimmed, ct);
        if (clash)
            throw new InvalidOperationException($"«{trimmed}» پێشتر لەم بژاردەیەدا هەیە.");

        var value = new FeatureValue
        {
            AxisId = axisId,
            NameKu = trimmed,
            Code = string.IsNullOrWhiteSpace(code) ? null : code.Trim(),
            SortOrder = await _db.FeatureValues.CountAsync(v => v.AxisId == axisId, ct) + 1,
        };

        _db.FeatureValues.Add(value);
        await _db.SaveChangesAsync(ct);

        return value.Id;
    }

    // ═══════════════════════════════════════════════════════════════════════
    // The question a teacher actually reads
    // ═══════════════════════════════════════════════════════════════════════

    /// <summary>
    /// Sets the plain-language question shown above this dropdown on the entry form.
    ///
    /// Blank CLEARS it, and clearing is a supported answer rather than an error: the form falls back
    /// to the technical label, which is honest. A bare «جۆری کار» gets asked about; a plausible but
    /// wrong question gets answered confidently, and the wrong answer looks finished.
    ///
    /// Writing anything here clears <see cref="FeatureAxis.PromptNeedsReview"/> — a human has now
    /// decided the wording, so the system's suggestion is no longer what is on screen.
    /// </summary>
    public async Task SetPromptAsync(int axisId, string? prompt, CancellationToken ct = default)
    {
        var trimmed = prompt?.Trim();

        var axis = await _db.FeatureAxes.FirstAsync(a => a.Id == axisId, ct);

        axis.PromptKu = string.IsNullOrEmpty(trimmed) ? null : trimmed;
        axis.PromptNeedsReview = false;

        await _db.SaveChangesAsync(ct);
    }

    /// <summary>
    /// Sets the short worked example shown in muted text beside one option. Blank clears it.
    ///
    /// Never required, and never part of the answer — it is help text, so nothing validates it and
    /// nothing keys on it.
    /// </summary>
    public async Task SetOptionHintAsync(int valueId, string? hint, CancellationToken ct = default)
    {
        var trimmed = hint?.Trim();

        var value = await _db.FeatureValues.FirstAsync(v => v.Id == valueId, ct);
        value.OptionHintKu = string.IsNullOrEmpty(trimmed) ? null : trimmed;

        await _db.SaveChangesAsync(ct);
    }

    private async Task<int> NextSortOrderAsync(int partOfSpeechId, int? parentValueId, CancellationToken ct) =>
        await _db.PartOfSpeechAxes
            .Where(a => a.PartOfSpeechId == partOfSpeechId && a.RequiresValueId == parentValueId)
            .CountAsync(ct) + 1;

    // ═══════════════════════════════════════════════════════════════════════
    // «چەند بژاردە دەکرێت هەڵبژێردرێت؟»
    // ═══════════════════════════════════════════════════════════════════════

    /// <summary>
    /// What lowering the cap would cost. Called before the change, always.
    ///
    /// The one thing this must never do is truncate. A settings screen that quietly drops a
    /// teacher's third answer to fit a new limit destroys work in a place nobody would think to look
    /// for it, so the count is shown, the senses are named, and the change is refused until they are
    /// fixed.
    /// </summary>
    public async Task<SelectionCapPreviewDto> PreviewSelectionCapAsync(
        int axisId, int? proposedMax, CancellationToken ct = default)
    {
        var axis = await _db.FeatureAxes.AsNoTracking().FirstAsync(a => a.Id == axisId, ct);

        var preview = new SelectionCapPreviewDto
        {
            AxisId = axisId,
            AxisName = axis.NameKu,
            CurrentMax = axis.MaxSelections,
            ProposedMax = proposedMax,
        };

        // Unlimited, or a cap that is not lower than what senses already hold, cannot lose anything.
        if (proposedMax is not { } cap) return preview;

        var over = await _db.SenseFeatures
            .Where(f => f.AxisId == axisId && f.ValueId != null)
            .GroupBy(f => f.SenseId)
            .Where(g => g.Count() > cap)
            .Select(g => new { SenseId = g.Key, HeldCount = g.Count() })
            .ToListAsync(ct);

        preview.AffectedSenseCount = over.Count;

        var sample = over.OrderByDescending(x => x.HeldCount).Take(25).ToList();
        var senseIds = sample.Select(x => x.SenseId).ToList();

        var words = await _db.Senses
            .Where(s => senseIds.Contains(s.Id))
            .Select(s => new { s.Id, s.WordId, Word = s.Word.Kurdish })
            .ToListAsync(ct);

        preview.Affected = sample
            .Join(words, x => x.SenseId, w => w.Id, (x, w) => new SelectionCapSenseDto
            {
                SenseId = x.SenseId,
                WordId = w.WordId,
                Word = w.Word,
                HeldCount = x.HeldCount,
            })
            .ToList();

        return preview;
    }

    /// <summary>
    /// Sets how many values this axis may hold. Refuses a cap that would truncate stored answers.
    ///
    /// <paramref name="maxSelections"/>: 1 = تەنها یەک · null = هەرچەند بێت · n = تا n.
    /// <paramref name="minSelections"/> feeds the work queue and the completeness score and NOTHING
    /// else — it must never gate a save, at any depth.
    /// </summary>
    public async Task SetSelectionModeAsync(
        int axisId, int minSelections, int? maxSelections, CancellationToken ct = default)
    {
        if (minSelections < 0)
            throw new InvalidOperationException("ژمارەی کەمترین ناتوانێت لە سفر کەمتر بێت.");

        if (maxSelections is { } max)
        {
            if (max < 1)
                throw new InvalidOperationException("لانیکەم یەک بژاردە دەبێت ڕێگەپێدراو بێت.");

            if (minSelections > max)
                throw new InvalidOperationException("ژمارەی کەمترین ناتوانێت لە زۆرترین زیاتر بێت.");

            var preview = await PreviewSelectionCapAsync(axisId, max, ct);

            if (!preview.IsSafe)
                throw new InvalidOperationException(
                    $"{preview.AffectedSenseCount} مانا زیاتر لە {max} بژاردەیان هەیە — " +
                    "سەرەتا ئەوان ڕاست بکەرەوە، ئەم گۆڕانکارییە وەڵامەکانیان ناسڕێتەوە.");
        }

        var axis = await _db.FeatureAxes.FirstAsync(a => a.Id == axisId, ct);
        axis.MinSelections = minSelections;
        axis.MaxSelections = maxSelections;

        await _db.SaveChangesAsync(ct);
    }

    // ═══════════════════════════════════════════════════════════════════════
    // Reorder, rename, deactivate
    // ═══════════════════════════════════════════════════════════════════════

    /// <summary>
    /// Drag to reorder SIBLINGS — the axes hanging off one value, or the first level.
    ///
    /// Scoped to one parent on purpose: reordering across the whole tree would let a drag inside
    /// تێپەڕ silently change what a چاوگ asks first.
    /// </summary>
    public async Task ReorderSiblingsAsync(
        int partOfSpeechId, int? parentValueId, IReadOnlyList<int> orderedAssignmentIds,
        CancellationToken ct = default)
    {
        var siblings = await _db.PartOfSpeechAxes
            .Where(a => a.PartOfSpeechId == partOfSpeechId && a.RequiresValueId == parentValueId)
            .ToListAsync(ct);

        for (var i = 0; i < orderedAssignmentIds.Count; i++)
        {
            var sibling = siblings.FirstOrDefault(a => a.Id == orderedAssignmentIds[i]);
            if (sibling is not null) sibling.SortOrder = i + 1;
        }

        await _db.SaveChangesAsync(ct);
    }

    public Task ReorderValuesAsync(IReadOnlyList<int> orderedValueIds, CancellationToken ct = default) =>
        _admin.ReorderValuesAsync(orderedValueIds, ct);

    /// <summary>
    /// Deactivates a whole group rather than deleting it.
    ///
    /// Deletion is refused while anything uses it, and deactivation is always available, because the
    /// two are different intentions: retiring a term from NEW entries, versus removing something
    /// created by mistake. Deactivating never touches the answers senses already gave — they stay
    /// readable on the sense and in history.
    /// </summary>
    public async Task SetAxisActiveAsync(int axisId, bool isActive, CancellationToken ct = default)
    {
        var axis = await _db.FeatureAxes.FirstAsync(a => a.Id == axisId, ct);
        axis.IsActive = isActive;
        await _db.SaveChangesAsync(ct);
    }

    /// <summary>
    /// Detaches a group from a part of speech entirely.
    ///
    /// Refused while senses hold answers on it — the answers would become unreachable rather than
    /// deleted, which is worse than either — and refused while it still has children, because
    /// removing a parent would orphan a whole subtree in one click.
    /// </summary>
    public async Task RemoveAxisAsync(int partOfSpeechId, int axisId, CancellationToken ct = default)
    {
        var assignment = await _db.PartOfSpeechAxes
            .FirstOrDefaultAsync(a => a.PartOfSpeechId == partOfSpeechId && a.AxisId == axisId, ct);

        if (assignment is null) return;

        var answered = await _db.Senses.CountAsync(
            s => s.PartOfSpeechId == partOfSpeechId && s.Features.Any(f => f.AxisId == axisId), ct);

        if (answered > 0)
            throw new InvalidOperationException(
                $"{answered} مانا وەڵامیان لەم بژاردەیەدا هەیە — ناتوانرێت لابردرێت، تەنها ناچالاک بکرێت.");

        var valueIds = await _db.FeatureValues
            .Where(v => v.AxisId == axisId)
            .Select(v => v.Id)
            .ToListAsync(ct);

        var children = await _db.PartOfSpeechAxes.CountAsync(
            a => a.PartOfSpeechId == partOfSpeechId &&
                 a.RequiresValueId != null &&
                 valueIds.Contains(a.RequiresValueId.Value), ct);

        if (children > 0)
            throw new InvalidOperationException(
                $"{children} بژاردەی ژێرەوەی هەیە — سەرەتا ئەوان لاببە.");

        _db.PartOfSpeechAxes.Remove(assignment);
        await _db.SaveChangesAsync(ct);
    }

    /// <summary>
    /// Moves a group under a different value — the drag that changes the shape of the tree.
    /// Delegates to the guard that keeps it a tree: same part of speech, no cycle, one parent.
    /// </summary>
    public Task ReparentAsync(int assignmentId, int? parentValueId, CancellationToken ct = default) =>
        _admin.SetConditionAsync(assignmentId, parentValueId, ct);
}
