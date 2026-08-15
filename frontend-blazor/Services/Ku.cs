using System.Globalization;

namespace frontend_blazor.Services;

/// <summary>
/// Number rendering for the admin.
///
/// Numbers are shown in WESTERN digits (0-9), not Arabic-Indic (٠-٩). The dictionary's prose is
/// Kurdish; its figures are read as figures, and the team asked for the Latin forms.
///
/// The digits are also set in Calibri rather than NRT — done entirely in CSS via a unicode-range
/// @font-face in Styles/app.css, so no call site has to know about it and no element needs a
/// special class. See the "Numerals" section of the admin-design-system skill.
///
/// This type stays as the single place numbers are formatted even though the substitution is gone:
/// grouping, percentage and signed-delta formatting still belong in one place, and a call site
/// doing its own ToString will drift on separators the first time somebody changes the culture.
/// </summary>
public static class Ku
{
    /// <summary>Grouped integer: 2853 → "2,853".</summary>
    public static string N(long value) => value.ToString("N0", CultureInfo.InvariantCulture);

    public static string N(int value) => N((long)value);

    /// <summary>
    /// A ratio as a whole percentage: 0.96 → "96%". Pass 0.96, not 96.
    /// Invariant's "P0" inserts a non-breaking space before the sign; the design does not use one.
    /// </summary>
    public static string P(double ratio) =>
        ratio.ToString("P0", CultureInfo.InvariantCulture).Replace(" ", "").Replace(" ", "");

    /// <summary>A decimal with a fixed number of places: 4.5 → "4.5".</summary>
    public static string D(double value, int decimals = 1) =>
        value.ToString("N" + decimals, CultureInfo.InvariantCulture);

    /// <summary>Digits only, no grouping — ids, years, time components.</summary>
    public static string Raw(long value) => value.ToString(CultureInfo.InvariantCulture);

    /// <summary>A signed delta for card footnotes: 966 → "+966", -12 → "−12".</summary>
    public static string Delta(long value) =>
        value >= 0 ? "+" + N(value) : "−" + N(-value);   // U+2212 minus, not a hyphen

    /// <summary>
    /// Passes a preformatted string through unchanged.
    ///
    /// Kept as a no-op rather than deleted: it is called on dates and clock times all over the app,
    /// and it is the seam where digit substitution would go back if the decision is ever reversed.
    /// </summary>
    public static string Sub(string s) => s;
}
