using backend.Data;
using backend.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services.Lexicon;

/// <summary>
/// The options tree for one part of speech: بەشی ئاخاوتن ← تەوەر ← نرخ ← تەوەر ← …
///
/// One rule produces the whole structure. <see cref="PartOfSpeechAxis.RequiresValueId"/> IS the
/// parent link: an axis whose RequiresValue points at a <see cref="FeatureValue"/> is a child group
/// of that value. Chain it and the depth is 3, 4, 5 — as deep as the data goes, with no schema
/// change and no <c>if (level == 2)</c> anywhere.
///
/// This class is deliberately a plain in-memory graph with no DbContext of its own. It is built once
/// per part of speech (see <see cref="OptionsTreeService"/>, which caches it) and then resolved
/// against a particular sense's answers many times — by the entry form, by the validator, by the
/// work queue and by the settings preview, all from the same code so they cannot disagree about
/// which questions a word is being asked.
/// </summary>
public sealed class OptionsTree
{
    private readonly Dictionary<int, OptionsAxis> _byAxisId;
    private readonly Dictionary<int, OptionsValue> _byValueId;

    private OptionsTree(int partOfSpeechId, List<OptionsAxis> all, List<OptionsAxis> roots)
    {
        PartOfSpeechId = partOfSpeechId;
        All = all;
        Roots = roots;

        _byAxisId = all.ToDictionary(a => a.AxisId);
        _byValueId = all.SelectMany(a => a.Values).ToDictionary(v => v.Id);
    }

    public int PartOfSpeechId { get; }

    /// <summary>Every axis assigned to this part of speech, at every depth.</summary>
    public IReadOnlyList<OptionsAxis> All { get; }

    /// <summary>The unconditional axes — the first level, visible the moment the part of speech is chosen.</summary>
    public IReadOnlyList<OptionsAxis> Roots { get; }

    public OptionsAxis? Axis(int axisId) => _byAxisId.GetValueOrDefault(axisId);
    public OptionsValue? Value(int valueId) => _byValueId.GetValueOrDefault(valueId);

    /// <summary>The empty tree — a part of speech nobody has configured yet. Day one, not an error.</summary>
    public static OptionsTree Empty(int partOfSpeechId) => new(partOfSpeechId, new(), new());

    // ═══════════════════════════════════════════════════════════════════════
    // Loading
    // ═══════════════════════════════════════════════════════════════════════

    public static async Task<OptionsTree> LoadAsync(
        AppDbContext db, int partOfSpeechId, CancellationToken ct = default)
    {
        var assignments = await db.PartOfSpeechAxes
            .AsNoTracking()
            .Include(a => a.Axis).ThenInclude(x => x.Values)
            .Where(a => a.PartOfSpeechId == partOfSpeechId)
            .ToListAsync(ct);

        var all = assignments
            .Select(a => new OptionsAxis
            {
                AssignmentId = a.Id,
                AxisId = a.AxisId,
                Name = a.Axis.NameKu,
                Prompt = a.Axis.PromptKu,
                PromptNeedsReview = a.Axis.PromptNeedsReview,
                Description = a.Axis.Description,
                IsRequired = a.IsRequired,
                AllowsNotApplicable = a.Axis.AllowsNotApplicable,
                IsActive = a.Axis.IsActive,
                MinSelections = a.Axis.MinSelections,
                MaxSelections = a.Axis.MaxSelections,
                SortOrder = a.SortOrder,
                AxisSortOrder = a.Axis.SortOrder,
                ParentValueId = a.RequiresValueId,
                Values = a.Axis.Values
                    .Where(v => !v.IsDeleted)
                    .OrderBy(v => v.SortOrder).ThenBy(v => v.Id)
                    .Select(v => new OptionsValue
                    {
                        Id = v.Id,
                        AxisId = v.AxisId,
                        Name = v.NameKu,
                        Hint = v.OptionHintKu,
                        Code = v.Code,
                        SortOrder = v.SortOrder,
                        IsActive = v.IsActive,
                    })
                    .ToList(),
            })
            .ToList();

        return Link(partOfSpeechId, all);
    }

