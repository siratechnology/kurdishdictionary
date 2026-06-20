using Microsoft.AspNetCore.Http;

namespace backend.Data;

/// <summary>
/// Extracts the real client IP and user-agent from an HttpRequest, accounting for
/// the Cloudflare proxy in front of the origin. Cloudflare puts the visitor's real
/// IP in "CF-Connecting-IP"; without this we would only ever log Cloudflare's edge IP.
/// </summary>
public static class ClientInfo
{
    public static string? GetClientIp(HttpRequest request)
    {
        // Cloudflare's canonical header for the original visitor IP.
        var cf = request.Headers["CF-Connecting-IP"].FirstOrDefault();
        if (!string.IsNullOrWhiteSpace(cf)) return cf.Trim();

        // Generic proxy header: take the first (left-most) entry = original client.
        var fwd = request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (!string.IsNullOrWhiteSpace(fwd))
            return fwd.Split(',')[0].Trim();

        // Direct connection (no proxy).
        return request.HttpContext.Connection.RemoteIpAddress?.ToString();
    }

    public static string? GetUserAgent(HttpRequest request) =>
        request.Headers.UserAgent.FirstOrDefault();
}
