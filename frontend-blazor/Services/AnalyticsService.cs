using System.Net.Http.Json;
using Shared.Dtos;

namespace frontend_blazor.Services;

public class AnalyticsService
{
    private readonly ApiClient _api;

    public AnalyticsService(ApiClient api) => _api = api;

    public async Task<AnalyticsSummaryDto?> GetSummaryAsync(int days = 14)
    {
        var http = await _api.CreateAsync();
        var response = await http.GetAsync($"api/analytics/summary?days={days}");
        response.EnsureAuthorizedAndSuccess();
        return await response.Content.ReadFromJsonAsync<AnalyticsSummaryDto>();
    }

    /// <summary>Poll for events newer than the last id we rendered. Pass 0 on first load.</summary>
    public async Task<List<VisitorEventDto>> GetLiveAsync(long afterId = 0)
    {
        var http = await _api.CreateAsync();
        var response = await http.GetAsync($"api/analytics/live?afterId={afterId}");
        response.EnsureAuthorizedAndSuccess();
        return await response.Content.ReadFromJsonAsync<List<VisitorEventDto>>() ?? new();
    }
}
