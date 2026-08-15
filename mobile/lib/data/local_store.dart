import 'dart:convert';

import 'package:shared_preferences/shared_preferences.dart';

import '../models/word.dart';

/// All on-device persistence: settings, the saved token, and the favourites /
/// history caches. Favourites store a trimmed copy of the word itself so the
/// list still renders with no network.
class LocalStore {
  LocalStore(this._prefs);

  final SharedPreferences _prefs;

  static Future<LocalStore> open() async =>
      LocalStore(await SharedPreferences.getInstance());

  static const _kApiBase = 'api_base';
  static const _kShareBase = 'share_base';
  static const _kThemeMode = 'theme_mode';
  static const _kTextScale = 'text_scale';
  static const _kToken = 'auth_token';
  static const _kTokenExpiry = 'auth_token_expiry';
  static const _kUser = 'auth_user';
  static const _kFavorites = 'favorites';
  static const _kHistory = 'history';
  static const _kRecentSearches = 'recent_searches';

  /// History and recent searches are convenience caches, not archives — keeping
  /// them bounded stops the prefs blob growing without limit.
  static const _historyLimit = 60;
  static const _recentSearchLimit = 12;

  // ── Settings ────────────────────────────────────────────────────────────

  String? get apiBase => _prefs.getString(_kApiBase);
  Future<void> setApiBase(String v) => _prefs.setString(_kApiBase, v);

  String? get shareBase => _prefs.getString(_kShareBase);
  Future<void> setShareBase(String v) => _prefs.setString(_kShareBase, v);

  /// 0 = system, 1 = light, 2 = dark.
  int get themeMode => _prefs.getInt(_kThemeMode) ?? 2;
  Future<void> setThemeMode(int v) => _prefs.setInt(_kThemeMode, v);

  double get textScale => _prefs.getDouble(_kTextScale) ?? 1.0;
  Future<void> setTextScale(double v) => _prefs.setDouble(_kTextScale, v);

  // ── Auth ────────────────────────────────────────────────────────────────

  String? get token {
    final t = _prefs.getString(_kToken);
    if (t == null || t.isEmpty) return null;
    final expiry = _prefs.getString(_kTokenExpiry);
    if (expiry != null) {
      final at = DateTime.tryParse(expiry);
      // Drop it locally the moment it lapses rather than waiting for the server
      // to reject it and having the UI flicker through a signed-in state.
      if (at != null && at.isBefore(DateTime.now().toUtc())) return null;
    }
    return t;
  }

  Map<String, dynamic>? get cachedUser {
    final raw = _prefs.getString(_kUser);
    if (raw == null) return null;
    try {
      final decoded = jsonDecode(raw);
      return decoded is Map<String, dynamic> ? decoded : null;
    } catch (_) {
      return null;
    }
  }

  Future<void> saveSession({
    required String token,
    DateTime? expiresAt,
    required Map<String, dynamic> user,
  }) async {
    await _prefs.setString(_kToken, token);
    await _prefs.setString(_kUser, jsonEncode(user));
    if (expiresAt != null) {
      await _prefs.setString(_kTokenExpiry, expiresAt.toUtc().toIso8601String());
    } else {
      await _prefs.remove(_kTokenExpiry);
    }
  }

  Future<void> clearSession() async {
    await _prefs.remove(_kToken);
    await _prefs.remove(_kTokenExpiry);
    await _prefs.remove(_kUser);
  }

  // ── Favourites ──────────────────────────────────────────────────────────

  List<Word> get favorites => _readWords(_kFavorites);

  bool isFavorite(int id) => favorites.any((w) => w.id == id);

  /// Returns the state the word ended up in, so the caller can show the right
  /// confirmation without re-reading the list.
  Future<bool> toggleFavorite(Word word) async {
    final list = favorites;
    final existing = list.indexWhere((w) => w.id == word.id);
    if (existing >= 0) {
      list.removeAt(existing);
      await _writeWords(_kFavorites, list);
      return false;
    }
    // Newest first — the favourites screen reads top-down.
    list.insert(0, word);
    await _writeWords(_kFavorites, list);
    return true;
  }

  Future<void> clearFavorites() => _prefs.remove(_kFavorites);

  // ── History ─────────────────────────────────────────────────────────────

  List<Word> get history => _readWords(_kHistory);

  Future<void> pushHistory(Word word) async {
    final list = history..removeWhere((w) => w.id == word.id);
    list.insert(0, word);
    if (list.length > _historyLimit) list.removeRange(_historyLimit, list.length);
    await _writeWords(_kHistory, list);
  }

  Future<void> clearHistory() => _prefs.remove(_kHistory);

  // ── Recent searches ─────────────────────────────────────────────────────

  List<String> get recentSearches =>
      _prefs.getStringList(_kRecentSearches) ?? const [];

  Future<void> pushRecentSearch(String term) async {
    final t = term.trim();
    if (t.isEmpty) return;
    final list = recentSearches.toList()..removeWhere((e) => e == t);
    list.insert(0, t);
    if (list.length > _recentSearchLimit) {
      list.removeRange(_recentSearchLimit, list.length);
    }
    await _prefs.setStringList(_kRecentSearches, list);
  }

  Future<void> clearRecentSearches() => _prefs.remove(_kRecentSearches);

  // ── Helpers ─────────────────────────────────────────────────────────────

  List<Word> _readWords(String key) {
    final raw = _prefs.getStringList(key) ?? const [];
    final out = <Word>[];
    for (final entry in raw) {
      try {
        final decoded = jsonDecode(entry);
        if (decoded is Map<String, dynamic>) out.add(Word.fromJson(decoded));
      } catch (_) {
        // A single corrupt entry (e.g. from an older schema) shouldn't wipe out
        // the whole list — skip it and keep the rest.
      }
    }
    return out;
  }

  Future<void> _writeWords(String key, List<Word> words) => _prefs.setStringList(
        key,
        [for (final w in words) jsonEncode(w.toCacheJson())],
      );
}
