namespace backend.Services;

/// <summary>
/// Extracts browser / OS / device family from a user-agent string.
///
/// Deliberately a small heuristic rather than a UA-parsing library: the dashboard only needs to
/// answer "mostly phones or mostly desktops? Chrome or Safari?", and a wrong answer on an exotic
/// UA costs nothing. Order matters — Edge and Opera both claim to be Chrome, so they're checked first.
/// </summary>
public static class UserAgentParser
{
    public static (string Browser, string Os, string DeviceType) Parse(string? ua)
    {
        if (string.IsNullOrWhiteSpace(ua)) return ("Unknown", "Unknown", "Unknown");

        return (Browser(ua), Os(ua), Device(ua));
    }

    private static string Browser(string ua)
    {
        if (Has(ua, "Edg/") || Has(ua, "Edge/")) return "Edge";
        if (Has(ua, "OPR/") || Has(ua, "Opera")) return "Opera";
        if (Has(ua, "SamsungBrowser")) return "Samsung Internet";
        if (Has(ua, "Firefox")) return "Firefox";
        if (Has(ua, "Chrome") || Has(ua, "CriOS")) return "Chrome";
        // Safari's UA also contains "Safari" on Chrome, so it must lose to Chrome above.
        if (Has(ua, "Safari")) return "Safari";
        if (Has(ua, "bot") || Has(ua, "crawler") || Has(ua, "spider")) return "Bot";
        return "Other";
    }

    private static string Os(string ua)
    {
        if (Has(ua, "Windows")) return "Windows";
        if (Has(ua, "Android")) return "Android";
        if (Has(ua, "iPhone") || Has(ua, "iPad") || Has(ua, "iOS")) return "iOS";
        if (Has(ua, "Mac OS X") || Has(ua, "Macintosh")) return "macOS";
        if (Has(ua, "Linux")) return "Linux";
        return "Other";
    }

    private static string Device(string ua)
    {
        if (Has(ua, "iPad") || Has(ua, "Tablet")) return "Tablet";
        if (Has(ua, "Mobi") || Has(ua, "iPhone") || Has(ua, "Android")) return "Mobile";
        return "Desktop";
    }

    private static bool Has(string ua, string token) =>
        ua.Contains(token, StringComparison.OrdinalIgnoreCase);
}
