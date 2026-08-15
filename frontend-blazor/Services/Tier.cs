namespace frontend_blazor.Services;

/// <summary>
/// The contributor tiers — twenty positions from بڕۆنز ٤ to ئەڵماس ١.
///
/// A note on why this exists, because the code around it says the opposite in several places:
/// پڕۆمپت ٧ ruled out XP, ranking and accuracy scores, on the grounds that these are expert
/// language teachers whose names go in the published credits and that a scoreboard turns
/// colleagues into competitors. The client has since asked for tiers explicitly. This is that
/// decision, not a drift back into the thing the handoff forbade — so it is built to stay on
/// the safe side of it:
///
///   • The tier is a function of WORDS WRITTEN and nothing else. There is no accuracy score,
///     no speed, no quality grade, and no way for one person's tier to be lowered by another
///     person's work.
///   • Nobody is ever ranked "last". The floor is بڕۆنز ٤ at zero words, which is where a new
///     teacher starts, not a failing grade.
///   • Tiers never go DOWN. Thresholds are on a cumulative total that only grows.
///
/// The thresholds are fitted to the real distribution rather than round numbers: at the time
/// of writing the sixteen accounts run 0–1,233 words with most between 100 and 300, so the
/// low end is dense and the top end stretches. Round-number thresholds (100/200/300…) would
/// have put nine people in the same tier and left the top four empty.
/// </summary>
public static class Tier
{
    /// <summary>Ordered low → high. Index is the position, 0..19.</summary>
    public static readonly IReadOnlyList<TierLevel> All = new List<TierLevel>
    {
        //         family      grade  min words
        new("bronze",   "بڕۆنز",   4,     0),
        new("bronze",   "بڕۆنز",   3,    10),
        new("bronze",   "بڕۆنز",   2,    25),
        new("bronze",   "بڕۆنز",   1,    50),

        new("silver",   "زیو",     4,    75),
        new("silver",   "زیو",     3,   100),
        new("silver",   "زیو",     2,   150),
        new("silver",   "زیو",     1,   200),

        new("gold",     "زێڕ",     4,   250),
        new("gold",     "زێڕ",     3,   300),
        new("gold",     "زێڕ",     2,   400),
        new("gold",     "زێڕ",     1,   500),

        new("platinum", "پلاتین",  4,   600),
        new("platinum", "پلاتین",  3,   700),
        new("platinum", "پلاتین",  2,   850),
        new("platinum", "پلاتین",  1,  1000),

        new("diamond",  "ئەڵماس",  4,  1200),
        new("diamond",  "ئەڵماس",  3,  1500),
        new("diamond",  "ئەڵماس",  2,  2000),
        new("diamond",  "ئەڵماس",  1,  3000),
    };

    /// <summary>
    /// Index into <see cref="All"/> for a word count. Everything else is derived from this, so
    /// the "which tier" question is answered in exactly one place.
    /// </summary>
    private static int IndexFor(int wordCount)
    {
        // Walked from the top so the first match is the highest earned.
        for (var i = All.Count - 1; i >= 0; i--)
            if (wordCount >= All[i].MinWords) return i;

        return 0;
    }

    /// <summary>The tier a word count earns. Never returns null — zero words is a real tier.</summary>
    public static TierLevel For(int wordCount) => All[IndexFor(wordCount)];

    /// <summary>The next tier up, or null at ئەڵماس ١ where there is nothing left to reach.</summary>
    public static TierLevel? Next(int wordCount)
    {
        var idx = IndexFor(wordCount);
        return idx < All.Count - 1 ? All[idx + 1] : null;
    }

    /// <summary>
    /// How far through the current tier, 0..1. Used for the thin bar under a contributor row.
    /// Returns 1 at the top tier — a full bar reads better there than an empty one.
    /// </summary>
    public static double Progress(int wordCount)
    {
        var current = For(wordCount);
        var next = Next(wordCount);
        if (next is null) return 1;

        var span = next.MinWords - current.MinWords;
        if (span <= 0) return 1;

        return Math.Clamp((wordCount - current.MinWords) / (double)span, 0, 1);
    }

    /// <summary>Words still needed for the next tier; 0 at the top.</summary>
    public static int ToNext(int wordCount) =>
        Next(wordCount) is { } next ? Math.Max(0, next.MinWords - wordCount) : 0;

    /// <summary>Position 1..20 on the ladder. A distance travelled, never a rank against others.</summary>
    public static int Position(int wordCount) => IndexFor(wordCount) + 1;
}

/// <param name="Key">Family slug — also the CSS class suffix, e.g. <c>tier-gold</c>.</param>
/// <param name="Family">Kurdish family name shown on the badge.</param>
/// <param name="Grade">1..4 within the family, where 1 is the highest. Games order it this way.</param>
/// <param name="MinWords">Inclusive lower bound.</param>
public record TierLevel(string Key, string Family, int Grade, int MinWords)
{
    public string Label => $"{Family} {Grade}";
}
