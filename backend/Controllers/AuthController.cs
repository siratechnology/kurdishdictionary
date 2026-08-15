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
    private readonly ILogger<AuthController> _log;

    public AuthController(
        UserManager<AppUser> users,
        RoleManager<AppRole> roles,
        ITokenService tokens,
        ICurrentUser current,
        AppDbContext db,
        ILogger<AuthController> log)
    {
        _users = users;
        _roles = roles;
        _tokens = tokens;
        _current = current;
        _db = db;
        _log = log;
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

    // PUT api/auth/me
    // Everything a signed-in user may change about themselves — which is everything EXCEPT
    // their roles and their active flag. Those two are the whole of the permission system, so
    // they stay on the admin-only endpoint and are not merely ignored here: UpdateProfileDto
    // does not carry them, so no future edit to this method can accidentally start honouring
    // a Roles field arriving from the browser.
    [HttpPut("me")]
    [Authorize]
    public async Task<ActionResult<AuthResultDto>> UpdateMe([FromBody] UpdateProfileDto dto)
    {
        var user = await _users.FindByIdAsync(_current.UserId.ToString()!);
        if (user is null || !user.IsActive) return Unauthorized();

        if (!string.IsNullOrWhiteSpace(dto.Email) &&
            !string.Equals(dto.Email, user.Email, StringComparison.OrdinalIgnoreCase))
        {
            var taken = await _users.FindByEmailAsync(dto.Email);
            if (taken is not null && taken.Id != user.Id)
                return Ok(Fail("ئەم ئیمەیلە پێشتر بەکارهاتووە."));

            user.Email = dto.Email.Trim();
            user.NormalizedEmail = _users.NormalizeEmail(user.Email);
        }

        user.FullName = string.IsNullOrWhiteSpace(dto.FullName) ? null : dto.FullName.Trim();

        var result = await _users.UpdateAsync(user);
        if (!result.Succeeded)
            return Ok(Fail(string.Join(" ", result.Errors.Select(e => e.Description))));

        var roles = await _users.GetRolesAsync(user);
        return Ok(new AuthResultDto { Succeeded = true, User = ToDto(user, roles) });
    }

    // POST api/auth/me/avatar
    [HttpPost("me/avatar")]
    [Authorize]
    [RequestSizeLimit(AvatarService.MaxBytes + 4096)]   // + a little for the multipart envelope
    public async Task<ActionResult<AuthResultDto>> UploadAvatar(
        IFormFile file, [FromServices] AvatarService avatars, CancellationToken ct)
    {
        var user = await _users.FindByIdAsync(_current.UserId.ToString()!);
        if (user is null || !user.IsActive) return Unauthorized();

        if (file is null || file.Length == 0)
            return Ok(Fail("هیچ فایلێک نەنێردرا."));

        string saved;
        try
        {
            await using var stream = file.OpenReadStream();
            saved = await avatars.SaveAsync(stream, file.Length, ct);
        }
        catch (InvalidOperationException ex)
        {
            // Thrown by AvatarService for everything the user can fix — too big, not an image.
            return Ok(Fail(ex.Message));
        }
        catch (Exception ex)
        {
            // Anything left is ours, not theirs. Letting it escape produces a 500, and a 500 has
            // no body — the screen can only say "internal error", which is the least useful thing
            // an upload button can say. Log the truth, answer with a sentence.
            _log.LogError(ex, "Avatar upload failed for {User}", user.UserName);
            return Ok(Fail("ناردنی وێنە سەرکەوتوو نەبوو. دووبارە هەوڵبدەوە."));
        }

        var previous = user.AvatarFile;
        user.AvatarFile = saved;

        var result = await _users.UpdateAsync(user);
        if (!result.Succeeded)
        {
            // The row did not change, so the file just written is an orphan. Remove it rather
            // than leaving the folder to grow one dead image per failed save.
            avatars.TryDelete(saved);
            return Ok(Fail(string.Join(" ", result.Errors.Select(e => e.Description))));
        }

        // Only once the new name is committed — losing the old file before that would leave the
        // user with no picture at all if the update failed.
        avatars.TryDelete(previous);

        var roles = await _users.GetRolesAsync(user);
        return Ok(new AuthResultDto { Succeeded = true, User = ToDto(user, roles) });
    }

    // DELETE api/auth/me/avatar
    [HttpDelete("me/avatar")]
    [Authorize]
    public async Task<ActionResult<AuthResultDto>> RemoveAvatar([FromServices] AvatarService avatars)
    {
        var user = await _users.FindByIdAsync(_current.UserId.ToString()!);
        if (user is null || !user.IsActive) return Unauthorized();

        var previous = user.AvatarFile;
        user.AvatarFile = null;

        var result = await _users.UpdateAsync(user);
        if (!result.Succeeded)
            return Ok(Fail(string.Join(" ", result.Errors.Select(e => e.Description))));

        avatars.TryDelete(previous);

        var roles = await _users.GetRolesAsync(user);
        return Ok(new AuthResultDto { Succeeded = true, User = ToDto(user, roles) });
    }

    // GET api/auth/leaderboard
    // Every signed-in user may read this — it is the dashboard's motivation board.
    // Editors only (admins are excluded), and it exposes no personal details.
    [HttpGet("leaderboard")]
    [Authorize]
    public async Task<ActionResult<List<ContributorDto>>> GetLeaderboard()
    {
        // EVERY active account except بەڕێوەبەر.
        //
        // Three narrower rules were tried and each hid somebody. Editors-minus-admins dropped an
        // administrator who writes. "Anyone who has ever authored a word" dropped the whole team's
        // station work, because the imported words have no author. Adding "admins with ledger
        // entries" patched that but still left a new وشەچن invisible until their first save.
        //
        // The honest rule is the simple one: this is the team, so list the team. Somebody with
        // nothing yet shows a zero, which is a true statement about a new colleague and not a
        // reason to leave them off the board.
        //
        // بەڕێوەبەر stays out. The card is the team's view of who is doing the lexicography, and
        // the seeded admin owns forty headwords from setting the system up — enough to sit above
        // real وشەچن on a list that is not about system administration.
        var adminIds = (await _users.GetUsersInRoleAsync(Roles.Admin))
            .Select(u => u.Id)
            .ToHashSet();

        // Words FIXED, from the contribution ledger. Distinct per word: a teacher who corrects one
        // word fifteen times has fixed one word, and a board that says fifteen rewards fiddling.
        var updatedCounts = await _db.ContributionEvents
            .Where(e => e.WordId != null && e.SourceKind == ContributionSourceKind.Human)
            .GroupBy(e => e.UserId)
            .Select(g => new { UserId = g.Key, Count = g.Select(e => e.WordId).Distinct().Count() })
            .ToDictionaryAsync(x => x.UserId, x => x.Count);

        var candidates = await _db.Users
            .AsNoTracking()
            .Where(u => u.IsActive && !adminIds.Contains(u.Id))
            .ToListAsync();

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
                WordsUpdated = updatedCounts.GetValueOrDefault(u.Id),
                AvatarUrl = AvatarUrl(u.AvatarFile),
            })
            // Ranked on everything they have done — written plus fixed. Ordering on either half
            // alone puts whoever specialises in the other half at the bottom of the board.
            .OrderByDescending(c => c.WordCount + c.WordsUpdated)
            .ThenByDescending(c => c.WordsUpdated)
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
    // POST api/auth/users/{id}/avatar
    // An admin setting someone else's picture — the same validation and the same storage as the
    // self-service route, differing only in whose row is written. Kept as a separate endpoint
    // rather than a userId parameter on the self route, so that "change my own picture" can never
    // be turned into "change anyone's" by supplying an extra field.
    [HttpPost("users/{id:guid}/avatar")]
    [Authorize(Roles = Roles.Admin)]
    [RequestSizeLimit(AvatarService.MaxBytes + 4096)]
    public async Task<ActionResult<AuthResultDto>> UploadUserAvatar(
        Guid id, IFormFile file, [FromServices] AvatarService avatars, CancellationToken ct)
    {
        var user = await _users.FindByIdAsync(id.ToString());
        if (user is null) return NotFound();

        if (file is null || file.Length == 0)
            return Ok(Fail("هیچ فایلێک نەنێردرا."));

        string saved;
        try
        {
            await using var stream = file.OpenReadStream();
            saved = await avatars.SaveAsync(stream, file.Length, ct);
        }
        catch (InvalidOperationException ex)
        {
            return Ok(Fail(ex.Message));
        }
        catch (Exception ex)
        {
            // Same reasoning as the self-service route above: never answer an upload with a 500.
            _log.LogError(ex, "Avatar upload failed for {User}", user.UserName);
            return Ok(Fail("ناردنی وێنە سەرکەوتوو نەبوو. دووبارە هەوڵبدەوە."));
        }

        var previous = user.AvatarFile;
        user.AvatarFile = saved;

        var result = await _users.UpdateAsync(user);
        if (!result.Succeeded)
        {
            avatars.TryDelete(saved);
            return Ok(Fail(string.Join(" ", result.Errors.Select(e => e.Description))));
        }

        avatars.TryDelete(previous);

        return Ok(new AuthResultDto { Succeeded = true, User = ToDto(user, await _users.GetRolesAsync(user)) });
    }

    // DELETE api/auth/users/{id}/avatar
    [HttpDelete("users/{id:guid}/avatar")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<ActionResult<AuthResultDto>> RemoveUserAvatar(
        Guid id, [FromServices] AvatarService avatars)
    {
        var user = await _users.FindByIdAsync(id.ToString());
        if (user is null) return NotFound();

        var previous = user.AvatarFile;
        user.AvatarFile = null;

        var result = await _users.UpdateAsync(user);
        if (!result.Succeeded)
            return Ok(Fail(string.Join(" ", result.Errors.Select(e => e.Description))));

        avatars.TryDelete(previous);

        return Ok(new AuthResultDto { Succeeded = true, User = ToDto(user, await _users.GetRolesAsync(user)) });
    }

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
        AvatarUrl = AvatarUrl(user.AvatarFile),
    };

    /// <summary>
    /// The public URL for a stored avatar. Composed here and nowhere else: the client is never
    /// given a file name to build a path from, so a bad name in the column cannot become a
    /// request for something outside the avatar folder.
    /// </summary>
    private static string? AvatarUrl(string? file) =>
        string.IsNullOrWhiteSpace(file) ? null : $"/avatars/{file}";
}
