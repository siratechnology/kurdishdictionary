import 'package:flutter/material.dart';

/// The design tokens from `frontend-nextjs/src/app/globals.css`, ported so the
/// phone app is visibly the same product as the web app: same indigo accent,
/// same deep-navy night surface, same "clear morning sky" day surface.
@immutable
class AppTokens extends ThemeExtension<AppTokens> {
  const AppTokens({
    required this.background,
    required this.backgroundGradient,
    required this.auroraColors,
    required this.surface,
    required this.surfaceRaised,
    required this.surfaceCard,
    required this.border,
    required this.borderStrong,
    required this.borderSubtle,
    required this.text1,
    required this.text2,
    required this.text3,
    required this.text4,
    required this.accent,
    required this.accentLight,
    required this.accentGlow,
    required this.inputFill,
    required this.skeletonBase,
    required this.shimmer,
    required this.isDark,
  });

  final Color background;

  /// Painted behind everything; the light theme is a four-stop sky gradient
  /// while the dark theme is a flat navy with the aurora blobs on top.
  final List<Color> backgroundGradient;

  /// The three ambient radial glows that replace the web app's `body::before`.
  final List<Color> auroraColors;

  final Color surface;
  final Color surfaceRaised;
  final Color surfaceCard;

  final Color border;
  final Color borderStrong;
  final Color borderSubtle;

  final Color text1;
  final Color text2;
  final Color text3;
  final Color text4;

  final Color accent;
  final Color accentLight;
  final Color accentGlow;

  final Color inputFill;
  final Color skeletonBase;
  final Color shimmer;

  final bool isDark;

  static const dark = AppTokens(
    background: Color(0xFF080C1A),
    backgroundGradient: [Color(0xFF080C1A), Color(0xFF080C1A)],
    auroraColors: [
      Color(0x1F6366F1), // indigo, bottom-left
      Color(0x178B5CF6), // violet, top-right
      Color(0x143B82F6), // blue, bottom-centre
    ],
    surface: Color(0xBF0F172A),
    surfaceRaised: Color(0x8C1E293B),
    surfaceCard: Color(0xCC0F172A),
    border: Color(0x246366F1),
    borderStrong: Color(0x526366F1),
    borderSubtle: Color(0x0DFFFFFF),
    text1: Color(0xFFE2E8F0),
    text2: Color(0xFF94A3B8),
    text3: Color(0xFF64748B),
    text4: Color(0xFF475569),
    accent: Color(0xFF6366F1),
    accentLight: Color(0xFF818CF8),
    accentGlow: Color(0x476366F1),
    inputFill: Color(0x8C1E293B),
    skeletonBase: Color(0xFF1E293B),
    shimmer: Color(0x14FFFFFF),
    isDark: true,
  );

  static const light = AppTokens(
    background: Color(0xFFDBEAFE),
    backgroundGradient: [
      Color(0xFFBFDBFE),
      Color(0xFFDBEAFE),
      Color(0xFFE0F2FE),
      Color(0xFFF0F9FF),
    ],
    auroraColors: [
      Color(0x8CBAE6FF),
      Color(0x73E0F2FE),
      Color(0x59F0F9FF),
    ],
    surface: Color(0xB3FFFFFF),
    surfaceRaised: Color(0xCCF8FAFF),
    surfaceCard: Color(0xBFFFFFFF),
    border: Color(0x296366F1),
    borderStrong: Color(0x576366F1),
    borderSubtle: Color(0x0F000000),
    text1: Color(0xFF1E293B),
    text2: Color(0xFF334155),
    text3: Color(0xFF475569),
    text4: Color(0xFF64748B),
    accent: Color(0xFF6366F1),
    accentLight: Color(0xFF4F46E5),
    accentGlow: Color(0x2E6366F1),
    inputFill: Color(0xBFFFFFFF),
    skeletonBase: Color(0xFFCBD5E1),
    shimmer: Color(0x8CFFFFFF),
    isDark: false,
  );

  @override
  AppTokens copyWith() => this;

  @override
  AppTokens lerp(ThemeExtension<AppTokens>? other, double t) {
    if (other is! AppTokens) return this;
    // Tokens only ever swap between the two constants above, and Flutter already
    // cross-fades the whole subtree on a theme change, so snapping at the
    // midpoint avoids muddy half-blended greys mid-animation.
    return t < 0.5 ? this : other;
  }
}

/// Relation-type colours, matching `MindMap.tsx`. The API spells the types in
/// PascalCase (`Synonym`), the web map keys them lower-case — we normalise.
class RelationStyle {
  const RelationStyle(this.color, this.label);

  final Color color;
  final String label;

  static const _map = <String, RelationStyle>{
    'synonym': RelationStyle(Color(0xFF22D36D), 'هاوماناکە'),
    'antonym': RelationStyle(Color(0xFFF87171), 'دژەوشە'),
    'related': RelationStyle(Color(0xFF60A5FA), 'پەیوەندیدار'),
    'example': RelationStyle(Color(0xFFFBBF24), 'نموونە'),
    'usage': RelationStyle(Color(0xFFA78BFA), 'بەکارهێنان'),
    'contextual': RelationStyle(Color(0xFF34D4F0), 'چوارچێوەیی'),
  };

