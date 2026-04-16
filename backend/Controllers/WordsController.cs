using backend.Data;
using backend.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WordsController : ControllerBase
{
    private readonly AppDbContext _db;

    public WordsController(AppDbContext db)
    {
        _db = db;
    }

    // GET api/words
    [HttpGet]
    public async Task<ActionResult<PagedResultDto<WordDto>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        [FromQuery] string? category = null)
    {
        // Use only the lowest-ID entry per unique Kurdish name to skip seeded duplicates
        var minIds = _db.Words.AsNoTracking()
            .GroupBy(w => w.Kurdish)
            .Select(g => g.Min(w => w.Id));

        var query = _db.Words.AsNoTracking()
            .Where(w => minIds.Contains(w.Id));

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(w => w.Kurdish.Contains(search));

        if (!string.IsNullOrWhiteSpace(category))
            query = query.Where(w => w.Category == category);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(w => w.Kurdish)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Ok(new PagedResultDto<WordDto>
        {
            Items = items.Select(MapToDto).ToList(),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        });
    }

    // GET api/words/categories
    [HttpGet("categories")]
    public async Task<ActionResult<List<string>>> GetCategories()
    {
        var categories = await _db.Words
            .AsNoTracking()
            .Where(w => w.Category != null)
            .Select(w => w.Category!)
            .Distinct()
            .OrderBy(c => c)
            .ToListAsync();
        return Ok(categories);
    }

    // GET api/words/5 (بۆ بینینی وشەکە و هەموو پەیوەندییەکانی لە مایند ماپدا)
    [HttpGet("{id:int}")]
    public async Task<ActionResult<WordDto>> GetById(int id)
    {
        var word = await _db.Words
            .AsNoTracking()
            .Include(w => w.OutgoingRelations).ThenInclude(r => r.TargetWord)
            .Include(w => w.IncomingRelations).ThenInclude(r => r.Word)
            .FirstOrDefaultAsync(w => w.Id == id);

        if (word is null) return NotFound();

        return Ok(MapToDto(word));
    }

    // POST api/words
    [HttpPost]
    public async Task<ActionResult<WordDto>> Create([FromBody] CreateWordDto dto)
    {
        var word = new Word
        {
            Kurdish = dto.Kurdish,
            Meaning = dto.Meaning,
            Category = dto.Category,
            Description = dto.Description,
            CreatedAt = DateTime.UtcNow
        };

        if (dto.RelatedWords != null)
        {
            foreach (var rel in dto.RelatedWords)
            {
                word.OutgoingRelations.Add(new RelatedWord
                {
                    TargetWordId = rel.TargetWordId,
                    RelationType = rel.RelationType,
                    Weight = rel.Weight
                });
            }
        }

        _db.Words.Add(word);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = word.Id }, await GetWordWithRelations(word.Id));
    }

    // PUT api/words/5
    [HttpPut("{id:int}")]
    public async Task<ActionResult<WordDto>> Update(int id, [FromBody] UpdateWordDto dto)
    {
        var word = await _db.Words
            .Include(w => w.OutgoingRelations)
            .FirstOrDefaultAsync(w => w.Id == id);

        if (word is null) return NotFound();

        word.Kurdish = dto.Kurdish;
        word.Meaning = dto.Meaning;
        word.Category = dto.Category;
        word.Description = dto.Description;

        // نوێکردنەوەی پەیوەندییەکان (سڕینەوەی کۆن و دانانی نوێ)
        _db.RelatedWords.RemoveRange(word.OutgoingRelations);
        word.OutgoingRelations.Clear();

        foreach (var rel in dto.RelatedWords)
        {
            word.OutgoingRelations.Add(new RelatedWord
            {
                WordId = id,
                TargetWordId = rel.TargetWordId,
                RelationType = rel.RelationType,
                Weight = rel.Weight
            });
        }

        await _db.SaveChangesAsync();
        return Ok(await GetWordWithRelations(id));
    }

    // GET api/words/5/graph  — داتای D3.js بۆ مایند ماپ
    [HttpGet("{id:int}/graph")]
    public async Task<ActionResult<GraphDto>> GetGraph(int id)
    {
        var word = await _db.Words
            .AsNoTracking()
            .Include(w => w.OutgoingRelations).ThenInclude(r => r.TargetWord)
            .Include(w => w.IncomingRelations).ThenInclude(r => r.Word)
            .FirstOrDefaultAsync(w => w.Id == id);

        if (word is null) return NotFound();

        var nodes = new List<GraphNodeDto>();
        var links = new List<GraphLinkDto>();

        // نۆدی ناوەند
        nodes.Add(new GraphNodeDto
        {
            Id = word.Id.ToString(),
            Label = word.Kurdish,
            Meaning = word.Meaning,
            Category = word.Category,
            IsCenter = true,
            Weight = word.OutgoingRelations.Count + word.IncomingRelations.Count,
            Color = "#6366f1"
        });

        // پەیوەندییە دەرچووەکان
        foreach (var rel in word.OutgoingRelations.Where(r => r.TargetWord != null))
        {
            var nodeId = rel.TargetWord!.Id.ToString();
            if (!nodes.Any(n => n.Id == nodeId))
            {
                nodes.Add(new GraphNodeDto
                {
                    Id = nodeId,
                    Label = rel.TargetWord.Kurdish,
                    Meaning = rel.TargetWord.Meaning,
                    Category = rel.TargetWord.Category,
                    IsCenter = false,
                    Weight = rel.Weight,
                    RelationType = rel.RelationType
                });
            }
            links.Add(new GraphLinkDto
            {
                Source = word.Id.ToString(),
                Target = nodeId,
                RelationType = rel.RelationType,
                Weight = rel.Weight,
                IsIncoming = false
            });
        }

        // پەیوەندییە هاتووەکان
        foreach (var rel in word.IncomingRelations.Where(r => r.Word != null))
        {
            var nodeId = rel.Word!.Id.ToString();
            if (!nodes.Any(n => n.Id == nodeId))
            {
                nodes.Add(new GraphNodeDto
                {
                    Id = nodeId,
                    Label = rel.Word.Kurdish,
                    Meaning = rel.Word.Meaning,
                    Category = rel.Word.Category,
                    IsCenter = false,
                    Weight = rel.Weight,
                    RelationType = rel.RelationType
                });
            }
            links.Add(new GraphLinkDto
            {
                Source = nodeId,
                Target = word.Id.ToString(),
                RelationType = rel.RelationType,
                Weight = rel.Weight,
                IsIncoming = true
            });
        }

        return Ok(new GraphDto { Nodes = nodes, Links = links });
    }

    // میتۆدێکی یارمەتیدەر بۆ هێنانەوەی وشە بە هەموو پەیوەندییەکانیەوە
    private async Task<WordDto> GetWordWithRelations(int id)
    {
        var word = await _db.Words
            .AsNoTracking()
            .Include(w => w.OutgoingRelations).ThenInclude(r => r.TargetWord)
            .Include(w => w.IncomingRelations).ThenInclude(r => r.Word)
            .FirstAsync(w => w.Id == id);
        return MapToDto(word);
    }

    private static WordDto MapToDto(Word w)
    {
        return new WordDto
        {
            Id = w.Id,
            Kurdish = w.Kurdish,
            Meaning = w.Meaning,
            Category = w.Category,
            Description = w.Description,
            CreatedAt = w.CreatedAt,
            OutgoingRelations = w.OutgoingRelations?.Select(r => new RelatedWordDto
            {
                Id = r.Id,
                RelatedWordId = r.TargetWordId,
                RelatedKurdish = r.TargetWord?.Kurdish,
                RelationType = r.RelationType,
                IsIncoming = false,
                Weight = r.Weight
            }).ToList() ?? new(),
            IncomingRelations = w.IncomingRelations?.Select(r => new RelatedWordDto
            {
                Id = r.Id,
                RelatedWordId = r.WordId,
                RelatedKurdish = r.Word?.Kurdish,
                RelationType = r.RelationType,
                IsIncoming = true,
                Weight = r.Weight
            }).ToList() ?? new()
        };
    }
}