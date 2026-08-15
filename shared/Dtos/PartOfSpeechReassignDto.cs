namespace Shared.Dtos;

/// <summary>
/// What a bulk part-of-speech re-assignment would do, worked out before anything is written.
///
/// Nothing here is a total the caller could have guessed. The count of senses is the easy half;
/// the half that decides whether this is safe is <see cref="FeaturesDropped"/> — grammatical
/// answers that cannot come with the sense because the target does not ask that question.
/// </summary>
public class PartOfSpeechReassignPreviewDto
{
    public int FromId { get; set; }
    public string FromName { get; set; } = string.Empty;

    public int ToId { get; set; }
    public string ToName { get; set; } = string.Empty;

    /// <summary>Every sense currently sitting on the source.</summary>
    public int SensesToMove { get; set; }

    /// <summary>Distinct words behind those senses — the number the team thinks in.</summary>
    public int WordsAffected { get; set; }

    /// <summary>
    /// Stored answers that will be cleared, because they sit on an axis the target part of speech
    /// does not have. A sense moved from ناو to کار cannot keep its ژمارە answer: کار never asks.
    /// </summary>
    public int FeaturesDropped { get; set; }

    /// <summary>The axes those dropped answers belong to, named, so the loss is legible.</summary>
    public List<string> DroppedAxisNames { get; set; } = new();

    /// <summary>A few headwords, to make the count concrete. Not the whole list.</summary>
    public List<string> SampleWords { get; set; } = new();

    /// <summary>The sentence shown on the confirm button's dialog.</summary>
    public string Summary { get; set; } = string.Empty;
}

/// <summary>The result of running one, and what can still be undone.</summary>
public class PartOfSpeechReassignResultDto
{
    public int SensesMoved { get; set; }
    public int FeaturesDropped { get; set; }

    /// <summary>Stamp of this run — an undo names it so a later run is not caught by mistake.</summary>
    public DateTime ReassignedAt { get; set; }
}

/// <summary>
/// A run that can still be put back, read out of the senses themselves rather than a log table.
///
/// Derived, not stored: the provenance columns on Sense already say which rows moved, where from
/// and when, so grouping them IS the list. A separate table would be a second source of truth that
/// could disagree with the rows the undo actually operates on.
/// </summary>
public class PartOfSpeechReassignRunDto
{
    public int FromId { get; set; }
    public string FromName { get; set; } = string.Empty;

    public int ToId { get; set; }
    public string ToName { get; set; } = string.Empty;

    public DateTime At { get; set; }

    /// <summary>Senses still sitting where this run put them — undoing moves exactly these.</summary>
    public int SenseCount { get; set; }
}
