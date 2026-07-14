using System.ComponentModel.DataAnnotations;

namespace backend.Data.Models;

public class Word
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
}

// Join entity: Word <-> SpeechPaneType (many-to-many via enum, no separate table needed)
public class WordSpeechPane
{
    public int WordId { get; set; }
    public Word Word { get; set; } = null!;
    public SpeechPaneType SpeechPaneType { get; set; }
}

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<WordCategory> WordCategories { get; set; } = new List<WordCategory>();
}

// Join entity: Word <-> Category (many-to-many)
public class WordCategory
{
    public int WordId { get; set; }
    public Word Word { get; set; } = null!;
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
}

public class RelatedWord
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
}

public class WordMeans
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

public enum RelationType
{
    Synonym = 1,
    Antonym = 2,
    Related = 3,
    Example = 4
}

public static class RelationTypeExtensions
{
    public static string ToKurdish(this RelationType type) => type switch
    {
        RelationType.Synonym => "هاومانا",
        RelationType.Antonym => "دژمانا",
        RelationType.Related => "پەیوەندیدار",
        RelationType.Example => "نموونە",
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
    };

    public static List<(int Id, string Kurdish)> ToList() =>
        Enum.GetValues<RelationType>()
            .Select(x => ((int)x, x.ToKurdish()))
            .ToList();
}
