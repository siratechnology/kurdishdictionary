namespace Shared.Dtos;

/// <summary>
/// A taxonomy value with everything the settings screen needs to decide its fate.
///
/// UsageCount and LastUsedAt are on every row deliberately (پڕۆمپت ١١): once the team can see that
/// most values hold two or three senses each, the sprawl becomes self-correcting.
/// </summary>
public class TaxonomyValueDto
{
    public int Id { get; set; }
    public string NameKu { get; set; } = string.Empty;

    /// <summary>Set only on values a RULE keys on. Renaming never touches it.</summary>
    public string? Code { get; set; }

    public string? AxisName { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; }

    public int UsageCount { get; set; }
    public DateTime? LastUsedAt { get; set; }

    /// <summary>Conditional rules pointing at this value. Non-zero means it cannot be deleted.</summary>
    public int DependentRuleCount { get; set; }

    public int? MergedIntoValueId { get; set; }
    public DateTime? MergedAt { get; set; }
}

/// <summary>What a merge would do, shown before it commits. Never after.</summary>
public class MergePreviewDto
{
    public int SourceValueId { get; set; }
    public string SourceName { get; set; } = string.Empty;

    public int TargetValueId { get; set; }
    public string TargetName { get; set; } = string.Empty;

    public string AxisName { get; set; } = string.Empty;

    public int RowsToMove { get; set; }

    /// <summary>
    /// Senses that already hold the target on this axis. They cannot simply be repointed —
    /// UNIQUE(SenseId, AxisId) forbids it — so they are named rather than discovered mid-merge.
    /// </summary>
    public List<MergeConflictDto> Conflicts { get; set; } = new();

    public int DependentRuleCount { get; set; }

    /// <summary>«٤٨٤ مانا لە سیفەت دەچنە هاوەڵناو» — the sentence the confirm screen shows.</summary>
    public string Summary { get; set; } = string.Empty;
}

public class MergeConflictDto
{
    public int SenseId { get; set; }
    public string Word { get; set; } = string.Empty;
    public string? Definition { get; set; }
}
