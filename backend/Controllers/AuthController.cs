using backend.Data;
using backend.Data.Models;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos;

namespace backend.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<AppUser> _users;
    private readonly RoleManager<AppRole> _roles;
    private readonly ITokenService _tokens;
    private readonly ICurrentUser _current;
    private readonly AppDbContext _db;

    public AuthController(
        UserManager<AppUser> users,
        RoleManager<AppRole> roles,
        ITokenService tokens,
        ICurrentUser current,
        AppDbContext db)
    {
        _users = users;
        _roles = roles;
        _tokens = tokens;
        _current = current;
        _db = db;
    }

    // POST api/auth/login
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResultDto>> Login([FromBody] LoginDto dto)
    {
        // Accept either the username or the email address — users reliably remember one of the two.
        var user = await _users.FindByNameAsync(dto.UserName)
                   ?? await _users.FindByEmailAsync(dto.UserName);

        if (user is null || !await _users.CheckPasswordAsync(user, dto.Password))
        {
            // Log the attempt (with IP) but tell the caller nothing about which half was wrong.
            await LogAuthEventAsync(AuditActions.LoginFailed, user?.Id, dto.UserName);
            return Ok(Fail("ناوی بەکارهێنەر یان وشەی نهێنی هەڵەیە"));
        }

        if (!user.IsActive)
        {
            await LogAuthEventAsync(AuditActions.LoginFailed, user.Id, user.UserName);
            return Ok(Fail("ئەم هەژمارە ڕاگیراوە. پەیوەندی بە بەڕێوەبەرەوە بکە"));
        }

        var roles = await _users.GetRolesAsync(user);
        var (token, expiresAt) = _tokens.Create(user, roles);

        user.LastLoginAt = DateTime.UtcNow;
        user.LastLoginIp = _current.IpAddress;
        await _users.UpdateAsync(user);

        await LogAuthEventAsync(AuditActions.Login, user.Id, user.UserName);

        return Ok(new AuthResultDto
        {
            Succeeded = true,
            Token = token,
            ExpiresAt = expiresAt,
            User = ToDto(user, roles),
        });
    }

    // GET api/auth/me — lets the Blazor app re-validate a cookie-held token against live user state
    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<UserDto>> Me()
    {
        var user = await _users.FindByIdAsync(_current.UserId.ToString()!);
        if (user is null || !user.IsActive) return Unauthorized();

        return Ok(ToDto(user, await _users.GetRolesAsync(user)));
    }

    // POST api/auth/change-password
    [HttpPost("change-password")]
    [Authorize]
    public async Task<ActionResult<AuthResultDto>> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        var user = await _users.FindByIdAsync(_current.UserId.ToString()!);
        if (user is null) return Unauthorized();

        var result = await _users.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
        if (!result.Succeeded)
            return Ok(Fail(string.Join(" ", result.Errors.Select(e => e.Description))));

        user.MustChangePassword = false;
        await _users.UpdateAsync(user);

        // The old token still carries the stale MustChangePassword flag, so hand back a fresh one.
        var roles = await _users.GetRolesAsync(user);
        var (token, expiresAt) = _tokens.Create(user, roles);

        return Ok(new AuthResultDto
        {
            Succeeded = true,
            Token = token,
            ExpiresAt = expiresAt,
            User = ToDto(user, roles),
        });
    }

    // GET api/auth/leaderboard
    // Every signed-in user may read this — it is the dashboard's motivation board.
    // Editors only (admins are excluded), and it exposes no personal details.
    [HttpGet("leaderboard")]
    [Authorize]
    public async Task<ActionResult<List<ContributorDto>>> GetLeaderboard()
    {
        var editors = await _users.GetUsersInRoleAsync(Roles.Editor);
        var admins = (await _users.GetUsersInRoleAsync(Roles.Admin)).Select(u => u.Id).ToHashSet();

        var candidates = editors
            .Where(u => u.IsActive && !admins.Contains(u.Id))
            .ToList();

        // One grouped query instead of a count per user.
        var wordCounts = await _db.Words
            .Where(w => w.CreatedByUserId != null)
            .GroupBy(w => w.CreatedByUserId!.Value)
            .Select(g => new { UserId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.UserId, x => x.Count);

        var result = candidates
            .Select(u => new ContributorDto
            {
                Id = u.Id,
                UserName = u.UserName ?? string.Empty,
                FullName = u.FullName,
                WordCount = wordCounts.GetValueOrDefault(u.Id),
            })
            .OrderByDescending(c => c.WordCount)
            .ThenBy(c => c.UserName)
            .ToList();

        return Ok(result);
    }

    // ── User management (Admin only) ───────────────────────────────────────

    // GET api/auth/users
    [HttpGet("users")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<ActionResult<List<UserDto>>> GetUsers()
    {
        var users = await _users.Users.AsNoTracking().OrderBy(u => u.UserName).ToListAsync();

        // One grouped query instead of a count per user.
        var wordCounts = await _db.Words
            .Where(w => w.CreatedByUserId != null)
            .GroupBy(w => w.CreatedByUserId!.Value)
            .Select(g => new { UserId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.UserId, x => x.Count);

        var result = new List<UserDto>(users.Count);
        foreach (var user in users)
        {
            var dto = ToDto(user, await _users.GetRolesAsync(user));
            dto.WordCount = wordCounts.GetValueOrDefault(user.Id);
            result.Add(dto);
        }

        return Ok(result);
    }

    // POST api/auth/users
    [HttpPost("users")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<ActionResult<AuthResultDto>> CreateUser([FromBody] CreateUserDto dto)
    {
        if (await _users.FindByNameAsync(dto.UserName) is not null)
            return Ok(Fail("ئەم ناوەی بەکارهێنەر پێشتر هەیە"));

        var user = new AppUser
        {
            Id = Guid.NewGuid(),
            UserName = dto.UserName,
            Email = dto.Email,
            FullName = dto.FullName,
            EmailConfirmed = true,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
        };

        var result = await _users.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            return Ok(Fail(string.Join(" ", result.Errors.Select(e => e.Description))));

        await SyncRolesAsync(user, dto.Roles);
        await LogAuthEventAsync("CreateUser", user.Id, user.UserName);

        return Ok(new AuthResultDto { Succeeded = true, User = ToDto(user, dto.Roles) });
    }

    // PUT api/auth/users/{id}
    [HttpPut("users/{id:guid}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<ActionResult<AuthResultDto>> UpdateUser(Guid id, [FromBody] UpdateUserDto dto)
    {
        var user = await _users.FindByIdAsync(id.ToString());
        if (user is null) return NotFound();

        // An admin who removes their own Admin role, or deactivates themselves, locks everyone out
        // of user management if they're the last one. Refuse rather than let that happen.
        if (id == _current.UserId && (!dto.IsActive || !dto.Roles.Contains(Roles.Admin)))
            return Ok(Fail("ناتوانیت ڕۆڵی بەڕێوەبەری خۆت لابەریت یان هەژمارەکەت ڕابگریت"));

        user.Email = dto.Email;
        user.FullName = dto.FullName;
        user.IsActive = dto.IsActive;

        var update = await _users.UpdateAsync(user);
        if (!update.Succeeded)
            return Ok(Fail(string.Join(" ", update.Errors.Select(e => e.Description))));

        if (!string.IsNullOrWhiteSpace(dto.NewPassword))
        {
            var token = await _users.GeneratePasswordResetTokenAsync(user);
            var reset = await _users.ResetPasswordAsync(user, token, dto.NewPassword);
            if (!reset.Succeeded)
                return Ok(Fail(string.Join(" ", reset.Errors.Select(e => e.Description))));
        }

        await SyncRolesAsync(user, dto.Roles);
        await LogAuthEventAsync("UpdateUser", user.Id, user.UserName);

        return Ok(new AuthResultDto { Succeeded = true, User = ToDto(user, dto.Roles) });
    }

    // DELETE api/auth/users/{id}
    [HttpDelete("users/{id:guid}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<ActionResult<AuthResultDto>> DeleteUser(Guid id)
    {
        if (id == _current.UserId)
            return Ok(Fail("ناتوانیت هەژماری خۆت بسڕیتەوە"));

        var user = await _users.FindByIdAsync(id.ToString());
        if (user is null) return NotFound();

        // Words.CreatedByUserId is SET NULL at the database level, but UpdatedByUserId cannot be
        // (SQL Server allows only one such path per table), so clear it here or the delete fails
        // on a foreign key violation. Either way the words themselves survive — they just lose
        // their author, which is the whole point of not cascading.
        await _db.Words
            .Where(w => w.UpdatedByUserId == id)
            .ExecuteUpdateAsync(s => s.SetProperty(w => w.UpdatedByUserId, (Guid?)null));

        var name = user.UserName;
        var result = await _users.DeleteAsync(user);
        if (!result.Succeeded)
            return Ok(Fail(string.Join(" ", result.Errors.Select(e => e.Description))));

        await LogAuthEventAsync("DeleteUser", null, name);
        return Ok(new AuthResultDto { Succeeded = true });
    }

    // GET api/auth/roles
    [HttpGet("roles")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<ActionResult<List<RoleDto>>> GetRoles()
    {
        var roles = await _roles.Roles.AsNoTracking().ToListAsync();
        var result = new List<RoleDto>();

        foreach (var role in roles)
        {
            var members = await _users.GetUsersInRoleAsync(role.Name!);
            result.Add(new RoleDto
            {
                Name = role.Name!,
                Description = role.Description,
                UserCount = members.Count,
            });
        }

        return Ok(result);
    }

    // ── Helpers ───────────────────────────────────────────────────────────

    private async Task SyncRolesAsync(AppUser user, List<string> wanted)
    {
        var valid = wanted.Where(r => Roles.All.Any(x => x.Name == r)).ToList();
        var current = await _users.GetRolesAsync(user);

        var toRemove = current.Except(valid).ToList();
        var toAdd = valid.Except(current).ToList();

        if (toRemove.Count > 0) await _users.RemoveFromRolesAsync(user, toRemove);
        if (toAdd.Count > 0) await _users.AddToRolesAsync(user, toAdd);
    }

    /// <summary>
    /// Sign-in events are written straight to the audit table: the interceptor only sees content
    /// entities, and a failed login has no entity at all — but it's exactly what you want to see
    /// when someone is guessing passwords.
    /// </summary>
    private async Task LogAuthEventAsync(string action, Guid? userId, string? userName)
    {
        _db.AuditLogs.Add(new AuditLog
        {
            Action = action,
            EntityType = "User",
            EntityId = 0,
            Summary = userName,
            UserId = userId,
            UserName = userName,
            IpAddress = _current.IpAddress,
            UserAgent = _current.UserAgent,
            Country = _current.Country,
            City = _current.City,
            CreatedAt = DateTime.UtcNow,
        });

        await _db.SaveChangesAsync();
    }

    private static AuthResultDto Fail(string error) => new() { Succeeded = false, Error = error };

    private static UserDto ToDto(AppUser user, IEnumerable<string> roles) => new()
    {
        Id = user.Id,
        UserName = user.UserName ?? string.Empty,
        Email = user.Email,
        FullName = user.FullName,
        IsActive = user.IsActive,
        MustChangePassword = user.MustChangePassword,
        Roles = roles.ToList(),
        CreatedAt = user.CreatedAt,
        LastLoginAt = user.LastLoginAt,
        LastLoginIp = user.LastLoginIp,
    };
}
