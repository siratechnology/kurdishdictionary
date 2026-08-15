import 'package:flutter/material.dart';

import 'glass.dart';

/// A left-to-right sheen over placeholder blocks. One controller drives every
/// descendant [SkeletonBox], so a screenful of placeholders costs one animation
/// rather than a dozen.
class Shimmer extends StatefulWidget {
  const Shimmer({super.key, required this.child});

  final Widget child;

  @override
  State<Shimmer> createState() => _ShimmerState();

  static _ShimmerState? _of(BuildContext context) =>
      context.findAncestorStateOfType<_ShimmerState>();
}

class _ShimmerState extends State<Shimmer> with SingleTickerProviderStateMixin {
  late final AnimationController controller = AnimationController(
    vsync: this,
    duration: const Duration(milliseconds: 1400),
  )..repeat();

  @override
  void dispose() {
    controller.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) => widget.child;
}

class SkeletonBox extends StatelessWidget {
  const SkeletonBox({
    super.key,
    required this.width,
    required this.height,
    this.radius = 8,
  });

  final double width;
  final double height;
  final double radius;

  @override
  Widget build(BuildContext context) {
    final t = tokensOf(context);
    final controller = Shimmer._of(context)?.controller;

    final base = DecoratedBox(
      decoration: BoxDecoration(
        color: t.skeletonBase.withValues(alpha: t.isDark ? 0.55 : 0.75),
        borderRadius: BorderRadius.circular(radius),
      ),
      child: SizedBox(width: width, height: height),
    );

    if (controller == null) return base;

    return ClipRRect(
      borderRadius: BorderRadius.circular(radius),
      child: AnimatedBuilder(
        animation: controller,
        builder: (_, child) => ShaderMask(
          blendMode: BlendMode.srcATop,
          shaderCallback: (rect) => LinearGradient(
            begin: Alignment.centerRight,
            end: Alignment.centerLeft,
            colors: [Colors.transparent, t.shimmer, Colors.transparent],
            stops: const [0.15, 0.5, 0.85],
            // Sweep a band twice the tile's width across it and back.
            transform: _SlideGradient(controller.value * 2 - 1),
          ).createShader(rect),
          child: child,
        ),
        child: base,
      ),
    );
  }
}

class _SlideGradient extends GradientTransform {
  const _SlideGradient(this.offset);

  final double offset;

  @override
  Matrix4 transform(Rect bounds, {TextDirection? textDirection}) =>
      Matrix4.translationValues(bounds.width * offset, 0, 0);
}

/// Placeholder shaped like a real [WordCard] so the list doesn't jump when the
/// data arrives.
class WordCardSkeleton extends StatelessWidget {
  const WordCardSkeleton({super.key, this.seed = 0});

  /// Varies the placeholder line lengths so a column of them doesn't look like
  /// a printed form.
  final int seed;

  @override
  Widget build(BuildContext context) {
    final meaningWidth = [220.0, 260.0, 180.0, 240.0][seed % 4];
    final titleWidth = [110.0, 90.0, 140.0, 120.0][seed % 4];

    return GlassCard(
      margin: const EdgeInsets.fromLTRB(16, 0, 16, 12),
      padding: const EdgeInsets.all(16),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Row(
            children: [
              SkeletonBox(width: titleWidth, height: 26, radius: 10),
              const Spacer(),
              const SkeletonBox(width: 28, height: 28, radius: 999),
            ],
          ),
          const SizedBox(height: 12),
          Row(
            children: const [
              SkeletonBox(width: 54, height: 22, radius: 999),
              SizedBox(width: 8),
              SkeletonBox(width: 68, height: 22, radius: 999),
            ],
          ),
          const SizedBox(height: 14),
          SkeletonBox(width: meaningWidth, height: 13, radius: 6),
          const SizedBox(height: 8),
          SkeletonBox(width: meaningWidth * 0.65, height: 13, radius: 6),
        ],
      ),
    );
  }
}

/// The first-load state of the feed.
class FeedSkeleton extends StatelessWidget {
  const FeedSkeleton({super.key, this.count = 7});

  final int count;

  @override
  Widget build(BuildContext context) => Shimmer(
        child: ListView.builder(
          padding: const EdgeInsets.only(top: 4, bottom: 24),
          physics: const NeverScrollableScrollPhysics(),
          itemCount: count,
          itemBuilder: (_, i) => WordCardSkeleton(seed: i),
        ),
      );
}
