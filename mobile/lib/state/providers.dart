import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../core/api_client.dart';
import '../core/config.dart';
import '../core/server_discovery.dart';
import '../core/sharing.dart';
import '../data/auth_repository.dart';
import '../data/local_store.dart';
import '../data/words_repository.dart';
import '../models/auth.dart';
import '../models/graph.dart';
import '../models/word.dart';

/// Overridden in `main()` once SharedPreferences has loaded — reading it before
/// then is a programming error, so it throws rather than returning a stub.
final localStoreProvider = Provider<LocalStore>(
  (ref) => throw StateError('localStoreProvider was not overridden'),
);

// ── Settings ──────────────────────────────────────────────────────────────

class Settings {
  const Settings({
    required this.apiBase,
    required this.shareBase,
    required this.themeMode,
    required this.textScale,
  });

  final String apiBase;
  final String shareBase;
  final ThemeMode themeMode;
  final double textScale;

  Settings copyWith({
    String? apiBase,
    String? shareBase,
    ThemeMode? themeMode,
    double? textScale,
  }) =>
      Settings(
        apiBase: apiBase ?? this.apiBase,
        shareBase: shareBase ?? this.shareBase,
        themeMode: themeMode ?? this.themeMode,
        textScale: textScale ?? this.textScale,
      );
}

class SettingsNotifier extends Notifier<Settings> {
  static const _modes = [ThemeMode.system, ThemeMode.light, ThemeMode.dark];

  LocalStore get _store => ref.read(localStoreProvider);

  @override
  Settings build() {
    final s = _store;
    return Settings(
      apiBase: s.apiBase ?? AppConfig.defaultApiBase,
      shareBase: s.shareBase ?? AppConfig.defaultShareBase,
      themeMode: _modes[s.themeMode.clamp(0, 2)],
      textScale: s.textScale,
    );
  }

  Future<void> setApiBase(String value) async {
    final v = _normalise(value);
    await _store.setApiBase(v);
    state = state.copyWith(apiBase: v);
  }

  /// True until the user (or [autoDetectApi]) has committed an address. Used to
  /// decide whether to probe automatically on first launch.
  bool get isApiConfigured => _store.apiBase != null;

  /// Probes the known candidate addresses and adopts whichever answers.
  /// Returns the address found, or null if none did.
  Future<String?> autoDetectApi() async {
    final discovery = ServerDiscovery();
    try {
      final found = await discovery.discover();
      if (found != null) await setApiBase(found);
      return found;
    } finally {
      discovery.close();
    }
  }

  Future<void> setShareBase(String value) async {
    final v = _normalise(value);
    await _store.setShareBase(v);
    state = state.copyWith(shareBase: v);
  }

  Future<void> setThemeMode(ThemeMode mode) async {
    await _store.setThemeMode(_modes.indexOf(mode));
    state = state.copyWith(themeMode: mode);
  }

  Future<void> setTextScale(double scale) async {
    await _store.setTextScale(scale);
    state = state.copyWith(textScale: scale);
  }

  /// Users type "192.168.1.14:6000" far more often than a full URL, and a
  /// trailing slash would produce `//api/words`.
  String _normalise(String raw) {
    var v = raw.trim();
    if (v.isEmpty) return AppConfig.defaultApiBase;
    if (!v.startsWith('http://') && !v.startsWith('https://')) v = 'http://$v';
    while (v.endsWith('/')) {
      v = v.substring(0, v.length - 1);
    }
    return v;
  }
}

final settingsProvider =
    NotifierProvider<SettingsNotifier, Settings>(SettingsNotifier.new);

// ── Auth ──────────────────────────────────────────────────────────────────

class AuthState {
  const AuthState({this.user, this.token, this.checking = false});

  final AuthUser? user;
  final String? token;

  /// True while a stored token is being revalidated on launch.
  final bool checking;

  bool get isSignedIn => user != null && (token ?? '').isNotEmpty;
  bool get canEdit => isSignedIn && user!.canEdit;
}

class AuthNotifier extends Notifier<AuthState> {
  LocalStore get _store => ref.read(localStoreProvider);

  @override
  AuthState build() {
    final token = _store.token;
    final cached = _store.cachedUser;
    if (token == null || cached == null) return const AuthState();
    // Show the cached identity straight away so the UI doesn't flash a
    // signed-out state, then confirm it with the server in the background.
    Future.microtask(_revalidate);
    return AuthState(
      user: AuthUser.fromJson(cached),
      token: token,
      checking: true,
    );
  }

