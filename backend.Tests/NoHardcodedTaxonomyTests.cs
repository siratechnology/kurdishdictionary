using System.Text.RegularExpressions;

namespace backend.Tests;

/// <summary>
/// Acceptance test ٩: grep for hardcoded Kurdish taxonomy terms across the solution → zero hits
/// outside seed data and test fixtures.
///
/// Written as a test rather than left as a command somebody is supposed to remember to run. The
/// whole system is a CONFIGURATION ENGINE: the team renames ئاوەڵناو to هاوەڵناو from the settings
/// screen, and any rule, label or branch keyed to the old spelling silently stops firing the moment
/// they do. That failure is invisible — nothing throws, a queue bucket just quietly reports zero
/// forever — so it has to be caught mechanically.
///
/// Rules key on <c>Code</c> (see <c>TaxonomyCodes</c>), never on <c>NameKu</c>. This test is what
/// keeps that true.
/// </summary>
public class NoHardcodedTaxonomyTests
{
    /// <summary>
    /// Taxonomy terms the TEAM owns — every one of them renameable from settings. A part of speech,
    /// an axis, or a value name appearing in application code is the bug.
    ///
    /// Deliberately not a list of every Kurdish word in the repo: UI copy is Kurdish and must stay
    /// that way. These are only the terms that name a row somebody can rename.
    /// </summary>
    private static readonly string[] TaxonomyTerms =
    {
        // Parts of speech, and the misspellings the live data carries.
        "ناو", "کار", "هاوەڵناو", "هاوەڵکار", "جێناو", "ئامڕاز", "چاوگ",
        "ئاوەڵناو", "هەوەڵناو", "هەوەڵکار", "سیفەت",

        // Axis names from the source deck.
        "ژمارە", "ڕەگەز", "دەمژمێر", "جۆری کار", "تێپەڕی", "ڕۆنان", "پلە",

        // Values, including the noun's fifth axis and the ones rules used to match on.
        "تاک", "کۆ", "نێر", "مێ", "تەواو", "ناتەواو", "تێپەڕ", "نەتێپەڕ",
        "داڕێژراو", "داڕێژڕاو", "لێکدراو", "سەربەخۆ", "لکاو",
        "گشتی", "تایبەت", "بەرجەستە", "واتای",
    };

    /// <summary>
    /// Where the terms are legitimate, and why.
    ///
    ///  · <c>TaxonomyModelConfiguration</c> — the seed. Two things are seeded in the whole taxonomy
    ///    (the seven parts of speech and the relation types) and both are named there by definition.
    ///  · <c>Migrations/</c> — historical seed data. A migration is a record of what was applied; it
    ///    cannot be edited to follow a later rename.
    ///  · <c>backend.Tests/</c> — fixtures. A test builds کار's cascade to prove it is buildable.
    ///  · <c>DbSeeder</c> — first-run demo content.
    /// </summary>
    private static readonly string[] Allowed =
    {
        Path.Combine("backend", "Data", "TaxonomyModelConfiguration.cs"),
        Path.Combine("backend", "Data", "DbSeeder.cs"),
        Path.Combine("backend", "Migrations"),
        "backend.Tests",
    };

    [Fact]
    public void No_application_code_matches_on_a_renameable_taxonomy_term()
    {
        var root = SolutionRoot();
        var offenders = new List<string>();

        foreach (var file in SourceFiles(root))
        {
            var relative = Path.GetRelativePath(root, file);

            if (Allowed.Any(a => relative.StartsWith(a, StringComparison.OrdinalIgnoreCase)))
                continue;

            var lines = File.ReadAllLines(file);

            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];

                // Comments are prose. Explaining WHY تێپەڕی only appears on a تەواو verb is exactly
                // the documentation this codebase wants; a comment cannot stop a rule from firing.
                if (IsComment(line)) continue;

                foreach (var term in TaxonomyTerms)
                {
                    if (!line.Contains(term)) continue;

                    // Kurdish UI copy legitimately contains these words — «مانا بەبێ بەشی ئاخاوتن»
                    // is a label, not a rule. What is banned is a COMPARISON: a branch, a match or a
                    // lookup keyed to a name the team can rename tomorrow.
                    if (!IsComparison(line, term)) continue;

                    offenders.Add($"{relative}:{i + 1}  {line.Trim()}");
                }
            }
        }

        Assert.True(offenders.Count == 0,
            "Application code matches on a taxonomy name the team can rename from settings. " +
            "Key the rule on FeatureValue.Code instead (see TaxonomyCodes):\n  " +
            string.Join("\n  ", offenders));
    }

    /// <summary>
    /// A comparison against a literal: <c>== "ناو"</c>, <c>Contains("کۆ")</c>, <c>case "تاک"</c>,
    /// <c>switch { "مێ" =&gt; … }</c>. These are the shapes that break on a rename.
    /// </summary>
    private static bool IsComparison(string line, string term)
    {
        var escaped = Regex.Escape(term);

        var patterns = new[]
        {
            $@"[=!]=\s*""[^""]*{escaped}",                        // x == "ناو"
            $@"""[^""]*{escaped}[^""]*""\s*[=!]=",                // "ناو" == x
            $@"\b(Contains|StartsWith|EndsWith|Equals|IndexOf)\s*\(\s*""[^""]*{escaped}",
            $@"\bcase\s+""[^""]*{escaped}",                       // case "تاک":
            $@"""[^""]*{escaped}[^""]*""\s*=>",                    // switch arm
        };

        return patterns.Any(p => Regex.IsMatch(line, p));
    }

    private static bool IsComment(string line)
    {
        var trimmed = line.TrimStart();

        return trimmed.StartsWith("//") || trimmed.StartsWith("*") || trimmed.StartsWith("/*")
               || trimmed.StartsWith("@*") || trimmed.StartsWith("<!--");
    }

    private static IEnumerable<string> SourceFiles(string root) =>
        new[] { "*.cs", "*.razor" }
            .SelectMany(pattern => Directory.EnumerateFiles(root, pattern, SearchOption.AllDirectories))
            .Where(f => !f.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}"))
            .Where(f => !f.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}"))
            .Where(f => !f.Contains($"{Path.DirectorySeparatorChar}node_modules{Path.DirectorySeparatorChar}"));

    /// <summary>Walks up from the test binary until the .sln is found.</summary>
    private static string SolutionRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);

        while (dir is not null && !dir.EnumerateFiles("*.sln").Any())
            dir = dir.Parent;

        Assert.NotNull(dir);
        return dir!.FullName;
    }
}
