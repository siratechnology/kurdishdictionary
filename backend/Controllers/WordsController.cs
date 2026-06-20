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

    // GET api/words/dashboard
    [HttpGet("dashboard")]
    public async Task<ActionResult<DashboardDto>> GetDashboard()
    {
        var now = DateTime.UtcNow;
        var sevenDaysAgo = now.AddDays(-7);
        var fourteenDaysAgo = now.Date.AddDays(-13);

        // Same dedupe-by-spelling rule as the words list, so counts match what users see
        var minIds = _db.Words.AsNoTracking()
            .GroupBy(w => w.Kurdish)
            .Select(g => g.Min(w => w.Id));
        var words = _db.Words.AsNoTracking().Where(w => minIds.Contains(w.Id));

        var dto = new DashboardDto
        {
            TotalWords = await words.CountAsync(),
            TotalCategories = await _db.Categories.CountAsync(),
            TotalRelations = await _db.RelatedWords.CountAsync(),
            TotalMeanings = await _db.WordMeans.CountAsync(),
            WordsWithoutRelations = await words.CountAsync(w => !w.OutgoingRelations.Any() && !w.IncomingRelations.Any()),
            WordsWithoutMeanings = await words.CountAsync(w => !w.Meanings.Any()),
            WordsWithoutCategory = await words.CountAsync(w => !w.WordCategories.Any()),
            WordsWithoutSpeechPane = await words.CountAsync(w => !w.SpeechPanes.Any()),
            WordsAddedLast7Days = await words.CountAsync(w => w.CreatedAt >= sevenDaysAgo)
        };

        dto.Genders = (await words
                .GroupBy(w => w.Gender)
                .Select(g => new { g.Key, Count = g.Count() })
                .ToListAsync())
            .Select(g => new NameCountDto { Name = g.Key.ToKurdish(), Count = g.Count })
            .ToList();

        dto.SpeechPanes = (await _db.WordSpeechPanes
                .GroupBy(sp => sp.SpeechPaneType)
                .Select(g => new { g.Key, Count = g.Count() })
                .ToListAsync())
            .Select(g => new NameCountDto { Name = g.Key.ToKurdish(), Count = g.Count })
            .OrderByDescending(g => g.Count)
            .ToList();

        dto.Categories = await _db.Categories
            .OrderByDescending(c => c.WordCategories.Count)
            .Select(c => new NameCountDto { Name = c.Name, Count = c.WordCategories.Count })
            .ToListAsync();

        dto.RelationTypes = await _db.RelatedWords
            .GroupBy(r => r.RelationType)
            .Select(g => new NameCountDto { Name = g.Key, Count = g.Count() })
            .OrderByDescending(g => g.Count)
            .ToListAsync();

        var dailyRaw = await words
            .Where(w => w.CreatedAt >= fourteenDaysAgo)
            .GroupBy(w => w.CreatedAt.Date)
            .Select(g => new { Date = g.Key, Count = g.Count() })
            .ToListAsync();

        dto.DailyAdded = Enumerable.Range(0, 14)
            .Select(i => fourteenDaysAgo.AddDays(i))
            .Select(d => new DailyCountDto { Date = d, Count = dailyRaw.FirstOrDefault(x => x.Date == d)?.Count ?? 0 })
            .ToList();

        dto.RecentWords = (await words
                .OrderByDescending(w => w.CreatedAt).ThenByDescending(w => w.Id)
                .Take(8)
                .Select(w => new
                {
                    w.Id,
                    w.Kurdish,
                    w.CreatedAt,
                    SpeechPane = w.SpeechPanes.Select(sp => (SpeechPaneType?)sp.SpeechPaneType).FirstOrDefault(),
                    Category = w.WordCategories.Select(wc => wc.Category.Name).FirstOrDefault(),
                    MeaningCount = w.Meanings.Count
                })
                .ToListAsync())
            .Select(w => new RecentWordDto
            {
                Id = w.Id,
                Kurdish = w.Kurdish,
                CreatedAt = w.CreatedAt,
                SpeechPane = w.SpeechPane?.ToKurdish(),
                Category = w.Category,
                MeaningCount = w.MeaningCount
            })
            .ToList();

        return Ok(dto);
    }

    // GET api/words/categories
    [HttpGet("categories")]
    public async Task<ActionResult<List<CategoryDto>>> GetCategories()
    {
        var categories = await _db.Categories
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .Select(c => new CategoryDto { Id = c.Id, Name = c.Name, WordCount = c.WordCategories.Count })
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

    // PUT api/words/categories/5
    [HttpPut("categories/{id:int}")]
    public async Task<ActionResult<CategoryDto>> UpdateCategory(int id, [FromBody] string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return BadRequest("Category name is required.");

        var category = await _db.Categories.FindAsync(id);
        if (category is null) return NotFound();

        var trimmed = name.Trim();
        var duplicate = await _db.Categories.AnyAsync(c => c.Id != id && c.Name == trimmed);
        if (duplicate) return Conflict("A category with this name already exists.");

        category.Name = trimmed;
        await _db.SaveChangesAsync();

        var wordCount = await _db.WordCategories.CountAsync(wc => wc.CategoryId == id);
        return Ok(new CategoryDto { Id = category.Id, Name = category.Name, WordCount = wordCount });
    }

    // DELETE api/words/categories/5
    [HttpDelete("categories/{id:int}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category = await _db.Categories.FindAsync(id);
        if (category is null) return NotFound();

        _db.Categories.Remove(category); // join rows cascade
        AddAudit("DeleteCategory", "Category", id, category.Name);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // GET api/words/categories/5/words
    [HttpGet("categories/{id:int}/words")]
    public async Task<ActionResult<PagedResultDto<WordDto>>> GetCategoryWords(
        int id,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 30,
        [FromQuery] string? search = null)
    {
        if (!await _db.Categories.AnyAsync(c => c.Id == id)) return NotFound();

        var query = _db.Words.AsNoTracking()
            .Where(w => w.WordCategories.Any(wc => wc.CategoryId == id));

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(w => w.Kurdish.Contains(search));

        return Ok(await PageMembershipWords(query, page, pageSize));
    }

    // POST api/words/categories/5/words/42
    [HttpPost("categories/{id:int}/words/{wordId:int}")]
    public async Task<IActionResult> AddWordToCategory(int id, int wordId)
    {
        if (!await _db.Categories.AnyAsync(c => c.Id == id)) return NotFound("Category not found.");
        if (!await _db.Words.AnyAsync(w => w.Id == wordId)) return NotFound("Word not found.");

        var exists = await _db.WordCategories.AnyAsync(wc => wc.CategoryId == id && wc.WordId == wordId);
        if (!exists)
        {
            _db.WordCategories.Add(new WordCategory { CategoryId = id, WordId = wordId });
            await _db.SaveChangesAsync();
        }
        return NoContent();
    }

    // DELETE api/words/categories/5/words/42
    [HttpDelete("categories/{id:int}/words/{wordId:int}")]
    public async Task<IActionResult> RemoveWordFromCategory(int id, int wordId)
    {
        var link = await _db.WordCategories
            .FirstOrDefaultAsync(wc => wc.CategoryId == id && wc.WordId == wordId);
        if (link is null) return NotFound();

        _db.WordCategories.Remove(link);
        AddAudit("RemoveWordFromCategory", "WordCategory", wordId, $"word {wordId} from category {id}");
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // GET api/words/speech-types/stats
    [HttpGet("speech-types/stats")]
    public async Task<ActionResult<List<SpeechPaneStatDto>>> GetSpeechTypeStats()
    {
        var counts = await _db.WordSpeechPanes
            .AsNoTracking()
            .GroupBy(sp => sp.SpeechPaneType)
            .Select(g => new { Type = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Type, x => x.Count);

        var stats = Enum.GetValues<SpeechPaneType>()
            .Select(t => new SpeechPaneStatDto
            {
                Id = (int)t,
                Kurdish = t.ToKurdish(),
                WordCount = counts.GetValueOrDefault(t)
            })
            .ToList();
        return Ok(stats);
    }

    // GET api/words/speech-types/3/words
    [HttpGet("speech-types/{typeId:int}/words")]
    public async Task<ActionResult<PagedResultDto<WordDto>>> GetSpeechTypeWords(
        int typeId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 30,
        [FromQuery] string? search = null)
    {
        if (!Enum.IsDefined((SpeechPaneType)typeId)) return NotFound("Unknown speech pane type.");
        var type = (SpeechPaneType)typeId;

        var query = _db.Words.AsNoTracking()
            .Where(w => w.SpeechPanes.Any(sp => sp.SpeechPaneType == type));

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(w => w.Kurdish.Contains(search));

        return Ok(await PageMembershipWords(query, page, pageSize));
    }

    // POST api/words/speech-types/3/words/42
    [HttpPost("speech-types/{typeId:int}/words/{wordId:int}")]
    public async Task<IActionResult> AddWordToSpeechType(int typeId, int wordId)
    {
        if (!Enum.IsDefined((SpeechPaneType)typeId)) return NotFound("Unknown speech pane type.");
        if (!await _db.Words.AnyAsync(w => w.Id == wordId)) return NotFound("Word not found.");

        var type = (SpeechPaneType)typeId;
        var exists = await _db.WordSpeechPanes.AnyAsync(sp => sp.WordId == wordId && sp.SpeechPaneType == type);
        if (!exists)
        {
            _db.WordSpeechPanes.Add(new WordSpeechPane { WordId = wordId, SpeechPaneType = type });
            await _db.SaveChangesAsync();
        }
        return NoContent();
    }

    // DELETE api/words/speech-types/3/words/42
    [HttpDelete("speech-types/{typeId:int}/words/{wordId:int}")]
    public async Task<IActionResult> RemoveWordFromSpeechType(int typeId, int wordId)
    {
        var type = (SpeechPaneType)typeId;
        var link = await _db.WordSpeechPanes
            .FirstOrDefaultAsync(sp => sp.WordId == wordId && sp.SpeechPaneType == type);
        if (link is null) return NotFound();

        _db.WordSpeechPanes.Remove(link);
        AddAudit("RemoveWordFromSpeechType", "WordSpeechPane", wordId, $"word {wordId} from speech-type {typeId}");
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // Shared paging for category/speech-pane membership lists (light includes)
    private async Task<PagedResultDto<WordDto>> PageMembershipWords(IQueryable<Word> query, int page, int pageSize)
    {
        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(w => w.Kurdish)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Include(w => w.SpeechPanes)
            .Include(w => w.WordCategories).ThenInclude(wc => wc.Category)
            .ToListAsync();

        return new PagedResultDto<WordDto>
        {
            Items = items.Select(w => MapToDto(w, totalRelations: 0)).ToList(),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
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

    // GET api/words/5/meta  (lightweight — for OG image generation)
    [HttpGet("{id:int}/meta")]
    [ResponseCache(Duration = 3600)]
    public async Task<ActionResult<WordMetaDto>> GetMeta(int id)
    {
        var word = await _db.Words
            .AsNoTracking()
            .Include(w => w.SpeechPanes)
            .Include(w => w.WordCategories).ThenInclude(wc => wc.Category)
            .Include(w => w.Meanings)
            .FirstOrDefaultAsync(w => w.Id == id);

        if (word is null) return NotFound();

        return Ok(new WordMetaDto
        {
            Id = word.Id,
            Kurdish = word.Kurdish,
            SpeechPanes = word.SpeechPanes.Select(sp => new SpeechPaneDto
            {
                Id = (int)sp.SpeechPaneType,
                Kurdish = sp.SpeechPaneType.ToKurdish()
            }).ToList(),
            Categories = word.WordCategories.Select(wc => new CategoryDto
            {
                Id = wc.CategoryId,
                Name = wc.Category?.Name ?? string.Empty
            }).ToList(),
            GenderKurdish = word.Gender.ToKurdish(),
            FirstMeaning = word.Meanings.FirstOrDefault()?.Meaning,
            Description = word.Description,
        });
    }

    // DELETE api/words/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var word = await _db.Words.FindAsync(id);
        if (word is null) return NotFound();
        _db.Words.Remove(word);
        AddAudit("DeleteWord", "Word", id, word.Kurdish);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // GET api/words/audit  — review who deleted what, from where, when
    [HttpGet("audit")]
    public async Task<ActionResult<PagedResultDto<AuditLogDto>>> GetAuditLog(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50)
    {
        var query = _db.AuditLogs.AsNoTracking().OrderByDescending(a => a.CreatedAt);
        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(a => new AuditLogDto
            {
                Id = a.Id,
                Action = a.Action,
                EntityType = a.EntityType,
                EntityId = a.EntityId,
                Summary = a.Summary,
                IpAddress = a.IpAddress,
                UserAgent = a.UserAgent,
                CreatedAt = a.CreatedAt
            })
            .ToListAsync();

        return Ok(new PagedResultDto<AuditLogDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        });
    }

    // Records a destructive action with the caller's IP + user-agent for tracing.
    private void AddAudit(string action, string entityType, int entityId, string? summary)
    {
        _db.AuditLogs.Add(new AuditLog
        {
            Action = action,
            EntityType = entityType,
            EntityId = entityId,
            Summary = summary,
            IpAddress = ClientInfo.GetClientIp(Request),
            UserAgent = ClientInfo.GetUserAgent(Request),
            CreatedAt = DateTime.UtcNow
        });
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
