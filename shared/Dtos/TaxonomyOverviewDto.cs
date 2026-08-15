namespace Shared.Dtos;

/// <summary>Everything the taxonomy settings screen draws, in one payload.</summary>
public class TaxonomyOverviewDto
{
    /// <summary>
    /// Words in the dictionary — the same 2,967 every other screen reports.
    ///
    /// Here because the per-part-of-speech counts CANNOT be summed into a total: the sets overlap.
    /// زۆر is both هاوەڵناو and هاوەڵکار (see Sense's remarks — it is the example the model was
    /// designed around), so adding the seven columns counts it twice and produces a number that is
    /// not the size of anything. Any screen wanting a total has to take it from here.
    /// </summary>
    public int TotalWords { get; set; }

    /// <summary>
    /// DISTINCT words with at least one classified sense. The honest numerator against
    /// <see cref="TotalWords"/>, and always less than the seven columns added together.
    /// </summary>
    public int ClassifiedWords { get; set; }

    public List<TaxonomyPartOfSpeechDto> PartsOfSpeech { get; set; } = new();
    public List<TaxonomyAxisDto> Axes { get; set; } = new();
    public List<TaxonomyAssignmentDto> Assignments { get; set; } = new();
}

public class TaxonomyPartOfSpeechDto
{
    public int Id { get; set; }
    public string NameKu { get; set; } = string.Empty;

    /// <summary>Stable key. Rules match on this, so renaming never touches it.</summary>
    public string Code { get; set; } = string.Empty;

    public int SortOrder { get; set; }
    public bool IsActive { get; set; }

    /// <summary>Live usage, in WORDS. Visible on every row so the long tail is obvious (پڕۆمپت ١١).</summary>
    public int WordCount { get; set; }
}

public class TaxonomyAxisDto
{
    public int Id { get; set; }
    public string NameKu { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; }

    /// <summary>Whether this axis offers an explicit "does not apply" answer.</summary>
    public bool AllowsNotApplicable { get; set; }

    public int ValueCount { get; set; }
    public int UsageCount { get; set; }
}

public class TaxonomyAssignmentDto
{
    public int Id { get; set; }
    public int PartOfSpeechId { get; set; }
    public int AxisId { get; set; }
    public bool IsRequired { get; set; }

    /// <summary>Set when this axis only appears once another axis holds a particular value.</summary>
    public int? RequiresValueId { get; set; }
    public string? RequiresValueName { get; set; }

    /// <summary>Words already answering on this axis — shown before any change to the mapping.</summary>
    public int WordCount { get; set; }
}
