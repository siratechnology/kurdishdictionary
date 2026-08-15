using backend.Data.Models;
using backend.Services.Lexicon;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace backend.Data;

/// <summary>
/// Invalidates the taxonomy cache the instant a settings write commits, and announces it.
///
/// Deliberately an interceptor rather than a call in each service method. Sixteen endpoints can
/// change the taxonomy today and more will exist next year; one of them forgetting to invalidate
/// would not fail a test, it would show a stale entry form to somebody else's browser for as long
/// as the process lived. There is no code path from a taxonomy write to a commit that does not run
/// through here.
///
/// Runs on SavedChanges — AFTER the transaction commits. Invalidating before the commit would open
/// a window where a reload could re-cache the OLD rows and then have nothing left to invalidate it.
/// </summary>
public class TaxonomyChangeInterceptor : SaveChangesInterceptor
{
    private readonly TaxonomyCache _cache;

    /// <summary>
    /// The tables the options tree is built from. SenseFeature is absent on purpose: answering a
    /// question is not changing the questions, and putting it here would invalidate the cache on
    /// every keystroke of every teacher in the building.
    /// </summary>
    private static readonly HashSet<Type> Structural = new()
    {
        typeof(PartOfSpeech), typeof(FeatureAxis), typeof(FeatureValue), typeof(PartOfSpeechAxis),
    };

    private bool _pending;

    public TaxonomyChangeInterceptor(TaxonomyCache cache) => _cache = cache;

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, InterceptionResult<int> result, CancellationToken ct = default)
    {
        _pending |= TouchesTaxonomy(eventData.Context);
        return base.SavingChangesAsync(eventData, result, ct);
    }

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData, InterceptionResult<int> result)
    {
        _pending |= TouchesTaxonomy(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData, int result, CancellationToken ct = default)
    {
        Flush();
        return base.SavedChangesAsync(eventData, result, ct);
    }

    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        Flush();
        return base.SavedChanges(eventData, result);
    }

    private void Flush()
    {
        if (!_pending) return;
        _pending = false;

        // Everything, not just the part of speech that was edited: axes and values are shared
        // between parts of speech, so a rename on one can change what another renders. Rebuilding a
        // tree is three queries, and being wrong here means a teacher works from a stale form.
        _cache.InvalidateAll("taxonomy-write");
    }

    private static bool TouchesTaxonomy(DbContext? context) =>
        context is not null &&
        context.ChangeTracker.Entries().Any(e =>
            e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted &&
            Structural.Contains(e.Entity.GetType()));
}
