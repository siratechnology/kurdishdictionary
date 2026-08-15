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

    /// <summary>
    /// The word list. <paramref name="search"/> is a PREFIX on the server — typing «س» returns
    /// words beginning with س, not every word containing it.
    /// </summary>
    /// <param name="missing">
    /// One of "meaning", "category", "pane", "relation" — the same four gaps the dashboard's
    /// alert rail counts, so an alert can link straight to the rows behind it.
    /// </param>
    /// <param name="partOfSpeech">v3 PartOfSpeech id — NOT the legacy SpeechPaneType.</param>
    /// <param name="axis">Narrows to words with a sense carrying this axis.</param>
    /// <param name="values">
    /// The cascade's answers, one value id per level drilled into. ANDed by the server against a
    /// single sense: a sub-answer only means anything alongside the answer that opened its group.
    /// A list rather than one id because the options tree has no fixed depth.
    /// </param>
    public async Task<PagedResultDto<WordDto>?> GetWordsAsync(
        int page = 1, int pageSize = 20,
        string? search = null, string? category = null, string? missing = null,
        int? partOfSpeech = null, int? axis = null, IEnumerable<int>? values = null)
    {
        var url = $"api/words?page={page}&pageSize={pageSize}";
        if (!string.IsNullOrWhiteSpace(search))
            url += $"&search={Uri.EscapeDataString(search)}";
        if (!string.IsNullOrWhiteSpace(category))
            url += $"&category={Uri.EscapeDataString(category)}";
        if (!string.IsNullOrWhiteSpace(missing))
            url += $"&missing={Uri.EscapeDataString(missing)}";
        if (partOfSpeech is { } pos) url += $"&partOfSpeech={pos}";
        if (axis is { } a) url += $"&axis={a}";

        var ids = values?.Where(v => v > 0).Distinct().ToList();
        if (ids is { Count: > 0 }) url += $"&values={string.Join(',', ids)}";

        return await (await Api()).GetFromJsonAsync<PagedResultDto<WordDto>>(url);
    }

    /// <summary>
    /// The word's grammatical classification — one answer for the whole word, stored across all
    /// of its senses. See WordClassificationDto for why the UI asks once and the storage does not.
    /// </summary>
    public async Task<WordClassificationDto?> GetClassificationAsync(int wordId) =>
        await (await Api()).GetFromJsonAsync<WordClassificationDto>($"api/words/{wordId}/classification");

    public async Task<WordClassificationDto?> SaveClassificationAsync(int wordId, WordClassificationDto dto)
    {
        var res = await (await Api()).PutAsJsonAsync($"api/words/{wordId}/classification", dto);
        res.EnsureAuthorizedAndSuccess();
        return await res.Content.ReadFromJsonAsync<WordClassificationDto>();
    }

    // ── Senses (schema v3) ─────────────────────────────────────────────────
    // What the editor actually writes. See WordSenseDto for why this is not WordMeans.

    public async Task<WordSensesDto?> GetSensesAsync(int wordId) =>
        await (await Api()).GetFromJsonAsync<WordSensesDto>($"api/words/{wordId}/senses");

    public async Task<WordSensesDto?> SaveSensesAsync(int wordId, List<WordSenseDto> senses)
    {
        var res = await (await Api()).PutAsJsonAsync(
            $"api/words/{wordId}/senses", new SaveWordSensesDto { Senses = senses });

        res.EnsureAuthorizedAndSuccess();
        return await res.Content.ReadFromJsonAsync<WordSensesDto>();
    }

    /// <summary>
    /// How fast the work is going and when it lands. Measured from the audit log and the word
    /// table, so a quiet fortnight lengthens the estimate without anyone editing a target.
    /// </summary>
    public async Task<PaceDto?> GetPaceAsync(int windowDays = 14, double hoursPerDay = 6) =>
        await (await Api()).GetFromJsonAsync<PaceDto>(
            $"api/words/pace?windowDays={windowDays}&hoursPerDay={hoursPerDay}");

    public async Task<DashboardDto?> GetDashboardAsync() =>
        await (await Api()).GetFromJsonAsync<DashboardDto>("api/words/dashboard");

    public async Task<WordDto?> GetWordByIdAsync(int id) =>
        await (await Api()).GetFromJsonAsync<WordDto>($"api/words/{id}");

    // ── بەشی فەرهەنگ ───────────────────────────────────────────────────────

    public async Task<List<DictionarySectionDto>> GetDictionarySectionsAsync() =>
        await (await Api()).GetFromJsonAsync<List<DictionarySectionDto>>("api/words/dictionary-sections") ?? new();

    /// <summary>
    /// Creates a section, or returns the existing one if the folded name already matches. The
    /// editor calls this when somebody types a section that is not in the list yet, so a word
    /// from a new field can be recorded without leaving the form.
    /// </summary>
    public async Task<DictionarySectionDto?> CreateDictionarySectionAsync(string name)
    {
        var res = await (await Api()).PostAsJsonAsync("api/words/dictionary-sections", new { Name = name });
        res.EnsureAuthorizedAndSuccess();
        return await res.Content.ReadFromJsonAsync<DictionarySectionDto>();
    }

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
