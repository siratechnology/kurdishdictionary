import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../core/api_client.dart';
import '../../core/theme.dart';
import '../../state/providers.dart';
import '../navigation.dart';
import '../widgets/glass.dart';
import '../widgets/skeletons.dart';
import '../widgets/states.dart';

/// Browse the dictionary by structure rather than by search: every category,
/// and every part of speech, each with its word count.
class BrowseScreen extends ConsumerWidget {
  const BrowseScreen({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final categories = ref.watch(categoriesProvider);
    final speechStats = ref.watch(speechStatsProvider);
    final topPadding = MediaQuery.paddingOf(context).top;

    return RefreshIndicator(
      onRefresh: () async {
        ref.invalidate(categoriesProvider);
        ref.invalidate(speechStatsProvider);
        await Future.wait([
          ref.read(categoriesProvider.future),
          ref.read(speechStatsProvider.future),
        ]);
      },
      child: CustomScrollView(
        physics: const BouncingScrollPhysics(
            parent: AlwaysScrollableScrollPhysics()),
        slivers: [
          SliverToBoxAdapter(
            child: Padding(
              padding: EdgeInsets.fromLTRB(20, topPadding + 20, 20, 8),
              child: _Header(),
            ),
          ),
          SliverToBoxAdapter(
            child: _SectionHeader(
              icon: Icons.local_offer_outlined,
              title: 'پۆلەکان',
              count: categories.value?.length,
            ),
          ),
          _Grid(
            state: categories,
            onRetry: () => ref.invalidate(categoriesProvider),
            builder: (list) => [
              for (final c in list.where((c) => c.wordCount > 0).toList()
                ..sort((a, b) => b.wordCount.compareTo(a.wordCount)))
                _TileData(
                  label: c.name,
                  count: c.wordCount,
                  color: tokensOf(context).accentLight,
                  onTap: () => openCategory(context, c),
                ),
            ],
          ),
          SliverToBoxAdapter(
            child: _SectionHeader(
              icon: Icons.category_outlined,
              title: 'جۆری وشە',
              count: speechStats.value?.length,
            ),
          ),
          _Grid(
            state: speechStats,
            onRetry: () => ref.invalidate(speechStatsProvider),
            builder: (list) => [
              for (final s in list.where((s) => s.wordCount > 0).toList()
                ..sort((a, b) => b.wordCount.compareTo(a.wordCount)))
                _TileData(
                  label: s.kurdish,
                  count: s.wordCount,
                  color: SpeechStyle.of(s.id),
                  onTap: () => openSpeechType(context, s),
                ),
            ],
          ),
          const SliverToBoxAdapter(child: SizedBox(height: 100)),
        ],
      ),
    );
  }
}

class _Header extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    final t = tokensOf(context);
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          'گەڕان بە پۆل',
          style: TextStyle(
            fontFamily: kFontFamily,
            fontSize: 27,
            fontWeight: FontWeight.w700,
            color: t.text1,
          ),
        ),
        const SizedBox(height: 4),
        Text(
          'فەرهەنگەکە بەپێی بابەت و جۆری وشە بگەڕێ',
          style:
              TextStyle(fontFamily: kFontFamily, fontSize: 13.5, color: t.text3),
        ),
      ],
    );
  }
}

class _SectionHeader extends StatelessWidget {
  const _SectionHeader({
    required this.icon,
    required this.title,
    required this.count,
  });

  final IconData icon;
  final String title;
  final int? count;

  @override
  Widget build(BuildContext context) {
    final t = tokensOf(context);
    return Padding(
      padding: const EdgeInsets.fromLTRB(20, 22, 20, 12),
      child: Row(
        children: [
          Icon(icon, size: 17, color: t.accentLight),
          const SizedBox(width: 8),
          Text(
            title,
            style: TextStyle(
              fontFamily: kFontFamily,
              fontSize: 16,
              fontWeight: FontWeight.w600,
              color: t.text1,
            ),
          ),
          if (count != null) ...[
            const SizedBox(width: 8),
            Text(
              '($count)',
              style: TextStyle(
                  fontFamily: kFontFamily, fontSize: 13, color: t.text4),
            ),
          ],
        ],
      ),
    );
  }
}

