namespace LexiconMigration;

/// <summary>
/// The mapping rules from پڕۆمپت ٤, in one place so the proposal is auditable rather than buried
/// in a loop. Nothing here guesses: a value that has no rule produces a blank and a conflict
/// reason, never an invented answer.
/// </summary>
public static class TaxonomyMap
{
    // ── جۆری وشە → PartOfSpeech ────────────────────────────────────────────
    // The live database's SpeechPaneType enum uses the WRONG terminology (ئاوەڵناو, ئاوەڵکار);
    // the deck's terms are هاوەڵناو and هاوەڵکار. The mapping normalises on the way across.
    public static readonly Dictionary<string, string> SpeechPaneToPartOfSpeech = new()
    {
        ["ناو"]        = "ناو",
        ["کار"]        = "کار",
        ["ئاوەڵناو"]   = "هاوەڵناو",
        ["سیفەت"]      = "هاوەڵناو",
        ["ئاوەڵکار"]   = "هاوەڵکار",
        ["چاوگ"]       = "چاوگ",
        ["جێناو"]      = "جێناو",
    };

    /// <summary>
    /// The deck's ten dictionaries (slide 13), stored WITHOUT the «فەرهەنگی» prefix.
    /// These are the ROOTS; the existing categories nest beneath them.
    /// </summary>
    public static readonly string[] DeckDomains =
    {
        "کولتووری",
        "زانستیی پزیشکی",   // note: not «پزیشکی» — copied character-for-character from the slide
        "زیندەوەرزانی",
        "کیمیا",
        "فیزیا",
        "ڕووەکناسی",
        "ڕامیاری",
        "ناو",
        "کشتوکاڵ",
        "ئاژەڵداری",
    };

    /// <summary>
    /// Categories that are a PART OF SPEECH in disguise. These must never be silently routed to a
    /// Domain — they are the duplication the whole refactor exists to remove, so they are flagged
    /// as conflicts and left for a human.
    /// </summary>
    public static readonly Dictionary<string, string> CategoriesThatArePartsOfSpeech = new()
    {
        ["سیفەت"]            = "هاوەڵناو",
        ["ئەنجامدانی کارێک"] = "کار",
        ["ئاوەڵناو"]         = "هاوەڵناو",
        ["ئاوەڵکار"]         = "هاوەڵکار",
        ["کار"]              = "کار",
        ["ناو"]              = "ناو",
        ["چاوگ"]             = "چاوگ",
    };

    /// <summary>
    /// Category → parent domain from the deck's ten. Deliberately conservative: only entries whose
    /// home is obvious from the deck appear here. Everything else lands in unmapped.csv rather than
    /// being forced somewhere plausible.
    /// </summary>
    public static readonly Dictionary<string, string> CategoryToDomain = new()
    {
        ["نەخۆشی"]            = "زانستیی پزیشکی",
        ["بارودۆخی جەستەیی"]  = "زانستیی پزیشکی",
        ["ئەندامی جەستە"]     = "زانستیی پزیشکی",
        ["دەروونی"]           = "زانستیی پزیشکی",

        ["ڕووەک"]             = "ڕووەکناسی",
        ["دار"]               = "ڕووەکناسی",
        ["گوڵ"]               = "ڕووەکناسی",

        ["ئاژەڵ"]             = "زیندەوەرزانی",
        ["باڵندە"]            = "زیندەوەرزانی",
        ["مێروو"]             = "زیندەوەرزانی",
        ["ماسی"]              = "زیندەوەرزانی",

        ["کشتوکاڵ"]           = "کشتوکاڵ",
        ["ئاژەڵداری"]         = "ئاژەڵداری",

        ["ڕامیاری"]           = "ڕامیاری",
        ["یاسا"]              = "ڕامیاری",

        ["کیمیا"]             = "کیمیا",
        ["فیزیا"]             = "فیزیا",

        ["کەس"]               = "ناو",
        ["شوێن"]              = "ناو",
        ["ناوی کەس"]          = "ناو",
        ["ناوی شوێن"]         = "ناو",

        ["کولتوور"]           = "کولتووری",
        ["ئایین"]             = "کولتووری",
        ["خواردن"]            = "کولتووری",
        ["جل و بەرگ"]         = "کولتووری",
        ["یاری"]              = "کولتووری",
        ["مۆسیقا"]            = "کولتووری",
        ["کەرەستە"]           = "کولتووری",
        ["ئامراز"]            = "کولتووری",
        ["مرۆڤ"]              = "کولتووری",
    };

