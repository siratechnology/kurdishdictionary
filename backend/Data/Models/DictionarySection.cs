namespace backend.Data.Models;

/// <summary>
/// بەشی فەرهەنگ — which section of the dictionary a word belongs to.
///
/// A property of the WORD, not of its meanings. «هەناسە» is a medical word whichever of its
/// senses you are reading; the section does not change between them. That distinction was got
/// wrong once already, with the part of speech, and the rule is the same here: anything true of
/// the headword regardless of sense belongs on the word.
///
/// Deliberately NOT the same thing as:
///
///   • <see cref="Category"/> (پۆل) — those 79 rows are SEMANTIC (سیفەت, کەرەستە, شوێن, کەس).
///     A word is in the medical section AND is a کەرەستە; the two answer different questions.
///   • <see cref="Domain"/> — 24 rows carried over by the legacy import and hanging off senses.
///     Left exactly where they are; nothing about this table touches them.
///
/// The list is open. New sections can be typed in the word editor rather than requiring a trip
/// to ڕێکخستن first, because a lexicographer meeting a word from a field nobody has entered yet
/// should not have to stop and go configure something before recording it.
/// </summary>
public class DictionarySection : ISoftDeletable
{
    public int Id { get; set; }

    public string NameKu { get; set; } = string.Empty;

    /// <summary>
    /// Folded name, for matching. Keeps «کیمیا» typed with a Persian kaf from becoming a second
    /// section beside the one already there — the same fold every other name in this system uses.
    /// </summary>
    public string Normalized { get; set; } = string.Empty;

    public string? Description { get; set; }

    public int SortOrder { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid? CreatedByUserId { get; set; }

    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedByUserId { get; set; }

    public ICollection<Word> Words { get; set; } = new List<Word>();
}
