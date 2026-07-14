namespace backend.Data.Models;

/// <summary>
/// Records every write (create / update / delete) so it can be traced who changed what,
/// from where, and when. Rows are written automatically by the AuditSaveChangesInterceptor —
/// controllers do not need to log by hand.
///
/// Older rows (written before authentication existed) have a null <see cref="UserId"/>; the
/// client IP and user-agent are the only signals available for those.
/// </summary>
public class AuditLog
{
    public int Id { get; set; }

    /// <summary>"Create", "Update" or "Delete". Legacy rows hold values like "DeleteWord".</summary>
    public string Action { get; set; } = string.Empty;

    /// <summary>Type of entity affected, e.g. "Word", "Category".</summary>
    public string EntityType { get; set; } = string.Empty;

    /// <summary>Primary key of the affected entity.</summary>
    public int EntityId { get; set; }

    /// <summary>Human-readable snapshot of what changed (e.g. the Kurdish word text).</summary>
    public string? Summary { get; set; }

    /// <summary>
    /// JSON array of the fields that changed: [{"field":"Kurdish","old":"...","new":"..."}].
    /// Null for creates and deletes, where the whole row is the change.
    /// </summary>
    public string? Changes { get; set; }

    // ── Who ────────────────────────────────────────────────────────────────
    /// <summary>The signed-in user who made the change. Null for pre-auth rows or system tasks.</summary>
    public Guid? UserId { get; set; }
    public AppUser? User { get; set; }

    /// <summary>Denormalised so the log still reads correctly if the user is later renamed or deleted.</summary>
    public string? UserName { get; set; }

    // ── From where ─────────────────────────────────────────────────────────
    /// <summary>Real client IP, resolved from CF-Connecting-IP / X-Forwarded-For when behind Cloudflare.</summary>
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public static class AuditActions
{
    public const string Create = "Create";
    public const string Update = "Update";
    public const string Delete = "Delete";
    public const string Login = "Login";
    public const string LoginFailed = "LoginFailed";
    public const string Logout = "Logout";
}