    /// <summary>
    /// The gender enum is the ONE axis value that is mechanically derivable from the current data:
    /// it is already a closed enum on the word, not free text. Everything else is left blank —
    /// پڕۆمپت ٤ is explicit that grammatical features must never be invented.
    /// </summary>
    public static readonly Dictionary<string, string> GenderToAxisValue = new()
    {
        ["نێر"]      = "ڕەگەز=نێر",
        ["مێ"]       = "ڕەگەز=مێ",
        ["بێلایەن"]  = "ڕەگەز=بێلایەن",
        ["دوولایەن"] = "ڕەگەز=دوولایەن",
        // "نییە" (None) is the enum's default and means "nobody said", not "genderless".
        // Treating it as an answer would fabricate 2,000-odd feature values.
    };

    /// <summary>
    /// The same Kurdish fold پڕۆمپت ٥ will apply at search time, used here so the mapping tables
    /// are spelling-insensitive.
    ///
    /// This is not theoretical. The live category «ڕووەك» is written with the ARABIC kaf (U+0643)
    /// while the obvious map key «ڕووەک» uses the Kurdish one (U+06A9) — they are different strings,
    /// the lookup missed, and 49 words were reported as having no home. Every lookup below folds
    /// both sides so a keystroke cannot silently cost a mapping.
    /// </summary>
    public static string Fold(string? s)
    {
        if (string.IsNullOrWhiteSpace(s)) return "";

        var sb = new System.Text.StringBuilder(s.Length);
        foreach (var ch in s.Trim())
        {
            switch (ch)
            {
                case 'ي': case 'ى': sb.Append('ی'); break;   // Arabic yeh / alef maksura → Kurdish yeh
                case 'ك': sb.Append('ک'); break;             // Arabic kaf → Kurdish kaf
                case 'ھ': sb.Append('ه'); break;             // heh doachashmee → heh
                case 'ڕ': sb.Append('ر'); break;             // collapsed for matching only
                case '‌': break;                        // ZWNJ
                case 'ـ': break;                        // tatweel
                default:
                    // Arabic diacritics carry no lexical weight here.
                    if (ch is >= 'ً' and <= 'ْ') break;
                    sb.Append(ch);
                    break;
            }
        }

        return sb.ToString().Replace("وو", "و");
    }

    /// <summary>Dictionary lookup that folds both the key and the query.</summary>
    public static bool TryFolded(Dictionary<string, string> map, string? key, out string value)
    {
        value = "";
        if (string.IsNullOrWhiteSpace(key)) return false;

        var folded = Fold(key);
        foreach (var (k, v) in map)
        {
            if (Fold(k) != folded) continue;
            value = v;
            return true;
        }
        return false;
    }

    public static bool ContainsFolded(Dictionary<string, string> map, string? key) =>
        TryFolded(map, key, out _);

    /// <summary>
    /// The eight spellings of «کوردی سۆرانی» and the two of «فارسی», folded to BCP-47.
    /// A row whose label is not recognised is reported rather than assumed to be Sorani.
    /// </summary>
    public static string? LanguageCodeFor(string? locate)
    {
        if (string.IsNullOrWhiteSpace(locate)) return null;

        var t = locate.Trim()
                      .Replace("_", " ")
                      .Replace("-", " ")
                      .Replace("  ", " ")
                      .ToLowerInvariant();

        if (t.Contains("سۆران") || t.Contains("کوردی") || t == "کورد" || t.Contains("kurdish") || t.Contains("sorani"))
            return "ckb";

        if (t.Contains("فارس") || t.Contains("persian") || t.Contains("farsi"))
            return "fa";

        if (t.Contains("عەرەب") || t.Contains("arabic")) return "ar";
        if (t.Contains("ئینگلیز") || t.Contains("english")) return "en";
        if (t.Contains("تورکی") || t.Contains("turkish")) return "tr";
        if (t.Contains("بادین") || t.Contains("badini")) return "kmr";
        if (t.Contains("زازا") || t.Contains("zazaki")) return "zza";
        if (t.Contains("هەورام") || t.Contains("gorani")) return "hac";

        return null;
    }
}
