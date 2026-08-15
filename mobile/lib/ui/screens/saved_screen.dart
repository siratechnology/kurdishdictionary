import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../core/sharing.dart';
import '../../core/theme.dart';
import '../../models/word.dart';
import '../../state/providers.dart';
import '../navigation.dart';
import '../widgets/glass.dart';
import '../widgets/states.dart';
import '../widgets/word_card.dart';

/// Everything stored on the device: bookmarked words and recently viewed ones.
/// Both read from the local cache, so this tab works with no connection.
class SavedScreen extends ConsumerStatefulWidget {
  const SavedScreen({super.key});

  @override
  ConsumerState<SavedScreen> createState() => _SavedScreenState();
}

class _SavedScreenState extends ConsumerState<SavedScreen>
    with SingleTickerProviderStateMixin {
  late final TabController _tabs = TabController(length: 2, vsync: this);

  @override
  void dispose() {
    _tabs.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    final t = tokensOf(context);
    final favorites = ref.watch(favoritesProvider);
    final history = ref.watch(historyProvider);
    final topPadding = MediaQuery.paddingOf(context).top;

    return Column(
      children: [
        Padding(
          padding: EdgeInsets.fromLTRB(20, topPadding + 20, 20, 10),
          child: Row(
            children: [
              Text(
                'پاشەکەوتکراوەکان',
                style: TextStyle(
                  fontFamily: kFontFamily,
                  fontSize: 27,
                  fontWeight: FontWeight.w700,
                  color: t.text1,
                ),
              ),
              const Spacer(),
              if ((_tabs.index == 0 && favorites.isNotEmpty) ||
                  (_tabs.index == 1 && history.isNotEmpty))
                IconButton(
                  tooltip: 'سڕینەوەی هەموو',
                  icon: Icon(Icons.delete_sweep_outlined, color: t.text3),
                  onPressed: _confirmClear,
                ),
            ],
          ),
        ),
        Padding(
          padding: const EdgeInsets.symmetric(horizontal: 16),
          child: GlassCard(
            radius: 16,
            padding: const EdgeInsets.all(4),
            child: TabBar(
              controller: _tabs,
              onTap: (_) => setState(() {}),
              dividerColor: Colors.transparent,
              indicatorSize: TabBarIndicatorSize.tab,
              indicator: BoxDecoration(
                color: t.accent.withValues(alpha: 0.18),
                borderRadius: BorderRadius.circular(12),
              ),
              labelColor: t.accentLight,
              unselectedLabelColor: t.text3,
              labelStyle: const TextStyle(
                fontFamily: kFontFamily,
                fontSize: 14,
                fontWeight: FontWeight.w600,
              ),
              tabs: [
                Tab(text: 'پاشەکەوت (${favorites.length})'),
                Tab(text: 'دواترین (${history.length})'),
              ],
            ),
          ),
        ),
        const SizedBox(height: 12),
        Expanded(
          child: TabBarView(
            controller: _tabs,
            children: [
              _WordList(
                words: favorites,
                empty: const EmptyState(
                  icon: Icons.bookmark_border_rounded,
                  title: 'هیچ وشەیەک پاشەکەوت نەکراوە',
                  message:
                      'لە لیستی وشەکاندا دەست بنێ بەسەر ئایکۆنی نیشانکردن بۆ زیادکردنی وشە بۆ ئێرە.',
                ),
              ),
              _WordList(
                words: history,
                empty: const EmptyState(
                  icon: Icons.history_rounded,
                  title: 'هێشتا هیچ وشەیەکت نەکردۆتەوە',
                  message: 'ئەو وشانەی دەیانکەیتەوە لێرە تۆمار دەکرێن.',
                ),
              ),
            ],
          ),
        ),
      ],
    );
  }

  Future<void> _confirmClear() async {
    final isFavorites = _tabs.index == 0;
    final confirmed = await showDialog<bool>(
      context: context,
      builder: (context) => AlertDialog(
        backgroundColor: tokensOf(context).surfaceCard,
        shape:
            RoundedRectangleBorder(borderRadius: BorderRadius.circular(20)),
        title: Text(isFavorites
            ? 'سڕینەوەی هەموو پاشەکەوتەکان؟'
            : 'سڕینەوەی مێژووی بینین؟'),
        content: const Text('ئەم کردارە ناگەڕێتەوە.'),
        actions: [
          TextButton(
            onPressed: () => Navigator.of(context).pop(false),
            child: const Text('پاشگەزبوونەوە'),
          ),
          FilledButton(
            style: FilledButton.styleFrom(
                backgroundColor: const Color(0xFFF87171)),
            onPressed: () => Navigator.of(context).pop(true),
            child: const Text('سڕینەوە'),
          ),
        ],
      ),
    );

    if (confirmed != true) return;
    if (isFavorites) {
      await ref.read(favoritesProvider.notifier).clear();
    } else {
      await ref.read(historyProvider.notifier).clear();
    }
  }
}

class _WordList extends ConsumerWidget {
  const _WordList({required this.words, required this.empty});

  final List<Word> words;
  final Widget empty;

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    if (words.isEmpty) return empty;

    return ListView.builder(
      padding: const EdgeInsets.only(top: 4, bottom: 100),
      physics: const BouncingScrollPhysics(),
      itemCount: words.length,
      itemBuilder: (context, i) {
        final word = words[i];
        return Dismissible(
          key: ValueKey('saved-${word.id}'),
          direction: DismissDirection.startToEnd,
          background: _SwipeBackground(),
          confirmDismiss: (_) async {
            // Sharing isn't destructive, so the row springs back rather than
            // disappearing.
            await ref
                .read(sharingProvider)
                .shareWord(word, origin: originOf(context));
            return false;
          },
          child: WordCard(
            word: word,
            onTap: () => openWord(context, word.id),
          ),
        );
      },
    );
  }
}

class _SwipeBackground extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    final t = tokensOf(context);
    return Container(
      margin: const EdgeInsets.fromLTRB(16, 0, 16, 12),
      padding: const EdgeInsetsDirectional.only(start: 24),
      alignment: AlignmentDirectional.centerStart,
      decoration: BoxDecoration(
        color: t.accent.withValues(alpha: 0.16),
        borderRadius: BorderRadius.circular(20),
      ),
      child: Row(
        mainAxisSize: MainAxisSize.min,
        children: [
          Icon(Icons.ios_share, size: 20, color: t.accentLight),
          const SizedBox(width: 8),
          Text(
            'هاوبەشکردن',
            style: TextStyle(
              fontFamily: kFontFamily,
              fontSize: 14,
              fontWeight: FontWeight.w600,
              color: t.accentLight,
            ),
          ),
        ],
      ),
    );
  }
}
