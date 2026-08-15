using backend.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Shared.Text;

namespace backend.Data;

/// <summary>
/// Keeps <c>Normalized</c> in step with the text it is derived from, on every save.
///
/// Done here rather than in the controllers because a normalised column that is only *sometimes*
/// written is worse than none: search would find most words and silently miss the ones saved
/// through whichever path forgot. There is one place to get it right, and it is not optional.
///
/// Registered FIRST, before the ledger, so the ledger records the finished row.
/// </summary>
public class NormalizationInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, InterceptionResult<int> result, CancellationToken ct = default)
    {
        if (eventData.Context is not null) Apply(eventData.Context);
        return base.SavingChangesAsync(eventData, result, ct);
    }

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData, InterceptionResult<int> result)
    {
        if (eventData.Context is not null) Apply(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    private static void Apply(DbContext context)
    {
        foreach (var entry in context.ChangeTracker.Entries<Word>())
        {
            if (entry.State is not (EntityState.Added or EntityState.Modified)) continue;

            var expected = KurdishText.Normalize(entry.Entity.Kurdish);
            if (entry.Entity.Normalized != expected) entry.Entity.Normalized = expected;
        }

        foreach (var entry in context.ChangeTracker.Entries<WordForm>())
        {
            if (entry.State is not (EntityState.Added or EntityState.Modified)) continue;

            var expected = KurdishText.Normalize(entry.Entity.Form);
            if (entry.Entity.Normalized != expected) entry.Entity.Normalized = expected;
        }
    }
}
