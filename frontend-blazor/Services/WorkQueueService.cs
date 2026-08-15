using System.Net.Http.Json;
using Microsoft.Extensions.Caching.Memory;
using Shared.Dtos;

namespace frontend_blazor.Services;

/// <summary>Reads ڕیزی کار — what is incomplete, and where to go to fix it.</summary>
public class WorkQueueService
{
    /// <summary>
    /// The sidebar badge is rendered by MainLayout, which is NOT interactive — it re-renders on
    /// every navigation. Without this, each page load would run the summary's six aggregate counts
    /// again just to redraw a number that changes a few times an hour.
    ///
    /// Short enough that a teacher finishing a batch sees the count drop almost immediately;
    /// long enough that clicking through five pages costs one query, not thirty.
    /// </summary>
    private static readonly TimeSpan SummaryTtl = TimeSpan.FromSeconds(60);
    private const string SummaryCacheKey = "work-queue-summary";

    private readonly ApiClient _api;
    private readonly IMemoryCache _cache;

    public WorkQueueService(ApiClient api, IMemoryCache cache)
    {
        _api = api;
        _cache = cache;
    }

    /// <param name="fresh">
    /// True on the queue page itself and after a save — someone looking straight at the number
    /// should never be shown a cached one.
    /// </param>
    public async Task<WorkQueueDto?> GetSummaryAsync(bool fresh = false)
    {
        if (!fresh && _cache.TryGetValue(SummaryCacheKey, out WorkQueueDto? cached))
            return cached;

        var http = await _api.CreateAsync();
        var response = await http.GetAsync("api/work-queue");
        response.EnsureAuthorizedAndSuccess();

        var summary = await response.Content.ReadFromJsonAsync<WorkQueueDto>();
        _cache.Set(SummaryCacheKey, summary, SummaryTtl);

        return summary;
    }

    public async Task<List<WorkQueueItemDto>> GetItemsAsync(WorkQueueBucket bucket, int take = 50)
    {
        var http = await _api.CreateAsync();
        var response = await http.GetAsync($"api/work-queue/{(int)bucket}?take={take}");
        response.EnsureAuthorizedAndSuccess();
        return await response.Content.ReadFromJsonAsync<List<WorkQueueItemDto>>() ?? new();
    }
}
