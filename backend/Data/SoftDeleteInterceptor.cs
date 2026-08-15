using backend.Data.Models;
using backend.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace backend.Data;

/// <summary>
/// Turns every <c>Remove()</c> on a lexicon entity into a soft delete.
///
/// The rule is that nothing in the lexicon is ever hard-deleted, because deleting a row deletes the
/// evidence of who wrote it. Rewriting the operation here rather than auditing call sites means a
/// service added next year cannot get it wrong: there is no code path from <c>Remove()</c> to an
/// actual DELETE for an <see cref="ISoftDeletable"/>.
///
/// Runs BEFORE <see cref="ContributionEventInterceptor"/> in the registration order, so the ledger
/// sees a Modified entity with IsDeleted=true and records WordDeleted rather than a hard delete.
/// </summary>
public class SoftDeleteInterceptor : SaveChangesInterceptor
{
    private readonly ICurrentUser _user;

    public SoftDeleteInterceptor(ICurrentUser user) => _user = user;

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, InterceptionResult<int> result, CancellationToken ct = default)
    {
        if (eventData.Context is not null)
            Rewrite(eventData.Context);

        return base.SavingChangesAsync(eventData, result, ct);
    }

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData, InterceptionResult<int> result)
    {
        if (eventData.Context is not null)
            Rewrite(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    private void Rewrite(DbContext context)
    {
        foreach (var entry in context.ChangeTracker.Entries<ISoftDeletable>())
        {
            if (entry.State != EntityState.Deleted) continue;

            entry.State = EntityState.Modified;
            entry.Entity.IsDeleted = true;
            entry.Entity.DeletedAt = DateTime.UtcNow;
            entry.Entity.DeletedByUserId = _user.UserId;

            // Only the three flag columns may be written by a delete. Without this, EF marks every
            // property modified and the UPDATE rewrites the whole row with whatever the tracked
            // instance happens to hold — silently clobbering a concurrent edit.
            foreach (var prop in entry.Properties)
            {
                prop.IsModified = prop.Metadata.Name
                    is nameof(ISoftDeletable.IsDeleted)
                    or nameof(ISoftDeletable.DeletedAt)
                    or nameof(ISoftDeletable.DeletedByUserId);
            }
        }
    }
}
