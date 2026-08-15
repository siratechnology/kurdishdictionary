import 'dart:math' as math;
import 'dart:ui';

import '../../models/graph.dart';

/// One node in the running simulation.
class LayoutNode {
  LayoutNode({
    required this.data,
    required this.position,
    required this.radius,
  });

  final GraphNode data;
  Offset position;
  Offset velocity = Offset.zero;
  double radius;

  /// Set while the user drags a node; the simulation stops integrating it and
  /// follows the finger instead.
  bool pinned = false;
}

/// A small force-directed layout in the spirit of d3-force: link springs,
/// many-body repulsion, a centring pull, and a cooling schedule.
///
/// The word graphs this renders are a focal word plus its direct neighbours —
/// tens of nodes, not thousands — so the O(n²) repulsion pass is cheaper than
/// the quadtree it would take to avoid it.
class ForceLayout {
  ForceLayout({
    required WordGraph graph,
    required this.size,
  }) {
    final center = Offset(size.width / 2, size.height / 2);
    final others = graph.nodes.where((n) => !n.isCenter).length;
    final random = math.Random(graph.nodes.length * 31 + others);
    var index = 0;

    for (final node in graph.nodes) {
      final Offset start;
      if (node.isCenter) {
        start = center;
      } else {
        // Seeding on a ring rather than at random keeps the first few frames
        // from looking like an explosion, and converges faster.
        final angle = (index / math.max(others, 1)) * 2 * math.pi;
        final jitter = 0.85 + random.nextDouble() * 0.3;
        start = center +
            Offset(math.cos(angle), math.sin(angle)) * (_ringRadius * jitter);
        index++;
      }

      nodes.add(LayoutNode(
        data: node,
        position: start,
        radius: _radiusFor(node),
      ));
    }

    _byId = {for (final n in nodes) n.data.id: n};

    for (final link in graph.links) {
      final a = _byId[link.source];
      final b = _byId[link.target];
      if (a == null || b == null || identical(a, b)) continue;
      links.add(LayoutLink(source: a, target: b, data: link));
    }
  }

  final Size size;
  final List<LayoutNode> nodes = [];
  final List<LayoutLink> links = [];
  late final Map<String, LayoutNode> _byId;

  static const _ringRadius = 190.0;
  static const _linkDistance = 128.0;
  static const _repulsion = 5200.0;
  static const _springStrength = 0.045;
  static const _centerStrength = 0.012;
  static const _velocityDecay = 0.82;

  /// d3's `alpha`: a global temperature that decays so the layout settles
  /// instead of jittering forever.
  double alpha = 1.0;
  static const _alphaDecay = 0.018;
  static const _alphaMin = 0.004;

  bool get settled => alpha <= _alphaMin;

  /// Nudges the simulation back to life after a drag or a node tap.
  void reheat([double to = 0.45]) => alpha = math.max(alpha, to);

  static double _radiusFor(GraphNode node) {
    if (node.isCenter) return 34;
    // Compress the weight range hard — one very connected neighbour shouldn't
    // dwarf everything else on a phone screen.
    final w = node.weight.clamp(0, 40).toDouble();
    return 15 + math.sqrt(w) * 3.2;
  }

  void tick() {
    if (settled) return;
    alpha -= _alphaDecay * alpha;

    final center = Offset(size.width / 2, size.height / 2);

    // Many-body repulsion.
    for (var i = 0; i < nodes.length; i++) {
      final a = nodes[i];
      for (var j = i + 1; j < nodes.length; j++) {
        final b = nodes[j];
        var delta = b.position - a.position;
        var distanceSq = delta.dx * delta.dx + delta.dy * delta.dy;

        if (distanceSq < 0.01) {
          // Perfectly coincident nodes have no direction to separate along;
          // give them an arbitrary one rather than dividing by zero.
          delta = Offset(0.5 - (i.isEven ? 0.0 : 1.0), 0.3);
          distanceSq = delta.distanceSquared;
        }

        final distance = math.sqrt(distanceSq);
        final force = _repulsion / distanceSq * alpha;
        final push = delta / distance * force;
        a.velocity -= push;
        b.velocity += push;
      }
    }

    // Link springs.
    for (final link in links) {
      final delta = link.target.position - link.source.position;
      final distance = math.max(delta.distance, 0.01);
      // Heavier links sit closer together.
      final ideal = _linkDistance / (1 + link.data.weight.clamp(0, 6) * 0.08);
      final force = (distance - ideal) * _springStrength * alpha;
      final pull = delta / distance * force;
      link.source.velocity += pull;
      link.target.velocity -= pull;
    }

    // Centring, plus a much stronger anchor on the focal word so the map keeps
    // its subject in the middle.
    for (final node in nodes) {
      final toCenter = center - node.position;
      final strength = node.data.isCenter ? _centerStrength * 12 : _centerStrength;
      node.velocity += toCenter * strength * alpha;
    }

    for (final node in nodes) {
      if (node.pinned) {
        node.velocity = Offset.zero;
        continue;
      }
      node.velocity *= _velocityDecay;
      node.position += node.velocity;
      // Keep everything inside the canvas so a node can never drift out of
      // reach of the viewport.
      node.position = Offset(
        node.position.dx.clamp(node.radius + 8, size.width - node.radius - 8),
        node.position.dy.clamp(node.radius + 8, size.height - node.radius - 8),
      );
    }
  }

  /// Runs the layout forward without painting, so the map appears already
  /// arranged instead of visibly untangling itself.
  void settle([int iterations = 220]) {
    for (var i = 0; i < iterations && !settled; i++) {
      tick();
    }
  }

  /// The node under [point], or null. Uses a generous touch radius — fingers
  /// are wider than the small satellite dots.
  LayoutNode? hitTest(Offset point) {
    LayoutNode? best;
    var bestDistance = double.infinity;
    for (final node in nodes) {
      final d = (node.position - point).distance;
      final touchRadius = math.max(node.radius, 26.0);
      if (d <= touchRadius && d < bestDistance) {
        best = node;
        bestDistance = d;
      }
    }
    return best;
  }

  /// The bounding box of the laid-out nodes, used to frame the initial view.
  Rect get bounds {
    if (nodes.isEmpty) return Offset.zero & size;
    var left = double.infinity, top = double.infinity;
    var right = -double.infinity, bottom = -double.infinity;
    for (final n in nodes) {
      left = math.min(left, n.position.dx - n.radius);
      top = math.min(top, n.position.dy - n.radius);
      right = math.max(right, n.position.dx + n.radius);
      bottom = math.max(bottom, n.position.dy + n.radius);
    }
    return Rect.fromLTRB(left, top, right, bottom);
  }
}

class LayoutLink {
  LayoutLink({required this.source, required this.target, required this.data});

  final LayoutNode source;
  final LayoutNode target;
  final GraphLink data;
}
