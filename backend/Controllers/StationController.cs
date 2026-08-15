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
/// The station — walking the dictionary one sense at a time (پڕۆمپت ٧).
/// </summary>
[ApiController]
[Route("api/station")]
[Authorize(Roles = Roles.AdminOrEditor)]
public class StationController : ControllerBase
{
    private readonly StationService _station;
    private readonly ClassificationService _classification;
    private readonly ClaimService _claims;
    private readonly AppDbContext _db;
    private readonly ICurrentUser _user;

    public StationController(
        StationService station, ClassificationService classification,
        ClaimService claims, AppDbContext db, ICurrentUser user)
    {
        _station = station;
        _classification = classification;
        _claims = claims;
        _db = db;
        _user = user;
    }

    /// <summary>The sense at a position in the walk, and how many there are.</summary>
    [HttpGet("at/{position:int}")]
    public async Task<ActionResult<StationSenseDto>> At(
        int position, [FromQuery] bool onlyUnclassified = false, CancellationToken ct = default)
    {
        if (_user.UserId is not { } userId) return Unauthorized();

        var sense = await _station.GetAtAsync(position, userId, onlyUnclassified, ct);
        return sense is null ? NoContent() : Ok(sense);
    }

    /// <summary>
    /// Where this person left off, or 1 if they have never been here.
    ///
    /// Resolved from the stored ANCHOR rather than the stored number: the walk is an index into an
    /// ordering that shifts whenever senses are added, so returning the raw position would put
    /// somebody on a different word than the one they were looking at. The sense id is the place;
    /// the number is re-derived from it.
    /// </summary>
    [HttpGet("resume")]
    public async Task<ActionResult<int>> Resume(CancellationToken ct)
    {
        if (_user.UserId is not { } userId) return Unauthorized();

        var saved = await _db.UserWalkPositions
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.UserId == userId && p.Walk == Walks.Station, ct);

        if (saved?.SenseId is not { } senseId) return Ok(saved?.Position ?? 1);

        var sense = await _station.GetBySenseAsync(senseId, userId, ct);

