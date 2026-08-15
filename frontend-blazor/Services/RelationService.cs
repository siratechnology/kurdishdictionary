using System.Net.Http.Json;
using Shared.Dtos;

namespace frontend_blazor.Services;

/// <summary>
/// The relation workspace's client (پەیوەندییەکان).
///
/// Talks to the v3 endpoints, which speak WordRelation / SenseRelation over the eleven seeded
/// types — not the legacy RelatedWord strings the old screen sent through UpdateWordAsync. That
/// old path resent the whole word on every relation change, so a dropped field read to the server
/// as a deletion; here a relation is its own resource and a write touches nothing else.
///
/// Refusals come back as the server's own sentence. They are all worth reading — "a word cannot
/// relate to itself", "that type is semantic, not morphological" — and swallowing them would
/// leave someone staring at a button that does nothing.
/// </summary>
public class RelationService
{
    private readonly ApiClient _api;

    public RelationService(ApiClient api) => _api = api;

    private Task<HttpClient> Api() => _api.CreateAsync();

    public async Task<List<RelationTypeDto>> GetTypesAsync() =>
        await (await Api()).GetFromJsonAsync<List<RelationTypeDto>>("api/relations/types") ?? new();

    public async Task<RelationCoverageDto?> GetCoverageAsync() =>
        await (await Api()).GetFromJsonAsync<RelationCoverageDto>("api/relations/coverage");

    /// <summary>The nth word in the walk, 1-based. The server clamps, so running off either end is safe.</summary>
    public async Task<WordRelationsDto?> GetAtAsync(int position) =>
        await (await Api()).GetFromJsonAsync<WordRelationsDto>($"api/relations/at/{position}");

    public async Task<WordRelationsDto?> GetWordAsync(int wordId) =>
        await (await Api()).GetFromJsonAsync<WordRelationsDto>($"api/relations/word/{wordId}");

    /// <summary>Position of the next word with no relations at all, or null when none is left.</summary>
    public async Task<int?> NextMissingAsync(int after) =>
        await (await Api()).GetFromJsonAsync<int?>($"api/relations/next-missing?after={after}");

    /// <summary>
    /// One page of candidate targets. Paged rather than top-N so the list can be browsed, not just
    /// searched — the count comes back with it so the pager knows whether there is a next page.
    /// </summary>
    public async Task<PagedResultDto<RelationTargetDto>?> SearchAsync(
        string? query, int excludeWordId, int page = 1, int pageSize = 20)
    {
        var url = $"api/relations/search?exclude={excludeWordId}&page={page}&pageSize={pageSize}";
        if (!string.IsNullOrWhiteSpace(query)) url += $"&q={Uri.EscapeDataString(query)}";

        return await (await Api()).GetFromJsonAsync<PagedResultDto<RelationTargetDto>>(url);
    }

    /// <summary>Returns the word's refreshed workspace, so the caller never has to guess what changed.</summary>
    public async Task<WordRelationsDto?> AddAsync(AddRelationDto dto)
    {
        var response = await (await Api()).PostAsJsonAsync("api/relations", dto);

        if (!response.IsSuccessStatusCode)
            throw new InvalidOperationException(await Message(response));

        return await response.Content.ReadFromJsonAsync<WordRelationsDto>();
    }

    public async Task<WordRelationsDto?> RemoveAsync(RelationEdgeDto edge, int wordId)
    {
        var scope = edge.Scope == "sense" ? "sense" : "word";
        var response = await (await Api()).DeleteAsync($"api/relations/{scope}/{edge.Id}?wordId={wordId}");

        if (!response.IsSuccessStatusCode)
            throw new InvalidOperationException(await Message(response));

        return await response.Content.ReadFromJsonAsync<WordRelationsDto>();
    }

    private static async Task<string> Message(HttpResponseMessage response)
    {
        var body = (await response.Content.ReadAsStringAsync()).Trim().Trim('"');
        return string.IsNullOrWhiteSpace(body) ? "کردارەکە سەرنەکەوت." : body;
    }
}
