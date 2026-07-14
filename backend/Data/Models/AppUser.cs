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

    /// <summary>Roles allowed to create/update content.</summary>
    public const string AdminOrEditor = Admin + "," + Editor;

    /// <summary>Every signed-in role, including read-only.</summary>
    public const string Any = Admin + "," + Editor + "," + Viewer;

    public static readonly (string Name, string Description)[] All =
    {
        (Admin,  "دەسەڵاتی تەواو — بەڕێوەبردنی بەکارهێنەران و سڕینەوە"),
        (Editor, "زیادکردن و دەستکاری وشە و پۆلەکان"),
        (Viewer, "تەنها بینین"),
    };
}