    /// <summary>
    /// Hangs every conditional axis under the value that opens it, and collects the unconditional
    /// ones as roots.
    ///
    /// An axis whose parent value belongs to an axis that is NOT on this part of speech is an
    /// orphan: the condition could never be satisfied because the form would never offer the answer.
    /// It is dropped rather than shown, which is the same choice the entry form makes everywhere —
    /// a question that cannot apply is absent, never greyed out.
    /// </summary>
    private static OptionsTree Link(int partOfSpeechId, List<OptionsAxis> all)
    {
        var valueIndex = all
            .SelectMany(a => a.Values)
            .ToDictionary(v => v.Id);

        var roots = new List<OptionsAxis>();

        foreach (var axis in all.OrderBy(a => a.SortOrder).ThenBy(a => a.AxisSortOrder).ThenBy(a => a.AxisId))
        {
            if (axis.ParentValueId is not { } parentValueId)
            {
                roots.Add(axis);
                continue;
            }

            if (valueIndex.TryGetValue(parentValueId, out var parent))
            {
                axis.Parent = parent;
                parent.Children.Add(axis);
            }
            // else: orphan. Not a root — an unconditional axis and an axis whose condition can never
            // hold are different things, and promoting the second to the first would put a question
            // on the form that the settings screen says is conditional.
        }

        return new OptionsTree(partOfSpeechId, all, roots);
    }

    // ═══════════════════════════════════════════════════════════════════════
    // Resolution — the only place that decides what a teacher sees
    // ═══════════════════════════════════════════════════════════════════════

    /// <summary>
    /// The axes that apply to a sense holding <paramref name="heldValueIds"/>, in the order they
    /// should be rendered: each axis immediately followed by the groups its answers opened.
    ///
    /// The rule is recursive and has no depth limit:
    ///
    ///   visible(axis) = axis is unconditional
    ///                 ∨ (visible(parent axis) ∧ the sense holds the parent value)
    ///
    /// Because it asks about the PARENT AXIS as well as the parent value, changing an answer at
    /// depth ١ hides depth ٢ and therefore depth ٣ in the same pass — even though depth ٣'s own
    /// parent value may still be sitting in the database, stale, waiting to be cleared. That is what
    /// makes the cascade a single resolution rather than a loop that has to be run to a fixpoint.
    ///
    /// It is also what makes multi-select work: a child group hangs off a VALUE, so deselecting one
    /// value of a multi-select axis hides that value's subtree and leaves its sibling's alone.
    /// </summary>
    public List<ResolvedAxis> Resolve(IReadOnlySet<int> heldValueIds, IReadOnlySet<int>? answeredAxisIds = null)
    {
        var resolved = new List<ResolvedAxis>();
        var onPath = new HashSet<int>();

        foreach (var root in Roots)
            Walk(root, depth: 0, parent: null);

        return resolved;

        void Walk(OptionsAxis axis, int depth, OptionsValue? parent)
        {
            // A cycle cannot be created through the settings service, but a legacy row could hold
            // one. Refusing to revisit an axis on the same path means a bad row costs a missing
            // question, never a hung circuit.
            if (!onPath.Add(axis.AxisId)) return;

            try
            {
                var isAnswered = answeredAxisIds?.Contains(axis.AxisId) ?? false;

                // Deactivating an axis hides it from NEW entries; it never erases the answers senses
                // already gave. So a retired axis still renders — but only on the senses that
                // actually answered it.
                if (!axis.IsActive && !isAnswered) return;

                // A group with nothing to offer is not a question. Half-built groups are normal —
                // you add the group, then its values — and rendering one produces an empty dropdown
                // that cannot be answered and cannot be dismissed. Absent, not disabled: the same
                // rule that governs a group whose condition is unmet.
                //
                // It can still open no children either, since children hang off values. So skipping
                // it loses nothing below it.
                if (!axis.Values.Any(v => v.IsActive) && !isAnswered) return;

                resolved.Add(new ResolvedAxis(axis, depth, parent));

                foreach (var value in axis.Values)
                {
                    if (!heldValueIds.Contains(value.Id)) continue;

                    foreach (var child in value.Children)
                        Walk(child, depth + 1, value);
                }
            }
            finally
            {
                onPath.Remove(axis.AxisId);
            }
        }
    }

