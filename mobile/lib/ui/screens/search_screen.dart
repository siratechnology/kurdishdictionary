import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../core/theme.dart';
import '../../models/word.dart';
import '../../state/providers.dart';
import '../../state/word_feed.dart';
import '../navigation.dart';
import '../widgets/chips.dart';
import '../widgets/glass.dart';
import '../widgets/search_field.dart';
import '../widgets/states.dart';
import '../widgets/word_feed_view.dart';

/// The home feed: every word in the dictionary, searchable, filterable by
/// category, scrolling forever.
class SearchScreen extends ConsumerStatefulWidget {
  const SearchScreen({super.key});

  @override
  ConsumerState<SearchScreen> createState() => _SearchScreenState();
}

class _SearchScreenState extends ConsumerState<SearchScreen> {
  final _searchController = TextEditingController();
  final _scrollController = ScrollController();
  final _debouncer = Debouncer();

  WordFeed? _feed;
  Category? _activeCategory;

  /// The repository the current feed was built against. When the API address or
  /// the signed-in user changes the repository is replaced, and the feed has to
  /// be rebuilt or it will keep talking to the old base URL.
  Object? _repositoryIdentity;

  @override
  void dispose() {
    _searchController.dispose();
    _scrollController.dispose();
    _debouncer.dispose();
    _feed?.dispose();
    super.dispose();
  }

  WordFeed _ensureFeed() {
    final repository = ref.watch(wordsRepositoryProvider);
    if (_feed == null || !identical(_repositoryIdentity, repository)) {
      _feed?.dispose();
      _feed = WordFeed(repository: repository)
        ..setQuery(
          search: _searchController.text,
          category: _activeCategory?.name,
        );
      _repositoryIdentity = repository;
    }
    return _feed!;
  }

  void _onSearchChanged(String value) {
    _debouncer.run(() {
      if (!mounted) return;
      _feed?.setQuery(search: value);
    });
  }

  void _onSearchSubmitted(String value) {
    _debouncer.flush(() {
      if (!mounted) return;
      _feed?.setQuery(search: value);
      ref.read(recentSearchesProvider.notifier).push(value);
    });
  }

  void _selectCategory(Category? category) {
    setState(() => _activeCategory = category);
    // `null` means "no filter", which setQuery can't express through its
    // nullable parameter — clearing is a separate call.
    if (category == null) {
      _feed?.clearCategory();
    } else {
      _feed?.setQuery(category: category.name);
    }
    if (_scrollController.hasClients) {
      _scrollController.animateTo(0,
          duration: const Duration(milliseconds: 280), curve: Curves.easeOut);
    }
  }

  @override
  Widget build(BuildContext context) {
    final t = tokensOf(context);
    final feed = _ensureFeed();
    final categories = ref.watch(categoriesProvider);
    final topPadding = MediaQuery.paddingOf(context).top;

    return WordFeedView(
      feed: feed,
      scrollController: _scrollController,
      onWordTap: (word) => openWord(context, word.id),
      onCategoryTap: _selectCategory,
      onOpenSettings: () => openSettings(context),
      emptyState: EmptyState(
        icon: Icons.search_off_rounded,
        title: 'هیچ وشەیەک نەدۆزرایەوە',
        message: _searchController.text.isEmpty
            ? 'فەرهەنگەکە بەتاڵە یان پەیوەندی نییە.'
            : '«${_searchController.text}» لە فەرهەنگەکەدا نییە.',
        action: _activeCategory == null
            ? null
            : FilledButton.icon(
                onPressed: () => _selectCategory(null),
                icon: const Icon(Icons.filter_alt_off_outlined, size: 18),
                label: const Text('لابردنی فلتەر'),
              ),
      ),
      slivers: [
        SliverPersistentHeader(
          pinned: true,
          delegate: PinnedSearchHeader(
            topPadding: topPadding,
            height: 72,
            child: Padding(
              padding: const EdgeInsets.fromLTRB(16, 10, 16, 10),
              child: SearchField(
                controller: _searchController,
                onChanged: _onSearchChanged,
                onSubmitted: _onSearchSubmitted,
              ),
            ),
          ),
        ),
        SliverToBoxAdapter(
          child: _CategoryFilterBar(
            categories: categories,
            active: _activeCategory,
            onSelected: _selectCategory,
          ),
        ),
        SliverToBoxAdapter(
          child: ListenableBuilder(
            listenable: feed,
            builder: (context, _) => _ResultSummary(
              total: feed.totalCount,
              loading: feed.isLoadingFirst,
              search: feed.search,
              category: _activeCategory,
              accent: t.text4,
            ),
          ),
        ),
      ],
    );
  }
}

class _CategoryFilterBar extends StatelessWidget {
  const _CategoryFilterBar({
    required this.categories,
    required this.active,
    required this.onSelected,
  });

  final AsyncValue<List<Category>> categories;
  final Category? active;
  final void Function(Category?) onSelected;

  @override
  Widget build(BuildContext context) {
    final t = tokensOf(context);

    return categories.when(
      loading: () => const SizedBox(height: 44),
      error: (_, _) => const SizedBox(height: 8),
      data: (list) {
        if (list.isEmpty) return const SizedBox(height: 8);
        // Busiest categories first — a 40-name alphabetical strip is unusable
        // on a phone, whereas the top few cover most browsing.
        final sorted = list.toList()
          ..sort((a, b) => b.wordCount.compareTo(a.wordCount));

        return Padding(
          padding: const EdgeInsets.only(top: 4, bottom: 2),
          child: ChipRow(
            height: 44,
            children: [
              TintedChip(
                label: 'هەموو',
                icon: Icons.auto_awesome_mosaic_outlined,
                color: active == null ? t.accentLight : t.text3,
                onTap: () => onSelected(null),
              ),
              for (final c in sorted)
                TintedChip(
                  label: c.name,
                  count: c.wordCount > 0 ? c.wordCount : null,
                  color: active?.id == c.id ? t.accentLight : t.text3,
                  onTap: () => onSelected(active?.id == c.id ? null : c),
                ),
            ],
          ),
        );
      },
    );
  }
}

class _ResultSummary extends StatelessWidget {
  const _ResultSummary({
    required this.total,
    required this.loading,
    required this.search,
    required this.category,
    required this.accent,
  });

  final int total;
  final bool loading;
  final String search;
  final Category? category;
  final Color accent;

  @override
  Widget build(BuildContext context) {
    if (loading && total == 0) return const SizedBox(height: 10);

    final parts = <String>['$total وشە'];
    if (search.trim().isNotEmpty) parts.add('بۆ «${search.trim()}»');
    if (category != null) parts.add('لە ${category!.name}');

    return Padding(
      padding: const EdgeInsets.fromLTRB(20, 6, 20, 10),
      child: Text(
        parts.join(' '),
        style: TextStyle(fontFamily: kFontFamily, fontSize: 12.5, color: accent),
      ),
    );
  }
}
