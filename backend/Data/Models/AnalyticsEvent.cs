namespace backend.Data.Models;

/// <summary>
/// One interaction on the public Next.js site: a page view, a search, a word open, etc.
/// Written by anonymous visitors, so everything here is best-effort and must never
/// be trusted for authorization — it exists purely for the traffic dashboard.
/// </summary>
public class AnalyticsEvent
{
    public long Id { get; set; }

    /// <summary>One of <see cref="AnalyticsEventTypes"/>.</summary>
    public string EventType { get; set; } = string.Empty;

    /// <summary>Path on the public site, e.g. "/word/10432".</summary>
    public string? Path { get; set; }

    /// <summary>What the visitor typed, for EventType = "search".</summary>
    public string? SearchTerm { get; set; }

    /// <summary>Number of results the search returned — 0 means we're missing that word.</summary>
    public int? ResultCount { get; set; }

    /// <summary>The word that was opened/shared, when the event is about a specific word.</summary>
    public int? WordId { get; set; }

    /// <summary>Anonymous per-browser id from sessionStorage — groups events into one visit.</summary>
    public string? SessionId { get; set; }

    public string? Referrer { get; set; }

    // ── Where they are ─────────────────────────────────────────────────────
    public string? IpAddress { get; set; }
    public string? Country { get; set; }
    public string? CountryCode { get; set; }
    public string? City { get; set; }
    public string? Region { get; set; }
    public string? Isp { get; set; }
    public string? Timezone { get; set; }

    // ── What they're using ─────────────────────────────────────────────────
    public string? UserAgent { get; set; }
    public string? Browser { get; set; }
    public string? Os { get; set; }
    /// <summary>"Mobile", "Tablet" or "Desktop".</summary>
    public string? DeviceType { get; set; }
    public int? ScreenWidth { get; set; }
    public int? ScreenHeight { get; set; }
    public int? ViewportWidth { get; set; }
    public int? ViewportHeight { get; set; }
    public string? Language { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public static class AnalyticsEventTypes
{
    public const string PageView = "page_view";
    public const string Search = "search";
    public const string WordView = "word_view";
    public const string Share = "share";
}
