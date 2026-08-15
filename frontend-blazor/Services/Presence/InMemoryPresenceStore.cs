using System.Collections.Concurrent;

namespace frontend_blazor.Services.Presence;

/// <summary>
/// Single-instance presence, held in memory.
///
/// Correct for one process and wrong for two — see <see cref="IPresenceStore"/>. Registered as a
/// singleton so every circuit on this instance shares one view.
/// </summary>
public class InMemoryPresenceStore : IPresenceStore
{
    /// <summary>Longer than this without input and someone is بێ‌چالاکی, however open their tab is.</summary>
    public static readonly TimeSpan IdleAfter = TimeSpan.FromMinutes(2);

    private sealed class Entry
    {
        public required Guid UserId { get; init; }
        public required string UserName { get; set; }
        public string? AvatarUrl { get; set; }
        public int Connections;
        public DateTime LastActivityAt = DateTime.UtcNow;
        public DateTime? LastSeenAt;
        public string? CurrentPage;

        /// <summary>False while the socket is down but the circuit is still being retained.</summary>
        public bool Connected = true;
        public int? CurrentSenseId;
        public bool Dirty;

        /// <summary>What subscribers were last told. Lets the sweep notify only on a real move.</summary>
        public PresenceStatus? LastBroadcast;
    }

    private readonly ConcurrentDictionary<Guid, Entry> _entries = new();

    public event Action? Changed;

    /// <summary>
    /// Announces a change. Wrapped so one badly-behaved subscriber — a circuit that has gone away
    /// mid-notification, say — cannot take down the caller that was only recording a heartbeat.
    /// </summary>
    private void Notify()
    {
        try { Changed?.Invoke(); }
        catch { /* a dead subscriber must never break presence tracking */ }
    }

    public void MarkOnline(Guid userId, string userName, string? avatarUrl = null)
    {
        var entry = _entries.GetOrAdd(userId, _ => new Entry { UserId = userId, UserName = userName });

        lock (entry)
        {
            entry.UserName = userName;
            if (avatarUrl is not null) entry.AvatarUrl = avatarUrl;

            // Counted, not a boolean: one person with the dashboard open in two tabs has two
            // circuits, and closing one must not report them as gone.
            entry.Connections++;
            entry.Connected = true;
            entry.LastActivityAt = DateTime.UtcNow;
            entry.LastSeenAt = null;
            entry.Dirty = true;
        }

        Notify();
    }

    public void MarkDisconnected(Guid userId)
    {
        if (!_entries.TryGetValue(userId, out var entry)) return;

        lock (entry)
        {
            entry.Connected = false;
            entry.Dirty = true;
        }

        Notify();
    }

    /// <summary>
    /// Signing out ends presence immediately, without waiting for the circuit.
    ///
    /// This exists because the circuit outlives the session. Blazor retains a disconnected
    /// circuit for three minutes so a flaky network can reconnect to it, so OnCircuitClosedAsync
    /// can arrive minutes after the person pressed «دەرچوون» — and until then they were still
    /// being shown as working. Connections is zeroed rather than decremented: a sign-out ends
    /// every tab that identity had open, not one of them.
    /// </summary>
    public void SignOut(Guid userId)
    {
        if (!_entries.TryGetValue(userId, out var entry)) return;

        lock (entry)
        {
            entry.Connections = 0;
            entry.Connected = false;
            entry.LastSeenAt = DateTime.UtcNow;
            entry.CurrentSenseId = null;
            entry.CurrentPage = null;
            entry.Dirty = true;
        }

        Notify();
    }

    public void MarkOffline(Guid userId)
    {
        if (!_entries.TryGetValue(userId, out var entry)) return;

        lock (entry)
        {
            entry.Connections = Math.Max(0, entry.Connections - 1);

            if (entry.Connections == 0)
            {
                entry.LastSeenAt = DateTime.UtcNow;
                entry.CurrentSenseId = null;
                entry.CurrentPage = null;
                entry.Dirty = true;
            }
        }

        Notify();
    }

    public void Touch(Guid userId, string? currentPage = null)
    {
        if (!_entries.TryGetValue(userId, out var entry)) return;

        lock (entry)
        {
            entry.LastActivityAt = DateTime.UtcNow;
            if (currentPage is not null) entry.CurrentPage = currentPage;
            entry.Dirty = true;
        }

        Notify();
    }

    public void SetCurrentSense(Guid userId, int? senseId)
    {
        if (!_entries.TryGetValue(userId, out var entry)) return;

        lock (entry)
        {
            entry.CurrentSenseId = senseId;
            entry.LastActivityAt = DateTime.UtcNow;
            entry.Dirty = true;
        }

        Notify();
    }

    public PresenceSnapshot? Get(Guid userId) =>
        _entries.TryGetValue(userId, out var entry) ? Snapshot(entry) : null;

    public IReadOnlyCollection<PresenceSnapshot> All() =>
        _entries.Values.Select(Snapshot).ToList();

    public int ActiveCount() =>
        _entries.Values.Count(e => StatusOf(e) == PresenceStatus.Active);

    public void SweepStatuses()
    {
        var moved = false;

        foreach (var entry in _entries.Values)
        {
            lock (entry)
            {
                var now = StatusOf(entry);
                if (entry.LastBroadcast == now) continue;

                entry.LastBroadcast = now;
                moved = true;
            }
        }

        // One notification for the whole sweep. Ten people going quiet together is still one
        // change to the list every subscriber is holding.
        if (moved) Notify();
    }

    public IReadOnlyCollection<PresenceSnapshot> DrainDirty()
    {
        var dirty = new List<PresenceSnapshot>();

        foreach (var entry in _entries.Values)
        {
            lock (entry)
            {
                if (!entry.Dirty) continue;
                entry.Dirty = false;
                dirty.Add(Snapshot(entry));
            }
        }

        return dirty;
    }

    private static PresenceSnapshot Snapshot(Entry e) => new(
        e.UserId, e.UserName, e.AvatarUrl, StatusOf(e), e.LastActivityAt, e.LastSeenAt,
        e.CurrentPage, e.CurrentSenseId);

    private static PresenceStatus StatusOf(Entry e)
    {
        if (e.Connections == 0) return PresenceStatus.Offline;

        // Socket down, circuit retained. Reporting دەرچوو here would claim someone had left
        // before we know that — a phone locking its screen drops the socket for a few seconds.
        // But چالاک would be a stronger claim still: we have stopped receiving their input, so
        // "typed something in the last two minutes" is no longer a fact we can assert. بێ‌چالاکی
        // is the honest middle, and it takes them out of the active list immediately.
        if (!e.Connected) return PresenceStatus.Idle;

        return DateTime.UtcNow - e.LastActivityAt > IdleAfter
            ? PresenceStatus.Idle
            : PresenceStatus.Active;
    }
}
