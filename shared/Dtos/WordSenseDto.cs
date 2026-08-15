namespace Shared.Dtos;

/// <summary>
/// A word's senses, as the editor needs them — the v3 shape.
///
/// This is the schema the station screen, the work queue and ڕێکخستن all operate on. The word
/// editor used to write <see cref="WordMeansDto"/> instead, which meant the two halves of the
/// application were maintaining different lists of what a word means.
/// </summary>
public class WordSensesDto
{
    public int WordId { get; set; }
    public string Kurdish { get; set; } = string.Empty;
    public List<WordSenseDto> Senses { get; set; } = new();
}

public class WordSenseDto
{
    /// <summary>0 for a sense that has not been saved yet.</summary>
    public int Id { get; set; }

    public string Definition { get; set; } = string.Empty;

    /// <summary>Required by the linguistic model — a sense needs a usage example in context.</summary>
    public string ExampleUsage { get; set; } = string.Empty;

    /// <summary>
    /// Null means NOBODY HAS DECIDED YET, not "none". The editor shows that as an empty
    /// dropdown rather than picking a default, because a default here would stamp a linguistic
    /// claim with the editor's name.
    /// </summary>
    public int? PartOfSpeechId { get; set; }
    public string? PartOfSpeechName { get; set; }

    public int? DomainId { get; set; }
    public string? DomainName { get; set; }

    public int SortOrder { get; set; }

    /// <summary>Answers on the axes that apply to this sense's part of speech.</summary>
    public List<SenseFeatureDto> Features { get; set; } = new();
}

public class SenseFeatureDto
{
    public int AxisId { get; set; }
    public string? AxisName { get; set; }

    public int? ValueId { get; set; }
    public string? ValueName { get; set; }

    /// <summary>The axis was asked and genuinely does not apply — different from unanswered.</summary>
    public bool IsNotApplicable { get; set; }
}

/// <summary>
/// Replaces a word's whole sense list in one call.
///
/// Whole-list rather than per-sense patches because that is what the form submits: the editor
/// holds every sense on screen at once, and a diff computed on the client would be one more
/// place for the two sides to disagree about what was deleted.
/// </summary>
public class SaveWordSensesDto
{
    public List<WordSenseDto> Senses { get; set; } = new();
}

/// <summary>
/// A word's grammatical classification — asked ONCE for the whole word, not per meaning.
///
/// That is the shape the lexicographers actually work in: «ئاتون» is a ناو and it is بێلایەن,
/// and those facts do not change between its three meanings. Asking the question on every
/// meaning invited three different answers to a question with one answer.
///
/// The database still stores the answer per SENSE, because that is what the station screen, the
/// validator and the work queue read. So one classification here is written onto every sense the
/// word has — the UI asks once, the storage stays normalised, and no reader has to change.
/// </summary>
public class WordClassificationDto
{
    /// <summary>Null means nobody has decided yet — not "none".</summary>
    public int? PartOfSpeechId { get; set; }
    public string? PartOfSpeechName { get; set; }

    /// <summary>One answer per axis assigned to that part of speech.</summary>
    public List<SenseFeatureDto> Features { get; set; } = new();
}
