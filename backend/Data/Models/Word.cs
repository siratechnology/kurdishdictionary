using System.ComponentModel.DataAnnotations;

namespace backend.Data.Models;

public class Word
{
    public int Id { get; set; }
    public string Kurdish { get; set; } = string.Empty;
    public SpeechPaneType SpeechPane { get; set; } = SpeechPaneType.Other;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? Category { get; set; }
    public ICollection<WordMeans> Meanings { get; set; } = new List<WordMeans>();
    public ICollection<RelatedWord> OutgoingRelations { get; set; } = new List<RelatedWord>();
    public ICollection<RelatedWord> IncomingRelations { get; set; } = new List<RelatedWord>();
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
}

public class WordMeans
{
    [Key]
    public int Id { get; set; }
    public int WordId { get; set; }
    public Word Word { get; set; } = null!;
    public string Meaning { get; set; } = string.Empty;
    public string? Locate { get; set; }
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
