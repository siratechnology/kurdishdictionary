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
/// The taxonomy settings area (پڕۆمپت ١١).
///
/// The three tiers are enforced HERE, at the endpoint, not in the Blazor UI. A permission that
/// lives only in a component is not a permission — it is a hidden button, and the API underneath
/// it is open to anyone with a token.
/// </summary>
[ApiController]
[Route("api/taxonomy-admin")]
public class TaxonomyAdminController : ControllerBase
{
    private readonly TaxonomyAdminService _admin;
    private readonly TaxonomyTreeService _tree;
    private readonly OptionsTreeService _options;
    private readonly MergeService _merge;
    private readonly PartOfSpeechReassignService _reassign;
    private readonly AppDbContext _db;
    private readonly ICurrentUser _user;

    public TaxonomyAdminController(
        TaxonomyAdminService admin, TaxonomyTreeService tree, OptionsTreeService options,
        MergeService merge, PartOfSpeechReassignService reassign, AppDbContext db, ICurrentUser user)
    {
        _admin = admin;
        _tree = tree;
        _options = options;
        _merge = merge;
        _reassign = reassign;
        _db = db;
        _user = user;
    }

    // ═══════════════════════════════════════════════════════════════════════
    // The options tree — بەشی ئاخاوتن ← تەوەر ← نرخ ← تەوەر ← …
    //
    // Same rows as the flat screen, presented as the tree they are. No new tables, no migration:
    // PartOfSpeechAxis.RequiresValueId IS the parent link, and chaining it gives unlimited depth.
    // ═══════════════════════════════════════════════════════════════════════

    [HttpGet("tree/{partOfSpeechId:int}")]
    [Authorize(Roles = Roles.Any)]
    public async Task<ActionResult<TaxonomyTreeDto>> Tree(int partOfSpeechId, CancellationToken ct)
    {
        var tree = await _tree.GetTreeAsync(partOfSpeechId, ct);
        return tree is null ? NotFound() : Ok(tree);
    }

    /// <summary>
    /// The ACTUAL entry form the current tree produces, resolved against a hypothetical set of
    /// answers. The settings screen renders this beside the tree so nobody configures blind and finds
    /// out after two hundred words.
    ///
    /// It goes through <see cref="OptionsTreeService"/> — the same resolution the real form uses —
    /// rather than reimplementing the cascade in the preview, which would make the preview a
    /// confident lie the moment the rule changed.
    /// </summary>
    [HttpPost("tree/{partOfSpeechId:int}/preview")]
    [Authorize(Roles = Roles.Any)]
    public async Task<ActionResult<List<StationAxisDto>>> PreviewForm(
        int partOfSpeechId, [FromBody] List<int>? selectedValueIds, CancellationToken ct)
    {
        var held = (selectedValueIds ?? new List<int>()).ToHashSet();
        var tree = await _options.GetAsync(partOfSpeechId, ct);

        // The preview has no sense, so nothing is "answered but retired": a deactivated group is
        // simply absent, which is what a NEW entry would see. That is the state being previewed.
        var resolved = tree.Resolve(held, answeredAxisIds: null);

        var dtos = resolved.Select(node =>
        {
            var axis = node.Axis;

            return new StationAxisDto
            {
                AxisId = axis.AxisId,
                Name = axis.Name,
                Prompt = axis.Prompt,
                Description = axis.Description,
                IsRequired = axis.IsRequired,
                AllowsNotApplicable = axis.AllowsNotApplicable,
                MinSelections = axis.MinSelections,
                MaxSelections = axis.MaxSelections,
                SelectedValueIds = axis.Values.Where(v => held.Contains(v.Id)).Select(v => v.Id).ToList(),
                Depth = node.Depth,
                ParentValueId = node.ParentValue?.Id,
                ParentValueName = node.ParentValue?.Name,
                Values = axis.Values
                    .Where(v => v.IsActive)
                    .Select((v, i) => new StationValueDto
                    {
                        ValueId = v.Id,
                        Name = v.Name,
                        Hint = v.Hint,
                        Digit = i < 9 ? i + 1 : 0,
                        OpensChildGroup = v.OpensChildGroup,
                    })
                    .ToList(),
            };
        }).ToList();

        return Ok(dtos);
    }

