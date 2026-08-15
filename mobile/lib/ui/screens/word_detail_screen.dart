import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../core/api_client.dart';
import '../../core/sharing.dart';
import '../../core/theme.dart';
import '../../models/word.dart';
import '../../state/providers.dart';
import '../navigation.dart';
import '../widgets/chips.dart';
import '../widgets/glass.dart';
import '../widgets/skeletons.dart';
import '../widgets/states.dart';

class WordDetailScreen extends ConsumerStatefulWidget {
  const WordDetailScreen({super.key, required this.wordId});

  final int wordId;

  @override
  ConsumerState<WordDetailScreen> createState() => _WordDetailScreenState();
}

class _WordDetailScreenState extends ConsumerState<WordDetailScreen> {
  /// Guards against pushing the same word into history twice when the provider
  /// rebuilds (e.g. after a favourite toggle).
  bool _recorded = false;

  @override
  Widget build(BuildContext context) {
    final async = ref.watch(wordProvider(widget.wordId));

    ref.listen(wordProvider(widget.wordId), (_, next) {
      final word = next.value;
      if (word != null && !_recorded) {
        _recorded = true;
        ref.read(historyProvider.notifier).push(word);
      }
    });

    return Scaffold(
      body: async.when(
        loading: () => const _DetailSkeleton(),
        error: (error, _) => _DetailError(
          error: error,
          onRetry: () => ref.invalidate(wordProvider(widget.wordId)),
        ),
        data: (word) => _DetailBody(word: word),
      ),
    );
  }
}

class _DetailBody extends ConsumerWidget {
  const _DetailBody({required this.word});

  final Word word;

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final t = tokensOf(context);
    final isFavorite = ref.watch(
        favoritesProvider.select((list) => list.any((w) => w.id == word.id)));
    final canEdit = ref.watch(authProvider.select((a) => a.canEdit));
    final relations = _groupRelations(word);

