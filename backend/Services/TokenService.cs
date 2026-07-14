using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using backend.Data.Models;
using Microsoft.IdentityModel.Tokens;

namespace backend.Services;

public class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Issuer { get; set; } = "KurdishDictionary";
    public string Audience { get; set; } = "KurdishDictionary";

    /// <summary>Signing key. MUST be overridden in production via Jwt__Key — see appsettings.</summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>Seven days: the user asked to stay signed in for a week without re-entering a password.</summary>
    public int ExpiryDays { get; set; } = 7;
}

public interface ITokenService
{
    (string Token, DateTime ExpiresAt) Create(AppUser user, IEnumerable<string> roles);
}

public class TokenService : ITokenService
{
    private readonly JwtOptions _options;

    public TokenService(JwtOptions options) => _options = options;

    public (string Token, DateTime ExpiresAt) Create(AppUser user, IEnumerable<string> roles)
    {
        var expiresAt = DateTime.UtcNow.AddDays(_options.ExpiryDays);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.UserName ?? string.Empty),
            new(ClaimTypes.Email, user.Email ?? string.Empty),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new("fullName", user.FullName ?? user.UserName ?? string.Empty),
        };

        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var creds = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: creds);

        return (new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
    }
}