    public record AddGroupRequest(int PartOfSpeechId, int? ParentValueId, string Name);

    /// <summary>
    /// «بژاردەی زیاتر زیاد بکە» — the one button, at any depth. Tier 2: the linguistic owner.
    /// </summary>
    [HttpPost("tree/groups")]
    [Authorize(Roles = Roles.LinguisticOwner)]
    public async Task<ActionResult<int>> AddGroup([FromBody] AddGroupRequest body, CancellationToken ct)
    {
        try
        {
            return Ok(await _tree.AddChildAxisAsync(body.PartOfSpeechId, body.ParentValueId, body.Name, ct));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    public record PromptRequest(string? Prompt);

    /// <summary>
    /// The plain-language question a teacher reads above this dropdown.
    ///
    /// Tier 1 (any admin), unlike the structural operations around it. Rewriting «جۆری کار» as
    /// «کارەکە چ جۆرێکە؟» changes not one thing about the grammar the tree encodes — it is editorial
    /// copy, and gating it behind the single linguistic owner is what leaves every dropdown reading
    /// like a schema field for another six months.
    /// </summary>
    [HttpPut("tree/axes/{axisId:int}/prompt")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> SetPrompt(
        int axisId, [FromBody] PromptRequest body, CancellationToken ct)
    {
        await _tree.SetPromptAsync(axisId, body.Prompt, ct);
        return NoContent();
    }

    public record OptionHintRequest(string? Hint);

    /// <summary>The muted worked example beside one option. Editorial copy, same tier as the prompt.</summary>
    [HttpPut("values/{valueId:int}/hint")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> SetOptionHint(
        int valueId, [FromBody] OptionHintRequest body, CancellationToken ct)
    {
        await _tree.SetOptionHintAsync(valueId, body.Hint, ct);
        return NoContent();
    }

    public record SelectionModeRequest(int MinSelections, int? MaxSelections);

    [HttpGet("tree/axes/{axisId:int}/selection-cap")]
    [Authorize(Roles = Roles.Any)]
    public async Task<ActionResult<SelectionCapPreviewDto>> PreviewSelectionCap(
        int axisId, [FromQuery] int? max, CancellationToken ct) =>
        Ok(await _tree.PreviewSelectionCapAsync(axisId, max, ct));

    /// <summary>«چەند بژاردە دەکرێت هەڵبژێردرێت؟» — تەنها یەک · هەرچەند بێت · تا ژمارەیەکی دیاریکراو.</summary>
    [HttpPut("tree/axes/{axisId:int}/selection-mode")]
    [Authorize(Roles = Roles.LinguisticOwner)]
    public async Task<IActionResult> SetSelectionMode(
        int axisId, [FromBody] SelectionModeRequest body, CancellationToken ct)
    {
        try
        {
            await _tree.SetSelectionModeAsync(axisId, body.MinSelections, body.MaxSelections, ct);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    public record ReorderSiblingsRequest(int PartOfSpeechId, int? ParentValueId, List<int> OrderedAssignmentIds);

    [HttpPut("tree/order")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> ReorderSiblings(
        [FromBody] ReorderSiblingsRequest body, CancellationToken ct)
    {
        await _tree.ReorderSiblingsAsync(
            body.PartOfSpeechId, body.ParentValueId, body.OrderedAssignmentIds, ct);
        return NoContent();
    }

    [HttpPut("tree/axes/{axisId:int}/active")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> SetAxisActive(int axisId, [FromQuery] bool active, CancellationToken ct)
    {
        await _tree.SetAxisActiveAsync(axisId, active, ct);
        return NoContent();
    }

    [HttpDelete("tree/{partOfSpeechId:int}/axes/{axisId:int}")]
    [Authorize(Roles = Roles.LinguisticOwner)]
    public async Task<IActionResult> RemoveGroup(int partOfSpeechId, int axisId, CancellationToken ct)
    {
        try
        {
            await _tree.RemoveAxisAsync(partOfSpeechId, axisId, ct);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    public record ReparentRequest(int? ParentValueId);

    [HttpPut("tree/assignments/{assignmentId:int}/parent")]
    [Authorize(Roles = Roles.LinguisticOwner)]
    public async Task<IActionResult> Reparent(
        int assignmentId, [FromBody] ReparentRequest body, CancellationToken ct)
    {
        try
        {
            await _tree.ReparentAsync(assignmentId, body.ParentValueId, ct);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // ── The whole taxonomy, for the settings screen ────────────────────────

    /// <summary>
    /// Parts of speech, axes and the assignment matrix in one call. The settings screen redraws
    /// after every edit and a screen that needs four round trips to repaint feels broken.
    /// </summary>
    [HttpGet("overview")]
    [Authorize(Roles = Roles.Any)]
    public async Task<ActionResult<TaxonomyOverviewDto>> Overview(CancellationToken ct)
    {
        // One row per distinct headword — the dedupe every other screen applies. Declared here so
        // the counts below are in the same unit as the word list and cannot drift from it.
        var wordIds = _db.Words.AsNoTracking().GroupBy(w => w.Kurdish).Select(g => g.Min(w => w.Id));

        var parts = await _db.PartsOfSpeech
            .AsNoTracking()
            .OrderBy(p => p.SortOrder)
            .Select(p => new TaxonomyPartOfSpeechDto
            {
                Id = p.Id,
                NameKu = p.NameKu,
                Code = p.Code,
                SortOrder = p.SortOrder,
                IsActive = p.IsActive,
                // Distinct WORDS, not senses: a word with three senses under کار is one verb.
                WordCount = _db.Senses.Where(s => s.PartOfSpeechId == p.Id && wordIds.Contains(s.WordId))
                                      .Select(s => s.WordId).Distinct().Count(),
            })
            .ToListAsync(ct);

        var axes = await _db.FeatureAxes
            .AsNoTracking()
            .OrderBy(a => a.SortOrder)
            .Select(a => new TaxonomyAxisDto
            {
                Id = a.Id,
                NameKu = a.NameKu,
                Code = a.Code,
                Description = a.Description,
                SortOrder = a.SortOrder,
                IsActive = a.IsActive,
                AllowsNotApplicable = a.AllowsNotApplicable,
                ValueCount = a.Values.Count(v => !v.IsDeleted),
                UsageCount = _db.SenseFeatures.Where(f => f.AxisId == a.Id && wordIds.Contains(f.Sense.WordId))
                                              .Select(f => f.Sense.WordId).Distinct().Count(),
            })
            .ToListAsync(ct);

        var assignments = await _db.PartOfSpeechAxes
            .AsNoTracking()
            .Select(a => new TaxonomyAssignmentDto
            {
                Id = a.Id,
                PartOfSpeechId = a.PartOfSpeechId,
                AxisId = a.AxisId,
                IsRequired = a.IsRequired,
                RequiresValueId = a.RequiresValueId,
                RequiresValueName = a.RequiresValue == null ? null : a.RequiresValue.NameKu,
                WordCount = _db.Senses.Where(s => s.PartOfSpeechId == a.PartOfSpeechId
                                               && s.Features.Any(f => f.AxisId == a.AxisId)
                                               && wordIds.Contains(s.WordId))
                                      .Select(s => s.WordId).Distinct().Count(),
            })
            .ToListAsync(ct);

        // Distinct, not the sum of `parts`. A word carrying two parts of speech appears in two of
        // those columns and must still be one word here.
        var classified = await _db.Senses
            .Where(s => s.PartOfSpeechId != null && wordIds.Contains(s.WordId))
            .Select(s => s.WordId)
            .Distinct()
            .CountAsync(ct);

        return Ok(new TaxonomyOverviewDto
        {
            TotalWords = await wordIds.CountAsync(ct),
            ClassifiedWords = classified,
            PartsOfSpeech = parts,
            Axes = axes,
            Assignments = assignments,
        });
    }

    public record AssignRequest(int PartOfSpeechId, int AxisId, bool Assigned, bool IsRequired);

    /// <summary>
    /// Assigns or unassigns an axis. Unassigning is refused while senses hold answers on it —
    /// the answers would become unreachable rather than deleted, which is worse than either.
    /// </summary>
    [HttpPost("assignments")]
    [Authorize(Roles = Roles.LinguisticOwner)]
    public async Task<IActionResult> SetAssignment([FromBody] AssignRequest body, CancellationToken ct)
    {
        var existing = await _db.PartOfSpeechAxes.FirstOrDefaultAsync(
            a => a.PartOfSpeechId == body.PartOfSpeechId && a.AxisId == body.AxisId, ct);

        if (!body.Assigned)
        {
            if (existing is null) return NoContent();

            var affected = await _admin.CountSensesAffectedByAssignmentAsync(
                body.PartOfSpeechId, body.AxisId, ct);

            if (affected > 0)
                return BadRequest($"{affected} مانا وەڵامیان لەم تەوەرەدا هەیە — سەرەتا ئەوان پاک بکەرەوە.");

            _db.PartOfSpeechAxes.Remove(existing);
            await _db.SaveChangesAsync(ct);
            return NoContent();
        }

        if (existing is null)
        {
            _db.PartOfSpeechAxes.Add(new PartOfSpeechAxis
            {
                PartOfSpeechId = body.PartOfSpeechId,
                AxisId = body.AxisId,
                IsRequired = body.IsRequired,
                SortOrder = await _db.PartOfSpeechAxes.CountAsync(a => a.PartOfSpeechId == body.PartOfSpeechId, ct) + 1,
            });
        }
        else
        {
            existing.IsRequired = body.IsRequired;
        }

        await _db.SaveChangesAsync(ct);
        return NoContent();
    }

    // ── Tier 1: any admin ──────────────────────────────────────────────────

    [HttpGet("axes/{axisId:int}/values")]
    [Authorize(Roles = Roles.Any)]
    public async Task<ActionResult<List<TaxonomyValueDto>>> GetValues(int axisId, CancellationToken ct) =>
        Ok(await _admin.GetValuesAsync(axisId, ct));

    [HttpGet("values/unused")]
    [Authorize(Roles = Roles.Any)]
    public async Task<ActionResult<List<TaxonomyValueDto>>> GetUnused(CancellationToken ct) =>
        Ok(await _admin.GetUnusedValuesAsync(ct));

    public record RenameRequest(string Name, string? Description);

    [HttpPut("values/{valueId:int}/name")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> RenameValue(int valueId, [FromBody] RenameRequest body, CancellationToken ct)
    {
        await _admin.RenameValueAsync(valueId, body.Name, ct);
        return NoContent();
    }

    [HttpPut("axes/{axisId:int}/name")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> RenameAxis(int axisId, [FromBody] RenameRequest body, CancellationToken ct)
    {
        await _admin.RenameAxisAsync(axisId, body.Name, body.Description, ct);
        return NoContent();
    }

    [HttpPut("parts-of-speech/{id:int}/name")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> RenamePartOfSpeech(int id, [FromBody] RenameRequest body, CancellationToken ct)
    {
        await _admin.RenamePartOfSpeechAsync(id, body.Name, body.Description, ct);
        return NoContent();
    }

    [HttpPut("values/order")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Reorder([FromBody] List<int> orderedIds, CancellationToken ct)
    {
        await _admin.ReorderValuesAsync(orderedIds, ct);
        return NoContent();
    }

    [HttpPut("values/{valueId:int}/active")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> SetActive(int valueId, [FromQuery] bool active, CancellationToken ct)
    {
        await _admin.SetValueActiveAsync(valueId, active, ct);
        return NoContent();
    }

    [HttpPost("values/bulk-deactivate")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<ActionResult<int>> BulkDeactivate([FromBody] List<int> valueIds, CancellationToken ct) =>
        Ok(await _admin.BulkDeactivateAsync(valueIds, ct));

    // ── Tier 2: the linguistic owner ───────────────────────────────────────

    public record AddValueRequest(int AxisId, string Name, string? Code);

    [HttpPost("values")]
    [Authorize(Roles = Roles.LinguisticOwner)]
    public async Task<ActionResult<int>> AddValue([FromBody] AddValueRequest body, CancellationToken ct)
    {
        var value = new FeatureValue
        {
            AxisId = body.AxisId,
            NameKu = body.Name.Trim(),
            Code = string.IsNullOrWhiteSpace(body.Code) ? null : body.Code.Trim(),
            SortOrder = await _db.FeatureValues.CountAsync(v => v.AxisId == body.AxisId, ct) + 1,
        };

        _db.FeatureValues.Add(value);
        await _db.SaveChangesAsync(ct);
        return Ok(value.Id);
    }

    [HttpDelete("values/{valueId:int}")]
    [Authorize(Roles = Roles.LinguisticOwner)]
    public async Task<IActionResult> DeleteValue(int valueId, CancellationToken ct)
    {
        await _admin.DeleteValueAsync(valueId, ct);
        return NoContent();
    }

    [HttpGet("merge/preview")]
    [Authorize(Roles = Roles.LinguisticOwner)]
    public async Task<ActionResult<MergePreviewDto>> PreviewMerge(
        [FromQuery] int source, [FromQuery] int target, CancellationToken ct) =>
        Ok(await _merge.PreviewAsync(source, target, ct));

    public record MergeRequest(int SourceValueId, int TargetValueId, string? Reason);

    [HttpPost("merge")]
    [Authorize(Roles = Roles.LinguisticOwner)]
    public async Task<ActionResult<int>> Merge([FromBody] MergeRequest body, CancellationToken ct)
    {
        if (_user.UserId is not { } userId) return Unauthorized();

        var moved = await _merge.ExecuteAsync(body.SourceValueId, body.TargetValueId, userId, body.Reason, ct);
        return Ok(moved);
    }

    [HttpPost("merge/{sourceValueId:int}/undo")]
    [Authorize(Roles = Roles.LinguisticOwner)]
    public async Task<ActionResult<int>> UndoMerge(int sourceValueId, CancellationToken ct)
    {
        if (_user.UserId is not { } userId) return Unauthorized();
        return Ok(await _merge.UndoAsync(sourceValueId, userId, ct));
    }

    // ═══════════════════════════════════════════════════════════════════════
    // Bulk part-of-speech re-assignment
    //
    // The counterpart to AddPartOfSpeech below. Creating one takes a sentence; filling it took the
    // station and two and a half thousand keystrokes, so a new بەشی ئاخاوتن sat at ٠ وشە and the
    // taxonomy could be corrected while the data could not.
    //
    // Every one of these answers InvalidOperationException as a 400 carrying the message rather
    // than letting it escape as a 500 — the service's refusals are written for the person reading
    // the screen, and a 500 has no body to put them in.
    // ═══════════════════════════════════════════════════════════════════════

    [HttpGet("parts-of-speech/reassign/preview")]
    [Authorize(Roles = Roles.LinguisticOwner)]
    public async Task<ActionResult<PartOfSpeechReassignPreviewDto>> PreviewReassign(
        [FromQuery] int from, [FromQuery] int to, CancellationToken ct)
    {
        try
        {
            return Ok(await _reassign.PreviewAsync(from, to, ct));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("parts-of-speech/reassign/recent")]
    [Authorize(Roles = Roles.LinguisticOwner)]
    public async Task<ActionResult<List<PartOfSpeechReassignRunDto>>> RecentReassignments(
        CancellationToken ct) =>
        Ok(await _reassign.RecentAsync(ct));

    public record ReassignRequest(int FromId, int ToId, string Reason);

    [HttpPost("parts-of-speech/reassign")]
    [Authorize(Roles = Roles.LinguisticOwner)]
    public async Task<ActionResult<PartOfSpeechReassignResultDto>> Reassign(
        [FromBody] ReassignRequest body, CancellationToken ct)
    {
        if (_user.UserId is not { } userId) return Unauthorized();

        try
        {
            return Ok(await _reassign.ExecuteAsync(body.FromId, body.ToId, userId, body.Reason, ct));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("parts-of-speech/reassign/undo")]
    [Authorize(Roles = Roles.LinguisticOwner)]
    public async Task<ActionResult<PartOfSpeechReassignResultDto>> UndoReassign(
        [FromQuery] int from, [FromQuery] DateTime at, CancellationToken ct)
    {
        if (_user.UserId is not { } userId) return Unauthorized();

        try
        {
            return Ok(await _reassign.UndoAsync(from, at, userId, ct));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    public record ConditionRequest(int? RequiresValueId);

    [HttpPut("assignments/{assignmentId:int}/condition")]
    [Authorize(Roles = Roles.LinguisticOwner)]
    public async Task<IActionResult> SetCondition(
        int assignmentId, [FromBody] ConditionRequest body, CancellationToken ct)
    {
        await _admin.SetConditionAsync(assignmentId, body.RequiresValueId, ct);
        return NoContent();
    }

    // ── Tier 3: elevated role AND a written reason ─────────────────────────
    //
    // A part of speech is a linguistic claim, not a data-entry convenience. The seven are closed by
    // default and opening that list must feel deliberate — so it costs a role AND a sentence, and
    // the sentence is stored on the ledger event where it can be read next year.

    public record AddAxisRequest(string Name, string? Code, string Reason);

    [HttpPost("axes")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<ActionResult<int>> AddAxis([FromBody] AddAxisRequest body, CancellationToken ct)
    {
        if (_user.UserId is not { } userId) return Unauthorized();

        if (string.IsNullOrWhiteSpace(body.Reason))
            return BadRequest("هۆکارێک بنووسە بۆ زیادکردنی تەوەرێکی نوێ.");

        var axis = new FeatureAxis
        {
            NameKu = body.Name.Trim(),
            Code = string.IsNullOrWhiteSpace(body.Code) ? Guid.NewGuid().ToString("N")[..8] : body.Code.Trim(),
            SortOrder = await _db.FeatureAxes.CountAsync(ct) + 1,
        };

        _db.FeatureAxes.Add(axis);
        await _db.SaveChangesAsync(ct);

        _db.ContributionEvents.Add(new ContributionEvent
        {
            UserId = userId,
            EventType = ContributionEventType.FeatureChanged,
            EntityType = nameof(FeatureAxis),
            EntityId = axis.Id,
            FieldName = "AxisAdded",
            NewValue = axis.NameKu,
            Note = body.Reason.Trim(),
        });

        await _db.SaveChangesAsync(ct);
        return Ok(axis.Id);
    }

    public record AddPartOfSpeechRequest(string Name, string Code, string Reason);

    [HttpPost("parts-of-speech")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<ActionResult<int>> AddPartOfSpeech(
        [FromBody] AddPartOfSpeechRequest body, CancellationToken ct)
    {
        if (_user.UserId is not { } userId) return Unauthorized();

        if (string.IsNullOrWhiteSpace(body.Reason))
            return BadRequest("بەشی ئاخاوتن بڕیارێکی زمانەوانییە — هۆکارەکەی بنووسە.");

        var pos = new PartOfSpeech
        {
            NameKu = body.Name.Trim(),
            Code = body.Code.Trim(),
            SortOrder = await _db.PartsOfSpeech.CountAsync(ct) + 1,
        };

        _db.PartsOfSpeech.Add(pos);
        await _db.SaveChangesAsync(ct);

        _db.ContributionEvents.Add(new ContributionEvent
        {
            UserId = userId,
            EventType = ContributionEventType.FeatureChanged,
            EntityType = nameof(PartOfSpeech),
            EntityId = pos.Id,
            FieldName = "PartOfSpeechAdded",
            NewValue = pos.NameKu,
            Note = body.Reason.Trim(),
        });

        await _db.SaveChangesAsync(ct);
        return Ok(pos.Id);
    }
}
