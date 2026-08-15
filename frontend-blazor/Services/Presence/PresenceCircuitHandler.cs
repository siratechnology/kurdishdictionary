using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.Circuits;

namespace frontend_blazor.Services.Presence;

/// <summary>
/// Presence, straight off the circuit every signed-in user already holds.
///
/// This app is Blazor Server: there is already exactly one live connection per user, and it already
/// knows when it opens and closes. Polling for presence, or opening a second WebSocket to carry it,
/// would be building a worse version of something the framework hands over for free.
/// </summary>
public class PresenceCircuitHandler : CircuitHandler
{
    private readonly IPresenceStore _store;
    private readonly AuthenticationStateProvider _auth;
    private readonly IServiceProvider _services;
    private readonly ILogger<PresenceCircuitHandler> _log;

    private Guid? _userId;

    public PresenceCircuitHandler(
        IPresenceStore store,
        AuthenticationStateProvider auth,
        IServiceProvider services,
        ILogger<PresenceCircuitHandler> log)
    {
        _store = store;
        _auth = auth;
        _services = services;
        _log = log;
    }

    public override async Task OnCircuitOpenedAsync(Circuit circuit, CancellationToken ct)
    {
        var state = await _auth.GetAuthenticationStateAsync();
        var user = state.User;

        if (user.Identity?.IsAuthenticated != true) return;

        if (!Guid.TryParse(user.FindFirstValue(ClaimTypes.NameIdentifier), out var id)) return;

        _userId = id;

        var name = user.FindFirstValue("fullName") ?? user.Identity.Name ?? "";

        // Registered once per circuit, not per render — the avatar is fetched here so the top bar
        // can draw everyone who is online without a lookup per face. A claim would not do: the
        // picture can change mid-session and the token would keep serving the old one.
        string? avatar = null;
        try
        {
            avatar = (await _services.GetRequiredService<AuthService>().GetCurrentAsync())?.AvatarUrl;
        }
        catch
        {
            // Presence must not depend on the API being reachable. Without a picture the stack
            // falls back to the initial letter, which is the same thing it does for anyone who
            // has not uploaded one.
        }

        _store.MarkOnline(id, name, avatar);
    }

    public override Task OnConnectionUpAsync(Circuit circuit, CancellationToken ct)
    {
        // A reconnect after a dropped socket. The circuit was retained, so this is the same person
        // returning rather than a new arrival.
        //
        // MarkReconnected, not Touch: Touch bumped the clock but left the disconnected flag set,
        // so coming back never actually undid going away.
        if (_userId is { } id) _store.MarkReconnected(id);
        return Task.CompletedTask;
    }

    public override Task OnConnectionDownAsync(Circuit circuit, CancellationToken ct)
    {
        // Not «دەرچوو» — a phone locking its screen drops the socket for a few seconds and the
        // circuit is retained for three minutes, so it will very likely come back. But not چالاک
        // either: from here on we receive no input from them, so the claim behind «چالاک» has
        // stopped being something we can check. MarkDisconnected drops them to بێ‌چالاکی, which
        // removes them from the active list without asserting they have gone.
        if (_userId is { } id) _store.MarkDisconnected(id);
        return Task.CompletedTask;
    }

    public override Task OnCircuitClosedAsync(Circuit circuit, CancellationToken ct)
    {
        if (_userId is { } id)
        {
            _store.MarkOffline(id);
            _log.LogDebug("Presence: circuit closed for {UserId}", id);
        }

        return Task.CompletedTask;
    }
}
