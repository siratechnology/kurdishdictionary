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
