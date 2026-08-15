namespace backend.Data.Models;

/// <summary>
/// Minutes actually worked, per person per day.
///
/// This replaces a widget that claimed to show «کاتی کارکردنی ئەمڕۆ» and in fact measured how
/// long its own component had existed — it restarted on every visit to the dashboard and counted
/// nothing at all while somebody was on the words list or the station, which is where the work
/// happens.
///
/// The figure here is accumulated from the presence heartbeat instead. Every 60 seconds the web
/// tier reports who is چالاک — meaning input within the last two minutes, not merely a tab left
/// open — and each of those reports credits the interval since that person was last credited.
/// So it counts time at the keyboard, on whatever screen, and it does not count a laptop left
/// open in an empty room.
///
/// Three properties fall out of that design and are worth stating, because each one is a bug if
/// it is ever lost:
///
///   • It is CAPPED per credit. If the API is down for three hours, the next heartbeat must
///     credit one interval, not three hours.
///   • It is MONOTONIC. Minutes only ever go up, so nothing about a restart or a clock change
///     can retroactively take away work somebody did.
///   • It is keyed by DATE in UTC, matching every other timestamp in the system. A local-day
///     key would double-count the hour a clock goes back.
/// </summary>
public class UserWorkDay
{
    public long Id { get; set; }

    public Guid UserId { get; set; }
    public AppUser? User { get; set; }

    /// <summary>Midnight UTC of the day being counted.</summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Whole minutes. Seconds are deliberately not stored: the source is a 60-second heartbeat,
    /// so anything finer would be precision the measurement does not have.
    /// </summary>
    public int Minutes { get; set; }

    /// <summary>
    /// When this row was last credited. The next credit is the gap since this moment, capped —
    /// which is what makes an outage cost one interval instead of the whole outage.
    /// </summary>
    public DateTime LastCreditedAt { get; set; } = DateTime.UtcNow;
}
