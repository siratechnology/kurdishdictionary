using System.Net;
using System.Net.Http.Json;
using Shared.Dtos;

namespace frontend_blazor.Services;

/// <summary>
/// The taxonomy settings screen's client (پڕۆمپت ١١).
///
/// Every call returns the server's own message on refusal rather than a generic failure: the
/// refusals here are all meaningful — "484 senses use this", "that rule closes a cycle" — and
/// swallowing them would leave the user staring at a button that does nothing.
/// </summary>
public class TaxonomyAdminService
{
    private readonly ApiClient _api;

    public TaxonomyAdminService(ApiClient api) => _api = api;

    public async Task<TaxonomyOverviewDto?> GetOverviewAsync()
    {
        var http = await _api.CreateAsync();
        var response = await http.GetAsync("api/taxonomy-admin/overview");
        response.EnsureAuthorizedAndSuccess();
        return await response.Content.ReadFromJsonAsync<TaxonomyOverviewDto>();
    }

    public async Task<List<TaxonomyValueDto>> GetValuesAsync(int axisId)
    {
        var http = await _api.CreateAsync();
        var response = await http.GetAsync($"api/taxonomy-admin/axes/{axisId}/values");
        response.EnsureAuthorizedAndSuccess();
        return await response.Content.ReadFromJsonAsync<List<TaxonomyValueDto>>() ?? new();
    }

    public Task<string?> RenameAxisAsync(int axisId, string name, string? description) =>
        SendAsync(HttpMethod.Put, $"api/taxonomy-admin/axes/{axisId}/name", new { Name = name, Description = description });

    public Task<string?> RenameValueAsync(int valueId, string name) =>
        SendAsync(HttpMethod.Put, $"api/taxonomy-admin/values/{valueId}/name", new { Name = name, Description = (string?)null });

    public Task<string?> RenamePartOfSpeechAsync(int id, string name) =>
        SendAsync(HttpMethod.Put, $"api/taxonomy-admin/parts-of-speech/{id}/name", new { Name = name, Description = (string?)null });

    public Task<string?> AddAxisAsync(string name, string reason) =>
        SendAsync(HttpMethod.Post, "api/taxonomy-admin/axes", new { Name = name, Code = (string?)null, Reason = reason });

    public Task<string?> AddValueAsync(int axisId, string name, string? code) =>
        SendAsync(HttpMethod.Post, "api/taxonomy-admin/values", new { AxisId = axisId, Name = name, Code = code });

    public Task<string?> SetValueActiveAsync(int valueId, bool active) =>
        SendAsync(HttpMethod.Put, $"api/taxonomy-admin/values/{valueId}/active?active={active}", null);

    /// <summary>
    /// Hard delete. The server refuses if the value is used by any sense or referenced by a
    /// conditional rule, and returns the count in the message — so a refusal here is a real
    /// answer ("484 senses use this") rather than a failure to surface.
    ///
    /// Deactivating is the usual move; this exists for values created by mistake, which have
    /// no usage by definition and should not clutter the list forever.
    /// </summary>
    public Task<string?> DeleteValueAsync(int valueId) =>
        SendAsync(HttpMethod.Delete, $"api/taxonomy-admin/values/{valueId}", null);

    public Task<string?> SetAssignmentAsync(int partOfSpeechId, int axisId, bool assigned, bool isRequired) =>
        SendAsync(HttpMethod.Post, "api/taxonomy-admin/assignments",
            new { PartOfSpeechId = partOfSpeechId, AxisId = axisId, Assigned = assigned, IsRequired = isRequired });

    public Task<string?> SetConditionAsync(int assignmentId, int? requiresValueId) =>
        SendAsync(HttpMethod.Put, $"api/taxonomy-admin/assignments/{assignmentId}/condition",
            new { RequiresValueId = requiresValueId });

    // ═══════════════════════════════════════════════════════════════════════
    // The options tree — بەشی ئاخاوتن ← تەوەر ← نرخ ← تەوەر ← …
    // ═══════════════════════════════════════════════════════════════════════

    public async Task<TaxonomyTreeDto?> GetTreeAsync(int partOfSpeechId)
    {
        var http = await _api.CreateAsync();
        var response = await http.GetAsync($"api/taxonomy-admin/tree/{partOfSpeechId}");

        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<TaxonomyTreeDto>();
    }

