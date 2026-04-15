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
        var query = _db.Words.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(w => w.Kurdish.Contains(search));

        if (!string.IsNullOrWhiteSpace(category))
            query = query.Where(w => w.Category == category);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(w => w.Kurdish)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Include(w => w.OutgoingRelations).ThenInclude(r => r.TargetWord)
            .Include(w => w.IncomingRelations).ThenInclude(r => r.Word)
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