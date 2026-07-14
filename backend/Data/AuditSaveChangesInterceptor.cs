using System.Text.Json;
using backend.Data.Models;
using backend.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace backend.Data;

/// <summary>
/// Writes an <see cref="AuditLog"/> row for every insert, update and delete of a content entity,
/// stamped with the signed-in user, their IP and their user-agent. Controllers get this for free —
/// they never call the audit code directly, which is what stops writes from silently going untracked.
///
/// Inserts are audited in a second pass (SavedChanges) because a new row's identity key does not
/// exist until the database has assigned it.
/// </summary>
public class AuditSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly ICurrentUser _user;

    /// <summary>Entities we log. Anything not listed (AuditLog, AnalyticsEvent, Identity tables) is ignored.</summary>
    private static readonly HashSet<Type> Audited = new()
    {
        typeof(Word), typeof(Category), typeof(WordMeans),
        typeof(RelatedWord), typeof(WordSpeechPane), typeof(WordCategory),
    };

    /// <summary>Noise fields — a change to these alone is not worth a log line.</summary>
    private static readonly HashSet<string> IgnoredProperties = new()
    {
        nameof(Word.UpdatedAt), nameof(Word.UpdatedByUserId),
    };

    private readonly List<PendingAudit> _pending = new();
    private bool _writingAudits;

    public AuditSaveChangesInterceptor(ICurrentUser user) => _user = user;

    private sealed record PendingAudit(EntityEntry Entry, AuditLog Log, bool NeedsKeyAfterSave);

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, InterceptionResult<int> result, CancellationToken ct = default)
    {
        if (eventData.Context is not null && !_writingAudits)
            Capture(eventData.Context);

        return base.SavingChangesAsync(eventData, result, ct);
    }

    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData, int result, CancellationToken ct = default)
    {
        if (eventData.Context is not null && _pending.Count > 0 && !_writingAudits)
        {
            var context = eventData.Context;
            var logs = new List<AuditLog>(_pending.Count);

            foreach (var p in _pending)
            {
                // Inserted rows only have their identity key now that the INSERT has run.
                if (p.NeedsKeyAfterSave)
                    p.Log.EntityId = ReadIntKey(p.Entry);
                logs.Add(p.Log);
            }

            _pending.Clear();

            // Guard against re-entering this interceptor while it saves its own rows.
            _writingAudits = true;
            try
            {
                context.Set<AuditLog>().AddRange(logs);
                await context.SaveChangesAsync(ct);
            }
            finally
            {
                _writingAudits = false;
            }
        }

        return await base.SavedChangesAsync(eventData, result, ct);
    }

    public override void SaveChangesFailed(DbContextErrorEventData eventData) => _pending.Clear();

    public override Task SaveChangesFailedAsync(DbContextErrorEventData eventData, CancellationToken ct = default)
    {
        _pending.Clear();
        return Task.CompletedTask;
    }

    private void Capture(DbContext context)
    {
        _pending.Clear();
        var now = DateTime.UtcNow;
        var userId = _user.UserId;

        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (!Audited.Contains(entry.Entity.GetType())) continue;
            if (entry.State is not (EntityState.Added or EntityState.Modified or EntityState.Deleted)) continue;

            // Stamp authorship on the row itself, so "who owns this" survives without joining the log.
            if (entry.State == EntityState.Added)
            {
                switch (entry.Entity)
                {
                    case Word w when w.CreatedByUserId is null:
                        w.CreatedByUserId = userId;
                        break;
                    case RelatedWord r when r.CreatedByUserId is null:
                        r.CreatedByUserId = userId;
                        break;
                    case WordMeans m when m.CreatedByUserId is null:
                        m.CreatedByUserId = userId;
                        break;
                }
            }
            else if (entry.State == EntityState.Modified && entry.Entity is Word word)
            {
                word.UpdatedByUserId = userId;
                word.UpdatedAt = now;
            }

            string action = entry.State switch
            {
                EntityState.Added => AuditActions.Create,
                EntityState.Modified => AuditActions.Update,
                _ => AuditActions.Delete,
            };

            string? changes = null;
            if (entry.State == EntityState.Modified)
            {
                changes = BuildDiff(entry);
                // Every changed property was an ignored one — nothing a human would care to read.
                if (changes is null) continue;
            }

            var log = new AuditLog
            {
                Action = action,
                EntityType = entry.Entity.GetType().Name,
                EntityId = entry.State == EntityState.Added ? 0 : ReadIntKey(entry),
                Summary = Describe(entry.Entity),
                Changes = changes,
                UserId = userId,
                UserName = _user.UserName,
                IpAddress = _user.IpAddress,
                UserAgent = _user.UserAgent,
                Country = _user.Country,
                City = _user.City,
                CreatedAt = now,
            };

            _pending.Add(new PendingAudit(entry, log, entry.State == EntityState.Added));
        }
    }

    /// <summary>Field-level before/after, as JSON, for the activity feed to render.</summary>
    private static string? BuildDiff(EntityEntry entry)
    {
        var diffs = new List<object>();

        foreach (var prop in entry.Properties)
        {
            if (!prop.IsModified) continue;
            if (prop.Metadata.Name is { } name && IgnoredProperties.Contains(name)) continue;
            if (Equals(prop.OriginalValue, prop.CurrentValue)) continue;

            diffs.Add(new
            {
                field = prop.Metadata.Name,
                old = prop.OriginalValue?.ToString(),
                @new = prop.CurrentValue?.ToString(),
            });
        }

        return diffs.Count == 0 ? null : JsonSerializer.Serialize(diffs);
    }

    /// <summary>
    /// The audit table stores an int key. Words/categories have one; the join tables key on
    /// (WordId, X), so we log the word's id — that's the row a human would go looking for.
    /// </summary>
    private static int ReadIntKey(EntityEntry entry) => entry.Entity switch
    {
        Word w => w.Id,
        Category c => c.Id,
        WordMeans m => m.Id,
        RelatedWord r => r.Id,
        WordSpeechPane wsp => wsp.WordId,
        WordCategory wc => wc.WordId,
        _ => 0,
    };

    /// <summary>A one-line, human-readable description of the row — this is what the activity feed shows.</summary>
    private static string? Describe(object entity) => entity switch
    {
        Word w => w.Kurdish,
        Category c => c.Name,
        WordMeans m => m.Meaning,
        RelatedWord r => $"{r.RelationType}: {r.WordId} → {r.TargetWordId}",
        WordSpeechPane wsp => wsp.SpeechPaneType.ToKurdish(),
        WordCategory wc => $"وشە {wc.WordId} ← پۆلی {wc.CategoryId}",
        _ => null,
    };
}