    /// <summary>
    /// The real entry form the current tree produces, for the preview beside the editor.
    ///
    /// Resolved on the SERVER against the same rule the real form uses. Reimplementing the cascade
    /// in the preview would make it a confident lie the first time the rule changed, which is worse
    /// than having no preview at all.
    /// </summary>
    public async Task<List<StationAxisDto>> PreviewFormAsync(int partOfSpeechId, IEnumerable<int> selectedValueIds)
    {
        var http = await _api.CreateAsync();

        var response = await http.PostAsJsonAsync(
            $"api/taxonomy-admin/tree/{partOfSpeechId}/preview", selectedValueIds.ToList());

        if (!response.IsSuccessStatusCode) return new();
        return await response.Content.ReadFromJsonAsync<List<StationAxisDto>>() ?? new();
    }

    /// <summary>«بژاردەی زیاتر زیاد بکە» — one call, at any depth. Null parent adds a first-level group.</summary>
    public Task<string?> AddGroupAsync(int partOfSpeechId, int? parentValueId, string name) =>
        SendAsync(HttpMethod.Post, "api/taxonomy-admin/tree/groups",
            new { PartOfSpeechId = partOfSpeechId, ParentValueId = parentValueId, Name = name });

    public async Task<SelectionCapPreviewDto?> PreviewSelectionCapAsync(int axisId, int? max)
    {
        var http = await _api.CreateAsync();

        var query = max is { } m ? $"?max={m}" : "";
        var response = await http.GetAsync($"api/taxonomy-admin/tree/axes/{axisId}/selection-cap{query}");

        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<SelectionCapPreviewDto>();
    }

    /// <summary>
    /// The plain-language question a teacher reads above the dropdown. Pass null or blank to clear it
    /// — the form then falls back to the technical label, which is a supported outcome, not an error.
    /// </summary>
    public Task<string?> SetPromptAsync(int axisId, string? prompt) =>
        SendAsync(HttpMethod.Put, $"api/taxonomy-admin/tree/axes/{axisId}/prompt", new { Prompt = prompt });

    /// <summary>The muted worked example beside one option. Blank clears it.</summary>
    public Task<string?> SetOptionHintAsync(int valueId, string? hint) =>
        SendAsync(HttpMethod.Put, $"api/taxonomy-admin/values/{valueId}/hint", new { Hint = hint });

    public Task<string?> SetSelectionModeAsync(int axisId, int minSelections, int? maxSelections) =>
        SendAsync(HttpMethod.Put, $"api/taxonomy-admin/tree/axes/{axisId}/selection-mode",
            new { MinSelections = minSelections, MaxSelections = maxSelections });

    public Task<string?> ReorderSiblingsAsync(int partOfSpeechId, int? parentValueId, List<int> orderedAssignmentIds) =>
        SendAsync(HttpMethod.Put, "api/taxonomy-admin/tree/order",
            new { PartOfSpeechId = partOfSpeechId, ParentValueId = parentValueId, OrderedAssignmentIds = orderedAssignmentIds });

    public Task<string?> SetAxisActiveAsync(int axisId, bool active) =>
        SendAsync(HttpMethod.Put, $"api/taxonomy-admin/tree/axes/{axisId}/active?active={active}", null);

    public Task<string?> RemoveGroupAsync(int partOfSpeechId, int axisId) =>
        SendAsync(HttpMethod.Delete, $"api/taxonomy-admin/tree/{partOfSpeechId}/axes/{axisId}", null);

    public Task<string?> ReparentAsync(int assignmentId, int? parentValueId) =>
        SendAsync(HttpMethod.Put, $"api/taxonomy-admin/tree/assignments/{assignmentId}/parent",
            new { ParentValueId = parentValueId });

    public async Task<MergePreviewDto?> PreviewMergeAsync(int source, int target)
    {
        var http = await _api.CreateAsync();
        var response = await http.GetAsync($"api/taxonomy-admin/merge/preview?source={source}&target={target}");

        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<MergePreviewDto>();
    }

    public Task<string?> MergeAsync(int source, int target, string? reason) =>
        SendAsync(HttpMethod.Post, "api/taxonomy-admin/merge",
            new { SourceValueId = source, TargetValueId = target, Reason = reason });

    /// <summary>Returns null on success, or the server's message on refusal.</summary>
    private async Task<string?> SendAsync(HttpMethod method, string url, object? body)
    {
        var http = await _api.CreateAsync();

        var request = new HttpRequestMessage(method, url);
        if (body is not null) request.Content = JsonContent.Create(body);

        var response = await http.SendAsync(request);
        if (response.IsSuccessStatusCode) return null;

        if (response.StatusCode == HttpStatusCode.Forbidden)
            return "دەسەڵاتت نییە بۆ ئەم کردارە.";

        var message = await response.Content.ReadAsStringAsync();
        return string.IsNullOrWhiteSpace(message) ? "کردارەکە سەرنەکەوت." : message.Trim('"');
    }
}
