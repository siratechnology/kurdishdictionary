using System.Net.Http.Json;
using Shared.Dtos;

namespace frontend_blazor.Services;

public record StationOptions(
    List<StationPartOption> Parts,
    List<StationDomainOption> Domains);

public record StationPartOption(int Id, string NameKu);
public record StationDomainOption(int Id, string NameKu, int? ParentId);

/// <summary>Drives the station screen — one sense at a time, walked start to end.</summary>
public class StationService
{
    private readonly ApiClient _api;

    public StationService(ApiClient api) => _api = api;

    /// <summary>
    /// Where this person left off, re-derived from the sense they were on. 1 the first time.
    /// </summary>
    public async Task<int> GetResumeAsync()
    {
        try
        {
            var http = await _api.CreateAsync();
            return await http.GetFromJsonAsync<int>("api/station/resume");
        }
        catch
        {
            // Never a blocker: a walk that refuses to open because it could not remember a number
            // is worse than one that starts at the beginning.
            return 1;
        }
    }

    /// <summary>Fire-and-forget: remembering the place must never slow down or fail a move.</summary>
    public async Task SaveResumeAsync(int position, int senseId, int wordId)
    {
        try
        {
            var http = await _api.CreateAsync();
            await http.PutAsync($"api/station/resume?position={position}&senseId={senseId}&wordId={wordId}", null);
        }
        catch { /* See above. */ }
    }

    /// <summary>
    /// What the walk can see. Only asked for when it has nothing to show, so an empty screen can
    /// tell the difference between a finished job and a walk that found nothing.
    /// </summary>
    public async Task<(int Total, int Unclassified)> GetCountsAsync()
    {
        try
        {
            var http = await _api.CreateAsync();
            var counts = await http.GetFromJsonAsync<StationCounts>("api/station/counts");
            return (counts?.Total ?? 0, counts?.Unclassified ?? 0);
        }
        catch
        {
            // The screen is already showing an empty state; failing to explain it must not
            // replace that with an error.
            return (0, 0);
        }
    }

    private record StationCounts(int Total, int Unclassified);

    public async Task<StationSenseDto?> GetAtAsync(int position, bool onlyUnclassified)
    {
        var http = await _api.CreateAsync();
        var response = await http.GetAsync($"api/station/at/{position}?onlyUnclassified={onlyUnclassified}");
        response.EnsureAuthorizedAndSuccess();

        if (response.StatusCode == System.Net.HttpStatusCode.NoContent) return null;
        return await response.Content.ReadFromJsonAsync<StationSenseDto>();
    }

    /// <summary>
    /// The sense as the server sees it right now, WITHOUT replacing what is on screen.
    ///
    /// Used when an admin changes the taxonomy while somebody is typing: the page compares this
    /// against the form it is already showing, marks whatever moved with an inline notice, and
    /// leaves the actual controls — and the unsaved input in them — completely alone.
    /// </summary>
    public async Task<StationSenseDto?> PeekAsync(int senseId)
    {
        var http = await _api.CreateAsync();
        var response = await http.GetAsync($"api/station/sense/{senseId}");

        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<StationSenseDto>();
    }

    public async Task<StationOptions> GetOptionsAsync()
    {
        var http = await _api.CreateAsync();
        var response = await http.GetAsync("api/station/options");
        response.EnsureAuthorizedAndSuccess();

        var raw = await response.Content.ReadFromJsonAsync<StationOptions>();
        return raw ?? new StationOptions(new(), new());
    }

    public async Task<StationSenseDto?> SaveAsync(SaveStationSenseDto dto)
    {
        var http = await _api.CreateAsync();
        var response = await http.PostAsJsonAsync("api/station/save", dto);
        response.EnsureAuthorizedAndSuccess();
        return await response.Content.ReadFromJsonAsync<StationSenseDto>();
    }

    public async Task<StationSenseDto?> SetFeatureAsync(int senseId, int axisId, int valueId)
    {
        var http = await _api.CreateAsync();
        var response = await http.PostAsJsonAsync(
            $"api/station/sense/{senseId}/feature", new { AxisId = axisId, ValueId = valueId });
        response.EnsureAuthorizedAndSuccess();
        return await response.Content.ReadFromJsonAsync<StationSenseDto>();
    }

    public async Task<(bool Ok, string? Error, StationSenseDto? Sense)> MarkNotApplicableAsync(
        int senseId, int axisId, string note)
    {
        var http = await _api.CreateAsync();
        var response = await http.PostAsJsonAsync(
            $"api/station/sense/{senseId}/not-applicable", new { AxisId = axisId, Note = note });

        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            return (false, await response.Content.ReadAsStringAsync(), null);

        response.EnsureAuthorizedAndSuccess();
        return (true, null, await response.Content.ReadFromJsonAsync<StationSenseDto>());
    }

    public async Task<string?> SubmitAsync(int senseId)
    {
        var http = await _api.CreateAsync();
        var response = await http.PostAsync($"api/station/sense/{senseId}/submit", null);
        response.EnsureAuthorizedAndSuccess();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task ReleaseAsync(int senseId)
    {
        var http = await _api.CreateAsync();
        await http.PostAsync($"api/station/sense/{senseId}/release", null);
    }
}
