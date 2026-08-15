using backend.Services.Lexicon;
using Microsoft.AspNetCore.SignalR;
using Shared.Dtos;

namespace backend.Hubs;

/// <summary>
/// Bridges <see cref="TaxonomyCache.Changed"/> to the hub: a settings write reaches every open
/// session immediately. No page refresh, no app restart, no logout.
///
/// A hosted singleton rather than something the settings service calls, for the same reason the cache
/// invalidation is an interceptor: every endpoint that can change the taxonomy is wired up by
/// existing, not by being remembered.
///
/// Fire-and-forget on purpose. This runs on the thread that has just committed somebody's settings
/// save, and awaiting a fan-out to every connected browser would make saving the taxonomy feel as
/// slow as the slowest client on the network.
/// </summary>
public class TaxonomyChangeBroadcaster : IHostedService
{
    private readonly TaxonomyCache _cache;
    private readonly IHubContext<ActivityHub> _hub;
    private readonly ILogger<TaxonomyChangeBroadcaster> _log;

    public TaxonomyChangeBroadcaster(
        TaxonomyCache cache, IHubContext<ActivityHub> hub, ILogger<TaxonomyChangeBroadcaster> log)
    {
        _cache = cache;
        _hub = hub;
        _log = log;
    }

    public Task StartAsync(CancellationToken ct)
    {
        _cache.Changed += OnChanged;
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken ct)
    {
        _cache.Changed -= OnChanged;
        return Task.CompletedTask;
    }

    private void OnChanged(TaxonomyChange change) => _ = SendAsync(change);

    private async Task SendAsync(TaxonomyChange change)
    {
        try
        {
            await _hub.Clients.All.SendAsync(
                ActivityHub.TaxonomyEvent,
                new TaxonomyChangedDto(change.Version, change.PartsOfSpeech));
        }
        catch (Exception ex)
        {
            // The rows are committed and the REST endpoints serve the new tree, so a dropped
            // broadcast costs a client its live update until it next reloads — never a save.
            _log.LogWarning(ex, "Failed to broadcast a taxonomy change (version {Version})", change.Version);
        }
    }
}
