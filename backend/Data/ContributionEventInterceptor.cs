using backend.Data.Models;
using backend.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace backend.Data;

/// <summary>
/// Emits <see cref="ContributionEvent"/> rows straight off the change tracker, so no service can
/// forget to log. Every changed property becomes one FieldName/OldValue/NewValue event — a save
/// that touches three fields produces three rows, because per-field history is what makes a word's
/// page able to show "who changed the definition" rather than just "somebody edited this".
///
/// This is deliberately separate from <see cref="AuditSaveChangesInterceptor"/>. The audit log is
/// operational (who hit which endpoint from which IP); the ledger is editorial (who contributed
/// what to the dictionary). They answer different questions, have different retention rules, and
/// the ledger must never be filterable by IP.
///
/// Inserts are recorded in the second pass because a new row has no identity key until the INSERT
/// has run.
/// </summary>
public class ContributionEventInterceptor : SaveChangesInterceptor
{
    private readonly ICurrentUser _user;

    /// <summary>
    /// Lexicon entities only. ContributionEvent itself is absent — logging the log would recurse —
    /// as are AuditLog, AnalyticsEvent and the Identity tables, which are not contributions.
    /// </summary>
    private static readonly HashSet<Type> Tracked = new()
    {
        // Schema v2 — still driving the running app until پڕۆمپت ٤ applies.
        typeof(Word), typeof(WordMeans), typeof(RelatedWord),
        typeof(WordSpeechPane), typeof(WordCategory), typeof(Category),

        // Schema v3. Adding these is not optional: work done through the new model would otherwise
        // be invisible in the ledger, and a teacher who classified a thousand senses would have
        // nothing to show for it in the credits.
        typeof(Sense), typeof(SenseFeature), typeof(SenseTranslation),
        typeof(WordForm), typeof(WordRelation), typeof(SenseRelation),
    };

    /// <summary>Bookkeeping columns. A change to these alone is not a contribution.</summary>
    private static readonly HashSet<string> IgnoredProperties = new()
    {
        nameof(Word.UpdatedAt), nameof(Word.UpdatedByUserId), nameof(Word.CreatedAt),
        nameof(ISoftDeletable.DeletedAt), nameof(ISoftDeletable.DeletedByUserId),

        // Derived from Kurdish by NormalizationInterceptor. Logging it would double every headword
        // edit in a teacher's history with a line they did not write.
        nameof(Word.Normalized),
    };

    private readonly List<Pending> _pending = new();
    private bool _writing;

    public ContributionEventInterceptor(ICurrentUser user) => _user = user;