    return CustomScrollView(
      physics: const BouncingScrollPhysics(
          parent: AlwaysScrollableScrollPhysics()),
      slivers: [
        SliverAppBar(
          pinned: true,
          expandedHeight: 0,
          backgroundColor: Colors.transparent,
          flexibleSpace: const GlassBar(child: SizedBox.expand()),
          title: Text(word.kurdish,
              maxLines: 1, overflow: TextOverflow.ellipsis),
          actions: [
            IconButton(
              tooltip: isFavorite ? 'لابردن' : 'پاشەکەوتکردن',
              icon: Icon(isFavorite ? Icons.bookmark : Icons.bookmark_border,
                  color: isFavorite ? t.accentLight : null),
              onPressed: () async {
                HapticFeedback.selectionClick();
                await ref.read(favoritesProvider.notifier).toggle(word);
              },
            ),
            if (canEdit)
              IconButton(
                tooltip: 'دەستکاری',
                icon: const Icon(Icons.edit_outlined),
                onPressed: () => _edit(context, ref),
              ),
          ],
        ),
        SliverToBoxAdapter(child: _Hero(word: word)),
        if (word.meanings.isNotEmpty)
          SliverToBoxAdapter(
            child: _Section(
              title: 'واتاکان',
              icon: Icons.menu_book_outlined,
              child: Column(
                children: [
                  for (var i = 0; i < word.meanings.length; i++)
                    _MeaningTile(index: i + 1, meaning: word.meanings[i]),
                ],
              ),
            ),
          ),
        if ((word.description ?? '').trim().isNotEmpty)
          SliverToBoxAdapter(
            child: _Section(
              title: 'ڕوونکردنەوە',
              icon: Icons.notes_outlined,
              child: Text(
                word.description!.trim(),
                style: TextStyle(
                    fontFamily: kFontFamily,
                    fontSize: 15,
                    color: t.text2,
                    height: 1.85),
              ),
            ),
          ),
        if (word.categories.isNotEmpty)
          SliverToBoxAdapter(
            child: _Section(
              title: 'پۆلەکان',
              icon: Icons.local_offer_outlined,
              child: Wrap(
                spacing: 8,
                runSpacing: 8,
                children: [
                  for (final c in word.categories)
                    CategoryChip(
                      category: c,
                      onTap: () => openCategory(context, c),
                    ),
                ],
              ),
            ),
          ),
        if (relations.isNotEmpty)
          SliverToBoxAdapter(
            child: _Section(
              title: 'پەیوەندییەکان',
              icon: Icons.hub_outlined,
              trailing: TextButton.icon(
                onPressed: () => openMindMap(context, word.id, word.kurdish),
                icon: const Icon(Icons.account_tree_outlined, size: 17),
                label: const Text('نەخشەی بیر'),
              ),
              child: Column(
                children: [
                  for (final entry in relations.entries)
                    _RelationGroup(
                      relationType: entry.key,
                      relations: entry.value,
                      onTap: (r) => openWord(context, r.relatedWordId),
                    ),
                ],
              ),
            ),
          ),
        SliverToBoxAdapter(child: _Actions(word: word)),
        SliverToBoxAdapter(child: _Meta(word: word)),
        const SliverToBoxAdapter(child: SizedBox(height: 40)),
      ],
    );
  }

  /// Groups both directions of the join by relation type, dropping duplicates —
  /// a mutual synonym pair appears in both the outgoing and incoming lists.
  Map<String, List<RelatedWord>> _groupRelations(Word word) {
    final grouped = <String, List<RelatedWord>>{};
    final seen = <int>{};
    for (final r in word.allRelations) {
      if (r.relatedWordId == 0 || !seen.add(r.relatedWordId)) continue;
      grouped.putIfAbsent(r.relationType.toLowerCase(), () => []).add(r);
    }

    // Present them in the canonical order, with anything unrecognised last.
    final ordered = <String, List<RelatedWord>>{};
    for (final key in RelationStyle.order) {
      if (grouped.containsKey(key)) ordered[key] = grouped.remove(key)!;
    }
    ordered.addAll(grouped);
    return ordered;
  }

  Future<void> _edit(BuildContext context, WidgetRef ref) async {
    final updated = await openWordEditor(context, word: word);
    if (updated != null) ref.invalidate(wordProvider(word.id));
  }
}

class _Hero extends StatelessWidget {
  const _Hero({required this.word});

  final Word word;

  @override
  Widget build(BuildContext context) {
    final t = tokensOf(context);
    return Padding(
      padding: const EdgeInsets.fromLTRB(16, 12, 16, 4),
      child: GlassCard(
        raised: true,
        blur: 18,
        padding: const EdgeInsets.fromLTRB(20, 24, 20, 20),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(
              word.kurdish,
              style: TextStyle(
                fontFamily: kFontFamily,
                fontSize: 36,
                fontWeight: FontWeight.w700,
                color: t.text1,
                height: 1.4,
              ),
            ),
            const SizedBox(height: 14),
            Wrap(
              spacing: 7,
              runSpacing: 7,
              children: [
                for (final p in word.speechPanes) SpeechChip(pane: p),
                if (GenderChip.isMeaningful(word.genderKurdish))
                  GenderChip(gender: word.genderKurdish!),
              ],
            ),
          ],
        ),
      ),
    );
  }
}

class _Section extends StatelessWidget {
  const _Section({
    required this.title,
    required this.icon,
    required this.child,
    this.trailing,
  });

  final String title;
  final IconData icon;
  final Widget child;
  final Widget? trailing;

  @override
  Widget build(BuildContext context) {
    final t = tokensOf(context);
    return Padding(
      padding: const EdgeInsets.fromLTRB(16, 14, 16, 0),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Padding(
            padding: const EdgeInsets.fromLTRB(4, 0, 4, 10),
            child: Row(
              children: [
                Icon(icon, size: 17, color: t.accentLight),
                const SizedBox(width: 8),
                Text(
                  title,
                  style: TextStyle(
                    fontFamily: kFontFamily,
                    fontSize: 15,
                    fontWeight: FontWeight.w600,
                    color: t.text2,
                  ),
                ),
                const Spacer(),
                ?trailing,
              ],
            ),
          ),
          GlassCard(padding: const EdgeInsets.all(16), child: child),
        ],
      ),
    );
  }
}

