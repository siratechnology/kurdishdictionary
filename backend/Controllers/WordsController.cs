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

    public WordsController(AppDbContext db) => _db = db;

    // GET api/words
    [HttpGet]
    public async Task<ActionResult<PagedResultDto<WordDto>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        [FromQuery] string? category = null)
    {
        var minIds = _db.Words.AsNoTracking()
            .GroupBy(w => w.Kurdish)
            .Select(g => g.Min(w => w.Id));

        var query = _db.Words.AsNoTracking()
            .Where(w => minIds.Contains(w.Id));

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(w => w.Kurdish.Contains(search));

        if (!string.IsNullOrWhiteSpace(category))
            query = query.Where(w => w.WordCategories.Any(wc => wc.Category.Name == category));

        var totalCount = await query.CountAsync();

        // Load page IDs first, then fetch with all includes (avoids EF paging+include cartesian explosion)
        var pageIds = await query
            .OrderBy(w => w.Kurdish)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(w => w.Id)
            .ToListAsync();

        var items = await _db.Words
            .AsNoTracking()
            .Where(w => pageIds.Contains(w.Id))
            .Include(w => w.SpeechPanes)
            .Include(w => w.WordCategories).ThenInclude(wc => wc.Category)
            .Include(w => w.Meanings)
            .OrderBy(w => w.Kurdish)
            .ToListAsync();

        // Compute relation counts in a separate lightweight query
        var relationCounts = await _db.Words
            .AsNoTracking()
            .Where(w => pageIds.Contains(w.Id))
            .Select(w => new { w.Id, Count = w.OutgoingRelations.Count + w.IncomingRelations.Count })
            .ToDictionaryAsync(x => x.Id, x => x.Count);

        return Ok(new PagedResultDto<WordDto>
        {
            Items = items.Select(w => MapToDto(w, totalRelations: relationCounts.GetValueOrDefault(w.Id))).ToList(),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        });
    }

    // GET api/words/categories
    [HttpGet("categories")]
    public async Task<ActionResult<List<CategoryDto>>> GetCategories()
    {
        var categories = await _db.Categories
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .Select(c => new CategoryDto { Id = c.Id, Name = c.Name })
            .ToListAsync();
        return Ok(categories);
    }

    // POST api/words/categories
    [HttpPost("categories")]
    public async Task<ActionResult<CategoryDto>> CreateCategory([FromBody] string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return BadRequest("Category name is required.");

        var trimmed = name.Trim();
        var existing = await _db.Categories.FirstOrDefaultAsync(c => c.Name == trimmed);
        if (existing is not null)
            return Ok(new CategoryDto { Id = existing.Id, Name = existing.Name });

        var category = new Category { Name = trimmed };
        _db.Categories.Add(category);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetCategories), new CategoryDto { Id = category.Id, Name = category.Name });
    }

    // GET api/words/locates
    [HttpGet("locates")]
    public async Task<ActionResult<List<string>>> GetLocates()
    {
        var locates = await _db.WordMeans
            .AsNoTracking()
            .Where(m => m.Locate != null && m.Locate != "")
            .Select(m => m.Locate!)
            .Distinct()
            .OrderBy(l => l)
            .ToListAsync();
        return Ok(locates);
    }

    // GET api/words/speech-types
    [HttpGet("speech-types")]
    public ActionResult<List<object>> GetSpeechTypes()
    {
        var types = SpeechPaneTypeExtensions.ToList()
            .Select(t => new { id = t.Id, kurdish = t.Kurdish })
            .ToList<object>();
        return Ok(types);
    }

    // GET api/words/genders
    [HttpGet("genders")]
    public ActionResult<List<object>> GetGenders()
    {
        var genders = GrammaticalGenderExtensions.ToList()
            .Select(g => new { id = g.Id, kurdish = g.Kurdish })
            .ToList<object>();
        return Ok(genders);
    }

    // GET api/words/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<WordDto>> GetById(int id)
    {
        var word = await _db.Words
            .AsNoTracking()
            .Include(w => w.SpeechPanes)
            .Include(w => w.WordCategories).ThenInclude(wc => wc.Category)
            .Include(w => w.OutgoingRelations).ThenInclude(r => r.TargetWord)
            .Include(w => w.IncomingRelations).ThenInclude(r => r.Word)
            .Include(w => w.Meanings)
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
            Gender = (GrammaticalGender)dto.Gender,
            Description = dto.Description,
            CreatedAt = DateTime.UtcNow
        };

        foreach (var sp in dto.SpeechPanes.Distinct())
            word.SpeechPanes.Add(new WordSpeechPane { SpeechPaneType = (SpeechPaneType)sp });

        foreach (var categoryId in dto.CategoryIds.Distinct())
            word.WordCategories.Add(new WordCategory { CategoryId = categoryId });

        foreach (var rel in dto.RelatedWords)
            word.OutgoingRelations.Add(new RelatedWord
            {
                TargetWordId = rel.TargetWordId,
                RelationType = rel.RelationType,
                Weight = rel.Weight
            });

        foreach (var m in dto.Meanings)
            word.Meanings.Add(new WordMeans { Meaning = m.Meaning, Locate = m.Locate });

        _db.Words.Add(word);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = word.Id }, await GetWordWithRelations(word.Id));
    }

    // PUT api/words/5
    [HttpPut("{id:int}")]
    public async Task<ActionResult<WordDto>> Update(int id, [FromBody] UpdateWordDto dto)
    {
        var word = await _db.Words
            .Include(w => w.SpeechPanes)
            .Include(w => w.WordCategories)
            .Include(w => w.OutgoingRelations)
            .Include(w => w.Meanings)
            .FirstOrDefaultAsync(w => w.Id == id);

        if (word is null) return NotFound();

        word.Kurdish = dto.Kurdish;
        word.Gender = (GrammaticalGender)dto.Gender;
        word.Description = dto.Description;

        _db.WordSpeechPanes.RemoveRange(word.SpeechPanes);
        word.SpeechPanes.Clear();
        foreach (var sp in dto.SpeechPanes.Distinct())
            word.SpeechPanes.Add(new WordSpeechPane { SpeechPaneType = (SpeechPaneType)sp });

        _db.WordCategories.RemoveRange(word.WordCategories);
        word.WordCategories.Clear();
        foreach (var catId in dto.CategoryIds.Distinct())
            word.WordCategories.Add(new WordCategory { CategoryId = catId });

        _db.RelatedWords.RemoveRange(word.OutgoingRelations);
        word.OutgoingRelations.Clear();
        foreach (var rel in dto.RelatedWords)
            word.OutgoingRelations.Add(new RelatedWord
            {
                WordId = id,
                TargetWordId = rel.TargetWordId,
                RelationType = rel.RelationType,
                Weight = rel.Weight
            });

        _db.WordMeans.RemoveRange(word.Meanings);
        word.Meanings.Clear();
        foreach (var m in dto.Meanings)
            word.Meanings.Add(new WordMeans { WordId = id, Meaning = m.Meaning, Locate = m.Locate });

        await _db.SaveChangesAsync();
        return Ok(await GetWordWithRelations(id));
    }

    // DELETE api/words/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var word = await _db.Words.FindAsync(id);
        if (word is null) return NotFound();
        _db.Words.Remove(word);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // GET api/words/5/graph
    [HttpGet("{id:int}/graph")]
    public async Task<ActionResult<GraphDto>> GetGraph(int id)
    {
        var word = await _db.Words
            .AsNoTracking()
            .Include(w => w.SpeechPanes)
            .Include(w => w.WordCategories).ThenInclude(wc => wc.Category)
            .Include(w => w.OutgoingRelations).ThenInclude(r => r.TargetWord)
                .ThenInclude(t => t!.SpeechPanes)
            .Include(w => w.OutgoingRelations).ThenInclude(r => r.TargetWord)
                .ThenInclude(t => t!.WordCategories).ThenInclude(wc => wc.Category)
            .Include(w => w.IncomingRelations).ThenInclude(r => r.Word)
                .ThenInclude(rw => rw!.SpeechPanes)
            .Include(w => w.IncomingRelations).ThenInclude(r => r.Word)
                .ThenInclude(rw => rw!.WordCategories).ThenInclude(wc => wc.Category)
            .FirstOrDefaultAsync(w => w.Id == id);

        if (word is null) return NotFound();

        var nodes = new List<GraphNodeDto>();
        var links = new List<GraphLinkDto>();

        nodes.Add(new GraphNodeDto
        {
            Id = word.Id.ToString(),
            Label = word.Kurdish,
            Category = word.WordCategories.FirstOrDefault()?.Category?.Name,
            IsCenter = true,
            Weight = word.OutgoingRelations.Count + word.IncomingRelations.Count,
            Color = "#6366f1",
            SpeechPane = (int)(word.SpeechPanes.FirstOrDefault()?.SpeechPaneType ?? SpeechPaneType.Other)
        });

        foreach (var rel in word.OutgoingRelations.Where(r => r.TargetWord != null))
        {
            var nodeId = rel.TargetWord!.Id.ToString();
            if (!nodes.Any(n => n.Id == nodeId))
                nodes.Add(new GraphNodeDto
                {
                    Id = nodeId,
                    Label = rel.TargetWord.Kurdish,
                    Category = rel.TargetWord.WordCategories.FirstOrDefault()?.Category?.Name,
                    IsCenter = false,
                    Weight = rel.Weight,
                    RelationType = rel.RelationType,
                    SpeechPane = (int)(rel.TargetWord.SpeechPanes.FirstOrDefault()?.SpeechPaneType ?? SpeechPaneType.Other)
                });
            links.Add(new GraphLinkDto
            {
                Source = word.Id.ToString(),
                Target = nodeId,
                RelationType = rel.RelationType,
                Weight = rel.Weight,
                IsIncoming = false
            });
        }

        foreach (var rel in word.IncomingRelations.Where(r => r.Word != null))
        {
            var nodeId = rel.Word!.Id.ToString();
            if (!nodes.Any(n => n.Id == nodeId))
                nodes.Add(new GraphNodeDto
                {
                    Id = nodeId,
                    Label = rel.Word.Kurdish,
                    Category = rel.Word.WordCategories.FirstOrDefault()?.Category?.Name,
                    IsCenter = false,
                    Weight = rel.Weight,
                    RelationType = rel.RelationType,
                    SpeechPane = (int)(rel.Word.SpeechPanes.FirstOrDefault()?.SpeechPaneType ?? SpeechPaneType.Other)
                });
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

    private async Task<WordDto> GetWordWithRelations(int id)
    {
        var word = await _db.Words
            .AsNoTracking()
            .Include(w => w.SpeechPanes)
            .Include(w => w.WordCategories).ThenInclude(wc => wc.Category)
            .Include(w => w.OutgoingRelations).ThenInclude(r => r.TargetWord)
            .Include(w => w.IncomingRelations).ThenInclude(r => r.Word)
            .Include(w => w.Meanings)
            .FirstAsync(w => w.Id == id);
        return MapToDto(word);
    }

    private static WordDto MapToDto(Word w, int totalRelations = -1) => new()
    {
        Id = w.Id,
        Kurdish = w.Kurdish,
        SpeechPanes = w.SpeechPanes?.Select(sp => new SpeechPaneDto
        {
            Id = (int)sp.SpeechPaneType,
            Kurdish = sp.SpeechPaneType.ToKurdish()
        }).ToList() ?? [],
        Categories = w.WordCategories?.Select(wc => new CategoryDto
        {
            Id = wc.CategoryId,
            Name = wc.Category?.Name ?? string.Empty
        }).ToList() ?? [],
        Gender = (int)w.Gender,
        GenderKurdish = w.Gender.ToKurdish(),
        Description = w.Description,
        CreatedAt = w.CreatedAt,
        TotalRelations = totalRelations >= 0
            ? totalRelations
            : (w.OutgoingRelations?.Count ?? 0) + (w.IncomingRelations?.Count ?? 0),
        Meanings = w.Meanings?.Select(m => new WordMeansDto
        {
            Id = m.Id,
            Meaning = m.Meaning,
            Locate = m.Locate
        }).ToList() ?? [],
        OutgoingRelations = w.OutgoingRelations?.Select(r => new RelatedWordDto
        {
            Id = r.Id,
            RelatedWordId = r.TargetWordId,
            RelatedKurdish = r.TargetWord?.Kurdish,
            RelationType = r.RelationType,
            IsIncoming = false,
            Weight = r.Weight
        }).ToList() ?? [],
        IncomingRelations = w.IncomingRelations?.Select(r => new RelatedWordDto
        {
            Id = r.Id,
            RelatedWordId = r.WordId,
            RelatedKurdish = r.Word?.Kurdish,
            RelationType = r.RelationType,
            IsIncoming = true,
            Weight = r.Weight
        }).ToList() ?? []
    };
}