    private sealed record Pending(EntityEntry Entry, ContributionEvent Event, bool NeedsKey);

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, InterceptionResult<int> result, CancellationToken ct = default)
    {
        if (eventData.Context is not null && !_writing)
            Capture(eventData.Context);

        return base.SavingChangesAsync(eventData, result, ct);
    }

    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData, int result, CancellationToken ct = default)
    {
        if (eventData.Context is not null && _pending.Count > 0 && !_writing)
        {
            var context = eventData.Context;
            var events = new List<ContributionEvent>(_pending.Count);

            foreach (var p in _pending)
            {
                if (p.NeedsKey)
                {
                    p.Event.EntityId = ReadKey(p.Entry);
                    // A word's own event carries its id in both places, so per-word history is a
                    // single indexed lookup on WordId regardless of which table changed.
                    p.Event.WordId ??= p.Entry.Entity is Word ? (int)p.Event.EntityId : null;
                }
                events.Add(p.Event);
            }

            _pending.Clear();

            _writing = true;
            try
            {
                context.Set<ContributionEvent>().AddRange(events);
                await context.SaveChangesAsync(ct);
            }
            finally
            {
                _writing = false;
            }
        }

        return await base.SavedChangesAsync(eventData, result, ct);
    }

    private void Capture(DbContext context)
    {
        // The ledger cannot record an anonymous contribution: UserId is non-null by design. A write
        // with no signed-in user is a system task and produces no ledger rows — it will still be in
        // the audit log, which does accept nulls.
        if (_user.UserId is not { } userId) return;

        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (!Tracked.Contains(entry.Entity.GetType())) continue;

            switch (entry.State)
            {
                case EntityState.Added:
                    _pending.Add(new Pending(entry, New(userId, entry, CreatedType(entry)), NeedsKey: true));
                    break;

                case EntityState.Modified:
                    CaptureModified(userId, entry);
                    break;

                case EntityState.Deleted:
                    // SoftDeleteInterceptor should have rewritten this already. Reaching here means
                    // a genuine hard delete of a lexicon row, which is a bug worth recording loudly
                    // rather than losing.
                    _pending.Add(new Pending(entry, New(userId, entry, DeletedType(entry)), NeedsKey: false));
                    break;
            }
        }
    }

    private void CaptureModified(Guid userId, EntityEntry entry)
    {
        // A soft delete is a delete, not an edit — record it as one and stop.
        var deletedProp = entry.Properties.FirstOrDefault(p => p.Metadata.Name == nameof(ISoftDeletable.IsDeleted));
        if (deletedProp is { IsModified: true })
        {
            var nowDeleted = deletedProp.CurrentValue is true;
            _pending.Add(new Pending(entry,
                New(userId, entry, nowDeleted ? DeletedType(entry) : ContributionEventType.WordRestored),
                NeedsKey: false));
            return;
        }

        foreach (var prop in entry.Properties)
        {
            if (!prop.IsModified) continue;
            if (IgnoredProperties.Contains(prop.Metadata.Name)) continue;

            var oldValue = prop.OriginalValue?.ToString();
            var newValue = prop.CurrentValue?.ToString();
            if (oldValue == newValue) continue;

            var ev = New(userId, entry, EditedType(entry));
            ev.FieldName = prop.Metadata.Name;
            ev.OldValue = oldValue;
            ev.NewValue = newValue;

            _pending.Add(new Pending(entry, ev, NeedsKey: false));
        }
    }

    private static ContributionEvent New(Guid userId, EntityEntry entry, ContributionEventType type) =>
        new()
        {
            UserId = userId,
            OccurredAt = DateTime.UtcNow,
            EventType = type,
            EntityType = entry.Entity.GetType().Name,
            EntityId = entry.State == EntityState.Added ? 0 : ReadKey(entry),
            WordId = ReadWordId(entry),
            SourceKind = ContributionSourceKind.Human,
        };

    // ── Entity → event type ─────────────────────────────────────────────────
    // Senses do not exist yet (پڕۆمپت ٣ adds them). WordMeans is today's nearest equivalent, so it
    // maps onto the Sense events rather than inventing a parallel vocabulary that would have to be
    // migrated later.
    private static ContributionEventType CreatedType(EntityEntry entry) => entry.Entity switch
    {
        Word => ContributionEventType.WordCreated,
        Sense or WordMeans or SenseTranslation => ContributionEventType.SenseCreated,
        SenseFeature => ContributionEventType.FeatureSet,
        WordForm => ContributionEventType.FormAdded,
        WordRelation or SenseRelation or RelatedWord => ContributionEventType.RelationAdded,
        WordSpeechPane or WordCategory => ContributionEventType.SenseClassified,
        _ => ContributionEventType.WordEdited,
    };

    private static ContributionEventType EditedType(EntityEntry entry) => entry.Entity switch
    {
        Word => ContributionEventType.WordEdited,
        Sense or WordMeans or SenseTranslation => ContributionEventType.SenseEdited,
        SenseFeature => ContributionEventType.FeatureChanged,
        _ => ContributionEventType.WordEdited,
    };

    private static ContributionEventType DeletedType(EntityEntry entry) => entry.Entity switch
    {
        Word => ContributionEventType.WordDeleted,
        WordRelation or SenseRelation or RelatedWord => ContributionEventType.RelationRemoved,
        WordForm => ContributionEventType.FormRemoved,

        // A cascade and a person are different authors of the same disappearance. When the options
        // tree retires a sub-group, its answers were not changed by anybody — the question stopped
        // being asked — and per-word history is unreadable if that reads as somebody retyping.
        SenseFeature { ClearedByCascade: true } => ContributionEventType.FeatureCleared,
        SenseFeature => ContributionEventType.FeatureChanged,

        WordSpeechPane or WordCategory => ContributionEventType.SenseReclassified,
        _ => ContributionEventType.WordDeleted,
    };

    private static long ReadKey(EntityEntry entry)
    {
        var key = entry.Metadata.FindPrimaryKey();
        if (key is null) return 0;

        // Composite-key join rows (WordSpeechPane, WordCategory) have no single id. The WordId
        // column is the one that matters for history, and it is already on the event.
        if (key.Properties.Count != 1) return 0;

        var value = entry.Property(key.Properties[0].Name).CurrentValue;
        return value switch
        {
            int i => i,
            long l => l,
            _ => 0,
        };
    }

    private static int? ReadWordId(EntityEntry entry) => entry.Entity switch
    {
        Word w => w.Id == 0 ? null : w.Id,
        WordMeans m => m.WordId,
        RelatedWord r => r.WordId,
        WordSpeechPane sp => sp.WordId,
        WordCategory wc => wc.WordId,

        Sense s => s.WordId,
        WordForm f => f.WordId,
        WordRelation wr => wr.FromWordId,

        // SenseFeature, SenseRelation and SenseTranslation hang off a Sense, not a Word. Resolving
        // the word would mean a query per event inside the save pipeline; the sense id is on the
        // event and per-word history joins through it.
        _ => null,
    };
}
