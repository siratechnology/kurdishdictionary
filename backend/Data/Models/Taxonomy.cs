using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Data.Models;

// ═══════════════════════════════════════════════════════════════════════════
// The linguistic model, as three INDEPENDENT dimensions. They are never collapsed
// into one column, because they answer different questions:
//
//   A. بەشی ئاخاوتن   — part of speech.   CLOSED list of exactly 7.
//   B. تەوەرەکانی ڕێزمانی — grammatical axes. One value per axis per sense; a noun carries six.
//   C. بواری زانستی    — subject field.     Hierarchical, orthogonal to A and B.
//
// This is a CONFIGURATION ENGINE. Apart from the seven parts of speech and the relation
// types below, nothing here is seeded — the team enters every axis, value and domain
// through the settings area. The app must work correctly with a completely empty taxonomy.
// ═══════════════════════════════════════════════════════════════════════════

/// <summary>
/// A. بەشی ئاخاوتن — closed list of seven. Not user-extensible: adding one is a linguistic
/// claim, not a data-entry convenience, and is gated behind an elevated role plus a written
/// reason (پڕۆمپت ١١ tier 3). The seven rows are seeded as empty named records the team can
/// rename; nothing else about them is fixed.
/// </summary>
public class PartOfSpeech : ISoftDeletable
{
    public int Id { get; set; }

    /// <summary>Stable machine key. Renaming NameKu must never change this — code and history point here.</summary>
    [MaxLength(32)]
    public string Code { get; set; } = string.Empty;

    [MaxLength(100)]
    public string NameKu { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    public int SortOrder { get; set; }

    /// <summary>Deactivated values still render in existing data and history; they only stop being offered.</summary>
    public bool IsActive { get; set; } = true;

    public ICollection<PartOfSpeechAxis> Axes { get; set; } = new List<PartOfSpeechAxis>();

    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedByUserId { get; set; }
}

/// <summary>
/// B. One grammatical axis — ژمارە, ڕەگەز, دەمژمێر, جۆری کار… Ships EMPTY; the team enters them.
/// </summary>
public class FeatureAxis : ISoftDeletable
{
    public int Id { get; set; }

    [MaxLength(32)]
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// The short TECHNICAL label — جۆری کار, تێپەڕی, لە ڕووی ڕەگەزەوە. Kept for admin reference: it is
    /// what the settings tree is navigated by, what history reads back, and what a linguist recognises.
    ///
    /// It is deliberately NOT what the entry form shows. See <see cref="PromptKu"/>.
    /// </summary>
    [MaxLength(100)]
    public string NameKu { get; set; } = string.Empty;

    /// <summary>
    /// The plain-language QUESTION a teacher reads above this dropdown — «کارەکە چ جۆرێکە؟».
    ///
    /// A teacher looking at «جۆری کار» does not know what is being asked of them; a teacher looking at
    /// «کارەکە چ جۆرێکە؟» does. Both strings are needed and neither can be derived from the other, so
    /// this is a second column rather than a rename of <see cref="NameKu"/>.
    ///
    /// Null falls back to <see cref="NameKu"/>. Null is the CORRECT state for a dropdown nobody has
    /// written a question for yet — better a bare category name than an invented one, because a
    /// plausible wrong question is answered confidently and a bare label gets asked about.
    /// </summary>
    [MaxLength(300)]
    public string? PromptKu { get; set; }