    /// <summary>
    /// Axis ids that hold an answer the tree no longer asks for.
    ///
    /// Everything not in the resolved set is stale — an axis dropped from the part of speech, an
    /// orphan, or a whole subtree below an answer that was changed away.
    /// </summary>
    public HashSet<int> StaleAxisIds(IReadOnlySet<int> heldValueIds, IReadOnlySet<int> answeredAxisIds)
    {
        var visible = Resolve(heldValueIds, answeredAxisIds)
            .Select(r => r.Axis.AxisId)
            .ToHashSet();

        return answeredAxisIds.Where(id => !visible.Contains(id)).ToHashSet();
    }
}

/// <summary>One axis in the tree, with the values that may open further groups beneath it.</summary>
public sealed class OptionsAxis
{
    public int AssignmentId { get; init; }
    public int AxisId { get; init; }

    /// <summary>The short technical label — جۆری کار. What the SETTINGS screen and history read.</summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// The plain-language question a teacher reads — «کارەکە چ جۆرێکە؟». Null until somebody writes
    /// one; <see cref="Ask"/> is what the form should render.
    /// </summary>
    public string? Prompt { get; init; }

    /// <summary>The prompt was suggested by the system, not written by the team. Settings-only signal.</summary>
    public bool PromptNeedsReview { get; init; }

    /// <summary>
    /// What the TEACHER is shown: the question if there is one, otherwise the bare label.
    ///
    /// The fallback lives here, once, so the entry form, the preview, the validator and the work queue
    /// cannot each decide it differently — which is how one screen ends up asking «جۆری کار» while
    /// another asks the question.
    /// </summary>
    public string Ask => string.IsNullOrWhiteSpace(Prompt) ? Name : Prompt!;

    public string? Description { get; init; }

    /// <summary>Drives the work queue and the completeness score. It does NOT gate the save.</summary>
    public bool IsRequired { get; init; }

    public bool AllowsNotApplicable { get; init; }
    public bool IsActive { get; init; }

    public int MinSelections { get; init; }

    /// <summary>1 = single choice · null = unlimited · n = up to n.</summary>
    public int? MaxSelections { get; init; }

    /// <summary>Order among siblings, from the assignment. Ties break on the axis's own order.</summary>
    public int SortOrder { get; init; }
    public int AxisSortOrder { get; init; }

    /// <summary>Null for the first level. Otherwise the value this group hangs off.</summary>
    public int? ParentValueId { get; init; }

    public OptionsValue? Parent { get; internal set; }

    public List<OptionsValue> Values { get; init; } = new();

    public bool IsSingleSelect => MaxSelections == 1;
}

/// <summary>One value, and the child groups it opens when a sense holds it.</summary>
public sealed class OptionsValue
{
    public int Id { get; init; }
    public int AxisId { get; init; }
    public string Name { get; init; } = string.Empty;

    /// <summary>A short worked example, shown muted beside the option. Never required.</summary>
    public string? Hint { get; init; }

    public string? Code { get; init; }
    public int SortOrder { get; init; }
    public bool IsActive { get; init; }

    public List<OptionsAxis> Children { get; } = new();

    /// <summary>
    /// The child groups choosing this value would REALLY open.
    ///
    /// Not the same as <see cref="Children"/>, and the difference is a bug the entry form used to
    /// show: a group that is deactivated, or that has no options yet, is linked here but is skipped
    /// by <see cref="OptionsTree.Resolve"/> — so counting raw Children made the form print «this
    /// opens another question» on a value that opens nothing. You pick it, nothing appears, and the
    /// cascade looks broken when it is behaving exactly as configured.
    ///
    /// Same two exclusions Resolve applies, in one place, so the promise and the behaviour cannot
    /// drift apart.
    /// </summary>
    public IEnumerable<OptionsAxis> OpenableChildren =>
        Children.Where(c => c.IsActive && c.Values.Any(v => v.IsActive));

    /// <summary>True when choosing this value actually reveals a further question.</summary>
    public bool OpensChildGroup => OpenableChildren.Any();
}

/// <summary>
/// An axis that applies right now, with where it sits. <paramref name="ParentValue"/> is what the
/// entry form labels the nested group with — without it, two child groups opened by two different
/// values of one multi-select axis are indistinguishable.
/// </summary>
public sealed record ResolvedAxis(OptionsAxis Axis, int Depth, OptionsValue? ParentValue);
