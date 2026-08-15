namespace Shared.Dtos;

// ═══════════════════════════════════════════════════════════════════════════
// The relation workspace (پەیوەندییەکان).
//
// These speak the v3 model — WordRelation / SenseRelation over RelationTypeDef — not the legacy
// string-typed RelatedWord rows. The eleven types are seeded, wired to their inverses, and split
// by scope for a reason that is grammatical rather than technical:
//
//   پەیوەندی وشەیی  (word scope)  — morphology. ڕەگ, پێکهاتە, چاوگی کارەکە. A property of the
//                                   WORD: جوانتر's root is جوان no matter which sense you mean.
//   پەیوەندی مانایی (sense scope) — semantics. هاومانا, پێچەوانە, مانای گشتیتر. A property of one
//                                   SENSE: زۆر is a synonym of فرە in quantity, not in intensity.
//
// Collapsing the two would force one of them to lie about what it points at.
// ═══════════════════════════════════════════════════════════════════════════

/// <summary>One of the eleven relation types, as the picker needs it.</summary>
public class RelationTypeDto
{
    public int Id { get; set; }

    /// <summary>Stable key — "root", "synonym". Rules and tests match on this, never on the label.</summary>
    public string Code { get; set; } = string.Empty;

    public string NameKu { get; set; } = string.Empty;

    /// <summary>"word" or "sense". Decides whether the editor has to ask which sense.</summary>
    public string Scope { get; set; } = string.Empty;

    public bool IsSymmetric { get; set; }

    /// <summary>
    /// What the FAR side will read once this is saved — «داڕێژراو لێی» when you pick «ڕەگ».
    ///
    /// Shown in the picker because the direction is the part people get wrong: choosing ڕەگ on جوانتر
    /// and pointing it at جوان is right, and the same click from جوان is the opposite claim. Naming
    /// the return edge up front is cheaper than discovering it inverted two hundred rows later.
    /// Null on symmetric types, where both sides read the same.
    /// </summary>
    public string? InverseNameKu { get; set; }

    public int SortOrder { get; set; }

    public bool IsSenseScoped => Scope == "sense";
}

/// <summary>A sense of a word, just enough to choose between them in a dropdown.</summary>
public class SenseBriefDto
{
    public int SenseId { get; set; }
    public string Definition { get; set; } = string.Empty;
    public string? PartOfSpeechName { get; set; }

    /// <summary>«١. ناو — دار» — what the sense picker prints.</summary>
    public string Label { get; set; } = string.Empty;
}

/// <summary>
/// One edge, from the point of view of the word being worked on.
///
/// Both directions arrive in one list. <see cref="IsIncoming"/> is what the UI reads to decide
/// whether the arrow points away or back, and whether the row can be deleted from here.
/// </summary>
public class RelationEdgeDto
{
    public int Id { get; set; }

    public int TypeId { get; set; }
    public string TypeCode { get; set; } = string.Empty;

    /// <summary>
    /// The label FOR THIS DIRECTION. On an incoming ڕەگ edge this reads «داڕێژراو لێی», because
    /// from here that is what the other word is — printing the stored type name on both sides is
    /// how a relation screen ends up claiming a word is its own root.
    /// </summary>
    public string TypeNameKu { get; set; } = string.Empty;

    public string Scope { get; set; } = string.Empty;

    public bool IsIncoming { get; set; }

    /// <summary>Created automatically as the mirror of the other side. Real, attributed, not editable here.</summary>
    public bool IsAutoInverse { get; set; }

    public int OtherWordId { get; set; }
    public string OtherWord { get; set; } = string.Empty;

    // ── Sense-scoped edges only ────────────────────────────────────────────
    public int? OwnSenseId { get; set; }
    public string? OwnSenseLabel { get; set; }
    public int? OtherSenseId { get; set; }
    public string? OtherSenseLabel { get; set; }
}

/// <summary>
/// One word's relation workspace, and where it sits in the walk.
/// </summary>
public class WordRelationsDto
{
    public int WordId { get; set; }
    public string Kurdish { get; set; } = string.Empty;
    public string? Description { get; set; }

    /// <summary>Empty when nobody has split this word into senses yet — sense-scoped types then say so.</summary>
    public List<SenseBriefDto> Senses { get; set; } = new();

    public List<RelationEdgeDto> Relations { get; set; } = new();

    /// <summary>Where this word sits in the walk, so the page can say «٤٥ لە ٢٬٩٦٧».</summary>
    public int Position { get; set; }
    public int Total { get; set; }

    public int OutgoingCount => Relations.Count(r => !r.IsIncoming);
    public int IncomingCount => Relations.Count(r => r.IsIncoming);
}

/// <summary>
/// A candidate target in the search list under the editor.
///
/// Carries its SENSES, not just a count. The list is how targets are picked, and a semantic
/// relation needs a sense on the far side — fetching that per click meant a request every time
/// somebody ticked a box, and a picker that stalls on selection is a picker people stop using.
/// Twenty rows of one-line senses is a few kilobytes; the round trip it removes is the whole
/// interaction.
/// </summary>
public class RelationTargetDto
{
    public int Id { get; set; }
    public string Kurdish { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int RelationCount { get; set; }

    public List<SenseBriefDto> Senses { get; set; } = new();

    public int SenseCount => Senses.Count;

    /// <summary>The sense a semantic relation defaults to — the only one, when there is only one.</summary>
    public int? DefaultSenseId => Senses.Count > 0 ? Senses[0].SenseId : null;
}

public class AddRelationDto
{
    public int FromWordId { get; set; }
    public int ToWordId { get; set; }
    public int TypeId { get; set; }

    /// <summary>Required for sense-scoped types, ignored for word-scoped ones.</summary>
    public int? FromSenseId { get; set; }
    public int? ToSenseId { get; set; }
}

/// <summary>
/// The real totals behind the page header.
///
/// Its own endpoint because the header used to count the twenty-five rows the grid happened to be
/// holding and print that as the state of the dictionary — «٢٥ وشە» when the answer is ٢٬٩٦٧.
/// A summary computed from a page is not a summary.
/// </summary>
public class RelationCoverageDto
{
    public int TotalWords { get; set; }
    public int WordsWithRelations { get; set; }
    public int WordsWithoutRelations { get; set; }
    public int TotalRelations { get; set; }
}
