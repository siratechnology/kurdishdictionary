namespace backend.Data.Models;

/// <summary>
/// The durable part of presence (پڕۆمپت ٩).
///
/// Live status is held in memory on the web tier and pushed here every 60 seconds — writing on
/// every heartbeat would be an UPDATE per user per 30 seconds, forever, to record a mouse move.
/// What this table is actually for is answering «دوایین جار X پێش ئێستا» after a restart, which
/// memory cannot do.
/// </summary>
public class UserPresence
{
    /// <summary>The user IS the key — one presence row per person, updated in place.</summary>
    public Guid UserId { get; set; }
    public AppUser User { get; set; } = null!;

    /// <summary>0 چالاک · 1 بێ‌چالاکی · 2 دەرچوو</summary>
    public int Status { get; set; }

    public DateTime LastActivityAt { get; set; }
    public DateTime? LastSeenAt { get; set; }

    public string? CurrentPage { get; set; }
    public int? CurrentSenseId { get; set; }

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
