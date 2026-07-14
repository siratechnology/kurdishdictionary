namespace Shared.Dtos;

/// <summary>
/// Sent by the public Next.js site for every tracked interaction. Everything is optional and
/// untrusted — the server fills in IP, geo and the timestamp itself rather than believing the client.
/// </summary>
public class TrackEventDto
{
    public string EventType { get; set; } = string.Empty;
    public string? Path { get; set; }
    public string? SearchTerm { get; set; }
    public int? ResultCount { get; set; }
    public int? WordId { get; set; }
    public string? SessionId { get; set; }
    public string? Referrer { get; set; }
    public int? ScreenWidth { get; set; }
    public int? ScreenHeight { get; set; }
    public int? ViewportWidth { get; set; }
    public int? ViewportHeight { get; set; }
    public string? Language { get; set; }
}

/// <summary>Everything the traffic dashboard renders, in one request.</summary>
public class AnalyticsSummaryDto
{
    public int TotalVisits { get; set; }
    public int VisitsToday { get; set; }
    public int UniqueVisitorsToday { get; set; }
    public int TotalSearches { get; set; }
    public int SearchesToday { get; set; }
    public int ActiveNow { get; set; }

    /// <summary>Searches that returned nothing — the highest-value list here: words worth adding.</summary>
    public int EmptySearches { get; set; }

    public List<DailyCountDto> DailyVisits { get; set; } = new();
    public List<DailyCountDto> DailySearches { get; set; } = new();

    public List<SearchTermDto> TopSearches { get; set; } = new();
    public List<SearchTermDto> FailedSearches { get; set; } = new();

    public List<NameCountDto> TopCountries { get; set; } = new();
    public List<NameCountDto> TopCities { get; set; } = new();
    public List<NameCountDto> Devices { get; set; } = new();
    public List<NameCountDto> Browsers { get; set; } = new();
    public List<NameCountDto> OperatingSystems { get; set; } = new();
    public List<NameCountDto> TopPages { get; set; } = new();
    public List<NameCountDto> TopReferrers { get; set; } = new();

    public List<VisitorEventDto> RecentEvents { get; set; } = new();
}

public class SearchTermDto
{
    public string Term { get; set; } = string.Empty;
    public int Count { get; set; }

    /// <summary>Results the most recent run of this search returned. 0 = the dictionary is missing it.</summary>
    public int? LastResultCount { get; set; }
}

/// <summary>A single visitor interaction, for the live feed.</summary>
public class VisitorEventDto
{
    public long Id { get; set; }
    public string EventType { get; set; } = string.Empty;
    public string? Path { get; set; }
    public string? SearchTerm { get; set; }
    public int? ResultCount { get; set; }
    public int? WordId { get; set; }
    public string? SessionId { get; set; }

    public string? IpAddress { get; set; }
    public string? Country { get; set; }
    public string? CountryCode { get; set; }
    public string? City { get; set; }
    public string? Region { get; set; }
    public string? Isp { get; set; }

    public string? Browser { get; set; }
    public string? Os { get; set; }
    public string? DeviceType { get; set; }
    public int? ScreenWidth { get; set; }
    public int? ScreenHeight { get; set; }
    public string? Language { get; set; }
    public string? Referrer { get; set; }
    public string? UserAgent { get; set; }

    public DateTime CreatedAt { get; set; }
}
