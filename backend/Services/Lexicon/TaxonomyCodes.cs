namespace backend.Services.Lexicon;

/// <summary>
/// The handful of taxonomy codes that RULES depend on.
///
/// Everything about the taxonomy is the team's to configure and rename — except that a validator
/// has to be able to say "this word is داڕێژراو, so it needs a ڕەگ". It finds that out by code,
/// never by label, because labels change. The whole list is here so it is obvious how small the
/// coupling is, and so a rename in settings can never quietly disable a rule.
///
/// None of these are seeded. Until the team creates an axis with a matching code, the rules that
/// depend on it simply do not fire — an empty taxonomy is day one, not an error.
/// </summary>
public static class TaxonomyCodes
{
    public static class Axis
    {
        /// <summary>ڕۆنان — word formation: چەسپاو / داڕێژراو / لێکدراو.</summary>
        public const string Formation = "ronan";

        /// <summary>ژمارە — number: تاک / کۆ.</summary>
        public const string Number = "jimare";

        /// <summary>پلە — degree: چەسپاو / پلەی بەراورد / پلەی باڵا.</summary>
        public const string Degree = "pile";
    }

    public static class Value
    {
        /// <summary>داڕێژراو — derived from a root, so it must HAVE a root.</summary>
        public const string Derived = "derived";

        /// <summary>لێکدراو — compound, so it must have at least two components.</summary>
        public const string Compound = "compound";

        /// <summary>کۆ — plural.</summary>
        public const string Plural = "plural";

        /// <summary>پلەی بەراورد — comparative.</summary>
        public const string Comparative = "comparative";

        /// <summary>پلەی باڵا — superlative.</summary>
        public const string Superlative = "superlative";
    }

    public static class Relation
    {
        public const string Root = "root";
        public const string Component = "component";
        public const string Synonym = "synonym";
        public const string Antonym = "antonym";
    }
}