    /// <summary>
    /// True on a <see cref="PromptKu"/> the SYSTEM suggested rather than the team wrote.
    ///
    /// The seeded questions are starting points, not linguistics: the settings screen flags them and
    /// lists them for review, and the flag clears the moment a human edits the wording. Without it a
    /// suggestion is indistinguishable from a decision the day after it is seeded.
    /// </summary>
    public bool PromptNeedsReview { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Whether this axis offers an explicit "does not apply" answer. Empty and not-applicable are
    /// different facts: a sense with no ڕەگەز value is unfinished work, while a ناتەواو verb having
    /// no تێپەڕی is a complete answer. Without this the work queue lists finished senses forever.
    /// </summary>
    public bool AllowsNotApplicable { get; set; }

    /// <summary>
    /// How few values a sense should hold on this axis to count as finished. Drives the work queue
    /// and the completeness score, and NOTHING ELSE.
    ///
    /// It must never gate a save. Every axis and every sub-option is optional at every depth — the
    /// only hard gate in the system is that a sense cannot be published without a part of speech. A
    /// teacher stopped by a required sub-option picks a wrong value to escape the form, and a wrong
    /// value that looks finished is worse than a blank that reports itself.
    /// </summary>
    public int MinSelections { get; set; }

    /// <summary>
    /// How many values a sense may hold on this axis: 1 = a single choice, null = unlimited, 3 = up
    /// to three. Configuration, not code — ناو's axes are one-each and others need two or three, and
    /// which is which is the team's decision rather than a branch on an axis name.
    ///
    /// The migration that introduced this column backfilled 1 on every axis that already existed, so
    /// ناو behaves exactly as it did before it. <c>NounRegressionTests</c> asserts that.
    /// </summary>
    public int? MaxSelections { get; set; } = 1;

    public ICollection<FeatureValue> Values { get; set; } = new List<FeatureValue>();

    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedByUserId { get; set; }
}

/// <summary>One value on an axis — تاک, کۆ, نێر, مێ… Ships EMPTY.</summary>
public class FeatureValue : ISoftDeletable
{
    public int Id { get; set; }

    public int AxisId { get; set; }
    public FeatureAxis Axis { get; set; } = null!;

    [MaxLength(100)]
    public string NameKu { get; set; } = string.Empty;

    /// <summary>
    /// Optional stable key, set only on values that a RULE depends on — داڕێژراو, لێکدراو, کۆ.
    ///
    /// Validators must never match on NameKu: the team can rename any label from settings (that is
    /// the entire point of پڕۆمپت ١١), and a rule keyed on a label silently stops firing the moment
    /// someone fixes a typo. A rule keyed on a code survives every rename.
    /// </summary>
    [MaxLength(32)]
    public string? Code { get; set; }

    /// <summary>
    /// A short worked example shown in muted text beside this option — under تێپەڕ, a verb that takes
    /// an object. NEVER required, on any option.
    ///
    /// It exists because the question and the option names together are still not always enough: a
    /// teacher who understands «ئایا کارەکە تێپەڕە؟» can still be unsure which of two options their
    /// word is, and one example settles it faster than any amount of label rewording.
    /// </summary>
    [MaxLength(300)]
    public string? OptionHintKu { get; set; }

    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Set by a merge (پڕۆمپت ١١): where this value's rows were moved to. Recorded on the row so a
    /// merge stays reversible for 30 days instead of being a one-way door.
    /// </summary>
    public int? MergedIntoValueId { get; set; }
    public DateTime? MergedAt { get; set; }

    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedByUserId { get; set; }
}

/// <summary>
/// Which axes apply to which part of speech, and under what condition.
///
/// <see cref="RequiresValueId"/> is what gives the entry form UNLIMITED depth without a schema
/// change: axis C requires a value on axis B, which itself required a value on axis A. Resolve the
/// visible axis set by walking the chain against the sense's current answers, recursively —
/// جێناو → سەربەخۆ → کەسی/پرس/نیشانە is already three levels, and there is no reason a fourth
/// will not appear. Do not build a two-level special case.
/// </summary>
public class PartOfSpeechAxis
{
    public int Id { get; set; }

    public int PartOfSpeechId { get; set; }
    public PartOfSpeech PartOfSpeech { get; set; } = null!;

    public int AxisId { get; set; }
    public FeatureAxis Axis { get; set; } = null!;

    public bool IsRequired { get; set; }