class _MeaningTile extends StatelessWidget {
  const _MeaningTile({required this.index, required this.meaning});

  final int index;
  final WordMeaning meaning;

  @override
  Widget build(BuildContext context) {
    final t = tokensOf(context);
    final locate = meaning.locate?.trim();

    return Padding(
      padding: EdgeInsets.only(bottom: index == 0 ? 0 : 4, top: index == 1 ? 0 : 10),
      child: Row(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Container(
            margin: const EdgeInsets.only(top: 3),
            width: 24,
            height: 24,
            alignment: Alignment.center,
            decoration: BoxDecoration(
              color: t.accent.withValues(alpha: 0.14),
              borderRadius: BorderRadius.circular(8),
            ),
            child: Text(
              '$index',
              style: TextStyle(
                fontFamily: kFontFamily,
                fontSize: 12,
                fontWeight: FontWeight.w700,
                color: t.accentLight,
                height: 1.2,
              ),
            ),
          ),
          const SizedBox(width: 12),
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                SelectableText(
                  meaning.meaning,
                  style: TextStyle(
                    fontFamily: kFontFamily,
                    fontSize: 15.5,
                    color: t.text1,
                    height: 1.8,
                  ),
                ),
                if (locate != null && locate.isNotEmpty)
                  Padding(
                    padding: const EdgeInsets.only(top: 4),
                    child: TintedChip(
                      label: locate,
                      color: t.text4,
                      icon: Icons.place_outlined,
                      dense: true,
                    ),
                  ),
              ],
            ),
          ),
        ],
      ),
    );
  }
}

class _RelationGroup extends StatelessWidget {
  const _RelationGroup({
    required this.relationType,
    required this.relations,
    required this.onTap,
  });

  final String relationType;
  final List<RelatedWord> relations;
  final void Function(RelatedWord) onTap;

  @override
  Widget build(BuildContext context) {
    final style = RelationStyle.of(relationType);
    return Padding(
      padding: const EdgeInsets.only(bottom: 14),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Row(
            children: [
              Container(
                width: 8,
                height: 8,
                decoration:
                    BoxDecoration(color: style.color, shape: BoxShape.circle),
              ),
              const SizedBox(width: 8),
              Text(
                '${style.label} · ${relations.length}',
                style: TextStyle(
                  fontFamily: kFontFamily,
                  fontSize: 13,
                  fontWeight: FontWeight.w600,
                  color: style.color,
                ),
              ),
            ],
          ),
          const SizedBox(height: 9),
          Wrap(
            spacing: 8,
            runSpacing: 8,
            children: [
              for (final r in relations)
                TintedChip(
                  label: r.relatedKurdish ?? 'وشە #${r.relatedWordId}',
                  color: style.color,
                  icon: r.isIncoming ? Icons.south_west : Icons.north_east,
                  onTap: () => onTap(r),
                ),
            ],
          ),
        ],
      ),
    );
  }
}

class _Actions extends ConsumerWidget {
  const _Actions({required this.word});

  final Word word;

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final sharing = ref.read(sharingProvider);

    return Padding(
      padding: const EdgeInsets.fromLTRB(16, 20, 16, 0),
      child: Row(
        children: [
          Expanded(
            child: FilledButton.icon(
              onPressed: () {
                HapticFeedback.lightImpact();
                sharing.shareWord(word, origin: originOf(context));
              },
              icon: const Icon(Icons.ios_share, size: 19),
              label: const Text('هاوبەشکردن'),
            ),
          ),
          const SizedBox(width: 10),
          Expanded(
            child: OutlinedButton.icon(
              onPressed: () => openMindMap(context, word.id, word.kurdish),
              style: OutlinedButton.styleFrom(
                padding: const EdgeInsets.symmetric(vertical: 15),
                side: BorderSide(color: tokensOf(context).borderStrong),
                shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(14)),
              ),
              icon: const Icon(Icons.account_tree_outlined, size: 19),
              label: const Text('نەخشەی بیر'),
            ),
          ),
          const SizedBox(width: 10),
          _CopyButton(text: sharing.linkFor(word.id)),
        ],
      ),
    );
  }
}