        // The sense was deleted since. Falling back to the stored number is better than 1: it is
        // still roughly where they were, and being a few rows out beats starting the walk over.
        return Ok(sense?.Position ?? Math.Max(saved.Position, 1));
    }

    /// <summary>
    /// Remembers where this person is. Called on every move, so it is an UPSERT of one row and
    /// deliberately cheap — a walk that only saved on exit would lose the position on a closed
    /// laptop, which is the exact case it exists for.
    /// </summary>
    [HttpPut("resume")]
    public async Task<IActionResult> SaveResume(
        [FromQuery] int position, [FromQuery] int senseId, [FromQuery] int wordId, CancellationToken ct)
    {
        if (_user.UserId is not { } userId) return Unauthorized();

        var row = await _db.UserWalkPositions
            .FirstOrDefaultAsync(p => p.UserId == userId && p.Walk == Walks.Station, ct);

        if (row is null)
        {
            row = new UserWalkPosition { UserId = userId, Walk = Walks.Station };
            _db.UserWalkPositions.Add(row);
        }

        row.Position = Math.Max(position, 1);
        row.SenseId = senseId > 0 ? senseId : null;
        row.WordId = wordId > 0 ? wordId : null;
        row.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync(ct);
        return NoContent();
    }

    [HttpGet("sense/{senseId:int}")]
    public async Task<ActionResult<StationSenseDto>> BySense(int senseId, CancellationToken ct)
    {
        if (_user.UserId is not { } userId) return Unauthorized();

        var sense = await _station.GetBySenseAsync(senseId, userId, ct);
        return sense is null ? NotFound() : Ok(sense);
    }

    /// <summary>The part-of-speech list and the domain tree, for the pickers. Cached by the client.</summary>
    [HttpGet("options")]
    public async Task<ActionResult<object>> Options(CancellationToken ct)
    {
        var parts = await _db.PartsOfSpeech
            .Where(p => p.IsActive)
            .OrderBy(p => p.SortOrder)
            .Select(p => new { p.Id, p.NameKu })
            .ToListAsync(ct);

        var domains = await _db.Domains
            .Where(d => d.IsActive)
            .OrderBy(d => d.ParentId == null ? 0 : 1)
            .ThenBy(d => d.SortOrder)
            .Select(d => new { d.Id, d.NameKu, d.ParentId })
            .ToListAsync(ct);

        return Ok(new { parts, domains });
    }

    [HttpPost("save")]
    public async Task<ActionResult<StationSenseDto>> Save(
        [FromBody] SaveStationSenseDto dto, CancellationToken ct)
    {
        if (_user.UserId is not { } userId) return Unauthorized();

        await _station.SaveAsync(dto, ct);

        // A part-of-speech change can retire whole axes. Their stored answers go with them, each
        // as its own ledger event, so the history reads as a clearing rather than a disappearance.
        await _station.ClearStaleAnswersAsync(dto.SenseId, ct);

        var refreshed = await _station.GetBySenseAsync(dto.SenseId, userId, ct);
        return Ok(refreshed);
    }

    public record SetFeatureRequest(int AxisId, int ValueId);

    [HttpPost("sense/{senseId:int}/feature")]
    public async Task<ActionResult<StationSenseDto>> SetFeature(
        int senseId, [FromBody] SetFeatureRequest body, CancellationToken ct)
    {
        if (_user.UserId is not { } userId) return Unauthorized();

        var change = await _classification.SetFeatureAsync(senseId, body.AxisId, body.ValueId, userId, ct);

        // Answering one axis can open a sub-group and retire another — at any depth, and for a
        // multi-select axis, only under the value that was actually deselected. All of it happens
        // here so the client never has to know the rules.
        await _station.ClearStaleAnswersAsync(senseId, ct);

        var refreshed = await _station.GetBySenseAsync(senseId, userId, ct);

        if (refreshed is not null)
        {
            if (change.Refusal is { } refusal)
                refreshed.Issues.Insert(0, refusal);

            if (change.Disagreement is { } disagreement)
            {
                // Not an error. Both readings stand, and the teacher is told the other one exists.
                refreshed.Issues.Insert(0,
                    $"جیاوازی ڕا: «{disagreement.FirstJudgement}» ← → «{disagreement.SecondJudgement}» — هەردوو ڕا تۆمارکران");
            }
        }

        return Ok(refreshed);
    }

    public record NotApplicableRequest(int AxisId, string Note);

    /// <summary>«ئەم تەوەرە بۆ ئەم وشە کار ناکات» — the escape hatch. Never blocks the save.</summary>
    [HttpPost("sense/{senseId:int}/not-applicable")]
    public async Task<ActionResult<StationSenseDto>> NotApplicable(
        int senseId, [FromBody] NotApplicableRequest body, CancellationToken ct)
    {
        if (_user.UserId is not { } userId) return Unauthorized();

        try
        {
            await _classification.MarkNotApplicableAsync(senseId, body.AxisId, body.Note, userId, ct);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }

        return Ok(await _station.GetBySenseAsync(senseId, userId, ct));
    }

    /// <summary>Submit: Senior publishes, Contributor's work waits in a Senior's queue.</summary>
    [HttpPost("sense/{senseId:int}/submit")]
    public async Task<ActionResult<string>> Submit(int senseId, CancellationToken ct)
    {
        if (_user.UserId is not { } userId) return Unauthorized();

        var state = await _classification.SubmitAsync(senseId, userId, ct);
        return Ok(state.ToString());
    }

    [HttpPost("sense/{senseId:int}/release")]
    public async Task<IActionResult> Release(int senseId, CancellationToken ct)
    {
        if (_user.UserId is not { } userId) return Unauthorized();

        await _claims.ReleaseAsync(senseId, userId, ct);
        return NoContent();
    }
}
