using System.Collections.Concurrent;

namespace backend.Services.Lexicon;

/// <summary>
/// The taxonomy, held once for the whole process, with a change notification.
///
/// Two reasons it is a singleton rather than a per-request load:
///
///  · The options tree is read on every keystroke-adjacent operation — every entry-form render,
///    every validation, every work-queue count — and it changes a few times a week. Rebuilding it
///    per request is three queries and a graph build for data that did not move.
///
///  · REAL TIME. A settings write must reach every open session immediately: no page refresh, no
///    app restart, no logout. That needs one authoritative copy that can be invalidated and a
///    <see cref="Changed"/> signal that the SignalR broadcaster hangs off. A per-request cache has
///    nothing to notify.
///
/// <see cref="OptionsTree"/> is a detached plain graph with no DbContext inside it, which is what
/// makes sharing one instance across concurrent requests safe.
/// </summary>
public sealed class TaxonomyCache
{
    private readonly ConcurrentDictionary<int, OptionsTree> _trees = new();

    /// <summary>
    /// Bumped on every taxonomy write. A client that holds a version can tell "nothing has changed"
    /// from "I have not asked yet" without diffing the tree, which is what lets a circuit skip a
    /// re-render it does not need.
    /// </summary>
    public long Version => Interlocked.Read(ref _version);

    private long _version;

    /// <summary>
    /// Raised after an invalidation, with the parts of speech affected — empty meaning "all of them",
    /// which is what a change to a shared axis or value means.
    /// <para>
    /// Handlers must not throw and must not block: this runs on the thread that just committed a
    /// settings write, and a slow listener would make saving the taxonomy feel broken.
    /// </para>
    /// </summary>
    public event Action<TaxonomyChange>? Changed;

    public async Task<OptionsTree> GetOrLoadAsync(int partOfSpeechId, Func<Task<OptionsTree>> load)
    {
        if (_trees.TryGetValue(partOfSpeechId, out var cached)) return cached;

        // Deliberately not locked. Two racing loads build the same graph from the same rows and one
        // of them wins; the cost is a duplicated query, and the alternative — holding a lock across
        // a database round trip — is how a settings save becomes a stall for everyone.
        var tree = await load();
        _trees[partOfSpeechId] = tree;

        return tree;
    }

    /// <summary>Drops the whole cache and announces it. The general case: a shared axis or value moved.</summary>
    public void InvalidateAll(string reason)
    {
        _trees.Clear();
        Interlocked.Increment(ref _version);
        Raise(new TaxonomyChange(Version, Array.Empty<int>(), reason));
    }

    /// <summary>Drops one part of speech's tree. Used when a change provably cannot reach the others.</summary>
    public void Invalidate(int partOfSpeechId, string reason)
    {
        _trees.TryRemove(partOfSpeechId, out _);
        Interlocked.Increment(ref _version);
        Raise(new TaxonomyChange(Version, new[] { partOfSpeechId }, reason));
    }

    private void Raise(TaxonomyChange change)
    {
        // A listener that throws must not take the save down with it — the rows are already
        // committed by the time this runs, so the worst case is a client that updates late.
        foreach (var handler in Changed?.GetInvocationList() ?? Array.Empty<Delegate>())
        {
            try { ((Action<TaxonomyChange>)handler)(change); }
            catch { /* a missed notification, never a failed save */ }
        }
    }
}

/// <summary>What changed, for the listeners that push it to open sessions.</summary>
/// <param name="Version">The cache version after the change.</param>
/// <param name="PartsOfSpeech">Affected parts of speech; empty means all of them.</param>
/// <param name="Reason">A short machine-readable tag — which table moved. Not shown to anyone.</param>
public sealed record TaxonomyChange(long Version, IReadOnlyList<int> PartsOfSpeech, string Reason);
