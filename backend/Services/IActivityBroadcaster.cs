using Shared.Dtos;

namespace backend.Services;

/// <summary>
/// Pushes freshly written audit rows to every connected admin client.
///
/// Declared as an interface so <c>AuditSaveChangesInterceptor</c> — which lives in the data layer and
/// runs inside a SaveChanges — has no compile-time dependency on SignalR. It also gives the interceptor
/// something harmless to call when no transport is wired up, e.g. in tests.
/// </summary>
public interface IActivityBroadcaster
{
    /// <summary>
    /// Fan out the given rows. Implementations must never throw: a failed broadcast is a missed
    /// notification, not a failed save, and the database transaction has already committed by then.
    /// </summary>
    Task BroadcastAsync(IReadOnlyList<AuditLogDto> entries, CancellationToken ct = default);
}
