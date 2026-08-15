import 'package:flutter/material.dart';

import '../../core/theme.dart';
import '../../models/word.dart';
import 'glass.dart';

/// The one chip shape used everywhere: a tinted pill whose colour carries the
/// meaning (part of speech, relation type, category).
class TintedChip extends StatelessWidget {
  const TintedChip({
    super.key,
    required this.label,
    required this.color,
    this.icon,
    this.dense = false,
    this.onTap,
    this.count,
  });

  final String label;
  final Color color;
  final IconData? icon;
  final bool dense;
  final VoidCallback? onTap;

  /// Rendered as a trailing badge — used by the browse grids.
  final int? count;

  @override
  Widget build(BuildContext context) {
    final t = tokensOf(context);
    final radius = BorderRadius.circular(999);

    final content = Padding(
      padding: EdgeInsets.symmetric(
        horizontal: dense ? 9 : 12,
        vertical: dense ? 3.5 : 6,
      ),
      child: Row(
        mainAxisSize: MainAxisSize.min,
        children: [
          if (icon != null) ...[
            Icon(icon, size: dense ? 12 : 14, color: color),
            const SizedBox(width: 5),
          ],
          Flexible(
            child: Text(
              label,
              maxLines: 1,
              overflow: TextOverflow.ellipsis,
              style: TextStyle(
                fontFamily: kFontFamily,
                fontSize: dense ? 11.5 : 13,
                fontWeight: FontWeight.w600,
                color: color,
                height: 1.35,
              ),
            ),
          ),
          if (count != null) ...[
            const SizedBox(width: 6),
            Container(
              padding: const EdgeInsets.symmetric(horizontal: 6, vertical: 1),
              decoration: BoxDecoration(
                color: color.withValues(alpha: 0.18),
                borderRadius: BorderRadius.circular(999),
              ),
              child: Text(
                '$count',
                style: TextStyle(
                  fontFamily: kFontFamily,
                  fontSize: dense ? 10.5 : 11.5,
                  fontWeight: FontWeight.w700,
                  color: color,
                  height: 1.3,
                ),
              ),
            ),
          ],
        ],
      ),
    );

    return Material(
      color: color.withValues(alpha: t.isDark ? 0.13 : 0.11),
      shape: RoundedRectangleBorder(
        borderRadius: radius,
        side: BorderSide(color: color.withValues(alpha: 0.32)),
      ),
      clipBehavior: Clip.antiAlias,
      child: onTap == null
          ? content
          : InkWell(onTap: onTap, borderRadius: radius, child: content),
    );
  }
}

class SpeechChip extends StatelessWidget {
  const SpeechChip({super.key, required this.pane, this.dense = false, this.onTap});

  final SpeechPane pane;
  final bool dense;
  final VoidCallback? onTap;

  @override
  Widget build(BuildContext context) => TintedChip(
        label: pane.kurdish,
        color: SpeechStyle.of(pane.id),
        dense: dense,
        onTap: onTap,
      );
}

class CategoryChip extends StatelessWidget {
  const CategoryChip({
    super.key,
    required this.category,
    this.dense = false,
    this.onTap,
  });

  final Category category;
  final bool dense;
  final VoidCallback? onTap;

  @override
  Widget build(BuildContext context) => TintedChip(
        label: category.name,
        color: tokensOf(context).accentLight,
        icon: Icons.local_offer_outlined,
        dense: dense,
        onTap: onTap,
      );
}

/// Grammatical gender. The API always sends a value, but `نییە` ("none") is
/// noise on a card, so callers get null back and skip rendering it.
class GenderChip extends StatelessWidget {
  const GenderChip({super.key, required this.gender, this.dense = false});

  final String gender;
  final bool dense;

  static bool isMeaningful(String? g) =>
      g != null && g.trim().isNotEmpty && g != 'نییە';

  @override
  Widget build(BuildContext context) {
    final color = switch (gender) {
      'نێر' => const Color(0xFF60A5FA),
      'مێ' => const Color(0xFFF472B6),
      _ => tokensOf(context).text3,
    };
    return TintedChip(label: gender, color: color, dense: dense);
  }
}

class RelationChip extends StatelessWidget {
  const RelationChip({
    super.key,
    required this.relationType,
    this.count,
    this.dense = false,
    this.onTap,
  });

  final String relationType;
  final int? count;
  final bool dense;
  final VoidCallback? onTap;

  @override
  Widget build(BuildContext context) {
    final style = RelationStyle.of(relationType);
    return TintedChip(
      label: style.label,
      color: style.color,
      dense: dense,
      count: count,
      onTap: onTap,
    );
  }
}

/// A horizontally scrolling row of chips that never wraps — used under the
/// search field for filters.
class ChipRow extends StatelessWidget {
  const ChipRow({
    super.key,
    required this.children,
    this.padding = const EdgeInsets.symmetric(horizontal: 16),
    this.height = 40,
  });

  final List<Widget> children;
  final EdgeInsetsGeometry padding;
  final double height;

  @override
  Widget build(BuildContext context) {
    if (children.isEmpty) return const SizedBox.shrink();
    return SizedBox(
      height: height,
      child: ListView.separated(
        scrollDirection: Axis.horizontal,
        padding: padding,
        physics: const BouncingScrollPhysics(),
        itemCount: children.length,
        separatorBuilder: (_, _) => const SizedBox(width: 8),
        itemBuilder: (_, i) => Center(child: children[i]),
      ),
    );
  }
}