    /// <summary>
    /// Null = unconditional; render it as soon as the part of speech is chosen. Otherwise this axis
    /// appears only once the sense holds this value on another axis, and is REMOVED — with its
    /// stored answer cleared and logged — if that value is changed away.
    /// </summary>
    public int? RequiresValueId { get; set; }
    public FeatureValue? RequiresValue { get; set; }

    public int SortOrder { get; set; }
}

/// <summary>
/// C. بواری زانستی — subject field. Hierarchical: the deck's ten dictionaries are the roots and
/// the existing 79 categories nest beneath them (پڕۆمپت ٤). Ships EMPTY — the names are the team's.
/// </summary>
public class Domain : ISoftDeletable
{
    public int Id { get; set; }

    /// <summary>Stored WITHOUT the «فەرهەنگی» prefix; the UI renders it.</summary>
    [MaxLength(150)]
    public string NameKu { get; set; } = string.Empty;

    public int? ParentId { get; set; }
    public Domain? Parent { get; set; }
    public ICollection<Domain> Children { get; set; } = new List<Domain>();

    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;

    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedByUserId { get; set; }
}

// ═══════════════════════════════════════════════════════════════════════════
// Sense — where the linguistic model actually lives
// ═══════════════════════════════════════════════════════════════════════════

/// <summary>
/// A single sense of a word.
///
/// Part of speech belongs HERE, never on the word. Proof from the source deck: زۆر and کەم are
/// both هاوەڵناوی نادیار AND هاوەڵکاری چەندێتی — one word, two parts of speech, which a column on
/// Word cannot express.
///
/// Added alongside <see cref="WordMeans"/> rather than replacing it. پڕۆمپت ٤ migrates the rows
/// across; nothing is dropped until that has been reviewed.
/// </summary>
public class Sense : ISoftDeletable
{
    public int Id { get; set; }

    public int WordId { get; set; }
    public Word Word { get; set; } = null!;

    /// <summary>The definition, in Sorani. This is a monolingual dictionary — not a translation.</summary>
    [MaxLength(2000)]
    public string Definition { get; set; } = string.Empty;

    /// <summary>
    /// Null means NOBODY HAS DECIDED YET — not "none".
    ///
    /// The migration leaves it null wherever the old data was ambiguous: پۆل and جۆری وشە
    /// contradicting each other, or a word carrying several parts of speech at once. Writing a
    /// guess here would stamp a linguistic claim with a real teacher's name, and the ledger would
    /// present it as their judgement forever.
    ///
    /// Required by the linguistic model, nullable in the column — the rule is enforced by the
    /// validator and the work queue, which can say "not yet". A NOT NULL constraint cannot.
    /// </summary>
    public int? PartOfSpeechId { get; set; }
    public PartOfSpeech? PartOfSpeech { get; set; }

    public int? DomainId { get; set; }
    public Domain? Domain { get; set; }

    /// <summary>
    /// Required, non-empty — deck slide 14: every sense needs a usage example in semantic context.
    /// Not optional, and not enforceable by the database alone (a service-layer validator rejects
    /// whitespace), but the column is NOT NULL so nothing can insert around it.
    /// </summary>
    [MaxLength(1000)]
    public string ExampleUsage { get; set; } = string.Empty;

    public SenseWorkflowState WorkflowState { get; set; } = SenseWorkflowState.Raw;

    public int SortOrder { get; set; }

    /// <summary>
    /// Persisted computed column, 0-100, over the fields that live ON this row.
    ///
    /// It deliberately does NOT include required-axis coverage: a computed column cannot join, and
    /// which axes are required depends on PartOfSpeechAxis. The work queue (پڕۆمپت ٦) computes that
    /// part. This column exists so the common "is this sense fleshed out" filter needs no join.
    /// </summary>
    public int Completeness { get; private set; }

