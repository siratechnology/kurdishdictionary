namespace Shared.Dtos;

/// <summary>
/// One sense, as the station screen needs it: the word, the definition, and exactly the axes that
/// apply right now — nothing greyed out, nothing absent that should be there.
/// </summary>
public class StationSenseDto
{
    public int SenseId { get; set; }
    public int WordId { get; set; }
    public string Word { get; set; } = string.Empty;
    public string Definition { get; set; } = string.Empty;
    public string ExampleUsage { get; set; } = string.Empty;

    public int? PartOfSpeechId { get; set; }
    public int? DomainId { get; set; }
    public string? DomainName { get; set; }

    public string WorkflowState { get; set; } = string.Empty;

    /// <summary>Where this SENSE sits in the walk. What next/previous move through.</summary>
    public int Position { get; set; }
    public int Total { get; set; }

    /// <summary>
    /// The same place counted in WORDS — «وشەی ٤٥ لە ٢٬٩٦٧».
    ///
    /// This is what the screen shows, because words are the unit every other page reports and a
    /// dictionary that is 2,967 words on one screen and 4,915 on another cannot be reconciled by
    /// anyone. The walk still steps sense by sense; a multi-sense word just holds this counter
    /// still for a step while <see cref="SenseIndex"/> moves.
    /// </summary>
    public int WordPosition { get; set; }
    public int WordTotal { get; set; }

    /// <summary>Which sense of this word, of how many. Both 1 on the ordinary single-sense word.</summary>
    public int SenseIndex { get; set; }
    public int SenseCount { get; set; }

    /// <summary>Null when this person holds the claim. Otherwise the name of whoever does.</summary>
    public string? HeldBy { get; set; }

    public List<StationAxisDto> Axes { get; set; } = new();
    public List<StationRelationDto> Relations { get; set; } = new();

    /// <summary>What is still missing, from the validator. Shown inline while typing, never as a blocker.</summary>
    public List<string> Issues { get; set; } = new();
}

/// <summary>
/// An axis that APPLIES to this sense right now.
///
/// Axes whose condition is not satisfied are absent from this list entirely — پڕۆمپت ٧ is explicit:
/// never render a disabled or greyed-out inapplicable axis. It is present or it is not.
///
/// The list is DEPTH-FIRST: every axis is immediately followed by the groups its answers opened, so
/// the form can be rendered by walking it once and indenting by <see cref="Depth"/>.
/// </summary>
public class StationAxisDto
{
    public int AxisId { get; set; }

    /// <summary>
    /// The short technical label — جۆری کار. Present so the ADMIN preview and any settings-side reader
    /// can still see it; the entry form renders <see cref="Ask"/>, never this.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The plain-language question written for teachers — «کارەکە چ جۆرێکە؟». Null when nobody has
    /// written one yet, which is a normal state rather than a gap to paper over.
    /// </summary>
    public string? Prompt { get; set; }

    /// <summary>
    /// What the entry form MUST print above the control: the question if there is one, otherwise the
    /// label. Derived here rather than in each view so the station, the preview and any future screen
    /// cannot disagree about which string a teacher reads.
    /// </summary>
    public string Ask => string.IsNullOrWhiteSpace(Prompt) ? Name : Prompt!;

    public string? Description { get; set; }

    /// <summary>
    /// Drives the work queue and the completeness score. It does NOT gate the save — nothing below
    /// the part of speech does, at any depth.
    /// </summary>
    public bool IsRequired { get; set; }

    /// <summary>The axis offers an explicit "does not apply" answer.</summary>
    public bool AllowsNotApplicable { get; set; }

    /// <summary>How many values this axis wants for the work queue to consider it answered. Never a gate.</summary>
    public int MinSelections { get; set; }

    /// <summary>1 = single choice · null = unlimited · n = up to n. Decides the control, not the code.</summary>
    public int? MaxSelections { get; set; } = 1;

    /// <summary>
    /// Every value this sense holds on this axis. Single-select axes carry exactly zero or one, which
    /// is why <see cref="SelectedValueId"/> can stay: it is this list's first entry.
    /// </summary>
    public List<int> SelectedValueIds { get; set; } = new();

    /// <summary>The single answer, for single-select axes. Null when the axis holds none.</summary>
    public int? SelectedValueId => SelectedValueIds.Count > 0 ? SelectedValueIds[0] : null;

    public bool IsNotApplicable { get; set; }
    public string? Note { get; set; }

    /// <summary>0 for the first level. Every level below is a group opened by a value.</summary>
    public int Depth { get; set; }

    /// <summary>
    /// The value that opened this group, null at the first level.
    ///
    /// The entry form MUST render a child group nested under this value and labelled with its name.
    /// When a multi-select axis has children hanging off two of its values and a teacher selects
    /// both, two groups open at once — and in a flat list nobody can tell which sub-answer belongs
    /// to which parent.
    /// </summary>
    public int? ParentValueId { get; set; }
    public string? ParentValueName { get; set; }

    /// <summary>
    /// The axis has been deactivated in settings but this sense already answered it. Deactivation
    /// hides an option from NEW entries; it never erases what is already recorded.
    /// </summary>
    public bool IsRetired { get; set; }

    public List<StationValueDto> Values { get; set; } = new();
}

public class StationValueDto
{
    public int ValueId { get; set; }
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// A short worked example, rendered in muted text so the option itself stays the thing being read.
    /// Optional on every value — a hint is help, never part of the answer.
    /// </summary>
    public string? Hint { get; set; }

    /// <summary>1-9. The digit that picks this value from the keyboard.</summary>
    public int Digit { get; set; }

    /// <summary>Deactivated in settings, but still shown because this sense holds it.</summary>
    public bool IsRetired { get; set; }

    /// <summary>
    /// True when choosing this value REALLY opens a further question. Lets the form hint at depth.
    ///
    /// Counts only groups the resolver would render — an inactive group, or one with no options yet,
    /// is linked in the data but skipped on the form. Marking a value that opens nothing is worse
    /// than not marking it: you pick it, nothing appears, and the cascade looks broken.
    /// </summary>
    public bool OpensChildGroup { get; set; }
}

public class StationRelationDto
{
    public int RelationId { get; set; }
    public string TypeName { get; set; } = string.Empty;
    public string TargetWord { get; set; } = string.Empty;
}

public class SaveStationSenseDto
{
    public int SenseId { get; set; }
    public int? PartOfSpeechId { get; set; }
    public int? DomainId { get; set; }
    public string ExampleUsage { get; set; } = string.Empty;
    public string Definition { get; set; } = string.Empty;
}
