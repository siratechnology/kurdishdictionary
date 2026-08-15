using System.ComponentModel.DataAnnotations;

namespace backend.Data.Models;

public class Word : ISoftDeletable
{
    public int Id { get; set; }
    public string Kurdish { get; set; } = string.Empty;
    public GrammaticalGender Gender { get; set; } = GrammaticalGender.None;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// The user this word belongs to. Nullable because words created before authentication
    /// existed have no known author; those are backfilled by first letter at startup
    /// (see WordOwnershipBackfill).
    /// </summary>
    public Guid? CreatedByUserId { get; set; }
    public AppUser? CreatedByUser { get; set; }

    public Guid? UpdatedByUserId { get; set; }
    public AppUser? UpdatedByUser { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public ICollection<WordSpeechPane> SpeechPanes { get; set; } = new List<WordSpeechPane>();
    public ICollection<WordCategory> WordCategories { get; set; } = new List<WordCategory>();
    public ICollection<WordMeans> Meanings { get; set; } = new List<WordMeans>();
    public ICollection<RelatedWord> OutgoingRelations { get; set; } = new List<RelatedWord>();
    public ICollection<RelatedWord> IncomingRelations { get; set; } = new List<RelatedWord>();

    // ── Schema v3 (پڕۆمپت ٣) — added alongside the old columns, which still drive the app ──
    /// <summary>
    /// Kurdish-folded headword: ي/ی/ى → ی, ك/ک → ک, final ه/ھ/ە → ە, ر/ڕ and و/وو collapsed, ZWNJ
    /// and diacritics stripped. Written at save time and matched at query time so that گەڕان for
    /// کوردی finds كوردي (پڕۆمپت ٥). Empty until the normaliser exists.
    /// </summary>
    [MaxLength(200)]
    public string Normalized { get; set; } = string.Empty;

    /// <summary>
    /// بەشی فەرهەنگ — which section of the dictionary this word belongs to.
    ///
    /// Nullable in the column, required by the editor. The form will not save a new word without
    /// one, but three thousand imported words do not have one yet, and a NOT NULL constraint
    /// would mean either refusing to load them or inventing a section for each — a claim about
    /// every word in the dictionary, made by a migration rather than by a lexicographer.
    /// </summary>
    public int? DictionarySectionId { get; set; }
    public DictionarySection? DictionarySection { get; set; }

    /// <summary>Senses. Replaces <see cref="Meanings"/> once پڕۆمپت ٤ has migrated the rows across.</summary>
    public ICollection<Sense> Senses { get; set; } = new List<Sense>();

    public ICollection<WordForm> Forms { get; set; } = new List<WordForm>();
    public ICollection<WordRelation> OutgoingWordRelations { get; set; } = new List<WordRelation>();
    public ICollection<WordRelation> IncomingWordRelations { get; set; } = new List<WordRelation>();

    // ── Soft delete (see ISoftDeletable) ──────────────────────────────────
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedByUserId { get; set; }
}

// Join entity: Word <-> SpeechPaneType (many-to-many via enum, no separate table needed)
public class WordSpeechPane : ISoftDeletable
{
    public int WordId { get; set; }
    public Word Word { get; set; } = null!;
    public SpeechPaneType SpeechPaneType { get; set; }

    // ── Soft delete (see ISoftDeletable) ──────────────────────────────────
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedByUserId { get; set; }
}

public class Category : ISoftDeletable
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<WordCategory> WordCategories { get; set; } = new List<WordCategory>();

    // ── Soft delete (see ISoftDeletable) ──────────────────────────────────
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedByUserId { get; set; }
}

// Join entity: Word <-> Category (many-to-many)
public class WordCategory : ISoftDeletable
{
    public int WordId { get; set; }
    public Word Word { get; set; } = null!;
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    // ── Soft delete (see ISoftDeletable) ──────────────────────────────────
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedByUserId { get; set; }
}

