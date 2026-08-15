using backend.Data;
using backend.Data.Models;
using backend.Services;
using backend.Services.Lexicon;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos;

namespace backend.Controllers;

/// <summary>
/// Presence persistence and the sense claim.
///
/// The two live together because they are the same fact: «who is working on this sense» is both
/// the thing shown next to a colleague's name and the thing that stops two people editing one
/// word. Splitting them across two controllers is how they end up disagreeing.
/// </summary>
[ApiController]
[Route("api/presence")]
[Authorize(Roles = Roles.Any)]
public class PresenceController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly ClaimService _claims;
    private readonly ICurrentUser _user;

    public PresenceController(AppDbContext db, ClaimService claims, ICurrentUser user)
    {
        _db = db;
        _claims = claims;
        _user = user;
    }

    /// <summary>
    /// The 60-second flush from the web tier. Upsert, one row per user.
    ///
    /// Machine-to-machine, so it authenticates with a shared key rather than a user token: the
    /// caller is the Blazor app reporting what it observed, and it may be reporting on people who
    /// have already disconnected.
    /// </summary>
    [HttpPost("flush")]
    [AllowAnonymous]
    public async Task<IActionResult> Flush(
        [FromBody] List<UserPresenceDto> rows,
        [FromServices] IConfiguration config,
        CancellationToken ct)
    {
        var expected = config["Internal:ApiKey"];

        // No key configured means the door is closed, not open. A missing secret must never be
        // the same as a matching one.
        if (string.IsNullOrWhiteSpace(expected)) return NotFound();

        if (!Request.Headers.TryGetValue("X-Internal-Key", out var provided) ||
            !System.Security.Cryptography.CryptographicOperations.FixedTimeEquals(
                System.Text.Encoding.UTF8.GetBytes(provided.ToString()),
                System.Text.Encoding.UTF8.GetBytes(expected)))
        {
            return Unauthorized();
        }

        if (rows.Count == 0) return NoContent();

        var ids = rows.Select(r => r.UserId).ToList();

        var existing = await _db.Set<UserPresence>()
            .Where(p => ids.Contains(p.UserId))
            .ToDictionaryAsync(p => p.UserId, ct);

        foreach (var row in rows)
        {
            if (!existing.TryGetValue(row.UserId, out var presence))
            {
                presence = new UserPresence { UserId = row.UserId };
                _db.Add(presence);
            }

            presence.Status = row.Status;
            presence.LastActivityAt = row.LastActivityAt;
            presence.LastSeenAt = row.LastSeenAt;
            presence.CurrentPage = row.CurrentPage;
            presence.CurrentSenseId = row.CurrentSenseId;
            presence.UpdatedAt = DateTime.UtcNow;
        }

        await CreditWorkedTimeAsync(rows, ct);

        await _db.SaveChangesAsync(ct);
        return NoContent();
    }

    /// <summary>
    /// How long the web tier waits between flushes. The credit is capped at a little over this,
    /// so a gap in the heartbeat costs one interval rather than the whole gap.
    /// </summary>
    private static readonly TimeSpan FlushInterval = TimeSpan.FromMinutes(1);

    /// <summary>Ceiling on a single credit. Generous enough to absorb a slow flush, tight enough
    /// that an overnight outage cannot be credited as a night's work.</summary>
    private static readonly TimeSpan MaxCredit = TimeSpan.FromMinutes(3);

    /// <summary>
    /// Adds worked time for everyone reported چالاک.
    ///
    /// چالاک already means "input within the last two minutes" — not "has a tab open" — so this
    /// counts time at the keyboard on whatever screen, and a laptop left open in an empty room
    /// stops counting on its own after two minutes.
    ///
    /// The credit is the gap since that person was last credited, CAPPED. That cap is the whole
    /// safety of the design: without it, an API that was down from Friday to Monday would credit
    /// every user a full weekend of work on the first heartbeat back.
    /// </summary>
    private async Task CreditWorkedTimeAsync(List<UserPresenceDto> rows, CancellationToken ct)
    {
        // 0 is چالاک. Idle and offline are not work — that distinction is the entire reason this
        // is built on presence rather than on "is a circuit open".
        var active = rows.Where(r => r.Status == 0).Select(r => r.UserId).ToList();
        if (active.Count == 0) return;

        var now = DateTime.UtcNow;
        var today = now.Date;

        var open = await _db.UserWorkDays
            .Where(w => active.Contains(w.UserId) && w.Date == today)
            .ToDictionaryAsync(w => w.UserId, ct);

        foreach (var userId in active)
        {
            if (!open.TryGetValue(userId, out var day))
            {
                // First credit of the day. Seeded with one interval rather than zero: the person
                // has demonstrably been working for at least as long as the heartbeat that
                // reported them, and starting at zero would lose the first minute every day.
                _db.UserWorkDays.Add(new UserWorkDay
                {
                    UserId = userId,
                    Date = today,
                    Minutes = (int)FlushInterval.TotalMinutes,
                    LastCreditedAt = now,
                });
                continue;
            }

            var elapsed = now - day.LastCreditedAt;
            if (elapsed <= TimeSpan.Zero) continue;   // clock moved backwards; never subtract

            var credit = elapsed > MaxCredit ? MaxCredit : elapsed;

            // Whole minutes only — the source is a 60-second heartbeat, so anything finer would
            // be precision the measurement does not have. Sub-minute gaps accumulate by leaving
            // LastCreditedAt alone until they add up to one.
            var minutes = (int)credit.TotalMinutes;
            if (minutes <= 0) continue;

            day.Minutes += minutes;
            day.LastCreditedAt = now;
        }
    }

    // GET api/presence/work-time
    //
    // Your OWN worked time, never anyone else's. پڕۆمپت ٧ is explicit that presence must not
    // become a timesheet colleagues can read about each other, and per-person worked minutes is
    // the most timesheet-like number in the system — so this endpoint has no user parameter at
    // all, rather than a parameter with a role check that a later edit could loosen.
    [HttpGet("work-time")]
    [Authorize]
    public async Task<ActionResult<WorkTimeDto>> GetWorkTime(
        [FromQuery] int windowDays = 14, CancellationToken ct = default)
    {
        windowDays = Math.Clamp(windowDays, 1, 180);

        var userId = _user.UserId;
        if (userId is null) return Unauthorized();

        var today = DateTime.UtcNow.Date;
        var since = today.AddDays(-(windowDays - 1));

        var rows = await _db.UserWorkDays.AsNoTracking()
            .Where(w => w.UserId == userId && w.Date >= since)
            .Select(w => new WorkDayDto { Date = w.Date, Minutes = w.Minutes })
            .ToListAsync(ct);

        var days = Enumerable.Range(0, windowDays)
            .Select(offset =>
            {
                var date = since.AddDays(offset);
                return new WorkDayDto
                {
                    Date = date,
                    Minutes = rows.FirstOrDefault(r => r.Date == date)?.Minutes ?? 0,
                };
            })
            .ToList();

        return Ok(new WorkTimeDto
        {
            TodayMinutes = days.LastOrDefault()?.Minutes ?? 0,
            WindowMinutes = days.Sum(d => d.Minutes),
            WindowDays = windowDays,
            ActiveDays = days.Count(d => d.Minutes > 0),
            Days = days,
        });
    }

    /// <summary>
    /// Last-seen for everyone, for the moment the web tier restarts and its memory is empty.
    /// Exact durations and CurrentPage are stripped unless the caller is an Admin — to peers,
    /// presence is a dot and a word, not a timesheet.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<UserPresenceDto>>> GetAll(CancellationToken ct)
    {
        var isAdmin = User.IsInRole(Roles.Admin);

        var rows = await _db.Set<UserPresence>().AsNoTracking().ToListAsync(ct);

        return Ok(rows.Select(p => new UserPresenceDto
        {
            UserId = p.UserId,
            Status = p.Status,
            LastActivityAt = p.LastActivityAt,
            LastSeenAt = p.LastSeenAt,

            // Deliberate: where a colleague is in the app is nobody's business but an admin's.
            CurrentPage = isAdmin ? p.CurrentPage : null,
            CurrentSenseId = p.CurrentSenseId,
        }).ToList());
    }

    // ── The claim lock, which is the same feature ──────────────────────────

    [HttpPost("sense/{senseId:int}/claim")]
    public async Task<ActionResult<SenseClaimDto>> Claim(int senseId, CancellationToken ct)
    {
        if (_user.UserId is not { } userId) return Unauthorized();

        var result = await _claims.ClaimAsync(senseId, userId, ct);

        return Ok(new SenseClaimDto
        {
            Granted = result.Granted,
            HolderName = result.HolderName,
            ExpiresAt = result.ExpiresAt,
        });
    }

    [HttpPost("sense/{senseId:int}/release")]
    public async Task<IActionResult> Release(int senseId, CancellationToken ct)
    {
        if (_user.UserId is not { } userId) return Unauthorized();

        await _claims.ReleaseAsync(senseId, userId, ct);
        return NoContent();
    }

    /// <summary>"Take the next word" — finds an unclaimed sense and claims it in one call.</summary>
    [HttpPost("sense/next")]
    public async Task<ActionResult<int?>> TakeNext(CancellationToken ct)
    {
        if (_user.UserId is not { } userId) return Unauthorized();

        var senseId = await _claims.TakeNextAsync(userId, ct);
        return senseId is null ? NoContent() : Ok(senseId);
    }
}
