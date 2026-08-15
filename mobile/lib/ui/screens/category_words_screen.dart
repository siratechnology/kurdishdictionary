import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../core/theme.dart';
import '../../models/word.dart';
import '../../state/providers.dart';
import '../../state/word_feed.dart';
import '../navigation.dart';
import '../widgets/glass.dart';
import '../widgets/search_field.dart';
import '../widgets/word_feed_view.dart';

/// The same infinite feed as the home screen, scoped to one category or one
/// part of speech. Exactly one of [category] / [speechType] must be supplied.
class CategoryWordsScreen extends ConsumerStatefulWidget {
  const CategoryWordsScreen({super.key, this.category, this.speechType})
      : assert(category != null || speechType != null,
            'CategoryWordsScreen needs a category or a speech type');

  final Category? category;
  final SpeechPaneStat? speechType;

  @override
  ConsumerState<CategoryWordsScreen> createState() =>
      _CategoryWordsScreenState();
}

class _CategoryWordsScreenState extends ConsumerState<CategoryWordsScreen> {
  final _searchController = TextEditingController();
  final _debouncer = Debouncer();
  WordFeed? _feed;
  Object? _repositoryIdentity;

  String get _title =>
      widget.category?.name ?? widget.speechType?.kurdish ?? '';

  int get _total =>
      widget.category?.wordCount ?? widget.speechType?.wordCount ?? 0;

  @override
  void dispose() {
    _searchController.dispose();
    _debouncer.dispose();
    _feed?.dispose();
    super.dispose();
  }

  WordFeed _ensureFeed() {
    final repository = ref.watch(wordsRepositoryProvider);
    if (_feed == null || !identical(_repositoryIdentity, repository)) {
      _feed?.dispose();
      _feed = WordFeed(
        repository: repository,
        source: widget.category != null
            ? FeedSource.category
            : FeedSource.speechType,
        sourceId: widget.category?.id ?? widget.speechType!.id,
      );
      _repositoryIdentity = repository;
    }
    return _feed!;
  }

  @override
  Widget build(BuildContext context) {
    final feed = _ensureFeed();

    return Scaffold(
      extendBodyBehindAppBar: true,
      appBar: AppBar(
        title: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            Text(_title),
            if (_total > 0)
              Text(
                '$_total وشە',
                style: TextStyle(
                  fontFamily: kFontFamily,
                  fontSize: 11.5,
                  fontWeight: FontWeight.w400,
                  color: tokensOf(context).text4,
                ),
              ),
          ],
        ),
        flexibleSpace: const GlassBar(child: SizedBox.expand()),
        bottom: PreferredSize(
          preferredSize: const Size.fromHeight(64),
          child: Padding(
            padding: const EdgeInsets.fromLTRB(16, 0, 16, 12),
            child: SearchField(
              controller: _searchController,
              hint: 'گەڕان لە $_title…',
              onChanged: (value) => _debouncer.run(() {
                if (mounted) feed.setQuery(search: value);
              }),
              onSubmitted: (value) => _debouncer.flush(() {
                if (mounted) feed.setQuery(search: value);
              }),
            ),
          ),
        ),
      ),
      body: WordFeedView(
        feed: feed,
        padding: const EdgeInsets.only(top: 8, bottom: 8),
        onWordTap: (word) => openWord(context, word.id),
        onOpenSettings: () => openSettings(context),
        slivers: [
          SliverToBoxAdapter(
            child: SizedBox(
              // Clears the transparent app bar the body extends behind.
              height: MediaQuery.paddingOf(context).top + kToolbarHeight + 68,
            ),
          ),
        ],
      ),
    );
  }
}
