namespace backend.Data.Models;

/// <summary>
/// Marks a lexicon entity that is never hard-deleted.
///
/// Deleting a row would delete its contributor's credit: the ledger points at an EntityId, and a
/// row that no longer exists turns a teacher's history into a gap. So the lexicon only ever hides
/// rows. <see cref="AppDbContext"/> applies a global query filter to every implementor, and
/// <c>SoftDeleteInterceptor</c> rewrites any <c>Remove()</c> that slips through into a flag change.
///
/// Deliberately NOT implemented by AuditLog, AnalyticsEvent, ContributionEvent or the Identity
/// tables. The ledger is append-only rather than soft-deletable, and analytics is not lexicon.
/// </summary>
public interface ISoftDeletable
{
    bool IsDeleted { get; set; }
    DateTime? DeletedAt { get; set; }

    /// <summary>Who hid it. The row's original author is untouched — both facts stand.</summary>
    Guid? DeletedByUserId { get; set; }
}
