using backend.Data.Models;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Shared.Dtos;

namespace backend.Hubs;

/// <summary>
/// One-way push channel for content changes. Every write that the audit interceptor records is
/// broadcast here the instant it commits, so the admin dashboard, the words grid and the notification
/// bell update without polling.
///
/// There are deliberately no client-to-server methods: clients only ever listen. Anything a client
/// wants to *change* still goes through the REST controllers, where the auth rules and the audit trail
/// already live — a hub method would be a second, unaudited way in.
/// </summary>
[Authorize(Roles = Roles.Any)]
public class ActivityHub : Hub
{
    /// <summary>Name of the client-side handler. Shared with the Blazor client via <see cref="ActivityBroadcaster"/>.</summary>
    public const string ActivityEvent = "ActivityHappened";

    /// <summary>
    /// Fired when the taxonomy itself changes — a group added, renamed, re-parented, retired.
    ///
    /// Separate from <see cref="ActivityEvent"/> because the reactions are different: content
    /// activity refreshes a list, a taxonomy change re-shapes the form somebody is typing into. That
    /// second one has to be handled carefully enough that it never destroys unsaved input, and
    /// hiding it inside the general feed would make it impossible to handle differently.
    /// </summary>
    public const string TaxonomyEvent = "TaxonomyChanged";
}

/// <summary>Sends audit rows down <see cref="ActivityHub"/>.</summary>
public class ActivityBroadcaster : IActivityBroadcaster
{
    private readonly IHubContext<ActivityHub> _hub;
    private readonly ILogger<ActivityBroadcaster> _log;

    public ActivityBroadcaster(IHubContext<ActivityHub> hub, ILogger<ActivityBroadcaster> log)
    {
        _hub = hub;
        _log = log;
    }

    public async Task BroadcastAsync(IReadOnlyList<AuditLogDto> entries, CancellationToken ct = default)
    {
        if (entries.Count == 0) return;

        try
        {
            await _hub.Clients.All.SendAsync(ActivityHub.ActivityEvent, entries, ct);
        }
        catch (Exception ex)
        {
            // The rows are already committed and the REST endpoints still serve them, so a dropped
            // broadcast costs a client its live update until the next reconnect catch-up — never a save.
            _log.LogWarning(ex, "Failed to broadcast {Count} activity entries", entries.Count);
        }
    }
}