class _TileData {
  const _TileData({
    required this.label,
    required this.count,
    required this.color,
    required this.onTap,
  });

  final String label;
  final int count;
  final Color color;
  final VoidCallback onTap;
}

/// A responsive grid over any async list, with its own loading and error states
/// so one failing section doesn't take the whole screen down.
class _Grid<T> extends StatelessWidget {
  const _Grid({
    required this.state,
    required this.builder,
    required this.onRetry,
  });

  final AsyncValue<List<T>> state;
  final List<_TileData> Function(List<T>) builder;
  final VoidCallback onRetry;

  @override
  Widget build(BuildContext context) {
    return state.when(
      loading: () => SliverToBoxAdapter(
        child: Shimmer(
          child: Padding(
            padding: const EdgeInsets.symmetric(horizontal: 16),
            child: Wrap(
              spacing: 12,
              runSpacing: 12,
              children: const [
                SkeletonBox(width: 150, height: 78, radius: 18),
                SkeletonBox(width: 150, height: 78, radius: 18),
                SkeletonBox(width: 150, height: 78, radius: 18),
                SkeletonBox(width: 150, height: 78, radius: 18),
              ],
            ),
          ),
        ),
      ),
      error: (error, _) => SliverToBoxAdapter(
        child: Padding(
          padding: const EdgeInsets.symmetric(horizontal: 16),
          child: ErrorState(
            message: error is ApiException
                ? error.kurdishMessage
                : 'نەهێنرایەوە',
            details: error is ApiException ? error.message : '$error',
            onRetry: onRetry,
          ),
        ),
      ),
      data: (list) {
        final tiles = builder(list);
        if (tiles.isEmpty) {
          return const SliverToBoxAdapter(
            child: Padding(
              padding: EdgeInsets.symmetric(horizontal: 20, vertical: 8),
              child: Text('هیچ نییە'),
            ),
          );
        }
        return SliverPadding(
          padding: const EdgeInsets.symmetric(horizontal: 16),
          sliver: SliverGrid(
            gridDelegate: const SliverGridDelegateWithMaxCrossAxisExtent(
              maxCrossAxisExtent: 210,
              mainAxisSpacing: 12,
              crossAxisSpacing: 12,
              mainAxisExtent: 82,
            ),
            delegate: SliverChildBuilderDelegate(
              (context, i) => _Tile(data: tiles[i]),
              childCount: tiles.length,
            ),
          ),
        );
      },
    );
  }
}

class _Tile extends StatelessWidget {
  const _Tile({required this.data});

  final _TileData data;

  @override
  Widget build(BuildContext context) {
    final t = tokensOf(context);
    return GlassCard(
      radius: 18,
      padding: const EdgeInsets.fromLTRB(14, 12, 14, 12),
      borderColor: data.color.withValues(alpha: 0.26),
      onTap: data.onTap,
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        mainAxisAlignment: MainAxisAlignment.spaceBetween,
        children: [
          Text(
            data.label,
            maxLines: 1,
            overflow: TextOverflow.ellipsis,
            style: TextStyle(
              fontFamily: kFontFamily,
              fontSize: 16,
              fontWeight: FontWeight.w600,
              color: t.text1,
            ),
          ),
          Row(
            children: [
              Container(
                width: 7,
                height: 7,
                decoration:
                    BoxDecoration(color: data.color, shape: BoxShape.circle),
              ),
              const SizedBox(width: 7),
              Text(
                '${data.count} وشە',
                style: TextStyle(
                    fontFamily: kFontFamily, fontSize: 12.5, color: t.text3),
              ),
              const Spacer(),
              Icon(Icons.chevron_left, size: 18, color: t.text4),
            ],
          ),
        ],
      ),
    );
  }
}
