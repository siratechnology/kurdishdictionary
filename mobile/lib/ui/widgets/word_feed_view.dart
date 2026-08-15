import 'package:flutter/material.dart';
import 'package:flutter/rendering.dart';

import '../../models/word.dart';
import '../../state/word_feed.dart';
import 'skeletons.dart';
import 'states.dart';
import 'word_card.dart';

/// The infinite word list.
///
/// Behaves the way an endless social feed does: pull down to refresh, and the
/// next page is fetched while there is still a screenful of runway left, so you
/// scroll into already-loaded content instead of into a spinner. The tail
/// spinner only ever appears if the network loses that race.
class WordFeedView extends StatefulWidget {
  const WordFeedView({
    super.key,
    required this.feed,
    required this.onWordTap,
    this.onCategoryTap,
    this.slivers = const [],
    this.emptyState,
    this.onOpenSettings,
    this.padding = const EdgeInsets.only(top: 4, bottom: 8),
    this.scrollController,
    this.onScrollDirectionChange,
  });

  final WordFeed feed;
  final void Function(Word word) onWordTap;
  final void Function(Category category)? onCategoryTap;

  /// Rendered above the list — search field, filter chips, section headers.
  final List<Widget> slivers;

  final Widget? emptyState;
  final VoidCallback? onOpenSettings;
  final EdgeInsets padding;
  final ScrollController? scrollController;

  /// Fires when the user reverses scroll direction; the shell uses it to hide
  /// and show the bottom bar.
  final void Function(ScrollDirection direction)? onScrollDirectionChange;

  @override
  State<WordFeedView> createState() => _WordFeedViewState();
}

class _WordFeedViewState extends State<WordFeedView> {
  ScrollDirection _lastDirection = ScrollDirection.idle;

  @override
  void initState() {
    super.initState();
    WidgetsBinding.instance.addPostFrameCallback((_) => widget.feed.loadFirst());
  }

  /// Requests the next page from inside `itemBuilder`. Deferred to after the
  /// frame because the feed notifies its listeners synchronously, and mutating
  /// state during build is illegal.
  void _maybePrefetch(int index) {
    final feed = widget.feed;
    if (index < feed.items.length - WordFeed.prefetchThreshold) return;
    if (!feed.hasMore || feed.isLoadingMore || feed.isLoadingFirst) return;
    WidgetsBinding.instance.addPostFrameCallback((_) {
      if (mounted) feed.loadMore();
    });
  }

  bool _onScroll(UserScrollNotification n) {
    if (n.direction != _lastDirection && n.direction != ScrollDirection.idle) {
      _lastDirection = n.direction;
      widget.onScrollDirectionChange?.call(n.direction);
    }
    return false;
  }

  @override
  Widget build(BuildContext context) {
    return NotificationListener<UserScrollNotification>(
      onNotification: _onScroll,
      child: RefreshIndicator(
        onRefresh: widget.feed.refresh,
        edgeOffset: 0,
        displacement: 28,
        child: ListenableBuilder(
          listenable: widget.feed,
          builder: (context, _) {
            final feed = widget.feed;
            return CustomScrollView(
              controller: widget.scrollController,
              // Always scrollable so pull-to-refresh works even when the body
              // is an empty or error state that doesn't fill the viewport.
              physics: const AlwaysScrollableScrollPhysics(
                parent: BouncingScrollPhysics(),
              ),
              slivers: [
                ...widget.slivers,
                ..._body(feed),
              ],
            );
          },
        ),
      ),
    );
  }

  List<Widget> _body(WordFeed feed) {
    if (feed.isLoadingFirst && feed.items.isEmpty) {
      return const [
        SliverToBoxAdapter(child: FeedSkeleton()),
      ];
    }

    if (feed.items.isEmpty && feed.error != null) {
      return [
        SliverFillRemaining(
          hasScrollBody: false,
          child: ErrorState(
            message: feed.error!,
            onRetry: feed.reset,
            onOpenSettings: widget.onOpenSettings,
          ),
        ),
      ];
    }

    if (feed.items.isEmpty) {
      return [
        SliverFillRemaining(
          hasScrollBody: false,
          child: widget.emptyState ??
              const EmptyState(
                icon: Icons.search_off_rounded,
                title: 'هیچ وشەیەک نەدۆزرایەوە',
                message: 'وشەیەکی تر تاقی بکەرەوە یان فلتەرەکان لاببە.',
              ),
        ),
      ];
    }

    return [
      SliverPadding(
        padding: widget.padding,
        sliver: SliverList.builder(
          itemCount: feed.items.length,
          itemBuilder: (context, index) {
            _maybePrefetch(index);
            final word = feed.items[index];
            return FeedEntrance(
              // Keying by word id means an entrance plays once per word, not
              // again every time the list rebuilds around it.
              key: ValueKey(word.id),
              index: index,
              animate: index < 8,
              child: WordCard(
                word: word,
                highlight: feed.search,
                onTap: () => widget.onWordTap(word),
                onCategoryTap: widget.onCategoryTap,
              ),
            );
          },
        ),
      ),
      SliverToBoxAdapter(child: _footer(feed)),
    ];
  }

  Widget _footer(WordFeed feed) {
    if (feed.isLoadingMore) {
      return const FooterLoader(label: 'وشەی زیاتر دێت…');
    }
    // A page failed mid-scroll: keep what's loaded and offer a retry rather
    // than replacing the list with a full-screen error.
    if (feed.error != null) {
      return Padding(
        padding: const EdgeInsets.fromLTRB(16, 12, 16, 28),
        child: Center(
          child: TextButton.icon(
            onPressed: feed.loadMore,
            icon: const Icon(Icons.refresh, size: 18),
            label: Text(feed.error!),
          ),
        ),
      );
    }
    if (!feed.hasMore) return EndOfFeed(total: feed.totalCount);
    return const SizedBox(height: 28);
  }
}
