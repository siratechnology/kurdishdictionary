namespace Shared.Dtos;

/// <summary>
/// How fast the dictionary is actually being finished, and when it will be.
///
/// The completeness dial answers "how much is done". This answers the question people ask
/// straight afterwards and could not previously get: at the rate we are working, how long is
/// left? A percentage that moves half a point a week and a percentage that moves half a point a
/// day look identical on a dial.
///
/// Everything here is measured, never configured. Pace comes from what was actually recorded in
/// the ledger and the audit log over the trailing window, so a quiet fortnight makes the estimate
/// longer on its own rather than needing someone to remember to change a target.
/// </summary>
public class PaceDto
{
    // ── The finish line ─────────────────────────────────────────────────────
    public int TotalWords { get; set; }

    /// <summary>Words that still need work — the same definition the completeness dial uses.</summary>
    public int RemainingWords { get; set; }

    public int CompletedWords => TotalWords - RemainingWords;

    // ── The rate ────────────────────────────────────────────────────────────

    /// <summary>New words in the trailing window.</summary>
    public int AddedInWindow { get; set; }

    /// <summary>Edits — including station answers — in the trailing window.</summary>
    public int EditedInWindow { get; set; }

    /// <summary>Length of the trailing window, in days.</summary>
    public int WindowDays { get; set; }

    /// <summary>
    /// Days in the window on which ANYTHING happened.
    ///
    /// The divisor is this, not WindowDays. Dividing by calendar days would tell a team that
    /// works Saturday to Wednesday that they are 30% slower than they are, and the estimate they
    /// were given would then never be met on a working day.
    /// </summary>
    public int ActiveDays { get; set; }

    /// <summary>Words finished per active day, measured. Zero when nothing has been recorded.</summary>
    public double WordsPerActiveDay { get; set; }

    /// <summary>Work items — adds plus edits — per active day.</summary>
    public double ItemsPerActiveDay { get; set; }

    // ── The projection ──────────────────────────────────────────────────────

    /// <summary>Working days left at the measured pace. Null when the pace is zero.</summary>
    public double? ProjectedDays { get; set; }

    /// <summary>
    /// The same projection in hours, for when the end is close enough that days are the wrong
    /// unit. Assumes <see cref="HoursPerDay"/> of work in an active day.
    /// </summary>
    public double? ProjectedHours { get; set; }

    /// <summary>The working day this estimate assumes. Stated so the number can be argued with.</summary>
    public double HoursPerDay { get; set; }

    /// <summary>Calendar date the work lands on, projected across active days only.</summary>
    public DateTime? ProjectedFinish { get; set; }

    // ── The shape of it ─────────────────────────────────────────────────────

    /// <summary>Day by day across the window, oldest first — the sparkline under the gauge.</summary>
    public List<PaceDayDto> Days { get; set; } = new();

    /// <summary>Who did it, over the same window.</summary>
    public List<PaceContributorDto> Contributors { get; set; } = new();
}

public class PaceDayDto
{
    public DateTime Date { get; set; }
    public int Added { get; set; }
    public int Edited { get; set; }
    public int Total => Added + Edited;
}

public class PaceContributorDto
{
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string? FullName { get; set; }
    public string? AvatarUrl { get; set; }

    /// <summary>Words this person was the FIRST to add. Never reassigned by a later edit.</summary>
    public int Added { get; set; }

    public int Edited { get; set; }

    public int Total => Added + Edited;
}
