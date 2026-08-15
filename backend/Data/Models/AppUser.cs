using Microsoft.AspNetCore.Identity;

namespace backend.Data.Models;

/// <summary>
/// Application user. Guid keys (rather than the default string) so that foreign keys
/// from content tables are compact and strongly typed.
/// </summary>
public class AppUser : IdentityUser<Guid>
{
    public string? FullName { get; set; }

    /// <summary>Set false to block sign-in without deleting the user (keeps their audit trail intact).</summary>
    public bool IsActive { get; set; } = true;

    /// <summary>Forces a password change on next sign-in; set on seeded accounts.</summary>
    public bool MustChangePassword { get; set; }

    /// <summary>
    /// Whether this person publishes directly or their work goes to a Senior's queue (پڕۆمپت ٧).
    /// A workflow level, not a rank — never render it as a badge or a tier.
    /// </summary>
    public TrustLevel TrustLevel { get; set; } = TrustLevel.Contributor;

    /// <summary>
    /// Profile picture, stored as a FILE NAME only — never a path and never a full URL.
    ///
    /// The name is generated server-side from a Guid, so nothing a user typed ever reaches the
    /// file system: an uploaded name like <c>../../appsettings.json</c> cannot escape the upload
    /// folder if it is discarded before it is used. The folder comes from configuration and the
    /// public URL is built at render time, so moving storage to a CDN later changes one place.
    /// </summary>
    public string? AvatarFile { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAt { get; set; }
    public string? LastLoginIp { get; set; }
}

public class AppRole : IdentityRole<Guid>
{
    public string? Description { get; set; }
}

/// <summary>Canonical role names. Kept as constants so [Authorize(Roles = ...)] can't drift from the seed.</summary>
public static class Roles
{
    public const string Admin = "Admin";
    public const string Editor = "Editor";
    public const string Viewer = "Viewer";

    /// <summary>
    /// The single designated linguistic owner (پڕۆمپت ١١ tier 2). Adds and merges VALUES inside an
    /// existing axis, and edits conditional rules.
    ///
    /// Separate from Admin on purpose. The disease in this project is category sprawl — 26 became
    /// 79 in a month because creating one was easy — so extending the taxonomy is deliberately not
    /// something every administrator can do.
    /// </summary>
    public const string LinguisticOwner = "LinguisticOwner";

    /// <summary>Roles allowed to create/update content.</summary>
    public const string AdminOrEditor = Admin + "," + Editor;

    /// <summary>Every signed-in role, including read-only.</summary>
    public const string Any = Admin + "," + Editor + "," + Viewer;

    public static readonly (string Name, string Description)[] All =
    {
        (Admin,  "دەسەڵاتی تەواو — بەڕێوەبردنی بەکارهێنەران و سڕینەوە"),
        (Editor, "زیادکردن و دەستکاری وشە و پۆلەکان"),
        (Viewer, "تەنها بینین"),
        (LinguisticOwner, "خاوەنی زمانەوانی — زیادکردن و تێکەڵکردنی نرخەکان"),
    };
}
