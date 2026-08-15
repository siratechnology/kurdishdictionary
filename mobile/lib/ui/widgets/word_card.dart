import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../core/sharing.dart';
import '../../core/theme.dart';
import '../../models/word.dart';
import '../../state/providers.dart';
import 'chips.dart';
import 'glass.dart';

/// One word in the feed.
///
/// Every card is independently shareable — tap the share button, or long-press
/// anywhere on the card — and independently favouritable, both without leaving
/// the list.
class WordCard extends ConsumerWidget {
  const WordCard({
    super.key,
    required this.word,
    this.onTap,
    this.onCategoryTap,
    this.highlight,
    this.showRelationCount = true,
  });

  final Word word;
  final VoidCallback? onTap;
  final void Function(Category category)? onCategoryTap;

  /// The active search term; matching runs are tinted in the headword.
  final String? highlight;
  final bool showRelationCount;

  static const _maxMeaningsOnCard = 2;

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final t = tokensOf(context);
    final isFavorite =
        ref.watch(favoritesProvider.select((list) => list.any((w) => w.id == word.id)));

    final meanings = word.meanings.take(_maxMeaningsOnCard).toList();
    final extraMeanings = word.meanings.length - meanings.length;

    return GlassCard(
      margin: const EdgeInsets.fromLTRB(16, 0, 16, 12),
      padding: const EdgeInsets.fromLTRB(16, 14, 16, 10),
      onTap: onTap,
      onLongPress: () => _share(context, ref),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Row(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Expanded(
                child: _Headword(text: word.kurdish, highlight: highlight),
              ),
              const SizedBox(width: 8),
              _IconAction(
                icon: isFavorite ? Icons.bookmark : Icons.bookmark_border,
                color: isFavorite ? t.accentLight : t.text3,
                tooltip: isFavorite ? 'لابردن لە پاشەکەوتەکان' : 'پاشەکەوتکردن',
                onPressed: () => _toggleFavorite(context, ref, isFavorite),
              ),
              _IconAction(
                icon: Icons.ios_share,
                color: t.text3,
                tooltip: 'هاوبەشکردن',
                onPressed: () => _share(context, ref),
              ),
            ],
          ),
          if (word.speechPanes.isNotEmpty ||
              GenderChip.isMeaningful(word.genderKurdish)) ...[
            const SizedBox(height: 8),
            Wrap(
              spacing: 6,
              runSpacing: 6,
              children: [
                for (final p in word.speechPanes) SpeechChip(pane: p, dense: true),
                if (GenderChip.isMeaningful(word.genderKurdish))
                  GenderChip(gender: word.genderKurdish!, dense: true),
              ],
            ),
          ],
          if (meanings.isNotEmpty) ...[
            const SizedBox(height: 12),
            for (var i = 0; i < meanings.length; i++)
              Padding(
                padding: EdgeInsets.only(bottom: i == meanings.length - 1 ? 0 : 7),
                child: _MeaningLine(
                  index: word.meanings.length > 1 ? i + 1 : null,
                  meaning: meanings[i],
                ),
              ),
            if (extraMeanings > 0)
              Padding(
                padding: const EdgeInsets.only(top: 7),
                child: Text(
                  '+ $extraMeanings مانای تر',
                  style: TextStyle(
                    fontFamily: kFontFamily,
                    fontSize: 12.5,
                    fontWeight: FontWeight.w600,
                    color: t.accentLight,
                  ),
                ),
              ),
          ] else if ((word.description ?? '').trim().isNotEmpty) ...[
            const SizedBox(height: 12),
            Text(
              word.description!.trim(),
              maxLines: 3,
              overflow: TextOverflow.ellipsis,
              style: TextStyle(
                fontFamily: kFontFamily,
                fontSize: 14.5,
                color: t.text2,
                height: 1.6,
              ),
            ),
          ],
          if (word.categories.isNotEmpty) ...[
            const SizedBox(height: 12),
            Wrap(
              spacing: 6,
              runSpacing: 6,
              children: [
                for (final c in word.categories)
                  CategoryChip(
                    category: c,
                    dense: true,
                    onTap: onCategoryTap == null ? null : () => onCategoryTap!(c),
                  ),
              ],
            ),
          ],
          if (showRelationCount && word.totalRelations > 0) ...[
            const SizedBox(height: 10),
            Row(
              children: [
                Icon(Icons.hub_outlined, size: 14, color: t.text4),
                const SizedBox(width: 5),
                Text(
                  '${word.totalRelations} پەیوەندی',
                  style: TextStyle(
                    fontFamily: kFontFamily,
                    fontSize: 12,
                    color: t.text4,
                  ),
                ),
              ],
            ),
          ],
        ],
      ),
    );
  }

  Future<void> _toggleFavorite(
      BuildContext context, WidgetRef ref, bool wasFavorite) async {
    HapticFeedback.selectionClick();
    await ref.read(favoritesProvider.notifier).toggle(word);
    if (!context.mounted) return;
    ScaffoldMessenger.of(context)
      ..clearSnackBars()
      ..showSnackBar(
        SnackBar(
          duration: const Duration(milliseconds: 1400),
          content: Text(wasFavorite
              ? '«${word.kurdish}» لابرا'
              : '«${word.kurdish}» پاشەکەوت کرا'),
        ),
      );
  }

  Future<void> _share(BuildContext context, WidgetRef ref) async {
    HapticFeedback.lightImpact();
    await ref.read(sharingProvider).shareWord(word, origin: originOf(context));
  }
}

