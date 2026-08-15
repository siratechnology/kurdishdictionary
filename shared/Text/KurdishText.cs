using System.Text;

namespace Shared.Text;

/// <summary>
/// The one Kurdish normalisation function (پڕۆمپت ٥).
///
/// Sorani is written in an Arabic-derived script that people type with whatever keyboard they have.
/// The same word arrives as کوردی or كوردي depending on whether the Kurdish or the Arabic layout was
/// used, and a database that compares the raw strings treats those as different words. This folds
/// every such variation to one form.
///
/// It is used at BOTH ends and must never be used at only one:
///   · write time — Word.Normalized and WordForm.Normalized
///   · query time — the search term, before it is compared
/// Normalising one side and not the other is worse than normalising neither, because it fails
/// silently on exactly the inputs it was added to fix.
///
/// The fold is LOSSY on purpose: ڕ and ر collapse, so ڕەگ and رەگ match. That is why the result
/// lives in a separate column and never replaces what the teacher typed — the headword keeps its
/// spelling, and only the matching key is flattened.
/// </summary>
public static class KurdishText
{
    private const char Zwnj = '‌';      // zero-width non-joiner
    private const char Tatweel = 'ـ';   // kashida, pure decoration

    /// <summary>Arabic combining marks: fatha…sukun, plus superscript alef.</summary>
    private static bool IsDiacritic(char c) =>
        c is >= 'ً' and <= 'ْ' or 'ٰ' or 'ٓ' or 'ٔ' or 'ٕ';

    /// <summary>
    /// Folds a Kurdish string to its matching key. Returns "" for null or whitespace.
    /// </summary>
    public static string Normalize(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return string.Empty;

        var trimmed = input.Trim();
        var sb = new StringBuilder(trimmed.Length);

        foreach (var c in trimmed)
        {
            switch (c)
            {
                // ي (Arabic yeh) · ى (alef maksura) → ی (Farsi/Kurdish yeh)
                case 'ي':
                case 'ى':
                    sb.Append('ی');
                    break;

                // ك (Arabic kaf) → ک (Kurdish kaf). The single most common mix-up: an Arabic
                // keyboard produces U+0643 where Kurdish wants U+06A9, and they look identical.
                case 'ك':
                    sb.Append('ک');
                    break;

                // ڕ (rreh) → ر. Collapsed for matching only.
                case 'ڕ':
                    sb.Append('ر');
                    break;

                // ھ (heh doachashmee) → ه. Word-final heh is handled after the loop.
                case 'ھ':
                    sb.Append('ه');
                    break;

                case Zwnj:
                case Tatweel:
                    break;

                default:
                    if (IsDiacritic(c)) break;

                    // Collapse runs of whitespace to a single space so "ماست  و خەیار" and
                    // "ماست و خەیار" are one key.
                    if (char.IsWhiteSpace(c))
                    {
                        if (sb.Length > 0 && sb[^1] != ' ') sb.Append(' ');
                        break;
                    }

                    sb.Append(char.ToLowerInvariant(c));
                    break;
            }
        }

        // وو → و. Done after the character pass so a ZWNJ or diacritic sitting between the two waws
        // cannot hide the pair from this step.
        CollapseDoubleWaw(sb);

        // Final ه → ە (ae). Only at the end of a word: medial heh is a different letter doing a
        // different job, and folding it everywhere would merge words that are genuinely distinct.
        FinalHehToAe(sb);

        return sb.ToString().Trim();
    }

    /// <summary>True when the two strings are the same word as far as search is concerned.</summary>
    public static bool Matches(string? a, string? b) =>
        string.Equals(Normalize(a), Normalize(b), StringComparison.Ordinal);

    private static void CollapseDoubleWaw(StringBuilder sb)
    {
        for (var i = sb.Length - 1; i > 0; i--)
        {
            if (sb[i] == 'و' && sb[i - 1] == 'و')
                sb.Remove(i, 1);
        }
    }

    private static void FinalHehToAe(StringBuilder sb)
    {
        for (var i = 0; i < sb.Length; i++)
        {
            if (sb[i] != 'ه') continue;

            var atEnd = i == sb.Length - 1 || sb[i + 1] == ' ';
            if (atEnd) sb[i] = 'ە';
        }
    }
}
