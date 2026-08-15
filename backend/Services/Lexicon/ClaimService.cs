using backend.Data;
using backend.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services.Lexicon;

public record ClaimResult(bool Granted, string? HolderName, DateTime? ExpiresAt);

/// <summary>
/// The 30-minute hold on a sense.
///
/// Concurrency control, and nothing else. Two experts opening the same word at once means one of
/// them loses their work, which is the only problem this solves. Nobody is measured by how long
/// they hold a claim and an expired claim is not a failure — a teacher closing their laptop must
/// never be able to lock a word out of the dictionary.
/// </summary>
public class ClaimService
{
    public static readonly TimeSpan Duration = TimeSpan.FromMinutes(30);

    private readonly AppDbContext _db;

    public ClaimService(AppDbContext db) => _db = db;

    /// <summary>
    /// Takes the claim, or reports who holds it. Re-claiming your own is a no-op that extends it —
    /// coming back to a word you are already working on must never be refused.
    /// </summary>
    public async Task<ClaimResult> ClaimAsync(int senseId, Guid userId, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var existing = await _db.SenseClaims
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.SenseId == senseId && c.ReleasedAt == null, ct);

        if (existing is not null)
        {
            if (existing.UserId == userId)
            {
                existing.ExpiresAt = now.Add(Duration);
                await _db.SaveChangesAsync(ct);
                return new ClaimResult(true, null, existing.ExpiresAt);
            }

            if (existing.ExpiresAt > now)
            {
                // Somebody else has it. Their NAME is shown, not their id — the point is for the
                // second person to go and talk to them, not to file a conflict.
                return new ClaimResult(false, existing.User.FullName ?? existing.User.UserName, existing.ExpiresAt);
            }

            // Expired. Release it so the unique index frees up, then fall through and re-take it.
            existing.ReleasedAt = now;
            await _db.SaveChangesAsync(ct);
        }

        var claim = new SenseClaim
        {
            SenseId = senseId,
            UserId = userId,
            ClaimedAt = now,
            ExpiresAt = now.Add(Duration),
        };

        _db.SenseClaims.Add(claim);

        try
        {
            await _db.SaveChangesAsync(ct);
        }
        catch (DbUpdateException)
        {
            // Two people pressed the button in the same instant and the unique index caught the
            // loser. That is the index doing its job; report the winner rather than throwing.
            _db.Entry(claim).State = EntityState.Detached;

            var winner = await _db.SenseClaims
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.SenseId == senseId && c.ReleasedAt == null, ct);

            return new ClaimResult(false, winner?.User.FullName ?? winner?.User.UserName, winner?.ExpiresAt);
        }

        return new ClaimResult(true, null, claim.ExpiresAt);
    }

    /// <summary>Esc on the station screen. Only the holder can release; anyone else waits for expiry.</summary>
    public async Task ReleaseAsync(int senseId, Guid userId, CancellationToken ct = default)
    {
        var claim = await _db.SenseClaims
            .FirstOrDefaultAsync(c => c.SenseId == senseId && c.UserId == userId && c.ReleasedAt == null, ct);

        if (claim is null) return;

        claim.ReleasedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
    }

    /// <summary>Who holds this sense right now, if anyone.</summary>
    public async Task<SenseClaim?> CurrentHolderAsync(int senseId, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        return await _db.SenseClaims
            .Include(c => c.User)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.SenseId == senseId && c.ReleasedAt == null && c.ExpiresAt > now, ct);
    }

    /// <summary>
    /// The next unclaimed sense in a bucket — "take the next word" on the station screen. Claims it
    /// in the same call so two people pressing the button do not get the same sense.
    /// </summary>
    public async Task<int?> TakeNextAsync(Guid userId, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var candidates = await _db.Senses
            .Where(s => s.WorkflowState == SenseWorkflowState.Raw &&
                        !_db.SenseClaims.Any(c => c.SenseId == s.Id && c.ReleasedAt == null && c.ExpiresAt > now))
            .OrderBy(s => s.Id)
            .Select(s => s.Id)
            .Take(10)
            .ToListAsync(ct);

        // Walk the shortlist rather than taking the first: between the query and the claim someone
        // else may have taken it, and the second-best sense is a perfectly good answer.
        foreach (var senseId in candidates)
        {
            var result = await ClaimAsync(senseId, userId, ct);
            if (result.Granted) return senseId;
        }

        return null;
    }
}