/// The headword, with the current search term tinted inside it.
class _Headword extends StatelessWidget {
  const _Headword({required this.text, this.highlight});

  final String text;
  final String? highlight;

  @override
  Widget build(BuildContext context) {
    final t = tokensOf(context);
    final base = TextStyle(
      fontFamily: kFontFamily,
      fontSize: 23,
      fontWeight: FontWeight.w700,
      color: t.text1,
      height: 1.45,
    );

    final term = highlight?.trim() ?? '';
    if (term.isEmpty || !text.contains(term)) {
      return Text(text, style: base, maxLines: 2, overflow: TextOverflow.ellipsis);
    }

    final spans = <TextSpan>[];
    var start = 0;
    while (true) {
      final index = text.indexOf(term, start);
      if (index < 0) {
        spans.add(TextSpan(text: text.substring(start)));
        break;
      }
      if (index > start) {
        spans.add(TextSpan(text: text.substring(start, index)));
      }
      spans.add(TextSpan(
        text: text.substring(index, index + term.length),
        style: TextStyle(color: t.accentLight),
      ));
      start = index + term.length;
    }

    return Text.rich(
      TextSpan(style: base, children: spans),
      maxLines: 2,
      overflow: TextOverflow.ellipsis,
    );
  }
}

class _MeaningLine extends StatelessWidget {
  const _MeaningLine({required this.index, required this.meaning});

  final int? index;
  final WordMeaning meaning;

  @override
  Widget build(BuildContext context) {
    final t = tokensOf(context);
    final locate = meaning.locate?.trim();

    return Row(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        if (index != null) ...[
          Container(
            margin: const EdgeInsets.only(top: 5),
            width: 18,
            height: 18,
            alignment: Alignment.center,
            decoration: BoxDecoration(
              color: t.accent.withValues(alpha: 0.14),
              borderRadius: BorderRadius.circular(6),
            ),
            child: Text(
              '$index',
              style: TextStyle(
                fontFamily: kFontFamily,
                fontSize: 10.5,
                fontWeight: FontWeight.w700,
                color: t.accentLight,
                height: 1.2,
              ),
            ),
          ),
          const SizedBox(width: 8),
        ],
        Expanded(
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Text(
                meaning.meaning,
                maxLines: 3,
                overflow: TextOverflow.ellipsis,
                style: TextStyle(
                  fontFamily: kFontFamily,
                  fontSize: 14.5,
                  color: t.text2,
                  height: 1.6,
                ),
              ),
              if (locate != null && locate.isNotEmpty)
                Padding(
                  padding: const EdgeInsets.only(top: 2),
                  child: Text(
                    locate,
                    style: TextStyle(
                      fontFamily: kFontFamily,
                      fontSize: 11.5,
                      color: t.text4,
                    ),
                  ),
                ),
            ],
          ),
        ),
      ],
    );
  }
}

class _IconAction extends StatelessWidget {
  const _IconAction({
    required this.icon,
    required this.color,
    required this.onPressed,
    this.tooltip,
  });

  final IconData icon;
  final Color color;
  final VoidCallback onPressed;
  final String? tooltip;

  @override
  Widget build(BuildContext context) => SizedBox(
        width: 38,
        height: 38,
        child: IconButton(
          icon: Icon(icon, size: 19, color: color),
          tooltip: tooltip,
          padding: EdgeInsets.zero,
          visualDensity: VisualDensity.compact,
          onPressed: onPressed,
        ),
      );
}

/// Fades and lifts a row into place the first time it is built. Applied only to
/// the first screenful — animating every row of an endless list would mean a
/// running animation for anything the user scrolls past quickly.
class FeedEntrance extends StatefulWidget {
  const FeedEntrance({
    super.key,
    required this.index,
    required this.child,
    this.animate = true,
  });

  final int index;
  final Widget child;
  final bool animate;

  @override
  State<FeedEntrance> createState() => _FeedEntranceState();
}

class _FeedEntranceState extends State<FeedEntrance>
    with SingleTickerProviderStateMixin {
  late final AnimationController _controller = AnimationController(
    vsync: this,
    duration: const Duration(milliseconds: 420),
  );

  @override
  void initState() {
    super.initState();
    if (!widget.animate) {
      _controller.value = 1;
      return;
    }
    // Stagger by position, capped so a fast scroll never waits on a queue.
    Future.delayed(
      Duration(milliseconds: (widget.index.clamp(0, 6)) * 55),
      () {
        if (mounted) _controller.forward();
      },
    );
  }

  @override
  void dispose() {
    _controller.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    final curve =
        CurvedAnimation(parent: _controller, curve: Curves.easeOutCubic);
    return FadeTransition(
      opacity: curve,
      child: SlideTransition(
        position: Tween(begin: const Offset(0, 0.06), end: Offset.zero)
            .animate(curve),
        child: widget.child,
      ),
    );
  }
}
