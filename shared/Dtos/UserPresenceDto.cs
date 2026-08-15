namespace Shared.Dtos;

/// <summary>
/// A presence row as flushed from the Blazor app to the API every 60 seconds.
///
/// The live answer lives in memory on the web tier; this is only what has to survive a restart so
/// «دوایین جار» is still answerable.
/// </summary>
public class UserPresenceDto
{
    public Guid UserId { get; set; }

    /// <summary>0 چالاک · 1 بێ‌چالاکی · 2 دەرچوو</summary>
    public int Status { get; set; }

    public DateTime LastActivityAt { get; set; }
    public DateTime? LastSeenAt { get; set; }

    /// <summary>
    /// Which page they are on. Surfaced ONLY on the users admin page — to peers this is not shown
    /// at all. These are teachers, not agents on a queue.
    /// </summary>
    public string? CurrentPage { get; set; }

    public int? CurrentSenseId { get; set; }
}

/// <summary>The answer to "can I have this sense?" — and if not, who has it.</summary>
public class SenseClaimDto
{
    public bool Granted { get; set; }
    public string? HolderName { get; set; }
    public DateTime? ExpiresAt { get; set; }
}

/// <summary>
/// Time actually worked, accumulated from the presence heartbeat.
///
/// Not "time with the app open": چالاک means input within the last two minutes, so a tab left
/// open in an empty room stops counting on its own.
/// </summary>
public class WorkTimeDto
{
    /// <summary>Minutes today, for the signed-in user.</summary>
    public int TodayMinutes { get; set; }

    /// <summary>Minutes across the trailing window, including today.</summary>
    public int WindowMinutes { get; set; }

    public int WindowDays { get; set; }

    /// <summary>Days in the window with any recorded work — the divisor for the average.</summary>
    public int ActiveDays { get; set; }

    public int AverageMinutesPerActiveDay => ActiveDays == 0 ? 0 : WindowMinutes / ActiveDays;

    /// <summary>Day by day, oldest first.</summary>
    public List<WorkDayDto> Days { get; set; } = new();
}

public class WorkDayDto
{
    public DateTime Date { get; set; }
    public int Minutes { get; set; }
}