    public ICollection<SenseFeature> Features { get; set; } = new List<SenseFeature>();

    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedByUserId { get; set; }
}

/// <summary>خام → پۆلێنکراو → بڵاوکراو, plus کێشەدار for senses the taxonomy does not fit.</summary>
public enum SenseWorkflowState
{
    /// <summary>خام — entered, not yet classified.</summary>
    Raw = 0,

    /// <summary>پۆلێنکراو — part of speech and required axes answered.</summary>
    Classified = 1,

    /// <summary>بڵاوکراو — published.</summary>
    Published = 2,

    /// <summary>
    /// کێشەدار — a teacher hit a word the taxonomy does not fit and said so. This queue is the
    /// input for revising the source taxonomy itself, not a rejection.
    /// </summary>
    Disputed = 3,
}

/// <summary>
/// One answer: this sense has this value on this axis.
///
/// UNIQUE(SenseId, AxisId, ValueId) is enforced in the database. It used to be
/// UNIQUE(SenseId, AxisId) — one answer per axis, full stop — which made a multi-select axis
/// impossible to express. The constraint is now "never the same value twice on one sense", and how
/// many values an axis may hold is <see cref="FeatureAxis.MaxSelections"/>, which is data.
///
/// The loosening is safe in that direction: no row that satisfied the old index can violate the new
/// one. What keeps ناو unchanged is not the index, it is MaxSelections = 1 on its axes.
/// </summary>
public class SenseFeature : ISoftDeletable
{
    public int Id { get; set; }

    public int SenseId { get; set; }
    public Sense Sense { get; set; } = null!;

    public int AxisId { get; set; }
    public FeatureAxis Axis { get; set; } = null!;

    /// <summary>
    /// Null together with <see cref="IsNotApplicable"/> = an explicit "this axis does not apply to
    /// this word", which is a complete answer rather than a blank.
    /// </summary>
    public int? ValueId { get; set; }
    public FeatureValue? Value { get; set; }

    public bool IsNotApplicable { get; set; }

    /// <summary>Required when IsNotApplicable — the escape hatch always costs a sentence of explanation.</summary>
    [MaxLength(1000)]
    public string? Note { get; set; }

    /// <summary>
    /// Set by <see cref="backend.Services.Lexicon.OptionsTreeService"/> just before this row is
    /// removed because the question stopped being asked — the parent value it hung off was changed
    /// away. The ledger interceptor reads it and writes FeatureCleared instead of FeatureChanged.
    ///
    /// Not a column. It exists for the length of one SaveChanges and is nobody's business
    /// afterwards; what survives is the event, which is the thing history is read from.
    /// </summary>
    [NotMapped]
    public bool ClearedByCascade { get; set; }

    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedByUserId { get; set; }
}

/// <summary>
/// A translation of a sense into another language.
///
/// This is NOT part of the linguistic model — the product is a monolingual Sorani dictionary and a
/// gloss in Persian is not a sense of a Kurdish word. It exists because the live data has ~2,670
/// Persian rows sitting in WordMeans alongside the Sorani definitions, and losing them to make the
/// sense count honest would be throwing away work that teachers did.
///
/// So they are parked here: preserved, attributed, out of the way of the sense model, and available
/// if a bilingual edition is ever wanted.
/// </summary>
public class SenseTranslation : ISoftDeletable
{
    public int Id { get; set; }

    public int SenseId { get; set; }
    public Sense Sense { get; set; } = null!;

    /// <summary>
    /// BCP-47 where it can be determined ("fa", "en", "ar"), so the eight spellings of
    /// «کوردی سۆرانی» and the two of «فارسی» collapse to one value each on the way in.
    /// </summary>
    [MaxLength(16)]
    public string LanguageCode { get; set; } = string.Empty;

    /// <summary>The label exactly as it was typed, kept so nothing about the original row is lost.</summary>
    [MaxLength(100)]
    public string? OriginalLabel { get; set; }

