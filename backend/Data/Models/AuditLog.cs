namespace backend.Data.Models;

/// <summary>
/// Records destructive actions (deletes) so it can be traced who removed data,
/// from where, and when. There is no user authentication yet, so the strongest
/// signals available are the client IP (resolved through Cloudflare) and the
/// browser user-agent. Add login/auth later to also capture a user identity.
/// </summary>
public class AuditLog
{
    public int Id { get; set; }

    /// <summary>e.g. "DeleteWord", "DeleteCategory", "RemoveWordFromCategory".</summary>
    public string Action { get; set; } = string.Empty;

    /// <summary>Type of entity affected, e.g. "Word", "Category".</summary>
    public string EntityType { get; set; } = string.Empty;

    /// <summary>Primary key of the affected entity.</summary>
    public int EntityId { get; set; }

    /// <summary>Human-readable snapshot of what was deleted (e.g. the Kurdish word text).</summary>
    public string? Summary { get; set; }

    /// <summary>Real client IP, resolved from CF-Connecting-IP / X-Forwarded-For when behind Cloudflare.</summary>
    public string? IpAddress { get; set; }

    /// <summary>Browser / client user-agent string.</summary>
    public string? UserAgent { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
