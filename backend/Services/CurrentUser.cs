using System.Security.Claims;
using backend.Data;

namespace backend.Services;

/// <summary>
/// The user behind the current request, as seen by the audit trail. Resolved from the JWT
/// claims on the incoming request; everything is nullable because background tasks and the
/// anonymous public endpoints have no user.
/// </summary>
public interface ICurrentUser
{
    Guid? UserId { get; }
    string? UserName { get; }
    string? IpAddress { get; }
    string? UserAgent { get; }
    string? Country { get; }
    string? City { get; }
}

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _accessor;

    public CurrentUser(IHttpContextAccessor accessor) => _accessor = accessor;

    private ClaimsPrincipal? Principal => _accessor.HttpContext?.User;

    public Guid? UserId =>
        Guid.TryParse(Principal?.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : null;

    public string? UserName =>
        Principal?.FindFirstValue(ClaimTypes.Name) ?? Principal?.FindFirstValue(ClaimTypes.Email);

    public string? IpAddress =>
        _accessor.HttpContext is { } ctx ? ClientInfo.GetClientIp(ctx.Request) : null;

    public string? UserAgent =>
        _accessor.HttpContext is { } ctx ? ClientInfo.GetUserAgent(ctx.Request) : null;

    // Cloudflare adds these at the edge; absent when running without the proxy in front.
    public string? Country =>
        _accessor.HttpContext?.Request.Headers["CF-IPCountry"].FirstOrDefault();

    public string? City =>
        _accessor.HttpContext?.Request.Headers["CF-IPCity"].FirstOrDefault();
}
