using System.Net.Http.Json;
using Shared.Dtos;

namespace frontend_blazor.Services.Presence;

/// <summary>
/// Posts last-seen timestamps to the API. Used only by the 60-second flush.
///
/// Deliberately NOT using ApiClient. That client attaches the signed-in user's JWT, which it reads
/// from AuthenticationStateProvider — and that provider only exists inside a Razor component's DI
/// scope. A BackgroundService has no component and no user, so calling it there throws.
///
/// This is machine-to-machine: the web tier telling the API what it observed. It authenticates as
/// the application with a shared key, not as whichever teacher happened to be online.
/// </summary>
public class PresenceApi
{
    private readonly IHttpClientFactory _factory;
    private readonly IConfiguration _config;

    public PresenceApi(IHttpClientFactory factory, IConfiguration config)
    {
        _factory = factory;
        _config = config;
    }

    public async Task FlushAsync(IReadOnlyCollection<PresenceSnapshot> rows, CancellationToken ct)
    {
        var payload = rows.Select(r => new UserPresenceDto
        {
            UserId = r.UserId,
            Status = (int)r.Status,
            LastActivityAt = r.LastActivityAt,
            LastSeenAt = r.LastSeenAt,
            CurrentPage = r.CurrentPage,
            CurrentSenseId = r.CurrentSenseId,
        }).ToList();

        var key = _config["Internal:ApiKey"];
        if (string.IsNullOrWhiteSpace(key))
        {
            // Not configured: presence still works live from memory, and «دوایین جار» simply does
            // not survive a restart. Better than crashing a background loop every 60 seconds.
            return;
        }

        var http = _factory.CreateClient(ApiClient.ClientName);
        http.DefaultRequestHeaders.Add("X-Internal-Key", key);

        var response = await http.PostAsJsonAsync("api/presence/flush", payload, ct);
        response.EnsureSuccessStatusCode();
    }
}

/// <summary>
/// Opening and leaving a sense.
///
/// Presence and the claim lock are ONE feature (پڕۆمپت ٩): CurrentSenseId and SenseClaim are the
/// same fact said twice, and if they are set in two places they will eventually disagree — which
/// would mean the UI showing a word as free while the database holds it locked, or the reverse.
/// One call sets both.
/// </summary>
public class PresenceService
{
    private readonly IPresenceStore _store;
    private readonly ApiClient _api;

    public PresenceService(IPresenceStore store, ApiClient api)
    {
        _store = store;
        _api = api;
    }

    /// <summary>
    /// Claims the sense and records that this person is in it. Returns the holder's name when
    /// somebody else already has it.
    /// </summary>
    public async Task<SenseClaimDto?> EnterSenseAsync(Guid userId, int senseId)
    {
        var http = await _api.CreateAsync();
        var response = await http.PostAsync($"api/presence/sense/{senseId}/claim", null);
        response.EnsureAuthorizedAndSuccess();

        var result = await response.Content.ReadFromJsonAsync<SenseClaimDto>();

        // Only mirror it into presence if the claim was actually granted. Showing someone as
        // working on a sense they were refused is exactly the disagreement this design avoids.
        if (result?.Granted == true) _store.SetCurrentSense(userId, senseId);

        return result;
    }

    /// <summary>Esc on the station screen, or navigating away.</summary>
    public async Task LeaveSenseAsync(Guid userId, int senseId)
    {
        _store.SetCurrentSense(userId, null);

        var http = await _api.CreateAsync();
        await http.PostAsync($"api/presence/sense/{senseId}/release", null);
    }

    /// <summary>
    /// Your own worked minutes. Goes through ApiClient — and therefore the signed-in user's
    /// token — because the endpoint deliberately has no user parameter: it answers only for
    /// whoever is asking. PresenceApi would be the wrong client here; it authenticates as the
    /// application and has no user at all.
    /// </summary>
    public async Task<WorkTimeDto?> GetWorkTimeAsync(int windowDays = 14)
    {
        var http = await _api.CreateAsync();
        var response = await http.GetAsync($"api/presence/work-time?windowDays={windowDays}");

        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<WorkTimeDto>();
    }

    public PresenceSnapshot? Get(Guid userId) => _store.Get(userId);
    public IReadOnlyCollection<PresenceSnapshot> All() => _store.All();
    public int ActiveCount() => _store.ActiveCount();
}
