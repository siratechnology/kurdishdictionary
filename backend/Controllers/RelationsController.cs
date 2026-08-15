using backend.Data;
using backend.Data.Models;
using backend.Services.Lexicon;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos;
using Shared.Text;

namespace backend.Controllers;

/// <summary>
/// The relation workspace — پەیوەندییەکان.
///
/// Speaks the v3 model (WordRelation / SenseRelation over the eleven seeded RelationTypeDef rows),
/// not the legacy string-typed RelatedWord table the old screen wrote. That table held zero rows
/// across all 2,967 words, so nothing was migrated and nothing was lost; what it bought is the
/// grammar — ڕەگ paired with داڕێژراو لێی, چاوگی کارەکە with کاری چاوگەکە — instead of seven
/// free-text strings that no inverse rule could act on.
///
/// Reads are open (the public dictionary draws the graph); every write needs an Editor or Admin.
/// Attribution and the audit trail are written by the interceptors, so nothing here logs by hand.
/// </summary>
[ApiController]
[Route("api/relations")]
public class RelationsController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly RelationService _relations;

    public RelationsController(AppDbContext db, RelationService relations)
    {
        _db = db;
        _relations = relations;
    }

    /// <summary>
    /// The words this walk steps through, in a STABLE order.
    ///
    /// Same dedupe-by-spelling rule as the words list (one row per distinct headword, lowest id),
    /// so «٤٥ لە ٢٬٩٦٧» agrees with the count every other screen prints. Ordering by Kurdish rather
    /// than by "needs work" is deliberate: a queue that reorders itself as you work means position
    /// ٤٥ is a different word each time you look, and you can never tell whether you have been
    /// through a letter already.
    /// </summary>
    private IQueryable<Word> WalkQuery()
    {
        var minIds = _db.Words.AsNoTracking()
            .GroupBy(w => w.Kurdish)
            .Select(g => g.Min(w => w.Id));

        return _db.Words.AsNoTracking().Where(w => minIds.Contains(w.Id));
    }

    // ═══════════════════════════════════════════════════════════════════════
    // Reference data
    // ═══════════════════════════════════════════════════════════════════════

    /// <summary>The eleven types, with the label the FAR side will read once an edge is saved.</summary>
    [HttpGet("types")]
    public async Task<ActionResult<List<RelationTypeDto>>> Types(CancellationToken ct)
    {
        var rows = await _db.RelationTypes
            .AsNoTracking()
            .Include(t => t.Inverse)
            .Where(t => t.IsActive)
            .OrderBy(t => t.SortOrder)
            .ToListAsync(ct);

        return Ok(rows.Select(t => new RelationTypeDto
        {
            Id = t.Id,
            Code = t.Code,
            NameKu = t.NameKu,
            Scope = t.Scope == RelationScope.Word ? "word" : "sense",
            IsSymmetric = t.IsSymmetric,
            InverseNameKu = t.IsSymmetric ? null : t.Inverse?.NameKu,
            SortOrder = t.SortOrder,
        }).ToList());
    }

    /// <summary>
    /// The real totals behind the page header.
    ///
    /// The screen this replaces counted the twenty-five rows its grid happened to be holding and
    /// printed that as the state of the dictionary. These are four server-side counts over the
    /// whole table, which is the only kind of summary worth showing.
    /// </summary>
    [HttpGet("coverage")]
    public async Task<ActionResult<RelationCoverageDto>> Coverage(CancellationToken ct)
    {
        var total = await WalkQuery().CountAsync(ct);

        // "Has a relation" means either scope: a word wired only through one of its senses is
        // still wired, and counting it as untouched would send someone back to redo it.
        //
        // Sense edges are reached by subquery rather than by a navigation property, because Sense
        // deliberately carries no relation collections — SenseRelation points at it twice (from
        // and to) and a pair of inferred collections there is exactly the ambiguity EF cannot map.
        var withRelations = await WalkQuery()
            .CountAsync(w => w.OutgoingWordRelations.Any()
                          || w.IncomingWordRelations.Any()
                          || _db.SenseRelations.Any(r => r.FromSense.WordId == w.Id || r.ToSense.WordId == w.Id), ct);

        return Ok(new RelationCoverageDto
        {
            TotalWords = total,
            WordsWithRelations = withRelations,
            WordsWithoutRelations = total - withRelations,
            TotalRelations = await _db.WordRelations.CountAsync(ct) + await _db.SenseRelations.CountAsync(ct),
        });
    }

    // ═══════════════════════════════════════════════════════════════════════
    // The walk
    // ═══════════════════════════════════════════════════════════════════════

    /// <summary>The nth word in the walk, 1-based, with everything the editor draws.</summary>
    [HttpGet("at/{position:int}")]
    public async Task<ActionResult<WordRelationsDto>> At(int position, CancellationToken ct)
    {
        var total = await WalkQuery().CountAsync(ct);
        if (total == 0) return NotFound("هیچ وشەیەک نییە.");

        // Clamp instead of 404: the walk is driven by next/previous buttons and by a jump box, and
        // running off either end is a normal thing to do, not an error worth emptying the screen for.
        var index = Math.Clamp(position, 1, total);

        var wordId = await WalkQuery()
            .OrderBy(w => w.Kurdish).ThenBy(w => w.Id)
            .Skip(index - 1).Take(1)
            .Select(w => w.Id)
            .FirstOrDefaultAsync(ct);

        if (wordId == 0) return NotFound();

        return Ok(await BuildAsync(wordId, index, total, ct));
    }

    /// <summary>
    /// One word by id, positioned in the same walk.
    ///
    /// Used when somebody picks a word out of the search list rather than stepping to it — the
    /// position still has to be computed, or the counter would freeze at whatever it last showed
    /// and the walk buttons would carry on from the wrong place.
    /// </summary>
    [HttpGet("word/{wordId:int}")]
    public async Task<ActionResult<WordRelationsDto>> Word(int wordId, CancellationToken ct)
    {
        var word = await WalkQuery().FirstOrDefaultAsync(w => w.Id == wordId, ct);
        if (word is null) return NotFound("وشەکە نەدۆزرایەوە.");

        var total = await WalkQuery().CountAsync(ct);

        // Position = how many words sort before this one. Cheaper and more robust than paging
        // until the id turns up, and it uses the same ordering key as the walk itself.
        var before = await WalkQuery()
            .CountAsync(w => string.Compare(w.Kurdish, word.Kurdish) < 0
                          || (w.Kurdish == word.Kurdish && w.Id < word.Id), ct);

        return Ok(await BuildAsync(wordId, before + 1, total, ct));
    }

    /// <summary>
    /// The next word after <paramref name="after"/> that still has no relations, so the walk can
    /// skip the stretches that are already done. Returns the position, or null when none is left.
    /// </summary>
    [HttpGet("next-missing")]
    public async Task<ActionResult<int?>> NextMissing([FromQuery] int after = 0, CancellationToken ct = default)
    {
        var ordered = WalkQuery()
            .OrderBy(w => w.Kurdish).ThenBy(w => w.Id)
            .Where(w => !w.OutgoingWordRelations.Any()
                     && !w.IncomingWordRelations.Any()
                     && !_db.SenseRelations.Any(r => r.FromSense.WordId == w.Id || r.ToSense.WordId == w.Id));

        var id = await ordered.Skip(Math.Max(after, 0)).Select(w => w.Id).FirstOrDefaultAsync(ct);

        // Skip(after) works because the walk order and this filter share their ordering key: the
        // nth unrelated word is at or after position n in the full walk, never before it.
        if (id == 0) return Ok((int?)null);

        var target = await _db.Words.AsNoTracking().FirstAsync(w => w.Id == id, ct);

        var before = await WalkQuery()
            .CountAsync(w => string.Compare(w.Kurdish, target.Kurdish) < 0
                          || (w.Kurdish == target.Kurdish && w.Id < target.Id), ct);

        return Ok((int?)(before + 1));
    }

    /// <summary>
    /// Candidate targets for the list under the editor. Prefix search on the folded headword, the
    /// same rule the words list uses, so typing «س» means "begins with س".
    ///
    /// PAGED, not top-N. A bare take of twenty meant the only way to reach a word was to spell
    /// enough of it — fine when you know what you are looking for, useless when you are browsing
    /// for what a word might relate to, which is most of this job. The count comes back too, so
    /// the pager can say how much is behind the current filter rather than just offering a "next"
    /// that may lead nowhere.
    /// </summary>
    [HttpGet("search")]
    public async Task<ActionResult<PagedResultDto<RelationTargetDto>>> Search(
        [FromQuery] string? q, [FromQuery] int exclude = 0,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var query = WalkQuery().Where(w => w.Id != exclude);

        if (!string.IsNullOrWhiteSpace(q))
        {
            var needle = KurdishText.Normalize(q);
            query = query.Where(w => w.Normalized.StartsWith(needle)
                                  || w.Forms.Any(f => f.Normalized.StartsWith(needle)));
        }

        var totalCount = await query.CountAsync(ct);

        var rows = await query
            .OrderBy(w => w.Kurdish).ThenBy(w => w.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(w => new
            {
                w.Id,
                w.Kurdish,
                w.Description,
                RelationCount = w.OutgoingWordRelations.Count + w.IncomingWordRelations.Count,
                Senses = w.Senses
                    .OrderBy(s => s.SortOrder).ThenBy(s => s.Id)
                    .Select(s => new { s.Id, s.Definition, PartOfSpeech = s.PartOfSpeech!.NameKu })
                    .ToList(),
            })
            .ToListAsync(ct);

        return Ok(new PagedResultDto<RelationTargetDto>
        {
            Items = rows.Select(w => new RelationTargetDto
            {
                Id = w.Id,
                Kurdish = w.Kurdish,
                Description = w.Description,
                RelationCount = w.RelationCount,
                Senses = w.Senses.Select((s, i) => new SenseBriefDto
                {
                    SenseId = s.Id,
                    Definition = s.Definition,
                    PartOfSpeechName = s.PartOfSpeech,
                    Label = SenseLabel(i + 1, s.PartOfSpeech, s.Definition),
                }).ToList(),
            }).ToList(),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
        });
    }

    // ═══════════════════════════════════════════════════════════════════════
    // Writes
    // ═══════════════════════════════════════════════════════════════════════

    [HttpPost]
    [Authorize(Roles = "Admin,Editor")]
    public async Task<ActionResult<WordRelationsDto>> Add([FromBody] AddRelationDto body, CancellationToken ct)
    {
        var type = await _db.RelationTypes.AsNoTracking().FirstOrDefaultAsync(t => t.Id == body.TypeId, ct);
        if (type is null) return BadRequest("جۆری پەیوەندی نەدۆزرایەوە.");

        try
        {
            if (type.Scope == RelationScope.Word)
            {
                await _relations.AddWordRelationAsync(body.FromWordId, body.ToWordId, body.TypeId, ct);
            }
            else
            {
                if (body.FromSenseId is not { } from || body.ToSenseId is not { } to)
                    return BadRequest("بۆ پەیوەندی مانایی، دەبێت مانا لە هەردوو لاوە دیاری بکرێت.");

                await _relations.AddSenseRelationAsync(from, to, body.TypeId, ct);
            }
        }
        catch (InvalidOperationException ex)
        {
            // The service's refusals are all sentences worth reading — "a word cannot relate to
            // itself", "that type is semantic, not morphological". Passing them through is the
            // difference between a fixable mistake and a button that does nothing.
            return BadRequest(ex.Message);
        }

        return Ok(await ReloadAsync(body.FromWordId, ct));
    }

    [HttpDelete("word/{relationId:int}")]
    [Authorize(Roles = "Admin,Editor")]
    public async Task<ActionResult<WordRelationsDto>> RemoveWord(
        int relationId, [FromQuery] int wordId, CancellationToken ct)
    {
        await _relations.RemoveWordRelationAsync(relationId, ct);
        return Ok(await ReloadAsync(wordId, ct));
    }

    [HttpDelete("sense/{relationId:int}")]
    [Authorize(Roles = "Admin,Editor")]
    public async Task<ActionResult<WordRelationsDto>> RemoveSense(
        int relationId, [FromQuery] int wordId, CancellationToken ct)
    {
        await _relations.RemoveSenseRelationAsync(relationId, ct);
        return Ok(await ReloadAsync(wordId, ct));
    }

    // ═══════════════════════════════════════════════════════════════════════

    private async Task<WordRelationsDto> ReloadAsync(int wordId, CancellationToken ct)
    {
        var total = await WalkQuery().CountAsync(ct);
        var word = await WalkQuery().FirstOrDefaultAsync(w => w.Id == wordId, ct);

        var before = word is null ? 0 : await WalkQuery()
            .CountAsync(w => string.Compare(w.Kurdish, word.Kurdish) < 0
                          || (w.Kurdish == word.Kurdish && w.Id < word.Id), ct);

        return await BuildAsync(wordId, before + 1, total, ct);
    }

    /// <summary>
    /// One word's workspace.
    ///
    /// Only OUTGOING edges are listed, and that is complete rather than half the picture: every
    /// type is either symmetric or paired with an inverse, and RelationService writes the mirror,
    /// so a relation involving this word ALWAYS has an edge pointing out of it. Listing both
    /// directions would show every relation twice — once as itself and once as its own mirror.
    ///
    /// The one exception is a type with no inverse and no symmetry, which would otherwise be
    /// invisible from the far side; those incoming edges are included so nothing can hide.
    /// </summary>
    private async Task<WordRelationsDto> BuildAsync(int wordId, int position, int total, CancellationToken ct)
    {
        var word = await _db.Words.AsNoTracking()
            .Include(w => w.Senses).ThenInclude(s => s.PartOfSpeech)
            .FirstAsync(w => w.Id == wordId, ct);

        var senses = word.Senses
            .OrderBy(s => s.SortOrder).ThenBy(s => s.Id)
            .Select((s, i) => new SenseBriefDto
            {
                SenseId = s.Id,
                Definition = s.Definition,
                PartOfSpeechName = s.PartOfSpeech?.NameKu,
                Label = SenseLabel(i + 1, s.PartOfSpeech?.NameKu, s.Definition),
            })
            .ToList();

        var senseLabels = senses.ToDictionary(s => s.SenseId, s => s.Label);

        var edges = new List<RelationEdgeDto>();

        // ── Word scope ────────────────────────────────────────────────────
        edges.AddRange(await _db.WordRelations.AsNoTracking()
            .Include(r => r.Type)
            .Where(r => r.FromWordId == wordId)
            .Select(r => new RelationEdgeDto
            {
                Id = r.Id,
                TypeId = r.TypeId,
                TypeCode = r.Type.Code,
                TypeNameKu = r.Type.NameKu,
                Scope = "word",
                IsIncoming = false,
                IsAutoInverse = r.IsAutoInverse,
                OtherWordId = r.ToWordId,
                OtherWord = r.ToWord.Kurdish,
            })
            .ToListAsync(ct));

        edges.AddRange(await _db.WordRelations.AsNoTracking()
            .Include(r => r.Type)
            .Where(r => r.ToWordId == wordId && !r.Type.IsSymmetric && r.Type.InverseId == null)
            .Select(r => new RelationEdgeDto
            {
                Id = r.Id,
                TypeId = r.TypeId,
                TypeCode = r.Type.Code,
                TypeNameKu = r.Type.NameKu,
                Scope = "word",
                IsIncoming = true,
                IsAutoInverse = r.IsAutoInverse,
                OtherWordId = r.FromWordId,
                OtherWord = r.FromWord.Kurdish,
            })
            .ToListAsync(ct));

        // ── Sense scope ───────────────────────────────────────────────────
        var senseIds = senses.Select(s => s.SenseId).ToList();

        if (senseIds.Count > 0)
        {
            var senseEdges = await _db.SenseRelations.AsNoTracking()
                .Include(r => r.Type)
                .Where(r => senseIds.Contains(r.FromSenseId))
                .Select(r => new
                {
                    r.Id,
                    r.TypeId,
                    TypeCode = r.Type.Code,
                    TypeName = r.Type.NameKu,
                    r.IsAutoInverse,
                    r.FromSenseId,
                    r.ToSenseId,
                    OtherWordId = r.ToSense.WordId,
                    OtherWord = r.ToSense.Word.Kurdish,
                    OtherDefinition = r.ToSense.Definition,
                })
                .ToListAsync(ct);

            edges.AddRange(senseEdges.Select(r => new RelationEdgeDto
            {
                Id = r.Id,
                TypeId = r.TypeId,
                TypeCode = r.TypeCode,
                TypeNameKu = r.TypeName,
                Scope = "sense",
                IsIncoming = false,
                IsAutoInverse = r.IsAutoInverse,
                OtherWordId = r.OtherWordId,
                OtherWord = r.OtherWord,
                OwnSenseId = r.FromSenseId,
                OwnSenseLabel = senseLabels.GetValueOrDefault(r.FromSenseId),
                OtherSenseId = r.ToSenseId,
                OtherSenseLabel = Truncate(r.OtherDefinition, 60),
            }));
        }

        return new WordRelationsDto
        {
            WordId = word.Id,
            Kurdish = word.Kurdish,
            Description = word.Description,
            Senses = senses,
            Relations = edges
                .OrderBy(e => e.Scope)
                .ThenBy(e => e.TypeId)
                .ThenBy(e => e.OtherWord)
                .ToList(),
            Position = position,
            Total = total,
        };
    }

    /// <summary>«٢. کار — بە خێرایی جووڵان» — the number, the part of speech, and enough definition
    /// to tell two senses of one word apart in a dropdown.</summary>
    private static string SenseLabel(int index, string? partOfSpeech, string definition)
    {
        var head = string.IsNullOrWhiteSpace(partOfSpeech) ? $"{index}." : $"{index}. {partOfSpeech} —";
        return $"{head} {Truncate(definition, 60)}".Trim();
    }

    private static string Truncate(string? text, int max)
    {
        if (string.IsNullOrWhiteSpace(text)) return "";
        text = text.Trim();
        return text.Length <= max ? text : text[..max] + "…";
    }
}
