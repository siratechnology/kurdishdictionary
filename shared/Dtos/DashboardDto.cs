namespace Shared.Dtos;

public class DashboardDto
{
    public int TotalWords { get; set; }
    public int TotalCategories { get; set; }
    public int TotalRelations { get; set; }
    public int TotalMeanings { get; set; }

    public int WordsWithoutRelations { get; set; }
    public int WordsWithoutMeanings { get; set; }
    public int WordsWithoutCategory { get; set; }
    public int WordsWithoutSpeechPane { get; set; }

    /// <summary>
    /// Words holding a meaning AND a category AND a part of speech — the dial's numerator.
    ///
    /// Sent as its own count because it cannot be derived from the three gaps above: they overlap,
    /// and by an amount only the rows know. Any arithmetic on them gives a bound, not an answer.
    /// </summary>
    public int WordsComplete { get; set; }
    public int WordsAddedLast7Days { get; set; }

    public List<NameCountDto> Genders { get; set; } = new();
    public List<NameCountDto> SpeechPanes { get; set; } = new();
    public List<NameCountDto> Categories { get; set; } = new();
    public List<NameCountDto> RelationTypes { get; set; } = new();
    public List<DailyCountDto> DailyAdded { get; set; } = new();
    public List<RecentWordDto> RecentWords { get; set; } = new();
}

public class NameCountDto
{
    public string Name { get; set; } = string.Empty;
    public int Count { get; set; }
}

public class DailyCountDto
{
    public DateTime Date { get; set; }
    public int Count { get; set; }
}

public class RecentWordDto
{
    public int Id { get; set; }
    public string Kurdish { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string? SpeechPane { get; set; }
    public string? Category { get; set; }
    public int MeaningCount { get; set; }
}
