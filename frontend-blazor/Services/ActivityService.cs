using System.Net.Http.Json;
using System.Text.Json;
using Shared.Dtos;

namespace frontend_blazor.Services;

/// <summary>Reads the audit trail — both the full activity page and the notification bell.</summary>
public class ActivityService
{
    private static readonly JsonSerializerOptions ChangeJson = new() { PropertyNameCaseInsensitive = true };

    private readonly ApiClient _api;

    public ActivityService(ApiClient api) => _api = api;

    public async Task<PagedResultDto<AuditLogDto>?> GetAsync(
        int page = 1,
        int pageSize = 50,
        string? action = null,
        string? entityType = null,
        string? search = null)
    {
        var url = $"api/words/audit?page={page}&pageSize={pageSize}";
        if (!string.IsNullOrWhiteSpace(action)) url += $"&action={Uri.EscapeDataString(action)}";
        if (!string.IsNullOrWhiteSpace(entityType)) url += $"&entityType={Uri.EscapeDataString(entityType)}";
        if (!string.IsNullOrWhiteSpace(search)) url += $"&search={Uri.EscapeDataString(search)}";

        var http = await _api.CreateAsync();
        var response = await http.GetAsync(url);
        response.EnsureAuthorizedAndSuccess();
        return await response.Content.ReadFromJsonAsync<PagedResultDto<AuditLogDto>>();
    }

    /// <summary>Everything that happened after the id the bell last showed.</summary>
    public async Task<List<AuditLogDto>> GetSinceAsync(int afterId, int take = 20)
    {
        var http = await _api.CreateAsync();
        var response = await http.GetAsync($"api/words/audit/since/{afterId}?take={take}");
        response.EnsureAuthorizedAndSuccess();
        return await response.Content.ReadFromJsonAsync<List<AuditLogDto>>() ?? new();
    }

    /// <summary>Decodes the Changes JSON into field-level diffs. Bad JSON yields nothing rather than throwing.</summary>
    public static List<AuditChangeDto> ParseChanges(string? changes)
    {
        if (string.IsNullOrWhiteSpace(changes)) return new();

        try
        {
            return JsonSerializer.Deserialize<List<AuditChangeDto>>(changes, ChangeJson) ?? new();
        }
        catch (JsonException)
        {
            return new();
        }
    }
}
