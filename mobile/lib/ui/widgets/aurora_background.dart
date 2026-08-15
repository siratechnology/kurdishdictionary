import 'dart:math' as math;

import 'package:flutter/material.dart';

import '../../core/theme.dart';
import 'glass.dart';

/// The app's backdrop: the `body::before` nebula from the web app, drifting.
///
/// Three soft radial blobs move along slow, mutually-prime cycles so the
/// composite never visibly repeats. It is one repaint-boundaried CustomPaint
/// behind the whole app, not per-screen decoration.
class AuroraBackground extends StatefulWidget {
  const AuroraBackground({super.key, required this.child});

  final Widget child;

  @override
  State<AuroraBackground> createState() => _AuroraBackgroundState();
}

class _AuroraBackgroundState extends State<AuroraBackground>
    with SingleTickerProviderStateMixin {
  late final AnimationController _controller = AnimationController(
    vsync: this,
    // One slow master cycle; the blobs derive different rates from it.
    duration: const Duration(seconds: 48),
  )..repeat();

  @override
  void dispose() {
    _controller.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    final t = tokensOf(context);
    return Stack(
      children: [
        Positioned.fill(
          child: DecoratedBox(
            decoration: BoxDecoration(
              gradient: LinearGradient(
                begin: Alignment.topRight,
                end: Alignment.bottomLeft,
                colors: t.backgroundGradient.length > 1
                    ? t.backgroundGradient
                    : [t.background, t.background],
              ),
            ),
          ),
        ),
        Positioned.fill(
          child: RepaintBoundary(
            child: AnimatedBuilder(
              animation: _controller,
              builder: (_, _) => CustomPaint(
                painter: _AuroraPainter(_controller.value, t),
              ),
            ),
          ),
        ),
        widget.child,
      ],
    );
  }
}

class _AuroraPainter extends CustomPainter {
  _AuroraPainter(this.progress, this.tokens);

  final double progress;
  final AppTokens tokens;

  /// Anchor point, radius (as a fraction of the shortest side), and how many
  /// master cycles this blob completes — kept irrational-ish relative to each
  /// other so the three never realign.
  static const _blobs = [
    (Offset(0.15, 0.62), 0.95, 1.0),
    (Offset(0.85, 0.18), 0.75, 0.61),
    (Offset(0.50, 1.02), 0.90, 0.37),
  ];

  @override
  void paint(Canvas canvas, Size size) {
    final shortest = math.min(size.width, size.height);

    for (var i = 0; i < _blobs.length; i++) {
      final (anchor, radiusFactor, rate) = _blobs[i];
      final phase = (progress * rate + i * 0.21) * 2 * math.pi;

      // A small Lissajous drift — enough to feel alive, small enough that it
      // never reads as movement you have to track.
      final center = Offset(
        (anchor.dx + 0.055 * math.sin(phase)) * size.width,
        (anchor.dy + 0.045 * math.cos(phase * 0.8)) * size.height,
      );
      final radius = shortest * radiusFactor * (1 + 0.06 * math.sin(phase * 0.5));
      final color = tokens.auroraColors[i % tokens.auroraColors.length];

      canvas.drawCircle(
        center,
        radius,
        Paint()
          ..shader = RadialGradient(
            colors: [color, color.withValues(alpha: 0)],
            stops: const [0, 1],
          ).createShader(Rect.fromCircle(center: center, radius: radius)),
      );
    }
  }

  @override
  bool shouldRepaint(_AuroraPainter old) =>
      old.progress != progress || old.tokens != tokens;
}
