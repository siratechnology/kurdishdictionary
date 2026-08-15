namespace frontend_blazor.Services.Presence;

/// <summary>
/// Writes LastActivityAt to the database every 60 seconds, and once more on shutdown. Also sweeps
/// presence statuses on the way past — see <see cref="IPresenceStore.SweepStatuses"/>: going quiet
/// is the one transition nobody triggers, so something has to come along and notice it.
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
            {
                // Before the write: the sweep may mark rows dirty, and doing it first means those
                // land in this flush rather than waiting a further minute for the next one.
                _store.SweepStatuses();
                await FlushAsync(ct);
            }
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
            //
            // Put them back first, though. DrainDirty cleared the flags on the way out, so without
            // this the "retried on the next tick" below was simply false — the rows were dropped
            // and nothing marked them again until the person moved.
            _store.MarkDirty(dirty.Select(r => r.UserId));

            _log.LogWarning(ex, "Presence flush failed; {Count} rows will be retried on the next tick", dirty.Count);
        }
    }
}
