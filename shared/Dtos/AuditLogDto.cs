namespace Shared.Dtos;

public class AuditLogDto
{
    public int Id { get; set; }
    public string Action { get; set; } = string.Empty;
    public string EntityType { get; set; } = string.Empty;
    public int EntityId { get; set; }
    public string? Summary { get; set; }

    /// <summary>JSON array of {field, old, new} — populated for updates only.</summary>
    public string? Changes { get; set; }

    public Guid? UserId { get; set; }
    public string? UserName { get; set; }

    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>One field that changed in an update, decoded from <see cref="AuditLogDto.Changes"/>.</summary>
public class AuditChangeDto
{
    public string Field { get; set; } = string.Empty;
    public string? Old { get; set; }
    public string? New { get; set; }
}
