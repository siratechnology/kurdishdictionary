using Shared.Dtos;
using System.Net.Http.Json;

namespace frontend_blazor.Services;

public record SpeechTypeItem(int Id, string Kurdish);

/// <summary>
/// Talks to the words API. Every call goes through <see cref="ApiClient"/>, which attaches the
/// signed-in user's bearer token — that is what lets the backend record who made each change.
/// </summary>
public class WordService
{
    private readonly ApiClient _api;

    public WordService(ApiClient api) => _api = api;

    private Task<HttpClient> Api() => _api.CreateAsync();

    public async Task<PagedResultDto<WordDto>?> GetWordsAsync(
        int page = 1, int pageSize = 20,
        string? search = null, string? category = null)
    {
        var url = $"api/words?page={page}&pageSize={pageSize}";
        if (!string.IsNullOrWhiteSpace(search))
            url += $"&search={Uri.EscapeDataString(search)}";
        if (!string.IsNullOrWhiteSpace(category))
            url += $"&category={Uri.EscapeDataString(category)}";
        return await (await Api()).GetFromJsonAsync<PagedResultDto<WordDto>>(url);
    }

    public async Task<DashboardDto?> GetDashboardAsync() =>
        await (await Api()).GetFromJsonAsync<DashboardDto>("api/words/dashboard");

    public async Task<WordDto?> GetWordByIdAsync(int id) =>
        await (await Api()).GetFromJsonAsync<WordDto>($"api/words/{id}");

    public async Task<List<CategoryDto>> GetCategoriesAsync() =>
        await (await Api()).GetFromJsonAsync<List<CategoryDto>>("api/words/categories") ?? new();

    public async Task<CategoryDto?> CreateCategoryAsync(string name)
    {
        var res = await (await Api()).PostAsJsonAsync("api/words/categories", name);
        res.EnsureAuthorizedAndSuccess();
        return await res.Content.ReadFromJsonAsync<CategoryDto>();
    }

    public async Task<CategoryDto?> UpdateCategoryAsync(int id, string name)
    {
        var res = await (await Api()).PutAsJsonAsync($"api/words/categories/{id}", name);
        res.EnsureAuthorizedAndSuccess();
        return await res.Content.ReadFromJsonAsync<CategoryDto>();
    }

    public async Task DeleteCategoryAsync(int id)
    {
        var res = await (await Api()).DeleteAsync($"api/words/categories/{id}");
        res.EnsureAuthorizedAndSuccess();
    }

    public async Task<PagedResultDto<WordDto>?> GetCategoryWordsAsync(
        int categoryId, int page = 1, int pageSize = 30, string? search = null)
    {
        var url = $"api/words/categories/{categoryId}/words?page={page}&pageSize={pageSize}";
        if (!string.IsNullOrWhiteSpace(search))
            url += $"&search={Uri.EscapeDataString(search)}";
        return await (await Api()).GetFromJsonAsync<PagedResultDto<WordDto>>(url);
    }

    public async Task AddWordToCategoryAsync(int categoryId, int wordId)
    {
        var res = await (await Api()).PostAsync($"api/words/categories/{categoryId}/words/{wordId}", null);
        res.EnsureAuthorizedAndSuccess();
    }

    public async Task RemoveWordFromCategoryAsync(int categoryId, int wordId)
    {
        var res = await (await Api()).DeleteAsync($"api/words/categories/{categoryId}/words/{wordId}");
        res.EnsureAuthorizedAndSuccess();
    }

    public async Task<List<SpeechPaneStatDto>> GetSpeechTypeStatsAsync() =>
        await (await Api()).GetFromJsonAsync<List<SpeechPaneStatDto>>("api/words/speech-types/stats") ?? new();

    public async Task<PagedResultDto<WordDto>?> GetSpeechTypeWordsAsync(
        int typeId, int page = 1, int pageSize = 30, string? search = null)
    {
        var url = $"api/words/speech-types/{typeId}/words?page={page}&pageSize={pageSize}";
        if (!string.IsNullOrWhiteSpace(search))
            url += $"&search={Uri.EscapeDataString(search)}";
        return await (await Api()).GetFromJsonAsync<PagedResultDto<WordDto>>(url);
    }

    public async Task AddWordToSpeechTypeAsync(int typeId, int wordId)
    {
        var res = await (await Api()).PostAsync($"api/words/speech-types/{typeId}/words/{wordId}", null);
        res.EnsureAuthorizedAndSuccess();
    }

    public async Task RemoveWordFromSpeechTypeAsync(int typeId, int wordId)
    {
        var res = await (await Api()).DeleteAsync($"api/words/speech-types/{typeId}/words/{wordId}");
        res.EnsureAuthorizedAndSuccess();
    }

    public async Task<List<GenderItem>> GetGendersAsync()
    {
        var raw = await (await Api()).GetFromJsonAsync<List<GenderRaw>>("api/words/genders");
        return raw?.Select(r => new GenderItem(r.Id, r.Kurdish)).ToList() ?? new();
    }

    public async Task<List<string>> GetLocatesAsync() =>
        await (await Api()).GetFromJsonAsync<List<string>>("api/words/locates") ?? new();

    public async Task<List<SpeechTypeItem>> GetSpeechTypesAsync()
    {
        var raw = await (await Api()).GetFromJsonAsync<List<SpeechTypeRaw>>("api/words/speech-types");
        return raw?.Select(r => new SpeechTypeItem(r.Id, r.Kurdish)).ToList() ?? new();
    }

    public async Task<WordDto?> CreateWordAsync(CreateWordDto dto)
    {
        var res = await (await Api()).PostAsJsonAsync("api/words", dto);
        res.EnsureAuthorizedAndSuccess();
        return await res.Content.ReadFromJsonAsync<WordDto>();
    }

    public async Task<WordDto?> UpdateWordAsync(int id, UpdateWordDto dto)
    {
        var res = await (await Api()).PutAsJsonAsync($"api/words/{id}", dto);
        res.EnsureAuthorizedAndSuccess();
        return await res.Content.ReadFromJsonAsync<WordDto>();
    }

    public async Task DeleteWordAsync(int id)
    {
        var res = await (await Api()).DeleteAsync($"api/words/{id}");
        res.EnsureAuthorizedAndSuccess();
    }

    public async Task<PagedResultDto<AuditLogDto>?> GetAuditLogAsync(int page = 1, int pageSize = 50) =>
        await (await Api()).GetFromJsonAsync<PagedResultDto<AuditLogDto>>(
            $"api/words/audit?page={page}&pageSize={pageSize}");

    private record SpeechTypeRaw(int Id, string Kurdish);
    private record GenderRaw(int Id, string Kurdish);
}

public record GenderItem(int Id, string Kurdish);
