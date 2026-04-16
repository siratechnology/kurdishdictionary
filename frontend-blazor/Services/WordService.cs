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

    public async Task<WordDto?> GetWordByIdAsync(int id) =>
        await _http.GetFromJsonAsync<WordDto>($"api/words/{id}");

    public async Task<List<string>> GetCategoriesAsync() =>
        await _http.GetFromJsonAsync<List<string>>("api/words/categories") ?? new();

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

    private record SpeechTypeRaw(int Id, string Kurdish);
}
