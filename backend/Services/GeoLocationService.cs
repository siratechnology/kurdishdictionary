using System.Net;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Caching.Memory;

namespace backend.Services;

public record GeoInfo(
    string? Country,
    string? CountryCode,
    string? City,
    string? Region,
    string? Isp,
    string? Timezone);

public interface IGeoLocationService
{
    Task<GeoInfo> LookupAsync(string? ip, CancellationToken ct = default);
}

/// <summary>
/// Resolves a visitor IP to a country/city.
///
/// Cloudflare already tells us the country for free at the edge (CF-IPCountry), but not the city,
/// so anything richer needs a lookup. ip-api.com's free tier allows ~45 requests/minute from one
/// host, which a public dictionary can exceed during a traffic spike — so results are cached per IP
/// and a failed or rate-limited lookup degrades to "whatever Cloudflare told us" rather than
/// failing the request. Analytics must never be able to break the page it is measuring.
/// </summary>
public class GeoLocationService : IGeoLocationService
{
    private static readonly TimeSpan CacheFor = TimeSpan.FromHours(12);

    private readonly HttpClient _http;
    private readonly IMemoryCache _cache;
    private readonly ILogger<GeoLocationService> _logger;

    public GeoLocationService(HttpClient http, IMemoryCache cache, ILogger<GeoLocationService> logger)
    {
        _http = http;
        _cache = cache;
        _logger = logger;
    }

    public async Task<GeoInfo> LookupAsync(string? ip, CancellationToken ct = default)
    {
        var empty = new GeoInfo(null, null, null, null, null, null);
        if (string.IsNullOrWhiteSpace(ip) || IsPrivate(ip)) return empty;

        if (_cache.TryGetValue<GeoInfo>(CacheKey(ip), out var cached) && cached is not null)
            return cached;

        try
        {
            var res = await _http.GetFromJsonAsync<IpApiResponse>(
                $"http://ip-api.com/json/{Uri.EscapeDataString(ip)}?fields=status,country,countryCode,city,regionName,isp,timezone",
                ct);

            if (res?.Status != "success")
                return empty;

            var geo = new GeoInfo(res.Country, res.CountryCode, res.City, res.RegionName, res.Isp, res.Timezone);
            _cache.Set(CacheKey(ip), geo, CacheFor);
            return geo;
        }
        catch (Exception ex)
        {
            // Rate limit, DNS failure, timeout — none of these justify losing the analytics event.
            _logger.LogDebug(ex, "Geo lookup failed for {Ip}", ip);
            return empty;
        }
    }

    private static string CacheKey(string ip) => $"geo:{ip}";

    /// <summary>LAN / loopback / container-network addresses have no meaningful geo answer.</summary>
    private static bool IsPrivate(string ip)
    {
        if (!IPAddress.TryParse(ip, out var addr)) return true;
        if (IPAddress.IsLoopback(addr)) return true;

        var b = addr.GetAddressBytes();
        return b.Length == 4 && (
            b[0] == 10 ||
            (b[0] == 172 && b[1] >= 16 && b[1] <= 31) ||
            (b[0] == 192 && b[1] == 168) ||
            (b[0] == 169 && b[1] == 254));
    }

    private sealed class IpApiResponse
    {
        [JsonPropertyName("status")] public string? Status { get; set; }
        [JsonPropertyName("country")] public string? Country { get; set; }
        [JsonPropertyName("countryCode")] public string? CountryCode { get; set; }
        [JsonPropertyName("city")] public string? City { get; set; }
        [JsonPropertyName("regionName")] public string? RegionName { get; set; }
        [JsonPropertyName("isp")] public string? Isp { get; set; }
        [JsonPropertyName("timezone")] public string? Timezone { get; set; }
    }
}
