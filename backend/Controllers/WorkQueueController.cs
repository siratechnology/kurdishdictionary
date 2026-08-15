using backend.Data.Models;
using backend.Services.Lexicon;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;

namespace backend.Controllers;

/// <summary>
/// ڕیزی کار — what is incomplete, and where to go to fix it.
///
/// Signed-in only: this is an editorial view of the dictionary's gaps, not something the public
/// site needs. Any role may read it, because knowing what is left to do is not a privilege.
/// </summary>
[ApiController]
[Route("api/work-queue")]
[Authorize(Roles = Roles.Any)]
public class WorkQueueController : ControllerBase
{
    private readonly WorkQueueService _queue;
    private readonly LexiconValidator _validator;

    public WorkQueueController(WorkQueueService queue, LexiconValidator validator)
    {
        _queue = queue;
        _validator = validator;
    }

    /// <summary>Counts per bucket. This is what the dashboard shows.</summary>
    [HttpGet]
    public async Task<ActionResult<WorkQueueDto>> GetSummary(CancellationToken ct) =>
        Ok(await _queue.GetSummaryAsync(ct));

    /// <summary>The rows in one bucket, each deep-linked to the field that needs filling.</summary>
    [HttpGet("{bucket}")]
    public async Task<ActionResult<List<WorkQueueItemDto>>> GetItems(
        WorkQueueBucket bucket, [FromQuery] int take = 50, CancellationToken ct = default)
    {
        if (!Enum.IsDefined(bucket)) return NotFound("Unknown bucket.");
        return Ok(await _queue.GetItemsAsync(bucket, Math.Clamp(take, 1, 200), ct));
    }

    /// <summary>
    /// Everything wrong with one sense. Used by the entry form to show its own gaps inline, so a
    /// teacher sees them while typing rather than discovering them on a queue page later.
    /// </summary>
    [HttpGet("sense/{senseId:int}/issues")]
    public async Task<ActionResult<IReadOnlyList<LexiconIssue>>> GetSenseIssues(int senseId, CancellationToken ct) =>
        Ok((await _validator.ValidateSenseAsync(senseId, ct)).Issues);

    [HttpGet("word/{wordId:int}/issues")]
    public async Task<ActionResult<IReadOnlyList<LexiconIssue>>> GetWordIssues(int wordId, CancellationToken ct) =>
        Ok((await _validator.ValidateWordAsync(wordId, ct)).Issues);
}
