import 'dart:math' as math;
import 'dart:ui' as ui;

import 'package:flutter/material.dart';

import '../../core/theme.dart';
import 'force_layout.dart';

/// Draws the mind map: curved links tinted by relation type, glowing nodes
/// sized by connection count, and Kurdish labels.
class GraphPainter extends CustomPainter {
  GraphPainter({
    required this.layout,
    required this.tokens,
    required this.pulse,
    required this.selectedId,
    required this.scale,
    required Listenable repaint,
  }) : super(repaint: repaint);

  final ForceLayout layout;
  final AppTokens tokens;

  /// 0…1, drives the halo around the focal node.
  final double pulse;
  final String? selectedId;

  /// Current viewer zoom — labels are hidden when zoomed out far enough that
  /// they would overlap into mush.
  final double scale;

  /// Text layout is the expensive part of each frame; the strings never change,
  /// so they are laid out once and reused.
  final Map<String, TextPainter> _labelCache = {};

  @override
  void paint(Canvas canvas, Size size) {
    _paintLinks(canvas);
    _paintNodes(canvas);
  }

  void _paintLinks(Canvas canvas) {
    for (final link in layout.links) {
      final style = RelationStyle.of(link.data.relationType);
      final a = link.source.position;
      final b = link.target.position;

      // A slight arc keeps reciprocal pairs from drawing on top of each other
      // and reads better than a straight line in a radial layout.
      final mid = (a + b) / 2;
      final delta = b - a;
      final normal = Offset(-delta.dy, delta.dx);
      final length = math.max(normal.distance, 0.01);
      final control = mid + (normal / length) * (delta.distance * 0.11);

      final path = Path()
        ..moveTo(a.dx, a.dy)
        ..quadraticBezierTo(control.dx, control.dy, b.dx, b.dy);

      final dimmed = selectedId != null &&
          link.source.data.id != selectedId &&
          link.target.data.id != selectedId;

      canvas.drawPath(
        path,
        Paint()
          ..style = PaintingStyle.stroke
          ..strokeCap = StrokeCap.round
          ..strokeWidth = 1.2 + link.data.weight.clamp(0, 6) * 0.32
          ..color = style.color.withValues(alpha: dimmed ? 0.10 : 0.42),
      );
    }
  }

  void _paintNodes(Canvas canvas) {
    // Focal node last so its halo sits above its neighbours' links.
    final ordered = layout.nodes.toList()
      ..sort((a, b) {
        if (a.data.isCenter != b.data.isCenter) return a.data.isCenter ? 1 : -1;
        return a.radius.compareTo(b.radius);
      });

    for (final node in ordered) {
      final isSelected = node.data.id == selectedId;
      final dimmed = selectedId != null && !isSelected && !_isNeighbour(node.data.id);
      final color = _colorFor(node);
      final position = node.position;
      final radius = node.radius;

      if (node.data.isCenter) {
        // Expanding halo — the CSS `halo-pulse` keyframe, in Flutter.
        final haloRadius = radius * (1.35 + pulse * 0.55);
        canvas.drawCircle(
          position,
          haloRadius,
          Paint()
            ..style = PaintingStyle.stroke
            ..strokeWidth = 1.6
            ..color = tokens.accentLight.withValues(alpha: (1 - pulse) * 0.55),
        );
      }

      // Soft outer glow.
      canvas.drawCircle(
        position,
        radius * 1.6,
        Paint()
          ..color = color.withValues(alpha: dimmed ? 0.03 : 0.13)
          ..maskFilter = const ui.MaskFilter.blur(ui.BlurStyle.normal, 10),
      );

      canvas.drawCircle(
        position,
        radius,
        Paint()
          ..shader = ui.Gradient.radial(
            position.translate(-radius * 0.3, -radius * 0.35),
            radius * 1.5,
            [
              Color.lerp(color, Colors.white, 0.32)!
                  .withValues(alpha: dimmed ? 0.30 : 1),
              color.withValues(alpha: dimmed ? 0.25 : 0.92),
            ],
          ),
      );

      canvas.drawCircle(
        position,
        radius,
        Paint()
          ..style = PaintingStyle.stroke
          ..strokeWidth = isSelected ? 2.6 : 1.2
          ..color = (isSelected ? Colors.white : color)
              .withValues(alpha: dimmed ? 0.18 : (isSelected ? 0.95 : 0.55)),
      );

      // Below roughly 0.55× the labels collide; the shapes still carry the
      // structure, so we drop the text rather than render a smear.
      if (scale >= 0.55 || node.data.isCenter) {
        _paintLabel(canvas, node, dimmed);
      }
    }
  }

  bool _isNeighbour(String id) {
    if (selectedId == null) return false;
    for (final link in layout.links) {
      if (link.source.data.id == selectedId && link.target.data.id == id) {
        return true;
      }
      if (link.target.data.id == selectedId && link.source.data.id == id) {
        return true;
      }
    }
    return false;
  }

  void _paintLabel(Canvas canvas, LayoutNode node, bool dimmed) {
    final painter = _labelCache.putIfAbsent(node.data.id, () {
      final tp = TextPainter(
        text: TextSpan(
          text: node.data.label,
          style: TextStyle(
            fontFamily: kFontFamily,
            fontSize: node.data.isCenter ? 16 : 13,
            fontWeight: node.data.isCenter ? FontWeight.w700 : FontWeight.w600,
            color: tokens.text1,
            height: 1.3,
            shadows: [
              // Stands in for the SVG `.lbl` stroke — keeps the text legible
              // wherever it crosses a link or a glow.
              Shadow(
                color: tokens.background.withValues(alpha: 0.95),
                blurRadius: 5,
              ),
            ],
          ),
        ),
        textDirection: TextDirection.rtl,
        maxLines: 1,
        ellipsis: '…',
      )..layout(maxWidth: 132);
      return tp;
    });

    final offset = Offset(
      node.position.dx - painter.width / 2,
      node.position.dy + node.radius + 7,
    );

    if (dimmed) {
      canvas.saveLayer(
        Rect.fromLTWH(offset.dx - 4, offset.dy - 4, painter.width + 8,
            painter.height + 8),
        Paint()..color = Colors.white.withValues(alpha: 0.22),
      );
      painter.paint(canvas, offset);
      canvas.restore();
    } else {
      painter.paint(canvas, offset);
    }
  }

  Color _colorFor(LayoutNode node) {
    if (node.data.isCenter) return tokens.accentLight;
    // Prefer the relation hue — how a word connects to the subject is the more
    // useful signal on this screen than what part of speech it is.
    if (node.data.relationType != null) {
      return RelationStyle.of(node.data.relationType).color;
    }
    if (node.data.speechPane > 0) return SpeechStyle.of(node.data.speechPane);
    return tokens.accent;
  }

  @override
  bool shouldRepaint(GraphPainter old) =>
      old.selectedId != selectedId ||
      old.pulse != pulse ||
      old.scale != scale ||
      old.tokens != tokens;
}
