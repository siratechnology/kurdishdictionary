using Shared.Dtos;
using System.Net.Http.Json;

namespace frontend_blazor.Services;

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

    public async Task<List<string>> GetCategoriesAsync() =>
        await _http.GetFromJsonAsync<List<string>>("api/words/categories") ?? new();

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
}