public class RelatedWord : ISoftDeletable
{
    public int Id { get; set; }
    public int WordId { get; set; }
    public Word Word { get; set; } = null!;
    public int TargetWordId { get; set; }
    public Word TargetWord { get; set; } = null!;
    public string RelationType { get; set; } = string.Empty;
    public int Weight { get; set; } = 1;

    /// <summary>
    /// Who authored this relation. Existing rows inherit the owner of the source word at startup —
    /// a relation hanging off someone's word is work they did on that word.
    /// </summary>
    public Guid? CreatedByUserId { get; set; }
    public AppUser? CreatedByUser { get; set; }

    // ── Soft delete (see ISoftDeletable) ──────────────────────────────────
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedByUserId { get; set; }
}

public class WordMeans : ISoftDeletable
{
    [Key]
    public int Id { get; set; }
    public int WordId { get; set; }
    public Word Word { get; set; } = null!;
    public string Meaning { get; set; } = string.Empty;
    public string? Locate { get; set; }

    /// <summary>Who authored this meaning. Backfilled from the parent word's owner. See <see cref="RelatedWord.CreatedByUserId"/>.</summary>
    public Guid? CreatedByUserId { get; set; }
    public AppUser? CreatedByUser { get; set; }

    // ── Soft delete (see ISoftDeletable) ──────────────────────────────────
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedByUserId { get; set; }
}

public enum GrammaticalGender
{
    None = 0,
    Masculine = 1,
    Feminine = 2,
    Neuter = 3,
    Common = 4
}

public static class GrammaticalGenderExtensions
{
    public static string ToKurdish(this GrammaticalGender gender) => gender switch
    {
        GrammaticalGender.None => "نییە",
        GrammaticalGender.Masculine => "نێر",
        GrammaticalGender.Feminine => "مێ",
        GrammaticalGender.Neuter => "بێلایەن",
        GrammaticalGender.Common => "دوولایەن",
        _ => throw new ArgumentOutOfRangeException(nameof(gender), gender, null)
    };

    public static List<(int Id, string Kurdish)> ToList() =>
        Enum.GetValues<GrammaticalGender>()
            .Select(x => ((int)x, x.ToKurdish()))
            .ToList();
}

public enum SpeechPaneType
{
    Noun = 1,
    Verb = 2,
    Adjective = 3,
    Adverb = 4,
    Pronoun = 5,
    Preposition = 6,
    Conjunction = 7,
    Interjection = 8,
    Determiner = 9,
    Number = 10,
    Particle = 11,
    Article = 12,
    Infinitive = 13,
    Other = 14
}

public static class SpeechPaneTypeExtensions
{
    public static string ToKurdish(this SpeechPaneType type) => type switch
    {
        SpeechPaneType.Noun => "ناو",
        SpeechPaneType.Verb => "کار",
        SpeechPaneType.Adjective => "ئاوەڵناو",
        SpeechPaneType.Adverb => "ئاوەڵکار",
        SpeechPaneType.Pronoun => "جێناو",
        SpeechPaneType.Preposition => "پێشگر",
        SpeechPaneType.Conjunction => "بەستەر",
        SpeechPaneType.Interjection => "بانگکردن",
        SpeechPaneType.Determiner => "دیارخەر",
        SpeechPaneType.Number => "ژمارە",
        SpeechPaneType.Particle => "وردە وشە",
        SpeechPaneType.Article => "ئامرازی ناساند",
        SpeechPaneType.Infinitive => "چاوگ",
        SpeechPaneType.Other => "هیتر",
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
    };

    public static List<(int Id, string Kurdish)> ToList() =>
        Enum.GetValues<SpeechPaneType>()
            .Select(x => ((int)x, x.ToKurdish()))
            .ToList();
}

// The RelationType enum that used to sit here is gone. It was dead code: RelatedWord.RelationType
// is a free string, the live data holds six values (synonym, antonym, related, contextual, example,
// usage) and the enum declared only four of them, so it could not describe its own table. Relation
// semantics now live in RelationTypeDef, which is data the team can rename.
