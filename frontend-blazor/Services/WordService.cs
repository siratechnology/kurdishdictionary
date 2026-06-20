using Shared.Dtos;
using System.Net.Http.Json;

namespace frontend_blazor.Services;

public record SpeechTypeItem(int Id, string Kurdish);

public class WordService
{
    private readonly HttpClient _http;

    public WordService(HttpClient http) => _http = http;

    public async Task<PagedResultDto<WordDto>?> GetWordsAsync(
        int page = 1, int pageSize = 20,
        string? search = null, string? category = null)
    {
        var url = $"api/words?page={page}&pageSize={pageSize}";
        if (!string.IsNullOrWhiteSpace(search))
            url += $"&search={Uri.EscapeDataString(search)}";
        if (!string.IsNullOrWhiteSpace(category))
            url += $"&category={Uri.EscapeDataString(category)}";
        return await _http.GetFromJsonAsync<PagedResultDto<WordDto>>(url);
    }

    public async Task<DashboardDto?> GetDashboardAsync() =>
        await _http.GetFromJsonAsync<DashboardDto>("api/words/dashboard");

    public async Task<WordDto?> GetWordByIdAsync(int id) =>
        await _http.GetFromJsonAsync<WordDto>($"api/words/{id}");

    public async Task<List<CategoryDto>> GetCategoriesAsync() =>
        await _http.GetFromJsonAsync<List<CategoryDto>>("api/words/categories") ?? new();

    public async Task<CategoryDto?> CreateCategoryAsync(string name)
    {
        var res = await _http.PostAsJsonAsync("api/words/categories", name);
        res.EnsureSuccessStatusCode();
        return await res.Content.ReadFromJsonAsync<CategoryDto>();
    }

    public async Task<CategoryDto?> UpdateCategoryAsync(int id, string name)
    {
        var res = await _http.PutAsJsonAsync($"api/words/categories/{id}", name);
        res.EnsureSuccessStatusCode();
        return await res.Content.ReadFromJsonAsync<CategoryDto>();
    }

    public async Task DeleteCategoryAsync(int id)
    {
        var res = await _http.DeleteAsync($"api/words/categories/{id}");
        res.EnsureSuccessStatusCode();
    }

    public async Task<PagedResultDto<WordDto>?> GetCategoryWordsAsync(
        int categoryId, int page = 1, int pageSize = 30, string? search = null)
    {
        var url = $"api/words/categories/{categoryId}/words?page={page}&pageSize={pageSize}";
        if (!string.IsNullOrWhiteSpace(search))
            url += $"&search={Uri.EscapeDataString(search)}";
        return await _http.GetFromJsonAsync<PagedResultDto<WordDto>>(url);
    }

    public async Task AddWordToCategoryAsync(int categoryId, int wordId)
    {
        var res = await _http.PostAsync($"api/words/categories/{categoryId}/words/{wordId}", null);
        res.EnsureSuccessStatusCode();
    }

    public async Task RemoveWordFromCategoryAsync(int categoryId, int wordId)
    {
        var res = await _http.DeleteAsync($"api/words/categories/{categoryId}/words/{wordId}");
        res.EnsureSuccessStatusCode();
    }

    public async Task<List<SpeechPaneStatDto>> GetSpeechTypeStatsAsync() =>
        await _http.GetFromJsonAsync<List<SpeechPaneStatDto>>("api/words/speech-types/stats") ?? new();

    public async Task<PagedResultDto<WordDto>?> GetSpeechTypeWordsAsync(
        int typeId, int page = 1, int pageSize = 30, string? search = null)
    {
        var url = $"api/words/speech-types/{typeId}/words?page={page}&pageSize={pageSize}";
        if (!string.IsNullOrWhiteSpace(search))
            url += $"&search={Uri.EscapeDataString(search)}";
        return await _http.GetFromJsonAsync<PagedResultDto<WordDto>>(url);
    }

    public async Task AddWordToSpeechTypeAsync(int typeId, int wordId)
    {
        var res = await _http.PostAsync($"api/words/speech-types/{typeId}/words/{wordId}", null);
        res.EnsureSuccessStatusCode();
    }

    public async Task RemoveWordFromSpeechTypeAsync(int typeId, int wordId)
    {
        var res = await _http.DeleteAsync($"api/words/speech-types/{typeId}/words/{wordId}");
        res.EnsureSuccessStatusCode();
    }

    public async Task<List<GenderItem>> GetGendersAsync()
    {
        var raw = await _http.GetFromJsonAsync<List<GenderRaw>>("api/words/genders");
        return raw?.Select(r => new GenderItem(r.Id, r.Kurdish)).ToList() ?? new();
    }

    public async Task<List<string>> GetLocatesAsync() =>
        await _http.GetFromJsonAsync<List<string>>("api/words/locates") ?? new();

    public async Task<List<SpeechTypeItem>> GetSpeechTypesAsync()
    {
        var raw = await _http.GetFromJsonAsync<List<SpeechTypeRaw>>("api/words/speech-types");
        return raw?.Select(r => new SpeechTypeItem(r.Id, r.Kurdish)).ToList() ?? new();
    }

    public async Task<WordDto?> CreateWordAsync(CreateWordDto dto)
    {
        var res = await _http.PostAsJsonAsync("api/words", dto);
        res.EnsureSuccessStatusCode();
        return await res.Content.ReadFromJsonAsync<WordDto>();
    }

    public async Task<WordDto?> UpdateWordAsync(int id, UpdateWordDto dto)
    {
        var res = await _http.PutAsJsonAsync($"api/words/{id}", dto);
        res.EnsureSuccessStatusCode();
        return await res.Content.ReadFromJsonAsync<WordDto>();
    }

    public async Task DeleteWordAsync(int id) =>
        await _http.DeleteAsync($"api/words/{id}");

    public async Task<PagedResultDto<AuditLogDto>?> GetAuditLogAsync(int page = 1, int pageSize = 50) =>
        await _http.GetFromJsonAsync<PagedResultDto<AuditLogDto>>($"api/words/audit?page={page}&pageSize={pageSize}");

    private record SpeechTypeRaw(int Id, string Kurdish);
    private record GenderRaw(int Id, string Kurdish);
}

public record GenderItem(int Id, string Kurdish);
