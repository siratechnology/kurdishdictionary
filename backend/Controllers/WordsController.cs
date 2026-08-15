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
/// Reads are anonymous — the public Next.js dictionary depends on them. Every write requires a
/// signed-in Editor or Admin, and deletes require an Admin. The audit trail is written automatically
/// by AuditSaveChangesInterceptor, so no action here logs by hand.
///
/// Note the absence of a controller-level [AllowAnonymous]: it would override the [Authorize] on
/// every action below and silently leave all the writes open to the public.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class WordsController : ControllerBase
{
    private readonly AppDbContext _db;

    /// <summary>
    /// The options tree, so the classification endpoint clears answers to questions that no longer
    /// apply — the same resolution the station uses, not a second copy of the rule.
    /// </summary>
    private readonly OptionsTreeService _tree;

    public WordsController(AppDbContext db, OptionsTreeService tree)
    {
        _db = db;
        _tree = tree;
    }

    // GET api/words
    [HttpGet]
    public async Task<ActionResult<PagedResultDto<WordDto>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        [FromQuery] string? category = null,
        [FromQuery] int? speechPane = null,
        [FromQuery] int? partOfSpeech = null,
        [FromQuery] int? axis = null,
        [FromQuery] int? value = null,
        [FromQuery] string? values = null,
        [FromQuery] string? missing = null)
    {
        // One id per level the caller drilled into. `value` is the single-value form the same
        // filter used before the cascade went dynamic; folding it in here keeps one code path
        // instead of two predicates that have to be kept saying the same thing.
        var valueIds = ParseValueIds(values, value);

        var minIds = _db.Words.AsNoTracking()
            .GroupBy(w => w.Kurdish)
            .Select(g => g.Min(w => w.Id));

        var query = _db.Words.AsNoTracking()
            .Where(w => minIds.Contains(w.Id));

        if (!string.IsNullOrWhiteSpace(search))
        {
            // Fold the QUERY with the same function that produced the column. Normalising one side
            // only would fail on exactly the inputs the column exists to fix.
            var needle = KurdishText.Normalize(search);

            // PREFIX, not substring. Typing «س» means "words that begin with س", which is how a
            // dictionary is read; Contains would answer «کەس» and «باس» to that first keystroke
            // and bury the words actually being looked for. StartsWith also lets SQL Server seek
            // the index on Normalized instead of scanning every row.
            //
            // An inflected form matches its headword: a search for جوانترین has to return جوان's
            // entry, never a separate row and never nothing.
            query = query.Where(w =>
                w.Normalized.StartsWith(needle) ||
                w.Forms.Any(f => f.Normalized.StartsWith(needle)));
        }

        if (!string.IsNullOrWhiteSpace(category))
            query = query.Where(w => w.WordCategories.Any(wc => wc.Category.Name == category));

        if (speechPane is { } paneId && Enum.IsDefined((SpeechPaneType)paneId))
        {
            var pane = (SpeechPaneType)paneId;
            query = query.Where(w => w.SpeechPanes.Any(sp => sp.SpeechPaneType == pane));
        }

        // ── The v3 cascade: بەشی ئاخاوتن → تەوەر → نرخ → تەوەر → … ─────────────────
        //
        // Every condition has to be met by the SAME sense, which is why this is one Any() rather
        // than chained Where clauses. Separate clauses would return a word whose first sense is a
        // noun and whose second sense happens to carry ژمارە=کۆ — two unrelated facts about two
        // unrelated senses, reported as if they described one.
        //
        // The value list is ANDed, not ORed, and that is the whole point of the dynamic cascade:
        // the ids arrive one per level, and a sub-answer is only meaningful together with the
        // answer that opened its group. ORing کردار + تێپەڕ would return every transitive word
        // AND every verb, which is the opposite of drilling down.
        //
        // Counting matched features rather than folding an All() keeps this translatable to SQL.
        // It is exact because UNIQUE(SenseId, AxisId, ValueId) forbids a sense holding the same
        // value twice, so n distinct ids can only be matched by n distinct rows.
        //
        // This is the v3 PartOfSpeech on Sense, not the legacy SpeechPaneType on Word. They are
        // different tables saying similar things; only this one has axes hanging off it.
        if (partOfSpeech is not null || axis is not null || valueIds.Count > 0)
        {
            var wanted = valueIds.Count;

            query = query.Where(w => w.Senses.Any(sense =>
                (partOfSpeech == null || sense.PartOfSpeechId == partOfSpeech) &&
                (axis == null || sense.Features.Any(f => f.AxisId == axis)) &&
                (wanted == 0 || sense.Features.Count(f =>
                    f.ValueId != null && valueIds.Contains(f.ValueId.Value)) == wanted)));
        }

        // The gap filters. These are the same four counts the dashboard's alert rail reports, so
        // clicking an alert and landing on a filtered list is the obvious next step — and the
        // list and the alert can never disagree, because both read this predicate.
        query = missing switch
        {
            "meaning"  => query.Where(w => !w.Meanings.Any()),
            "category" => query.Where(w => !w.WordCategories.Any()),
            "pane"     => query.Where(w => !w.SpeechPanes.Any()),
            "relation" => query.Where(w => !w.OutgoingRelations.Any() && !w.IncomingRelations.Any()),
            _          => query,
        };

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
            .Include(w => w.DictionarySection)
            .Include(w => w.Meanings)
            .OrderBy(w => w.Kurdish)
            .ToListAsync();

        // Compute relation counts in a separate lightweight query
        var relationCounts = await _db.Words
            .AsNoTracking()
            .Where(w => pageIds.Contains(w.Id))
            .Select(w => new { w.Id, Count = w.OutgoingRelations.Count + w.IncomingRelations.Count })
            .ToDictionaryAsync(x => x.Id, x => x.Count);

        // Which inflected form brought each result back, for the ones that did not match on the
        // headword. Its own query, so an ordinary search pays nothing for it.
        var matchedForms = await MatchedFormLabels(pageIds, search);

        return Ok(new PagedResultDto<WordDto>
        {
            Items = items.Select(w => MapToDto(
                        w,
                        totalRelations: relationCounts.GetValueOrDefault(w.Id),
                        matchedForm: matchedForms.GetValueOrDefault(w.Id))).ToList(),
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

            // Words that pass ALL THREE checks, counted per word rather than inferred from the gap
            // totals — which cannot be done, because they overlap in unknown ways.
            //
            // The dashboard dial used to take total minus the WORST single gap. That is an upper
            // bound, not a count: the 240 words with no پۆل are not the same 210 with no بەشی
            // ئاخاوتن. The gap grows with the overlap, so the dial was most wrong exactly when the
            // work was most scattered.
            //
            // پەیوەندی is deliberately NOT a fourth condition, and this is the second time that
            // decision has been made. A word with no هاوواتا, دژواتا or بەشێک لە is arguably not
            // finished — but RelatedWords is empty, all 3,696 of them, so adding it would move the
            // dial from ٨٥٪ to ٠٪ and hold it there until the first relation is entered. A
            // headline number that cannot move is one people stop reading. The gap is already
            // visible: the پەیوەندی tile shows zero in a danger state, which is where a metric
            // nobody has started on belongs. Revisit once relations are being entered.
            WordsComplete = await words.CountAsync(w =>
                w.Meanings.Any() && w.WordCategories.Any() && w.SpeechPanes.Any()),
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
    [Authorize(Roles = Roles.AdminOrEditor)]
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
    [Authorize(Roles = Roles.AdminOrEditor)]
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
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category = await _db.Categories.FindAsync(id);
        if (category is null) return NotFound();

        // The comment that used to sit here said "join rows cascade". They no longer do: a soft
        // delete is an UPDATE, and an UPDATE cascades nothing. The links have to be hidden by hand
        // or the category disappears from the list while its words still claim membership.
        var links = await _db.WordCategories.Where(wc => wc.CategoryId == id).ToListAsync();
        _db.WordCategories.RemoveRange(links);
        _db.Categories.Remove(category);

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
        {
            var needle = KurdishText.Normalize(search);

            // Prefix, matching the main word search. Two search boxes in one app that answer the
            // same keystroke differently is the kind of inconsistency people never report and
            // never stop being confused by.
            query = query.Where(w => w.Normalized.StartsWith(needle));
        }

        return Ok(await PageMembershipWords(query, page, pageSize));
    }

    // POST api/words/categories/5/words/42
    [HttpPost("categories/{id:int}/words/{wordId:int}")]
    [Authorize(Roles = Roles.AdminOrEditor)]
    public async Task<IActionResult> AddWordToCategory(int id, int wordId)
    {
        if (!await _db.Categories.AnyAsync(c => c.Id == id)) return NotFound("Category not found.");
        if (!await _db.Words.AnyAsync(w => w.Id == wordId)) return NotFound("Word not found.");

        // IgnoreQueryFilters: a previously removed link is still there, soft-deleted. Inserting a
        // second row with the same composite key would violate the primary key, so an "add" after a
        // "remove" has to resurrect rather than insert.
        var link = await _db.WordCategories
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(wc => wc.CategoryId == id && wc.WordId == wordId);

        if (link is null)
        {
            _db.WordCategories.Add(new WordCategory { CategoryId = id, WordId = wordId });
            await _db.SaveChangesAsync();
        }
        else if (link.IsDeleted)
        {
            link.IsDeleted = false;
            link.DeletedAt = null;
            link.DeletedByUserId = null;
            await _db.SaveChangesAsync();
        }

        return NoContent();
    }

    // DELETE api/words/categories/5/words/42
    [HttpDelete("categories/{id:int}/words/{wordId:int}")]
    [Authorize(Roles = Roles.AdminOrEditor)]
    public async Task<IActionResult> RemoveWordFromCategory(int id, int wordId)
    {
        var link = await _db.WordCategories
            .FirstOrDefaultAsync(wc => wc.CategoryId == id && wc.WordId == wordId);
        if (link is null) return NotFound();

        _db.WordCategories.Remove(link);
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
        {
            var needle = KurdishText.Normalize(search);

            // Prefix, matching the main word search. Two search boxes in one app that answer the
            // same keystroke differently is the kind of inconsistency people never report and
            // never stop being confused by.
            query = query.Where(w => w.Normalized.StartsWith(needle));
        }

        return Ok(await PageMembershipWords(query, page, pageSize));
    }

    // POST api/words/speech-types/3/words/42
    [HttpPost("speech-types/{typeId:int}/words/{wordId:int}")]
    [Authorize(Roles = Roles.AdminOrEditor)]
    public async Task<IActionResult> AddWordToSpeechType(int typeId, int wordId)
    {
        if (!Enum.IsDefined((SpeechPaneType)typeId)) return NotFound("Unknown speech pane type.");
        if (!await _db.Words.AnyAsync(w => w.Id == wordId)) return NotFound("Word not found.");

        var type = (SpeechPaneType)typeId;

        // See AddWordToCategory — a removed link is soft-deleted, not gone, so re-adding it has to
        // clear the flag rather than insert over the same composite key.
        var link = await _db.WordSpeechPanes
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(sp => sp.WordId == wordId && sp.SpeechPaneType == type);

        if (link is null)
        {
            _db.WordSpeechPanes.Add(new WordSpeechPane { WordId = wordId, SpeechPaneType = type });
            await _db.SaveChangesAsync();
        }
        else if (link.IsDeleted)
        {
            link.IsDeleted = false;
            link.DeletedAt = null;
            link.DeletedByUserId = null;
            await _db.SaveChangesAsync();
        }

        return NoContent();
    }

    // DELETE api/words/speech-types/3/words/42
    [HttpDelete("speech-types/{typeId:int}/words/{wordId:int}")]
    [Authorize(Roles = Roles.AdminOrEditor)]
    public async Task<IActionResult> RemoveWordFromSpeechType(int typeId, int wordId)
    {
        var type = (SpeechPaneType)typeId;
        var link = await _db.WordSpeechPanes
            .FirstOrDefaultAsync(sp => sp.WordId == wordId && sp.SpeechPaneType == type);
        if (link is null) return NotFound();

        _db.WordSpeechPanes.Remove(link);
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
    // GET api/words/dictionary-sections
    [HttpGet("dictionary-sections")]
    public async Task<ActionResult<List<DictionarySectionDto>>> GetDictionarySections(CancellationToken ct) =>
        Ok(await _db.DictionarySections.AsNoTracking()
            .Where(d => d.IsActive)
            .OrderBy(d => d.SortOrder).ThenBy(d => d.NameKu)
            .Select(d => new DictionarySectionDto
            {
                Id = d.Id,
                NameKu = d.NameKu,
                WordCount = d.Words.Count(),
            })
            .ToListAsync(ct));

    // POST api/words/dictionary-sections
    //
    // Created from the word editor, not only from ڕێکخستن. A lexicographer meeting a word from a
    // field nobody has entered yet should not have to abandon the word, go and configure a
    // section, and come back — that is how words get filed under the wrong heading.
    [HttpPost("dictionary-sections")]
    [Authorize(Roles = Roles.AdminOrEditor)]
    public async Task<ActionResult<DictionarySectionDto>> CreateDictionarySection(
        [FromBody] NewSectionRequest body, CancellationToken ct)
    {
        var name = body.Name?.Trim();
        if (string.IsNullOrWhiteSpace(name))
            return BadRequest("ناوی بەشی فەرهەنگ پێویستە.");

        var normalized = KurdishText.Normalize(name);

        // Matched on the FOLDED name. «کیمیا» typed with a Persian kaf and with an Arabic one is
        // one section, and returning the existing row rather than refusing means the editor just
        // works instead of showing an error about a difference nobody can see.
        var existing = await _db.DictionarySections
            .FirstOrDefaultAsync(d => d.Normalized == normalized, ct);

        if (existing is not null)
        {
            return Ok(new DictionarySectionDto { Id = existing.Id, NameKu = existing.NameKu });
        }

        var max = await _db.DictionarySections.MaxAsync(d => (int?)d.SortOrder, ct) ?? 0;

        var section = new DictionarySection
        {
            NameKu = name,
            Normalized = normalized,
            SortOrder = max + 1,
        };

        _db.DictionarySections.Add(section);
        await _db.SaveChangesAsync(ct);

        return Ok(new DictionarySectionDto { Id = section.Id, NameKu = section.NameKu });
    }

    public record NewSectionRequest(string Name);

    // GET api/words/pace
    //
    // Measured, never configured. Every figure comes from what was actually recorded, so a quiet
    // fortnight lengthens the estimate on its own instead of waiting for somebody to remember to
    // change a target nobody owns.
    [HttpGet("pace")]
    public async Task<ActionResult<PaceDto>> GetPace(
        [FromQuery] int windowDays = 14,
        [FromQuery] double hoursPerDay = 6,
        CancellationToken ct = default)
    {
        windowDays = Math.Clamp(windowDays, 1, 180);
        hoursPerDay = Math.Clamp(hoursPerDay, 1, 24);

        var since = DateTime.UtcNow.Date.AddDays(-(windowDays - 1));

        var words = _db.Words.AsNoTracking();

        var total = await words.CountAsync(ct);

        // The SAME definition the completeness dial uses — worst of the three gaps, not their sum.
        // Two cards on one screen disagreeing about how much is left is worse than neither.
        var noMeaning = await words.CountAsync(w => !w.Meanings.Any(), ct);
        var noCategory = await words.CountAsync(w => !w.WordCategories.Any(), ct);
        var noPane = await words.CountAsync(w => !w.SpeechPanes.Any(), ct);
        var remaining = Math.Max(noMeaning, Math.Max(noCategory, noPane));

        // ── Day by day ──────────────────────────────────────────────────────
        var addedByDay = await words
            .Where(w => w.CreatedAt >= since)
            .GroupBy(w => w.CreatedAt.Date)
            .Select(g => new { Date = g.Key, Count = g.Count() })
            .ToListAsync(ct);

        // Audit rows rather than the ledger: the ledger only started recording recently, and an
        // estimate built on three weeks of missing history would read as a collapse in pace that
        // never happened.
        var editedByDay = await _db.AuditLogs.AsNoTracking()
            .Where(a => a.CreatedAt >= since && a.UserId != null)
            .GroupBy(a => a.CreatedAt.Date)
            .Select(g => new { Date = g.Key, Count = g.Count() })
            .ToListAsync(ct);

        var days = Enumerable.Range(0, windowDays)
            .Select(offset =>
            {
                var date = since.AddDays(offset);
                return new PaceDayDto
                {
                    Date = date,
                    Added = addedByDay.FirstOrDefault(x => x.Date == date)?.Count ?? 0,
                    Edited = editedByDay.FirstOrDefault(x => x.Date == date)?.Count ?? 0,
                };
            })
            .ToList();

        var added = days.Sum(d => d.Added);
        var edited = days.Sum(d => d.Edited);

        // Days on which anything happened. Dividing by calendar days instead would tell a team
        // that works five days a week they are 30% slower than they are.
        var activeDays = days.Count(d => d.Total > 0);

        var wordsPerDay = activeDays == 0 ? 0 : added / (double)activeDays;
        var itemsPerDay = activeDays == 0 ? 0 : (added + edited) / (double)activeDays;

        // Completion is driven by finishing words, so the projection uses the WORD rate. Edits
        // are shown because they are the work, but a day of pure editing adds no new headwords
        // and must not shorten an estimate of when the headwords run out.
        double? projectedDays = wordsPerDay > 0 ? remaining / wordsPerDay : null;

        // ── Who ─────────────────────────────────────────────────────────────
        var addedPerUser = await words
            .Where(w => w.CreatedAt >= since && w.CreatedByUserId != null)
            .GroupBy(w => w.CreatedByUserId!.Value)
            .Select(g => new { UserId = g.Key, Count = g.Count() })
            .ToListAsync(ct);

        var editedPerUser = await _db.AuditLogs.AsNoTracking()
            .Where(a => a.CreatedAt >= since && a.UserId != null)
            .GroupBy(a => a.UserId!.Value)
            .Select(g => new { UserId = g.Key, Count = g.Count() })
            .ToListAsync(ct);

        var userIds = addedPerUser.Select(x => x.UserId)
            .Union(editedPerUser.Select(x => x.UserId))
            .ToList();

        var people = await _db.Users.AsNoTracking()
            .Where(u => userIds.Contains(u.Id))
            .Select(u => new { u.Id, u.UserName, u.FullName, u.AvatarFile })
            .ToListAsync(ct);

        var contributors = people
            .Select(u => new PaceContributorDto
            {
                UserId = u.Id,
                UserName = u.UserName ?? string.Empty,
                FullName = u.FullName,
                AvatarUrl = string.IsNullOrWhiteSpace(u.AvatarFile) ? null : $"/avatars/{u.AvatarFile}",
                Added = addedPerUser.FirstOrDefault(x => x.UserId == u.Id)?.Count ?? 0,
                Edited = editedPerUser.FirstOrDefault(x => x.UserId == u.Id)?.Count ?? 0,
            })
            .OrderByDescending(c => c.Total)
            .ToList();

        return Ok(new PaceDto
        {
            TotalWords = total,
            RemainingWords = remaining,
            AddedInWindow = added,
            EditedInWindow = edited,
            WindowDays = windowDays,
            ActiveDays = activeDays,
            WordsPerActiveDay = Math.Round(wordsPerDay, 1),
            ItemsPerActiveDay = Math.Round(itemsPerDay, 1),
            ProjectedDays = projectedDays is { } d ? Math.Round(d, 1) : null,
            ProjectedHours = projectedDays is { } h ? Math.Round(h * hoursPerDay, 1) : null,
            HoursPerDay = hoursPerDay,

            // Across ACTIVE days, then spread back over the calendar at the observed working
            // rhythm — otherwise a team that works five days in seven is promised a date it
            // cannot reach.
            ProjectedFinish = projectedDays is { } pd && activeDays > 0
                ? DateTime.UtcNow.Date.AddDays(Math.Ceiling(pd * windowDays / activeDays))
                : null,

            Days = days,
            Contributors = contributors,
        });
    }

    // ── Senses (schema v3) ─────────────────────────────────────────────────
    //
    // The word editor writes HERE. Everything else in the v3 half of the application — the
    // station, the work queue, ڕێکخستن and its axes — reads senses, and for a long time the
    // editor was writing WordMeans instead, so the two halves maintained different lists of
    // what a word means.

    // GET api/words/{id}/classification
    //
    // One answer for the whole word. It is READ from the word's senses, which is where the
    // database keeps it — the first sense that has been classified wins, because a word whose
    // senses disagree is a defect the station is there to fix, not a state this form should
    // render two ways at once.
    [HttpGet("{id:int}/classification")]
    public async Task<ActionResult<WordClassificationDto>> GetClassification(int id, CancellationToken ct)
    {
        if (!await _db.Words.AnyAsync(w => w.Id == id, ct)) return NotFound();

        var sense = await _db.Senses.AsNoTracking()
            .Where(s => s.WordId == id)
            .OrderByDescending(s => s.PartOfSpeechId != null)
            .ThenBy(s => s.SortOrder).ThenBy(s => s.Id)
            .Select(s => new WordClassificationDto
            {
                PartOfSpeechId = s.PartOfSpeechId,
                PartOfSpeechName = s.PartOfSpeech!.NameKu,
                Features = s.Features
                    .Where(f => !f.IsDeleted)
                    .Select(f => new SenseFeatureDto
                    {
                        AxisId = f.AxisId,
                        AxisName = f.Axis.NameKu,
                        ValueId = f.ValueId,
                        ValueName = f.Value!.NameKu,
                        IsNotApplicable = f.IsNotApplicable,
                    }).ToList(),
            })
            .FirstOrDefaultAsync(ct);

        return Ok(sense ?? new WordClassificationDto());
    }

    // PUT api/words/{id}/classification
    [HttpPut("{id:int}/classification")]
    [Authorize(Roles = Roles.AdminOrEditor)]
    public async Task<ActionResult<WordClassificationDto>> SaveClassification(
        int id, [FromBody] WordClassificationDto dto, CancellationToken ct)
    {
        var word = await _db.Words
            .Include(w => w.Senses).ThenInclude(s => s.Features)
            .FirstOrDefaultAsync(w => w.Id == id, ct);

        if (word is null) return NotFound();

        var senses = word.Senses.Where(s => !s.IsDeleted).ToList();

        // A word with no senses yet — a brand new one — needs somewhere to put the answer. The
        // definition stays empty; the validator and the work queue will ask for it, which is
        // exactly the backlog they exist to report.
        if (senses.Count == 0)
        {
            var created = new Sense { WordId = word.Id, SortOrder = 0 };
            word.Senses.Add(created);
            senses.Add(created);
        }

        // Written to EVERY sense. The word carries one classification, so its senses must agree;
        // updating only the first would leave the rest holding whatever the migration guessed,
        // and the queue would keep reporting them.
        foreach (var sense in senses)
        {
            sense.PartOfSpeechId = dto.PartOfSpeechId;
            ApplyFeatures(sense, dto.Features);
        }

        await _db.SaveChangesAsync(ct);

        // Then drop anything that answers a question this classification does not ask.
        //
        // The payload is whatever a client sent, and a client can be wrong or old: an answer on a
        // conditional axis whose condition is not satisfied — تێپەڕی on a ناتەواو verb — would
        // otherwise sit in the row asserting something the classification does not support, invisible
        // in every form that resolves the tree properly, and reported by the work queue forever.
        //
        // The rule is not re-implemented here. This is the same resolution the station runs, so the
        // endpoint cannot disagree with the form about which questions are being asked.
        foreach (var sense in senses)
            await _tree.ClearStaleAnswersAsync(sense.Id, ct);

        return await GetClassification(id, ct);
    }

    // GET api/words/{id}/senses
    [HttpGet("{id:int}/senses")]
    public async Task<ActionResult<WordSensesDto>> GetSenses(int id, CancellationToken ct)
    {
        var word = await _db.Words.AsNoTracking()
            .Where(w => w.Id == id)
            .Select(w => new { w.Id, w.Kurdish })
            .FirstOrDefaultAsync(ct);

        if (word is null) return NotFound();

        var senses = await _db.Senses.AsNoTracking()
            .Where(s => s.WordId == id)
            .OrderBy(s => s.SortOrder).ThenBy(s => s.Id)
            .Select(s => new WordSenseDto
            {
                Id = s.Id,
                Definition = s.Definition,
                ExampleUsage = s.ExampleUsage,
                PartOfSpeechId = s.PartOfSpeechId,
                PartOfSpeechName = s.PartOfSpeech!.NameKu,
                DomainId = s.DomainId,
                DomainName = s.Domain!.NameKu,
                SortOrder = s.SortOrder,
                Features = s.Features
                    .Where(f => !f.IsDeleted)
                    .Select(f => new SenseFeatureDto
                    {
                        AxisId = f.AxisId,
                        AxisName = f.Axis.NameKu,
                        ValueId = f.ValueId,
                        ValueName = f.Value!.NameKu,
                        IsNotApplicable = f.IsNotApplicable,
                    }).ToList(),
            })
            .ToListAsync(ct);

        return Ok(new WordSensesDto { WordId = word.Id, Kurdish = word.Kurdish, Senses = senses });
    }

    // PUT api/words/{id}/senses
    [HttpPut("{id:int}/senses")]
    [Authorize(Roles = Roles.AdminOrEditor)]
    public async Task<ActionResult<WordSensesDto>> SaveSenses(
        int id, [FromBody] SaveWordSensesDto dto, CancellationToken ct)
    {
        var word = await _db.Words
            .Include(w => w.Senses).ThenInclude(s => s.Features)
            .FirstOrDefaultAsync(w => w.Id == id, ct);

        if (word is null) return NotFound();

        var keptIds = dto.Senses.Where(x => x.Id > 0).Select(x => x.Id).ToHashSet();

        // SOFT delete, through the entity's own flag. A sense carries a claim, a ledger entry and
        // an audit trail; removing the row would orphan all three, and the interceptor exists so
        // that "deleted" always means the same thing everywhere.
        foreach (var gone in word.Senses.Where(s => !s.IsDeleted && !keptIds.Contains(s.Id)))
            _db.Senses.Remove(gone);

        var order = 0;

        foreach (var incoming in dto.Senses)
        {
            var sense = incoming.Id > 0
                ? word.Senses.FirstOrDefault(s => s.Id == incoming.Id)
                : null;

            if (sense is null)
            {
                sense = new Sense { WordId = word.Id };
                word.Senses.Add(sense);
            }

            sense.Definition = incoming.Definition?.Trim() ?? string.Empty;
            sense.ExampleUsage = incoming.ExampleUsage?.Trim() ?? string.Empty;
            sense.PartOfSpeechId = incoming.PartOfSpeechId;
            sense.DomainId = incoming.DomainId;
            sense.SortOrder = order++;

            ApplyFeatures(sense, incoming.Features);
        }

        await _db.SaveChangesAsync(ct);

        return await GetSenses(id, ct);
    }

    /// <summary>
    /// Reconciles one sense's axis answers.
    ///
    /// Answers for axes NOT in the incoming list are removed, which is what makes changing the
    /// part of speech safe: the form sends only the axes that apply to the new part, so the
    /// answers belonging to the old one go with it. Leaving them would mean a word reclassified
    /// from ناو to کردار silently keeping a ڕەگەز nobody can see or correct.
    /// </summary>
    private void ApplyFeatures(Sense sense, List<SenseFeatureDto> incoming)
    {
        var wanted = incoming.Where(f => f.ValueId is not null || f.IsNotApplicable).ToList();
        var wantedAxes = wanted.Select(f => f.AxisId).ToHashSet();

        foreach (var stale in sense.Features.Where(f => !f.IsDeleted && !wantedAxes.Contains(f.AxisId)).ToList())
            _db.SenseFeatures.Remove(stale);

        foreach (var f in wanted)
        {
            var existing = sense.Features.FirstOrDefault(x => !x.IsDeleted && x.AxisId == f.AxisId);

            if (existing is null)
            {
                sense.Features.Add(new SenseFeature
                {
                    AxisId = f.AxisId,
                    ValueId = f.IsNotApplicable ? null : f.ValueId,
                    IsNotApplicable = f.IsNotApplicable,
                });
                continue;
            }

            existing.ValueId = f.IsNotApplicable ? null : f.ValueId;
            existing.IsNotApplicable = f.IsNotApplicable;
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<WordDto>> GetById(int id)
    {
        var word = await _db.Words
            .AsNoTracking()
            .Include(w => w.SpeechPanes)
            .Include(w => w.WordCategories).ThenInclude(wc => wc.Category)
            .Include(w => w.OutgoingRelations).ThenInclude(r => r.TargetWord)
            .Include(w => w.IncomingRelations).ThenInclude(r => r.Word)
            .Include(w => w.DictionarySection)
            .Include(w => w.Meanings)
            .FirstOrDefaultAsync(w => w.Id == id);

        if (word is null) return NotFound();
        return Ok(MapToDto(word));
    }

    // POST api/words
    [HttpPost]
    [Authorize(Roles = Roles.AdminOrEditor)]
    public async Task<ActionResult<WordDto>> Create([FromBody] CreateWordDto dto)
    {
        var word = new Word
        {
            Kurdish = dto.Kurdish,
            Gender = (GrammaticalGender)dto.Gender,
            Description = dto.Description,
            DictionarySectionId = dto.DictionarySectionId,
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
    [Authorize(Roles = Roles.AdminOrEditor)]
    public async Task<ActionResult<WordDto>> Update(int id, [FromBody] UpdateWordDto dto)
    {
        var word = await _db.Words
            .Include(w => w.SpeechPanes)
            .Include(w => w.WordCategories)
            .Include(w => w.OutgoingRelations)
            .Include(w => w.DictionarySection)
            .Include(w => w.Meanings)
            .FirstOrDefaultAsync(w => w.Id == id);

        if (word is null) return NotFound();

        word.Kurdish = dto.Kurdish;
        word.Gender = (GrammaticalGender)dto.Gender;
        word.Description = dto.Description;
        word.DictionarySectionId = dto.DictionarySectionId;

        // ── Reconcile children, never replace them wholesale ────────────────────
        // This method used to RemoveRange every child and re-Add the lot. Under the ledger that is
        // wrong twice over: it emits a delete + create for rows that did not change, and it moves
        // CreatedByUserId onto whoever last pressed save — displacing the original author, which is
        // exactly what the provenance rules forbid. So each collection is diffed instead.

        await ReconcileSpeechPanes(word, dto.SpeechPanes.Distinct().ToList());
        await ReconcileCategories(word, dto.CategoryIds.Distinct().ToList());
        ReconcileRelations(word, dto.RelatedWords);
        ReconcileMeanings(word, dto.Meanings);

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
            .Include(w => w.DictionarySection)
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
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Delete(int id)
    {
        var word = await _db.Words
            .Include(w => w.SpeechPanes)
            .Include(w => w.WordCategories)
            .Include(w => w.OutgoingRelations)
            .Include(w => w.DictionarySection)
            .Include(w => w.Meanings)
            .FirstOrDefaultAsync(w => w.Id == id);

        if (word is null) return NotFound();

        // A soft delete is an UPDATE, so nothing cascades. Each child is hidden explicitly, and each
        // one emits its own ledger event — a word's removal should read as the removal of everything
        // that hung off it, not as a single unexplained line.
        _db.WordSpeechPanes.RemoveRange(word.SpeechPanes);
        _db.WordCategories.RemoveRange(word.WordCategories);
        _db.RelatedWords.RemoveRange(word.OutgoingRelations);
        _db.WordMeans.RemoveRange(word.Meanings);
        _db.Words.Remove(word);

        await _db.SaveChangesAsync();
        return NoContent();
    }

    // GET api/words/audit  — who changed what, from where, when
    [HttpGet("audit")]
    [Authorize(Roles = Roles.Any)]
    public async Task<ActionResult<PagedResultDto<AuditLogDto>>> GetAuditLog(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50,
        [FromQuery] string? action = null,
        [FromQuery] string? entityType = null,
        [FromQuery] Guid? userId = null,
        [FromQuery] string? search = null)
    {
        var query = _db.AuditLogs.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(action))
            query = query.Where(a => a.Action == action);

        if (!string.IsNullOrWhiteSpace(entityType))
            query = query.Where(a => a.EntityType == entityType);

        if (userId is not null)
            query = query.Where(a => a.UserId == userId);

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(a => (a.Summary != null && a.Summary.Contains(search))
                                     || (a.UserName != null && a.UserName.Contains(search))
                                     || (a.IpAddress != null && a.IpAddress.Contains(search)));

        var totalCount = await query.CountAsync();
        var items = await query
            .OrderByDescending(a => a.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(a => new AuditLogDto
            {
                Id = a.Id,
                Action = a.Action,
                EntityType = a.EntityType,
                EntityId = a.EntityId,
                Summary = a.Summary,
                Changes = a.Changes,
                UserId = a.UserId,
                UserName = a.UserName,
                IpAddress = a.IpAddress,
                UserAgent = a.UserAgent,
                Country = a.Country,
                City = a.City,
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

    /// <summary>Feeds the notification bell: anything that happened since the id the client last saw.</summary>
    // GET api/words/audit/since/{id}
    [HttpGet("audit/since/{afterId:int}")]
    [Authorize(Roles = Roles.Any)]
    public async Task<ActionResult<List<AuditLogDto>>> GetAuditSince(int afterId, [FromQuery] int take = 20)
    {
        var query = _db.AuditLogs.AsNoTracking();

        // afterId = 0 is a fresh client: hand it the latest few rather than the whole table.
        var rows = afterId > 0
            ? query.Where(a => a.Id > afterId).OrderByDescending(a => a.Id)
            : query.OrderByDescending(a => a.Id);

        var items = await rows
            .Take(Math.Clamp(take, 1, 50))
            .Select(a => new AuditLogDto
            {
                Id = a.Id,
                Action = a.Action,
                EntityType = a.EntityType,
                EntityId = a.EntityId,
                Summary = a.Summary,
                Changes = a.Changes,
                UserId = a.UserId,
                UserName = a.UserName,
                IpAddress = a.IpAddress,
                Country = a.Country,
                City = a.City,
                CreatedAt = a.CreatedAt
            })
            .ToListAsync();

        return Ok(items);
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
            .Include(w => w.DictionarySection)
            .Include(w => w.Meanings)
            .FirstAsync(w => w.Id == id);
        return MapToDto(word);
    }

    /// <summary>
    /// The cascade's answers, as a comma-separated list of value ids — «12,48,113».
    ///
    /// A list rather than a fixed axis/value pair because the options tree has no fixed depth:
    /// جێناو → سەربەخۆ → کەسی is already three levels and nothing stops a fourth. A query string
    /// shaped after the current tree would have to change every time the tree does.
    ///
    /// Garbage is dropped rather than rejected: these ids come from links people paste and
    /// bookmark, and a 400 on a stale bookmark is a worse answer than the unfiltered list.
    /// </summary>
    private static List<int> ParseValueIds(string? csv, int? single)
    {
        var ids = new List<int>();

        if (single is > 0) ids.Add(single.Value);

        if (!string.IsNullOrWhiteSpace(csv))
        {
            foreach (var part in csv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                if (int.TryParse(part, out var id) && id > 0)
                    ids.Add(id);
        }

        // Distinct matters: the count comparison in the predicate is the filter, so one id passed
        // twice would demand two matching rows and silently return nothing.
        return ids.Distinct().ToList();
    }

    private static WordDto MapToDto(Word w, int totalRelations = -1, string? matchedForm = null) => new()
    {
        Id = w.Id,
        Kurdish = w.Kurdish,
        MatchedForm = matchedForm,
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
        DictionarySectionId = w.DictionarySectionId,
        DictionarySectionName = w.DictionarySection != null ? w.DictionarySection.NameKu : null,
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

    /// <summary>
    /// For each result found through a form rather than its headword, a label like
    /// «جوانترین — پلەی باڵای جوان». Words matched on the headword are absent from the result.
    /// </summary>
    private async Task<Dictionary<int, string>> MatchedFormLabels(List<int> wordIds, string? search)
    {
        if (string.IsNullOrWhiteSpace(search) || wordIds.Count == 0)
            return new Dictionary<int, string>();

        var needle = KurdishText.Normalize(search);

        var hits = await _db.WordForms
            .AsNoTracking()
            .Where(f => wordIds.Contains(f.WordId)
                        && f.Normalized.StartsWith(needle)
                        && !f.Word.Normalized.StartsWith(needle))
            .Select(f => new { f.WordId, f.Form, TypeName = f.FormType.NameKu, Headword = f.Word.Kurdish })
            .ToListAsync();

        return hits
            .GroupBy(h => h.WordId)
            .ToDictionary(g => g.Key, g =>
            {
                var h = g.First();
                return $"{h.Form} — {h.TypeName}ی {h.Headword}";
            });
    }

    // ═══════════════════════════════════════════════════════════════════════
    // Child reconciliation
    //
    // Each helper leaves untouched rows untouched. That is the point: an unchanged row must not
    // produce a ledger event, and must not have its author rewritten.
    // ═══════════════════════════════════════════════════════════════════════

    private async Task ReconcileSpeechPanes(Word word, List<int> wanted)
    {
        var wantedTypes = wanted.Select(x => (SpeechPaneType)x).ToHashSet();

        // Soft-deleted links are invisible to word.SpeechPanes, so query past the filter — otherwise
        // re-adding a previously removed type collides on the composite key.
        var all = await _db.WordSpeechPanes
            .IgnoreQueryFilters()
            .Where(sp => sp.WordId == word.Id)
            .ToListAsync();

        foreach (var link in all)
        {
            var keep = wantedTypes.Contains(link.SpeechPaneType);

            if (keep && link.IsDeleted)
            {
                link.IsDeleted = false;
                link.DeletedAt = null;
                link.DeletedByUserId = null;
            }
            else if (!keep && !link.IsDeleted)
            {
                _db.WordSpeechPanes.Remove(link);
            }
        }

        var existing = all.Select(l => l.SpeechPaneType).ToHashSet();
        foreach (var type in wantedTypes.Where(t => !existing.Contains(t)))
            _db.WordSpeechPanes.Add(new WordSpeechPane { WordId = word.Id, SpeechPaneType = type });
    }

    private async Task ReconcileCategories(Word word, List<int> wanted)
    {
        var wantedIds = wanted.ToHashSet();

        var all = await _db.WordCategories
            .IgnoreQueryFilters()
            .Where(wc => wc.WordId == word.Id)
            .ToListAsync();

        foreach (var link in all)
        {
            var keep = wantedIds.Contains(link.CategoryId);

            if (keep && link.IsDeleted)
            {
                link.IsDeleted = false;
                link.DeletedAt = null;
                link.DeletedByUserId = null;
            }
            else if (!keep && !link.IsDeleted)
            {
                _db.WordCategories.Remove(link);
            }
        }

        var existing = all.Select(l => l.CategoryId).ToHashSet();
        foreach (var catId in wantedIds.Where(c => !existing.Contains(c)))
            _db.WordCategories.Add(new WordCategory { WordId = word.Id, CategoryId = catId });
    }

    /// <summary>Relations have no client-side id, so identity is (target, type) — the pair that
    /// makes two relations the same relation.</summary>
    private void ReconcileRelations(Word word, List<CreateRelatedWordDto> wanted)
    {
        var wantedKeys = wanted.Select(r => (r.TargetWordId, r.RelationType)).ToHashSet();

        foreach (var existing in word.OutgoingRelations.ToList())
        {
            if (wantedKeys.Contains((existing.TargetWordId, existing.RelationType))) continue;
            _db.RelatedWords.Remove(existing);
        }

        var existingKeys = word.OutgoingRelations
            .Select(r => (r.TargetWordId, r.RelationType))
            .ToHashSet();

        foreach (var rel in wanted)
        {
            if (existingKeys.Contains((rel.TargetWordId, rel.RelationType))) continue;

            _db.RelatedWords.Add(new RelatedWord
            {
                WordId = word.Id,
                TargetWordId = rel.TargetWordId,
                RelationType = rel.RelationType,
                Weight = rel.Weight,
            });
        }
    }

    /// <summary>Meanings carry their id back from the client, so an edited meaning updates in place
    /// and keeps its original author. Only a genuinely new meaning is inserted.</summary>
    private void ReconcileMeanings(Word word, List<WordMeansDto> wanted)
    {
        var keptIds = wanted.Where(m => m.Id > 0).Select(m => m.Id).ToHashSet();

        foreach (var existing in word.Meanings.ToList())
        {
            if (!keptIds.Contains(existing.Id))
            {
                _db.WordMeans.Remove(existing);
                continue;
            }

            var incoming = wanted.First(m => m.Id == existing.Id);

            // Assigning only on difference keeps EF from marking the property modified, which keeps
            // a no-op save out of the ledger.
            if (existing.Meaning != incoming.Meaning) existing.Meaning = incoming.Meaning;
            if (existing.Locate != incoming.Locate) existing.Locate = incoming.Locate;
        }

        foreach (var m in wanted.Where(m => m.Id <= 0))
            _db.WordMeans.Add(new WordMeans { WordId = word.Id, Meaning = m.Meaning, Locate = m.Locate });
    }
}