    [MaxLength(2000)]
    public string Text { get; set; } = string.Empty;

    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedByUserId { get; set; }
}

// ═══════════════════════════════════════════════════════════════════════════
// Forms and relations
// ═══════════════════════════════════════════════════════════════════════════

/// <summary>پلەی بەراورد, پلەی باڵا, کۆ… Ships EMPTY.</summary>
public class WordFormType : ISoftDeletable
{
    public int Id { get; set; }

    [MaxLength(100)]
    public string NameKu { get; set; } = string.Empty;

    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;

    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedByUserId { get; set; }
}

/// <summary>
/// An inflected form — جوانتر, جوانترین, کۆ. These are FORMS of a headword, never separate Word
/// rows. Search matches <see cref="Normalized"/> and returns the parent word's entry.
/// </summary>
public class WordForm : ISoftDeletable
{
    public int Id { get; set; }

    public int WordId { get; set; }
    public Word Word { get; set; } = null!;

    [MaxLength(200)]
    public string Form { get; set; } = string.Empty;

    public int FormTypeId { get; set; }
    public WordFormType FormType { get; set; } = null!;

    /// <summary>Kurdish-folded, written at save time and matched at query time (پڕۆمپت ٥).</summary>
    [MaxLength(200)]
    public string Normalized { get; set; } = string.Empty;

    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedByUserId { get; set; }
}

public enum RelationScope
{
    /// <summary>Morphological — ڕەگ, پێکهاتە. Sits on the WORD.</summary>
    Word = 0,

    /// <summary>Semantic — هاومانا, پێچەوانە. Sits on the SENSE.</summary>
    Sense = 1,
}

/// <summary>
/// The kinds of relation that exist. Renameable, but Scope and Inverse wiring are NOT editable:
/// making a symmetric relation directional would break the auto-inverse logic that saves a teacher
/// entering every edge twice.
///
/// This is one of only two things seeded in the whole taxonomy, because relation semantics are
/// structural rather than editorial.
/// </summary>
public class RelationTypeDef
{
    public int Id { get; set; }

    [MaxLength(32)]
    public string Code { get; set; } = string.Empty;

    [MaxLength(100)]
    public string NameKu { get; set; } = string.Empty;

    public RelationScope Scope { get; set; }

    /// <summary>Symmetric relations are their own inverse: saving A→B implies B→A of the same type.</summary>
    public bool IsSymmetric { get; set; }

    /// <summary>The opposite direction, for directional types. Null when symmetric.</summary>
    public int? InverseId { get; set; }
    public RelationTypeDef? Inverse { get; set; }

    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;
}

/// <summary>
/// MORPHOLOGICAL relation between two words — ڕەگ, پێکهاتە. Kept in a separate table from
/// <see cref="SenseRelation"/> on purpose: morphology is a property of the word, semantics of the
/// sense, and merging them would force one of the two to lie about what it points at.
/// </summary>
public class WordRelation : ISoftDeletable
{
    public int Id { get; set; }

    public int FromWordId { get; set; }
    public Word FromWord { get; set; } = null!;

    public int ToWordId { get; set; }
    public Word ToWord { get; set; } = null!;

    public int TypeId { get; set; }
    public RelationTypeDef Type { get; set; } = null!;

    /// <summary>
    /// True when this edge was created automatically as the inverse of another. Both edges are real
    /// and both are attributed to the same user; this flag only stops the UI showing the pair twice.
    /// </summary>
    public bool IsAutoInverse { get; set; }

    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedByUserId { get; set; }
}

/// <summary>SEMANTIC relation between two senses — هاومانا, پێچەوانە, مانای گشتیتر.</summary>
public class SenseRelation : ISoftDeletable
{
    public int Id { get; set; }

    public int FromSenseId { get; set; }
    public Sense FromSense { get; set; } = null!;

    public int ToSenseId { get; set; }
    public Sense ToSense { get; set; } = null!;

    public int TypeId { get; set; }
    public RelationTypeDef Type { get; set; } = null!;

    public bool IsAutoInverse { get; set; }

    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedByUserId { get; set; }
}
