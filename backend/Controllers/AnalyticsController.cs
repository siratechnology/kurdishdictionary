using backend.Data;
using backend.Data.Models;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos;

namespace backend.Controllers;

[ApiController]
[Route("api/analytics")]
public class AnalyticsController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IGeoLocationService _geo;

    /// <summary>A visitor seen within this window counts as "on the site right now".</summary>
    private static readonly TimeSpan ActiveWindow = TimeSpan.FromMinutes(5);

    public AnalyticsController(AppDbContext db, IGeoLocationService geo)
    {
        _db = db;
        _geo = geo;
    }

    /// <summary>
    /// Ingestion endpoint for the public Next.js site. Anonymous by necessity — visitors have no
    /// account. The client's claims about itself (screen size, language) are stored as-is; the
    /// things that matter for trust (IP, geo, time) are taken from the request, not the payload.
    /// </summary>
    // POST api/analytics/event
    [HttpPost("event")]
    [AllowAnonymous]
    public async Task<IActionResult> Track([FromBody] TrackEventDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.EventType))
            return BadRequest("EventType is required");

        var ip = ClientInfo.GetClientIp(Request);
        var ua = ClientInfo.GetUserAgent(Request);
        var (browser, os, device) = UserAgentParser.Parse(ua);

        // Cloudflare hands us the country for free; ip-api fills in the city (cached per IP).
        var geo = await _geo.LookupAsync(ip);
        var cfCountry = Request.Headers["CF-IPCountry"].FirstOrDefault();

        _db.AnalyticsEvents.Add(new AnalyticsEvent
        {
            EventType = dto.EventType,
            Path = Truncate(dto.Path, 512),
            SearchTerm = Truncate(dto.SearchTerm?.Trim(), 200),
            ResultCount = dto.ResultCount,
            WordId = dto.WordId,
            SessionId = Truncate(dto.SessionId, 64),
            Referrer = Truncate(dto.Referrer, 512),

            IpAddress = Truncate(ip, 64),
            Country = geo.Country,
            CountryCode = geo.CountryCode ?? cfCountry,
            City = geo.City,
            Region = geo.Region,
            Isp = Truncate(geo.Isp, 200),
            Timezone = geo.Timezone,

            UserAgent = Truncate(ua, 512),
            Browser = browser,
            Os = os,
            DeviceType = device,
            ScreenWidth = dto.ScreenWidth,
            ScreenHeight = dto.ScreenHeight,
            ViewportWidth = dto.ViewportWidth,
            ViewportHeight = dto.ViewportHeight,
            Language = Truncate(dto.Language, 32),

            CreatedAt = DateTime.UtcNow,
        });

        await _db.SaveChangesAsync();

        // 204: the browser is using sendBeacon and will not read a body.
        return NoContent();
    }

    // GET api/analytics/summary?days=14
    [HttpGet("summary")]
    [Authorize(Roles = Roles.Any)]
    public async Task<ActionResult<AnalyticsSummaryDto>> Summary([FromQuery] int days = 14)
    {
        days = Math.Clamp(days, 1, 90);

        var now = DateTime.UtcNow;
        var since = now.AddDays(-days);
        var todayStart = now.Date;
        var activeSince = now - ActiveWindow;

        var events = _db.AnalyticsEvents.AsNoTracking();
        var window = events.Where(e => e.CreatedAt >= since);
        var searches = window.Where(e => e.EventType == AnalyticsEventTypes.Search
                                         && e.SearchTerm != null && e.SearchTerm != "");

        var dto = new AnalyticsSummaryDto
        {
            TotalVisits = await events.CountAsync(e => e.EventType == AnalyticsEventTypes.PageView),
            VisitsToday = await events.CountAsync(e => e.EventType == AnalyticsEventTypes.PageView
                                                       && e.CreatedAt >= todayStart),
            UniqueVisitorsToday = await events.Where(e => e.CreatedAt >= todayStart && e.SessionId != null)
                                              .Select(e => e.SessionId)
                                              .Distinct()
                                              .CountAsync(),
            TotalSearches = await events.CountAsync(e => e.EventType == AnalyticsEventTypes.Search),
            SearchesToday = await events.CountAsync(e => e.EventType == AnalyticsEventTypes.Search
                                                         && e.CreatedAt >= todayStart),
            ActiveNow = await events.Where(e => e.CreatedAt >= activeSince && e.SessionId != null)
                                    .Select(e => e.SessionId)
                                    .Distinct()
                                    .CountAsync(),
            EmptySearches = await searches.CountAsync(e => e.ResultCount == 0),
        };

        dto.DailyVisits = await DailyAsync(window.Where(e => e.EventType == AnalyticsEventTypes.PageView), days);
        dto.DailySearches = await DailyAsync(window.Where(e => e.EventType == AnalyticsEventTypes.Search), days);

        dto.TopSearches = await searches
            .GroupBy(e => e.SearchTerm!)
            .Select(g => new SearchTermDto
            {
                Term = g.Key,
                Count = g.Count(),
                LastResultCount = g.OrderByDescending(e => e.CreatedAt).First().ResultCount,
            })
            .OrderByDescending(s => s.Count)
            .Take(20)
            .ToListAsync();

        // Searches that found nothing: the shortlist of words the dictionary is missing.
        dto.FailedSearches = await searches
            .Where(e => e.ResultCount == 0)
            .GroupBy(e => e.SearchTerm!)
            .Select(g => new SearchTermDto { Term = g.Key, Count = g.Count(), LastResultCount = 0 })
            .OrderByDescending(s => s.Count)
            .Take(20)
            .ToListAsync();

        dto.TopCountries = await TopAsync(window, e => e.Country);
        dto.TopCities = await TopAsync(window, e => e.City);
        dto.Devices = await TopAsync(window, e => e.DeviceType);
        dto.Browsers = await TopAsync(window, e => e.Browser);
        dto.OperatingSystems = await TopAsync(window, e => e.Os);
        dto.TopPages = await TopAsync(window.Where(e => e.EventType == AnalyticsEventTypes.PageView), e => e.Path);
        dto.TopReferrers = await TopAsync(window, e => e.Referrer);

        dto.RecentEvents = await events
            .OrderByDescending(e => e.Id)
            .Take(100)
            .Select(e => MapEvent(e))
            .ToListAsync();

        return Ok(dto);
    }

    /// <summary>The live feed polls this — cheap, keyset-paged on Id so it never re-reads the table.</summary>
    // GET api/analytics/live?afterId=123
    [HttpGet("live")]
    [Authorize(Roles = Roles.Any)]
    public async Task<ActionResult<List<VisitorEventDto>>> Live([FromQuery] long afterId = 0)
    {
        var query = _db.AnalyticsEvents.AsNoTracking();

        // afterId = 0 means "first load": give the latest page rather than the entire history.
        var items = afterId > 0
            ? await query.Where(e => e.Id > afterId)
                         .OrderBy(e => e.Id)
                         .Take(100)
                         .Select(e => MapEvent(e))
                         .ToListAsync()
            : await query.OrderByDescending(e => e.Id)
                         .Take(50)
                         .Select(e => MapEvent(e))
                         .ToListAsync();

        return Ok(items);
    }

    private static async Task<List<DailyCountDto>> DailyAsync(IQueryable<AnalyticsEvent> query, int days)
    {
        var counts = await query
            .GroupBy(e => e.CreatedAt.Date)
            .Select(g => new { Date = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Date, x => x.Count);

        // Fill the gaps: a day with no traffic must plot as 0, not vanish from the chart.
        var today = DateTime.UtcNow.Date;
        return Enumerable.Range(0, days)
            .Select(i => today.AddDays(-(days - 1 - i)))
            .Select(d => new DailyCountDto { Date = d, Count = counts.GetValueOrDefault(d) })
            .ToList();
    }

    private static async Task<List<NameCountDto>> TopAsync(
        IQueryable<AnalyticsEvent> query,
        System.Linq.Expressions.Expression<Func<AnalyticsEvent, string?>> selector,
        int take = 10)
    {
        return await query
            .Select(selector)
            .Where(v => v != null && v != "")
            .GroupBy(v => v!)
            .Select(g => new NameCountDto { Name = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(take)
            .ToListAsync();
    }

    private static VisitorEventDto MapEvent(AnalyticsEvent e) => new()
    {
        Id = e.Id,
        EventType = e.EventType,
        Path = e.Path,
        SearchTerm = e.SearchTerm,
        ResultCount = e.ResultCount,
        WordId = e.WordId,
        SessionId = e.SessionId,
        IpAddress = e.IpAddress,
        Country = e.Country,
        CountryCode = e.CountryCode,
        City = e.City,
        Region = e.Region,
        Isp = e.Isp,
        Browser = e.Browser,
        Os = e.Os,
        DeviceType = e.DeviceType,
        ScreenWidth = e.ScreenWidth,
        ScreenHeight = e.ScreenHeight,
        Language = e.Language,
        Referrer = e.Referrer,
        UserAgent = e.UserAgent,
        CreatedAt = e.CreatedAt,
    };

    private static string? Truncate(string? value, int max) =>
        string.IsNullOrEmpty(value) || value.Length <= max ? value : value[..max];
}
