namespace backend.Data.Models;

/// <summary>
/// The contribution ledger: append-only, INSERT only, forever.
///
/// This table is the single source of truth for who did what. Contribution counts are NEVER read
/// from columns on a word or sense row — they are always derived from here (see
/// <c>vw_ContributionStats</c>). A counter column can drift; an event log cannot.
///
/// It exists for ATTRIBUTION and TRACEABILITY, not policing. The contributors are expert language
/// teachers whose names go in the published dictionary's credits, and this is the evidence for those
/// credits. Nothing built on this table may become a score, a ranking, or a productivity metric.
///
/// UPDATE and DELETE are rejected by a database trigger (<c>TR_ContributionEvent_NoUpdateDelete</c>),
/// not by convention and not only by the interceptor — a trigger cannot be bypassed by a stray
/// SQL script, a migration, or a future service that forgets.
/// </summary>
public class ContributionEvent
{
    public long Id { get; set; }

    /// <summary>
    /// Never null. Migrated rows whose real author is unknown point at the dedicated
    /// <c>legacy-import</c> account rather than carrying a null, so "unattributed" is a visible
    /// fact in the data instead of an absence that every query has to remember to handle.
    /// </summary>
    public Guid UserId { get; set; }
    public AppUser? User { get; set; }

    public DateTime OccurredAt { get; set; } = DateTime.UtcNow;

    public ContributionEventType EventType { get; set; }

    /// <summary>Word | Sense | WordForm | WordRelation | SenseRelation | Category | Taxonomy…</summary>
    public string EntityType { get; set; } = string.Empty;

    public long EntityId { get; set; }

    /// <summary>
    /// Denormalised so per-word history is one indexed lookup rather than a union across every
    /// child table. Null for events that belong to no single word (taxonomy edits, reviews).
    /// </summary>
    public int? WordId { get; set; }

    /// <summary>For edits: which property changed. Null on create/delete, where the row is the change.</summary>
    public string? FieldName { get; set; }

    public string? OldValue { get; set; }
    public string? NewValue { get; set; }

    /// <summary>Free text: a reviewer's reason on reject, or the mandatory reason on a Tier 3 taxonomy change.</summary>
    public string? Note { get; set; }

    public ContributionSourceKind SourceKind { get; set; } = ContributionSourceKind.Human;
}

/// <summary>
/// What happened. Values are explicit rather than implicit so that renaming a C# member can never
/// silently repoint historical rows at a different meaning.
/// </summary>
public enum ContributionEventType
{
    WordCreated = 1,
    WordEdited = 2,
    WordDeleted = 3,
    WordRestored = 4,

    SenseCreated = 10,
    SenseEdited = 11,
    SenseClassified = 12,
    SenseReclassified = 13,

    FeatureSet = 20,
    FeatureChanged = 21,

    /// <summary>
    /// An answer was removed because the question stopped being asked — the parent value it hung
    /// off was changed away, so its whole sub-group left the form.
    ///
    /// Distinct from FeatureChanged on purpose. "Somebody changed تێپەڕی" and "تێپەڕی stopped
    /// applying and its answer was cleared" are different facts about a word, and per-word history
    /// is unreadable if a cascade looks like a person retyping. Exactly ONE of these per cleared
    /// answer, never one for the batch.
    /// </summary>
    FeatureCleared = 22,

    RelationAdded = 30,
    RelationRemoved = 31,

    FormAdded = 40,
    FormRemoved = 41,

    SubmittedForReview = 50,
    Approved = 51,
    Rejected = 52,
    Reopened = 53,
}

public enum ContributionSourceKind
{
    /// <summary>A person, working in the app.</summary>
    Human = 0,

    /// <summary>Written by a data migration. The UserId is the real original author where one is known.</summary>
    Migration = 1,

    /// <summary>Bulk import from an external file.</summary>
    Import = 2,
}

/// <summary>
/// Read model over <see cref="ContributionEvent"/>, backed by the <c>vw_ContributionStats</c> view.
/// Every number here is a COUNT over the ledger — there is no counter column anywhere in the schema
/// to fall out of step with it.
/// </summary>
public class ContributionStats
{
    public Guid UserId { get; set; }
    public string? UserName { get; set; }
    public string? FullName { get; set; }

    public int WordsCreated { get; set; }
    public int SensesClassified { get; set; }
    public int FeaturesSet { get; set; }
    public int RelationsAdded { get; set; }
    public int FormsAdded { get; set; }
    public int ReviewsDone { get; set; }

    /// <summary>Approved ÷ (approved + rejected), as a ratio. Zero reviews yields 0.</summary>
    public double ApprovalRate { get; set; }

    public DateTime? FirstContributionAt { get; set; }
    public DateTime? LastContributionAt { get; set; }
}