  static const fallback = RelationStyle(Color(0xFF94A3B8), 'پەیوەندی');

  static RelationStyle of(String? relationType) =>
      _map[(relationType ?? '').toLowerCase()] ?? fallback;

  /// The distinct types in the order the detail screen groups them.
  static const order = ['synonym', 'antonym', 'related', 'example'];
}

/// A colour per part of speech, so a noun always reads the same hue whether it
/// appears as a chip on a card or as a node in the mind map.
class SpeechStyle {
  static const _colors = <int, Color>{
    1: Color(0xFF818CF8), // ناو — noun
    2: Color(0xFF34D399), // کار — verb
    3: Color(0xFFFBBF24), // ئاوەڵناو — adjective
    4: Color(0xFFF472B6), // ئاوەڵکار — adverb
    5: Color(0xFF22D3EE), // جێناو — pronoun
    6: Color(0xFFA78BFA), // پێشگر — preposition
    7: Color(0xFF4ADE80), // بەستەر — conjunction
    8: Color(0xFFFB923C), // بانگکردن — interjection
    9: Color(0xFF38BDF8), // دیارخەر — determiner
    10: Color(0xFFE879F9), // ژمارە — number
    11: Color(0xFF2DD4BF), // وردە وشە — particle
    12: Color(0xFF94A3B8), // ئامرازی ناساند — article
    13: Color(0xFFFCA5A5), // چاوگ — infinitive
    14: Color(0xFF64748B), // هیتر — other
  };

  static Color of(int id) => _colors[id] ?? const Color(0xFF94A3B8);
}

const kFontFamily = 'NRT';

ThemeData buildAppTheme(AppTokens t) {
  final scheme = ColorScheme.fromSeed(
    seedColor: t.accent,
    brightness: t.isDark ? Brightness.dark : Brightness.light,
  ).copyWith(
    surface: t.background,
    primary: t.accent,
  );

  TextStyle body(double size, {FontWeight w = FontWeight.w400, Color? c}) =>
      TextStyle(
        fontFamily: kFontFamily,
        fontSize: size,
        fontWeight: w,
        color: c ?? t.text1,
        // NRT sits tall; a little extra leading keeps stacked Sorani lines from
        // colliding at the diacritics.
        height: 1.55,
      );

  return ThemeData(
    useMaterial3: true,
    brightness: t.isDark ? Brightness.dark : Brightness.light,
    colorScheme: scheme,
    scaffoldBackgroundColor: Colors.transparent,
    canvasColor: Colors.transparent,
    fontFamily: kFontFamily,
    splashFactory: InkSparkle.splashFactory,
    extensions: [t],
    textTheme: TextTheme(
      displayLarge: body(40, w: FontWeight.w700),
      displayMedium: body(32, w: FontWeight.w700),
      headlineMedium: body(24, w: FontWeight.w600),
      titleLarge: body(20, w: FontWeight.w600),
      titleMedium: body(17, w: FontWeight.w600),
      bodyLarge: body(16),
      bodyMedium: body(15),
      bodySmall: body(13, c: t.text2),
      labelLarge: body(14, w: FontWeight.w600),
      labelSmall: body(12, c: t.text3),
    ),
    iconTheme: IconThemeData(color: t.text2),
    dividerTheme: DividerThemeData(color: t.borderSubtle, thickness: 1),
    appBarTheme: AppBarTheme(
      backgroundColor: Colors.transparent,
      surfaceTintColor: Colors.transparent,
      elevation: 0,
      centerTitle: true,
      titleTextStyle: body(19, w: FontWeight.w600),
      iconTheme: IconThemeData(color: t.text1),
    ),
    snackBarTheme: SnackBarThemeData(
      backgroundColor: t.isDark ? const Color(0xFF1E293B) : const Color(0xFF1E293B),
      contentTextStyle: body(14, c: Colors.white),
      behavior: SnackBarBehavior.floating,
      shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(14)),
    ),
    inputDecorationTheme: InputDecorationTheme(
      filled: true,
      fillColor: t.inputFill,
      hintStyle: body(15, c: t.text3),
      contentPadding:
          const EdgeInsets.symmetric(horizontal: 16, vertical: 14),
      border: OutlineInputBorder(
        borderRadius: BorderRadius.circular(16),
        borderSide: BorderSide(color: t.border),
      ),
      enabledBorder: OutlineInputBorder(
        borderRadius: BorderRadius.circular(16),
        borderSide: BorderSide(color: t.border),
      ),
      focusedBorder: OutlineInputBorder(
        borderRadius: BorderRadius.circular(16),
        borderSide: BorderSide(color: t.accent.withValues(alpha: 0.75), width: 1.5),
      ),
    ),
    filledButtonTheme: FilledButtonThemeData(
      style: FilledButton.styleFrom(
        backgroundColor: t.accent,
        foregroundColor: Colors.white,
        textStyle: body(15, w: FontWeight.w600),
        padding: const EdgeInsets.symmetric(horizontal: 22, vertical: 14),
        shape:
            RoundedRectangleBorder(borderRadius: BorderRadius.circular(14)),
      ),
    ),
    textButtonTheme: TextButtonThemeData(
      style: TextButton.styleFrom(
        foregroundColor: t.accentLight,
        textStyle: body(14, w: FontWeight.w600),
      ),
    ),
  );
}
