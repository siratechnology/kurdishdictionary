namespace frontend_blazor.Services.Presence;

/// <summary>
/// Writes LastActivityAt to the database every 60 seconds, and once more on shutdown.
///
/// Not on every heartbeat. A heartbeat is at most one per user per 30 seconds, which sounds cheap
/// until you multiply it by every signed-in teacher for every hour of every day — an endless
/// stream of UPDATEs whose only purpose is to record that somebody moved a mouse. The live answer
/// lives in memory; the database only needs enough to answer «دوایین جار» after a restart.
/// </summary>
public class PresenceFlushService : BackgroundService
{
    private static readonly TimeSpan Interval = TimeSpan.FromSeconds(60);

    private readonly IPresenceStore _store;
    private readonly IServiceScopeFactory _scopes;
    private readonly ILogger<PresenceFlushService> _log;

    public PresenceFlushService(
        IPresenceStore store, IServiceScopeFactory scopes, ILogger<PresenceFlushService> log)
    {
        _store = store;
        _scopes = scopes;
        _log = log;
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        using var timer = new PeriodicTimer(Interval);

        try
        {
            while (await timer.WaitForNextTickAsync(ct))
                await FlushAsync(ct);
        }
        catch (OperationCanceledException)
        {
            // Shutting down. One last flush so «دوایین جار» survives the restart rather than
            // showing whatever the last 60-second tick happened to catch.
            await FlushAsync(CancellationToken.None);
        }
    }

    private async Task FlushAsync(CancellationToken ct)
    {
        var dirty = _store.DrainDirty();
        if (dirty.Count == 0) return;

        try
        {
            using var scope = _scopes.CreateScope();
            var api = scope.ServiceProvider.GetRequiredService<PresenceApi>();
            await api.FlushAsync(dirty, ct);
        }
        catch (Exception ex)
        {
            // Presence is a convenience. If the API is down the live view still works from memory,
            // and losing a last-seen timestamp is not worth taking the app down for.
            _log.LogWarning(ex, "Presence flush failed; {Count} rows will be retried on the next tick", dirty.Count);
        }
    }
}
