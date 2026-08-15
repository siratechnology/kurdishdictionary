using Shared.Text;

namespace backend.Tests;

/// <summary>
/// The fold, tested on the pairs پڕۆمپت ٥ names plus the ones that bit us for real.
/// </summary>
public class KurdishTextTests
{
    // ── The three pairs the prompt calls out ───────────────────────────────
    // These are EQUIVALENCE cases: two spellings of one word must fold to one key.

    [Theory]
    [InlineData("کورد", "كورد")]              // Kurdish kaf U+06A9 vs Arabic kaf U+0643
    [InlineData("ڕەگ", "رەگ")]                // rreh vs reh
    [InlineData("کوردی", "كوردي")]            // both swaps at once, plus yeh
    [InlineData("جوانی", "جوانى")]            // alef maksura → yeh
    public void Spelling_variants_fold_to_one_key(string a, string b)
    {
        Assert.Equal(KurdishText.Normalize(a), KurdishText.Normalize(b));
        Assert.True(KurdishText.Matches(a, b));
    }

    /// <summary>
    /// جوان and جوانی are NOT the same word — the prompt pairs them to check that search finds one
    /// from the other, which is a prefix match, not a fold. Folding them together would merge two
    /// distinct headwords and is the failure this asserts against.
    /// </summary>
    [Fact]
    public void A_suffix_is_not_folded_away()
    {
        Assert.NotEqual(KurdishText.Normalize("جوان"), KurdishText.Normalize("جوانی"));

        // …but the shorter one is a prefix of the longer, which is what makes the search work.
        Assert.StartsWith(KurdishText.Normalize("جوان"), KurdishText.Normalize("جوانی"), StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("کوردی‌زمان", "کوردیزمان")]      // ZWNJ stripped
    [InlineData("کــورد", "کورد")]                    // tatweel stripped
    [InlineData("کُورد", "کورد")]                     // diacritic stripped
    [InlineData("ماست  و   خەیار", "ماست و خەیار")]   // runs of whitespace collapsed
    [InlineData("  کورد  ", "کورد")]                  // trimmed
    public void Noise_is_removed(string noisy, string clean)
    {
        Assert.Equal(KurdishText.Normalize(clean), KurdishText.Normalize(noisy));
    }

    [Fact]
    public void Double_waw_collapses()
    {
        Assert.Equal(KurdishText.Normalize("ڕووەک"), KurdishText.Normalize("ڕوەک"));
    }

    /// <summary>
    /// The exact bug that cost 69 rows in the پڕۆمپت ٤ proposal: the live category «ڕووەك» is
    /// written with the Arabic kaf while the map key used the Kurdish one.
    /// </summary>
    [Fact]
    public void The_category_that_broke_the_migration_map_now_matches()
    {
        Assert.True(KurdishText.Matches("ڕووەك", "ڕووەک"));
    }

    [Theory]
    [InlineData("ماله", "ماڵە")]    // final heh → ae … but ڵ is not ل, so these stay distinct
    public void Final_heh_becomes_ae_without_merging_other_letters(string a, string b)
    {
        Assert.NotEqual(KurdishText.Normalize(a), KurdishText.Normalize(b));
        Assert.EndsWith("ە", KurdishText.Normalize(a), StringComparison.Ordinal);
    }

    [Fact]
    public void Medial_heh_is_left_alone()
    {
        // Folding every heh would merge words that differ only in the middle.
        Assert.Contains('ه', KurdishText.Normalize("شهر"));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Empty_input_gives_empty_output(string? input)
    {
        Assert.Equal(string.Empty, KurdishText.Normalize(input));
    }

    [Fact]
    public void Normalizing_twice_changes_nothing()
    {
        // The column is written by an interceptor on every save, so a value that shifted on each
        // pass would rewrite rows forever and fill the audit log with phantom edits.
        foreach (var word in new[] { "کوردی", "ڕووەك", "ماست و خەیار", "جوانترین", "شهر" })
        {
            var once = KurdishText.Normalize(word);
            Assert.Equal(once, KurdishText.Normalize(once));
        }
    }
}
