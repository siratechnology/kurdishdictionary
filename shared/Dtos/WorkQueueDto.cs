namespace Shared.Dtos;

/// <summary>
/// The ڕیزی کار buckets, in the order پڕۆمپت ٦ specifies: cheapest to fix first.
/// The numeric values are the display order and are stable, so a deep link keeps working.
/// </summary>
public enum WorkQueueBucket
{
    /// <summary>1 — senses with no part of speech. One click each.</summary>
    MissingPartOfSpeech = 1,

    /// <summary>2 — senses missing a required axis value.</summary>
    MissingRequiredAxis = 2,

    /// <summary>3 — داڕێژراو words with no ڕەگ link.</summary>
    DerivedWithoutRoot = 3,

    /// <summary>4 — لێکدراو words with fewer than two پێکهاتە links.</summary>
    CompoundWithoutComponents = 4,

    /// <summary>5 — senses with no usage example.</summary>
    MissingExample = 5,

    /// <summary>6 — senses with neither هاومانا nor پێچەوانە. Real thinking, so it is last.</summary>
    MissingSemanticRelation = 6,
}

public class WorkQueueBucketDto
{
    public WorkQueueBucket Bucket { get; set; }
    public string Label { get; set; } = string.Empty;
    public string Hint { get; set; } = string.Empty;
    public int Count { get; set; }

    /// <summary>
    /// False when the bucket depends on taxonomy the team has not configured yet. Shown as
    /// "not set up" rather than a reassuring zero — an empty axis list is not an empty backlog.
    /// </summary>
    public bool IsConfigured { get; set; } = true;
}

public class WorkQueueItemDto
{
    public int SenseId { get; set; }
    public int WordId { get; set; }
    public string Word { get; set; } = string.Empty;
    public string? Definition { get; set; }
    public string Reason { get; set; } = string.Empty;

    /// <summary>Deep link straight to the field that needs filling.</summary>
    public string Href { get; set; } = string.Empty;
}

public class WorkQueueDto
{
    public List<WorkQueueBucketDto> Buckets { get; set; } = new();
    public int Total { get; set; }
}