  Future<void> _revalidate() async {
    final token = state.token;
    if (token == null) return;
    try {
      final user = await ref.read(authRepositoryProvider).me();
      state = AuthState(user: user, token: token);
    } on ApiException catch (e) {
      if (e.isUnauthorized) {
        await signOut();
      } else {
        // A network blip shouldn't sign anyone out — keep the cached session
        // and just stop showing the checking spinner.
        state = AuthState(user: state.user, token: token);
      }
    }
  }

  /// Returns null on success, or a human-readable Kurdish error.
  Future<String?> signIn(String userName, String password) async {
    try {
      final result =
          await ref.read(authRepositoryProvider).login(userName, password);
      if (!result.succeeded || result.token == null || result.user == null) {
        return result.error ?? 'ناوی بەکارهێنەر یان وشەی نهێنی هەڵەیە';
      }
      await _store.saveSession(
        token: result.token!,
        expiresAt: result.expiresAt,
        user: result.user!.toJson(),
      );
      state = AuthState(user: result.user, token: result.token);
      return null;
    } on ApiException catch (e) {
      return e.kurdishMessage;
    }
  }

  Future<void> signOut() async {
    await _store.clearSession();
    state = const AuthState();
  }
}

final authProvider =
    NotifierProvider<AuthNotifier, AuthState>(AuthNotifier.new);

// ── Networking ────────────────────────────────────────────────────────────

/// Rebuilt whenever the API address or the token changes, which in turn
/// invalidates every repository and cached query below it.
final apiClientProvider = Provider<ApiClient>((ref) {
  final base = ref.watch(settingsProvider).apiBase;
  final token = ref.watch(authProvider.select((a) => a.token));
  final client = ApiClient(baseUrl: base, token: token);
  ref.onDispose(client.close);
  return client;
});

final wordsRepositoryProvider =
    Provider<WordsRepository>((ref) => WordsRepository(ref.watch(apiClientProvider)));

final authRepositoryProvider =
    Provider<AuthRepository>((ref) => AuthRepository(ref.watch(apiClientProvider)));

final sharingProvider = Provider<WordSharing>(
    (ref) => WordSharing(ref.watch(settingsProvider).shareBase));

// ── Queries ───────────────────────────────────────────────────────────────

final categoriesProvider = FutureProvider<List<Category>>(
    (ref) => ref.watch(wordsRepositoryProvider).categories());

final speechStatsProvider = FutureProvider<List<SpeechPaneStat>>(
    (ref) => ref.watch(wordsRepositoryProvider).speechTypeStats());

final gendersProvider = FutureProvider<List<NamedOption>>(
    (ref) => ref.watch(wordsRepositoryProvider).genders());

final speechTypesProvider = FutureProvider<List<NamedOption>>(
    (ref) => ref.watch(wordsRepositoryProvider).speechTypes());

final locatesProvider = FutureProvider<List<String>>(
    (ref) => ref.watch(wordsRepositoryProvider).locates());

final wordProvider = FutureProvider.autoDispose
    .family<Word, int>((ref, id) => ref.watch(wordsRepositoryProvider).byId(id));

final graphProvider = FutureProvider.autoDispose
    .family<WordGraph, int>((ref, id) => ref.watch(wordsRepositoryProvider).graph(id));

// ── Favourites & history ──────────────────────────────────────────────────

class FavoritesNotifier extends Notifier<List<Word>> {
  LocalStore get _store => ref.read(localStoreProvider);

  @override
  List<Word> build() => _store.favorites;

  bool contains(int id) => state.any((w) => w.id == id);

  Future<bool> toggle(Word word) async {
    final nowSaved = await _store.toggleFavorite(word);
    state = _store.favorites;
    return nowSaved;
  }

  Future<void> clear() async {
    await _store.clearFavorites();
    state = const [];
  }
}

final favoritesProvider =
    NotifierProvider<FavoritesNotifier, List<Word>>(FavoritesNotifier.new);

class HistoryNotifier extends Notifier<List<Word>> {
  LocalStore get _store => ref.read(localStoreProvider);

  @override
  List<Word> build() => _store.history;

  Future<void> push(Word word) async {
    await _store.pushHistory(word);
    state = _store.history;
  }

  Future<void> clear() async {
    await _store.clearHistory();
    state = const [];
  }
}

final historyProvider =
    NotifierProvider<HistoryNotifier, List<Word>>(HistoryNotifier.new);

class RecentSearchesNotifier extends Notifier<List<String>> {
  LocalStore get _store => ref.read(localStoreProvider);

  @override
  List<String> build() => _store.recentSearches;

  Future<void> push(String term) async {
    await _store.pushRecentSearch(term);
    state = _store.recentSearches;
  }

  Future<void> clear() async {
    await _store.clearRecentSearches();
    state = const [];
  }
}

final recentSearchesProvider =
    NotifierProvider<RecentSearchesNotifier, List<String>>(
        RecentSearchesNotifier.new);
