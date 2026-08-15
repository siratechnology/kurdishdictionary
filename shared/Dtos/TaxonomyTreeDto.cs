namespace Shared.Dtos;

/// <summary>
/// Pushed down the hub the moment a settings write commits, so every open session re-renders without
/// a page refresh, an app restart or a logout.
///
/// It carries a VERSION rather than the new tree. A payload would have to be built per part of
/// speech for every listener whether they were looking at it or not; a version lets each circuit
/// decide whether it cares and pull only what it is showing.
/// </summary>
/// <param name="Version">The taxonomy cache version after the change.</param>
/// <param name="PartsOfSpeech">Affected parts of speech; empty means all of them.</param>
public record TaxonomyChangedDto(long Version, IReadOnlyList<int> PartsOfSpeech);

/// <summary>
/// One part of speech's options tree, for the settings editor: بەشی ئاخاوتن ← تەوەر ← نرخ ← تەوەر ← …
///
/// The data model has not changed — this is the SAME PartOfSpeechAxis rows the flat screen showed,
/// presented as what they actually are. Configuring a sub-group by picking a "required value" out of
/// a list of every value in the system is technically identical and impossible to reason about: you
/// cannot see the shape you are building, so you find out it is wrong after two hundred words.
/// </summary>
public class TaxonomyTreeDto
{
    public int PartOfSpeechId { get; set; }
    public string PartOfSpeechName { get; set; } = string.Empty;

    /// <summary>
    /// WORDS with a sense classified as this part of speech — distinct, deduped by spelling.
    ///
    /// Words, not senses, and that is the point: the dictionary is 2,967 words, and a screen that
    /// reports 4,915 of anything cannot be reconciled with the one next to it. A word with three
    /// senses under کار counts once here, which is the number a person would give if you asked
    /// them how many words are verbs.
    /// </summary>
    public int WordCount { get; set; }

    /// <summary>The first level — the axes that appear the moment this part of speech is chosen.</summary>
    public List<TaxonomyTreeAxisDto> Axes { get; set; } = new();

    /// <summary>
    /// The cache version this tree was built from. A client holding an older one knows its screen is
    /// behind without diffing anything.
    /// </summary>
    public long Version { get; set; }
}

/// <summary>An axis node. Its children hang off its VALUES, never off the axis itself.</summary>
public class TaxonomyTreeAxisDto
{
    /// <summary>The PartOfSpeechAxis row — what re-parenting and reordering address.</summary>
    public int AssignmentId { get; set; }

    public int AxisId { get; set; }

    /// <summary>The short technical label — جۆری کار. Admin reference; not what a teacher reads.</summary>
    public string NameKu { get; set; } = string.Empty;

    /// <summary>
    /// The plain-language question shown to the teacher. The editor puts this beside
    /// <see cref="NameKu"/> so both are visible at once and it is obvious which one is being changed.
    /// </summary>
    public string? PromptKu { get; set; }

    /// <summary>
    /// The prompt was SUGGESTED, not written. Drives the «پێداچوونەوە پێویستە» flag and the
    /// review list at the top of the settings screen; cleared the moment somebody edits the wording.
    /// </summary>
    public bool PromptNeedsReview { get; set; }

    /// <summary>Exactly what the teacher will read — the question, or the label when there is none.</summary>
    public string Ask => string.IsNullOrWhiteSpace(PromptKu) ? NameKu : PromptKu!;

    public string? Description { get; set; }
    public bool IsActive { get; set; }

    /// <summary>Feeds the work queue and the completeness score. Never gates a save.</summary>
    public bool IsRequired { get; set; }

    public bool AllowsNotApplicable { get; set; }

    public int MinSelections { get; set; }

    /// <summary>1 = تەنها یەک · null = هەرچەند بێت · n = تا n. The words the editor shows, never the field name.</summary>
    public int? MaxSelections { get; set; }

    public int SortOrder { get; set; }

    /// <summary>0 at the first level. Every level below was opened by a value.</summary>
    public int Depth { get; set; }

    /// <summary>The value this group hangs off. Null at the first level.</summary>
    public int? ParentValueId { get; set; }

    /// <summary>Distinct WORDS holding any answer on this axis. Live, shown on the node.</summary>
    public int UsageCount { get; set; }

    /// <summary>
    /// Senses that already hold MORE values on this axis than a proposed lower cap would allow.
    /// Zero unless the caller asked. Populated by the cap-change preview, not by the tree read.
    /// </summary>
    public int OverCapSenseCount { get; set; }

    public List<TaxonomyTreeValueDto> Values { get; set; } = new();
}

/// <summary>A value node, and the groups it opens.</summary>
public class TaxonomyTreeValueDto
{
    public int ValueId { get; set; }
    public string NameKu { get; set; } = string.Empty;

    /// <summary>A short worked example shown muted beside this option on the entry form. Never required.</summary>
    public string? OptionHintKu { get; set; }

    /// <summary>Stable key, set only on values a RULE depends on. Shown, never editable.</summary>
    public string? Code { get; set; }

    public int SortOrder { get; set; }
    public bool IsActive { get; set; }

    /// <summary>How many distinct WORDS hold this value. Every node shows its live count.</summary>
    public int WordCount { get; set; }

    /// <summary>The child groups this value opens. This is the recursion — no fixed depth.</summary>
    public List<TaxonomyTreeAxisDto> Children { get; set; } = new();
}

/// <summary>
/// What lowering a selection cap would cost, before it is applied.
///
/// Truncating somebody's answers to fit a new limit is a silent data loss dressed as a settings
/// change, so the editor either blocks the change or lists the senses that need fixing first.
/// </summary>
public class SelectionCapPreviewDto
{
    public int AxisId { get; set; }
    public string AxisName { get; set; } = string.Empty;

    public int? CurrentMax { get; set; }
    public int? ProposedMax { get; set; }

    /// <summary>Senses holding more values than the proposed cap allows.</summary>
    public int AffectedSenseCount { get; set; }

    /// <summary>A readable sample of them, so "fix these first" is an instruction rather than a wall.</summary>
    public List<SelectionCapSenseDto> Affected { get; set; } = new();

    /// <summary>False when the change would truncate stored answers. The editor refuses it.</summary>
    public bool IsSafe => AffectedSenseCount == 0;
}

public class SelectionCapSenseDto
{
    public int SenseId { get; set; }
    public int WordId { get; set; }
    public string Word { get; set; } = string.Empty;
    public int HeldCount { get; set; }
}