class _CopyButton extends StatefulWidget {
  const _CopyButton({required this.text});

  final String text;

  @override
  State<_CopyButton> createState() => _CopyButtonState();
}

class _CopyButtonState extends State<_CopyButton> {
  bool _copied = false;

  @override
  Widget build(BuildContext context) {
    final t = tokensOf(context);
    return IconButton.filledTonal(
      tooltip: 'کۆپیکردنی بەستەر',
      style: IconButton.styleFrom(
        padding: const EdgeInsets.all(14),
        backgroundColor: t.surfaceRaised,
        shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(14)),
      ),
      icon: Icon(
        _copied ? Icons.check_rounded : Icons.link_rounded,
        size: 19,
        color: _copied ? const Color(0xFF22D36D) : t.text2,
      ),
      onPressed: () async {
        await Clipboard.setData(ClipboardData(text: widget.text));
        if (!mounted) return;
        setState(() => _copied = true);
        HapticFeedback.selectionClick();
        // Revert the tick so the button doesn't look permanently "done".
        await Future.delayed(const Duration(seconds: 2));
        if (mounted) setState(() => _copied = false);
      },
    );
  }
}

class _Meta extends StatelessWidget {
  const _Meta({required this.word});

  final Word word;

  @override
  Widget build(BuildContext context) {
    final t = tokensOf(context);
    final d = word.createdAt;
    final date =
        '${d.year}-${d.month.toString().padLeft(2, '0')}-${d.day.toString().padLeft(2, '0')}';

    return Padding(
      padding: const EdgeInsets.fromLTRB(24, 22, 24, 0),
      child: Row(
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          Icon(Icons.schedule, size: 13, color: t.text4),
          const SizedBox(width: 6),
          Text(
            'زیادکراوە لە $date · #${word.id}',
            style: TextStyle(
                fontFamily: kFontFamily, fontSize: 11.5, color: t.text4),
          ),
        ],
      ),
    );
  }
}

class _DetailSkeleton extends StatelessWidget {
  const _DetailSkeleton();

  @override
  Widget build(BuildContext context) => Shimmer(
        child: SafeArea(
          child: Padding(
            padding: const EdgeInsets.fromLTRB(16, 60, 16, 0),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: const [
                SkeletonBox(width: 190, height: 40, radius: 12),
                SizedBox(height: 18),
                Row(children: [
                  SkeletonBox(width: 62, height: 28, radius: 999),
                  SizedBox(width: 8),
                  SkeletonBox(width: 78, height: 28, radius: 999),
                ]),
                SizedBox(height: 34),
                SkeletonBox(width: 130, height: 16, radius: 6),
                SizedBox(height: 16),
                SkeletonBox(width: double.infinity, height: 15, radius: 6),
                SizedBox(height: 10),
                SkeletonBox(width: 240, height: 15, radius: 6),
                SizedBox(height: 10),
                SkeletonBox(width: 280, height: 15, radius: 6),
              ],
            ),
          ),
        ),
      );
}

class _DetailError extends StatelessWidget {
  const _DetailError({required this.error, required this.onRetry});

  final Object error;
  final VoidCallback onRetry;

  @override
  Widget build(BuildContext context) {
    final api = error is ApiException ? error as ApiException : null;
    return SafeArea(
      child: Column(
        children: [
          Align(
            alignment: AlignmentDirectional.centerStart,
            child: Padding(
              padding: const EdgeInsets.all(8),
              child: BackButton(onPressed: () => Navigator.of(context).pop()),
            ),
          ),
          Expanded(
            child: ErrorState(
              message: api?.kurdishMessage ?? 'هەڵەیەک ڕوویدا',
              details: api?.message ?? '$error',
              onRetry: onRetry,
            ),
          ),
        ],
      ),
    );
  }
}
