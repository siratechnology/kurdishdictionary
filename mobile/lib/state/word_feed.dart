import 'dart:async';

import 'package:flutter/foundation.dart';

import '../core/api_client.dart';
import '../data/words_repository.dart';
import '../models/word.dart';

/// Which slice of the dictionary a feed is showing. The three sources map onto
/// the three paged endpoints; everything else about paging is identical.
enum FeedSource { all, category, speechType }

/// A paginated, append-as-you-scroll list of words.
///
/// Deliberately a plain [ChangeNotifier] rather than a Riverpod notifier: the
/// feed is owned by the screen showing it, several can be alive at once with
/// different filters, and the scroll listener needs to call [loadMore] many
/// times per second without any provider bookkeeping in the way.
class WordFeed extends ChangeNotifier {
  WordFeed({
    required this.repository,
    this.source = FeedSource.all,
    this.sourceId,
    this.pageSize = 20,
  });

  final WordsRepository repository;
  final FeedSource source;

  /// Category id or speech-type id, depending on [source].
  final int? sourceId;
  final int pageSize;

  final List<Word> _items = [];
  List<Word> get items => List.unmodifiable(_items);

  int _page = 0;
  int _totalCount = 0;
  bool _hasMore = true;
  bool _loadingFirst = false;
  bool _loadingMore = false;
  String? _error;
  String _search = '';
  String? _category;

  /// Bumped on every reset so a slow in-flight page from a previous query can
  /// be recognised and dropped instead of appending results for a search the
  /// user has already moved on from.
  int _generation = 0;

  bool get isLoadingFirst => _loadingFirst;
  bool get isLoadingMore => _loadingMore;
  bool get hasMore => _hasMore;
  bool get isEmpty => _items.isEmpty && !_loadingFirst && _error == null;
  String? get error => _error;
  int get totalCount => _totalCount;
  String get search => _search;
  String? get category => _category;

  /// How many rows from the bottom to start fetching the next page. Two full
  /// screens' worth of runway means the spinner is rarely seen at all.
  static const prefetchThreshold = 6;

  Future<void> setQuery({String? search, String? category}) {
    final nextSearch = search ?? _search;
    final nextCategory = category ?? _category;
    if (nextSearch == _search && nextCategory == _category && _items.isNotEmpty) {
      return Future.value();
    }
    _search = nextSearch;
    _category = nextCategory;
    return reset();
  }

  Future<void> clearCategory() {
    if (_category == null) return Future.value();
    _category = null;
    return reset();
  }

  /// Full reload from page 1, keeping the existing items on screen until the
  /// new first page lands so pull-to-refresh doesn't blank the list.
  Future<void> refresh() => _load(reset: true, keepItems: true);

  Future<void> reset() => _load(reset: true, keepItems: false);

  Future<void> loadFirst() {
    if (_items.isNotEmpty || _loadingFirst) return Future.value();
    return _load(reset: true, keepItems: false);
  }

  Future<void> loadMore() {
    if (_loadingMore || _loadingFirst || !_hasMore || _error != null) {
      return Future.value();
    }
    return _load(reset: false, keepItems: true);
  }

  /// Called after an edit or delete so the row reflects the new server state
  /// without refetching the whole feed.
  void replaceItem(Word word) {
    final i = _items.indexWhere((w) => w.id == word.id);
    if (i < 0) return;
    _items[i] = word;
    notifyListeners();
  }

  void removeItem(int id) {
    final removed = _items.length;
    _items.removeWhere((w) => w.id == id);
    if (_items.length == removed) return;
    if (_totalCount > 0) _totalCount--;
    notifyListeners();
  }

  void insertItem(Word word) {
    _items.insert(0, word);
    _totalCount++;
    notifyListeners();
  }

  Future<void> _load({required bool reset, required bool keepItems}) async {
    final generation = reset ? ++_generation : _generation;
    final page = reset ? 1 : _page + 1;

    if (reset) {
      _error = null;
      _hasMore = true;
      if (!keepItems) _items.clear();
      _loadingFirst = true;
      _loadingMore = false;
    } else {
      _loadingMore = true;
    }
    notifyListeners();

    try {
      final result = await _fetch(page);
      // The user changed the query while this was in flight.
      if (generation != _generation) return;

      if (reset) _items.clear();
      _items.addAll(result.items);
      _page = page;
      _totalCount = result.totalCount;
      // Trust the item count over the reported page maths: a short page is the
      // only reliable end-of-list signal when rows are de-duplicated server-side.
      _hasMore = result.items.length >= pageSize && _items.length < result.totalCount;
      _error = null;
    } on ApiException catch (e) {
      if (generation != _generation) return;
      _error = e.kurdishMessage;
      // Don't strand the user on a half-loaded list with no way forward.
      if (reset && !keepItems) _items.clear();
    } catch (e) {
      if (generation != _generation) return;
      _error = 'هەڵەیەکی چاوەڕوان‌نەکراو ڕوویدا';
      if (kDebugMode) debugPrint('WordFeed: $e');
    } finally {
      if (generation == _generation) {
        _loadingFirst = false;
        _loadingMore = false;
        notifyListeners();
      }
    }
  }

  Future<Paged<Word>> _fetch(int page) {
    final term = _search.trim().isEmpty ? null : _search.trim();
    switch (source) {
      case FeedSource.category:
        return repository.categoryWords(sourceId!,
            page: page, pageSize: pageSize, search: term);
      case FeedSource.speechType:
        return repository.speechTypeWords(sourceId!,
            page: page, pageSize: pageSize, search: term);
      case FeedSource.all:
        return repository.search(
            page: page, pageSize: pageSize, search: term, category: _category);
    }
  }
}

/// Collapses a burst of keystrokes into one request.
class Debouncer {
  Debouncer([this.duration = const Duration(milliseconds: 320)]);

  final Duration duration;
  Timer? _timer;

  void run(VoidCallback action) {
    _timer?.cancel();
    _timer = Timer(duration, action);
  }

  /// Skips the pending delay — used when the user submits the field explicitly.
  void flush(VoidCallback action) {
    _timer?.cancel();
    action();
  }

  void dispose() => _timer?.cancel();
}
