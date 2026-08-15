using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos;

public class LoginDto
{
    [Required(ErrorMessage = "ناوی بەکارهێنەر پێویستە")]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "وشەی نهێنی پێویستە")]
    public string Password { get; set; } = string.Empty;

    /// <summary>Keeps the session alive for a week instead of just the browser session.</summary>
    public bool RememberMe { get; set; } = true;
}

public class AuthResultDto
{
    public bool Succeeded { get; set; }
    public string? Error { get; set; }

    public string? Token { get; set; }
    public DateTime? ExpiresAt { get; set; }

    public UserDto? User { get; set; }
}

public class UserDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? FullName { get; set; }
    public bool IsActive { get; set; }
    public bool MustChangePassword { get; set; }
    public List<string> Roles { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public string? LastLoginIp { get; set; }

    /// <summary>How many words this user owns — shown in the user list.</summary>
    public int WordCount { get; set; }

    /// <summary>
    /// Relative URL of the profile picture, or null for the initial-letter fallback.
    /// Built by the server from the stored file name — the client never composes a path.
    /// </summary>
    public string? AvatarUrl { get; set; }
}

/// <summary>
/// Public-to-every-signed-in-user slice of a user: just enough to render the
/// contributor leaderboard, with no email, IP or account flags attached.
/// </summary>
public class ContributorDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string? FullName { get; set; }
    /// <summary>Words this person AUTHORED — the headword is theirs.</summary>
    public int WordCount { get; set; }

    /// <summary>
    /// Distinct words this person has FIXED — classified, corrected, related, given a sense.
    ///
    /// A different question from <see cref="WordCount"/> and the more useful one once the import
    /// has happened: nearly three thousand words arrived without an author, so authorship measures
    /// who typed a headword years ago while this measures who is doing the work now. Counted from
    /// the contribution ledger as DISTINCT words, so fifteen edits to one word is one word fixed,
    /// not fifteen.
    /// </summary>
    public int WordsUpdated { get; set; }

    /// <summary>
    /// Everything this person has done to the dictionary: words they wrote plus words they fixed.
    ///
    /// Neither half tells the story alone. Authorship misses the whole team's station work, since
    /// nearly three thousand words arrived from the import with no author; fixes miss whoever sat
    /// down and typed new headwords. A card showing one number was always understating somebody.
    ///
    /// The two sets do overlap — writing a word and later correcting it counts in both — so this
    /// is a sum of ACTIVITY, not a distinct count of words, and the card prints the halves beside
    /// it so the figure can always be taken apart.
    /// </summary>
    public int TotalContribution => WordCount + WordsUpdated;

    /// <summary>Relative URL of the profile picture, or null for the initial-letter fallback.</summary>
    public string? AvatarUrl { get; set; }
}

public class CreateUserDto
{
    [Required] public string UserName { get; set; } = string.Empty;
    [Required, EmailAddress] public string Email { get; set; } = string.Empty;
    public string? FullName { get; set; }
    [Required, MinLength(6)] public string Password { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
}

public class UpdateUserDto
{
    [Required, EmailAddress] public string Email { get; set; } = string.Empty;
    public string? FullName { get; set; }
    public bool IsActive { get; set; } = true;
    public List<string> Roles { get; set; } = new();

    /// <summary>Optional — when set, resets the user's password to this value.</summary>
    public string? NewPassword { get; set; }
}

public class ChangePasswordDto
{
    [Required] public string CurrentPassword { get; set; } = string.Empty;
    [Required, MinLength(6, ErrorMessage = "وشەی نهێنی دەبێت لانیکەم ٦ پیت بێت")]
    public string NewPassword { get; set; } = string.Empty;
}

public class RoleDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int UserCount { get; set; }
}

/// <summary>
/// What a signed-in user may change about THEMSELVES.
///
/// Deliberately not <see cref="UpdateUserDto"/>. That one carries Roles and IsActive, and
/// reusing it here would mean the endpoint had to remember to ignore two fields on every
/// request — a check that works until somebody adds a third. A separate shape cannot carry a
/// privilege it does not declare.
/// </summary>
public class UpdateProfileDto
{
    public string? FullName { get; set; }

    [EmailAddress] public string? Email { get; set; }
}
