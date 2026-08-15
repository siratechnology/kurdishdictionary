import 'package:flutter/material.dart';
import 'package:flutter/scheduler.dart';
import 'package:flutter/services.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../core/api_client.dart';
import '../../core/theme.dart';
import '../../models/graph.dart';
import '../../state/providers.dart';
import '../graph/force_layout.dart';
import '../graph/graph_painter.dart';
import '../navigation.dart';
import '../widgets/chips.dart';
import '../widgets/glass.dart';
import '../widgets/states.dart';

/// The interactive mind map for a single word — the phone counterpart of the
/// D3 graph on the web. Pinch to zoom, drag to pan, drag a node to pull it
/// around, tap a node to focus it, tap again to open that word.
class MindMapScreen extends ConsumerWidget {
  const MindMapScreen({super.key, required this.wordId, required this.title});

  final int wordId;
  final String title;

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final async = ref.watch(graphProvider(wordId));

    return Scaffold(
      extendBodyBehindAppBar: true,
      appBar: AppBar(
        title: Text(title),
        flexibleSpace: const GlassBar(child: SizedBox.expand()),
        actions: [
          IconButton(
            tooltip: 'نوێکردنەوە',
            icon: const Icon(Icons.refresh),
            onPressed: () => ref.invalidate(graphProvider(wordId)),
          ),
        ],
      ),
      body: async.when(
        loading: () => const Center(child: CircularProgressIndicator()),
        error: (error, _) {
          final api = error is ApiException ? error : null;
          return ErrorState(
            message: api?.kurdishMessage ?? 'نەخشەکە نەهێنرا',
            details: api?.message ?? '$error',
            onRetry: () => ref.invalidate(graphProvider(wordId)),
          );
        },
        data: (graph) {
          if (graph.isolated) {
            return EmptyState(
              icon: Icons.hub_outlined,
              title: 'هێشتا پەیوەندی نییە',
              message: '«$title» بە هیچ وشەیەکی ترەوە نەبەستراوەتەوە.',
              action: FilledButton.icon(
                onPressed: () => Navigator.of(context).pop(),
                icon: const Icon(Icons.arrow_forward, size: 18),
                label: const Text('گەڕانەوە'),
              ),
            );
          }
          return _GraphCanvas(graph: graph, rootId: wordId);
        },
      ),
    );
  }
}

class _GraphCanvas extends StatefulWidget {
  const _GraphCanvas({required this.graph, required this.rootId});

  final WordGraph graph;
  final int rootId;

  @override
  State<_GraphCanvas> createState() => _GraphCanvasState();
}

class _GraphCanvasState extends State<_GraphCanvas>
    with SingleTickerProviderStateMixin {
  /// A fixed world the simulation lives in, larger than any viewport so nodes
  /// have room to spread before the clamp in [ForceLayout] catches them.
  static const _canvas = Size(1400, 1400);

  late final ForceLayout _layout;
  late final Ticker _ticker;
  final _viewer = TransformationController();

  /// Repaints the CustomPaint without rebuilding the widget tree.
  final _repaint = ValueNotifier<int>(0);

  double _pulse = 0;
  double _scale = 1;
  String? _selectedId;
  LayoutNode? _dragging;

  @override
  void initState() {
    super.initState();
    _layout = ForceLayout(graph: widget.graph, size: _canvas)..settle();
    _ticker = createTicker(_onTick)..start();
    _viewer.addListener(_onViewerChanged);
    WidgetsBinding.instance.addPostFrameCallback((_) => _frameGraph());
  }

  @override
  void dispose() {
    _ticker.dispose();
    _viewer.removeListener(_onViewerChanged);
    _viewer.dispose();
    _repaint.dispose();
    super.dispose();
  }

  void _onTick(Duration elapsed) {
    final seconds = elapsed.inMicroseconds / 1e6;
    // The halo keeps breathing after the layout has settled — it's what makes
    // the map feel alive rather than a static diagram.
    final pulse = (seconds % 2.4) / 2.4;
    final moving = !_layout.settled || _dragging != null;
    if (moving) _layout.tick();
    _pulse = pulse;
    _repaint.value++;
  }

  void _onViewerChanged() {
    final scale = _viewer.value.getMaxScaleOnAxis();
    if ((scale - _scale).abs() > 0.02) {
      _scale = scale;
      _repaint.value++;
    }
  }

  /// Centres and zooms so the whole graph is visible on first paint.
  void _frameGraph() {
    if (!mounted) return;
    final viewport = context.size;
    if (viewport == null) return;

    final bounds = _layout.bounds.inflate(60);
    final scale = (viewport.width / bounds.width)
        .clamp(0.35, 1.0)
        .toDouble();
    final heightScale = (viewport.height / bounds.height).clamp(0.35, 1.0);
    final chosen = scale < heightScale ? scale : heightScale.toDouble();

    final center = bounds.center;
    _viewer.value = Matrix4.identity()
      ..translateByDouble(viewport.width / 2, viewport.height / 2, 0, 1)
      ..scaleByDouble(chosen, chosen, chosen, 1)
      ..translateByDouble(-center.dx, -center.dy, 0, 1);
    _scale = chosen;
  }

  Offset _toCanvas(Offset viewportPoint) {
    final inverse = Matrix4.inverted(_viewer.value);
    return MatrixUtils.transformPoint(inverse, viewportPoint);
  }

  void _onTapUp(TapUpDetails details) {
    final node = _layout.hitTest(_toCanvas(details.localPosition));
    if (node == null) {
      if (_selectedId != null) setState(() => _selectedId = null);
      return;
    }

    // First tap focuses and dims everything unrelated; a second tap on the
    // already-focused node navigates. Opening on a single tap made it far too
    // easy to leave the map by accident while panning.
    if (_selectedId == node.data.id) {
      HapticFeedback.selectionClick();
      if (node.data.wordId == widget.rootId) {
        Navigator.of(context).pop();
      } else {
        openWord(context, node.data.wordId);
      }
      return;
    }

    HapticFeedback.lightImpact();
    setState(() => _selectedId = node.data.id);
    _layout.reheat(0.2);
  }

  void _onPanStart(DragStartDetails details) {
    final node = _layout.hitTest(_toCanvas(details.localPosition));
    if (node == null) return;
    _dragging = node..pinned = true;
    _layout.reheat();
  }

  void _onPanUpdate(DragUpdateDetails details) {
    final node = _dragging;
    if (node == null) return;
    node.position = _toCanvas(details.localPosition);
    _repaint.value++;
  }

  void _onPanEnd() {
    _dragging?.pinned = false;
    _dragging = null;
    _layout.reheat();
  }

  @override
  Widget build(BuildContext context) {
    final t = tokensOf(context);
    final selected = _selectedId == null
        ? null
        : _layout.nodes
            .where((n) => n.data.id == _selectedId)
            .firstOrNull;

    return Stack(
      children: [
        Positioned.fill(
          child: InteractiveViewer(
            transformationController: _viewer,
            minScale: 0.3,
            maxScale: 3.2,
            boundaryMargin: const EdgeInsets.all(400),
            // A node drag must win over the viewer's pan, so panning is handled
            // by the gesture detector below and disabled here.
            panEnabled: _dragging == null,
            scaleEnabled: true,
            child: SizedBox(
              width: _canvas.width,
              height: _canvas.height,
              child: GestureDetector(
                behavior: HitTestBehavior.opaque,
                onTapUp: _onTapUp,
                onPanStart: _onPanStart,
                onPanUpdate: _onPanUpdate,
                onPanEnd: (_) => _onPanEnd(),
                onPanCancel: _onPanEnd,
                child: CustomPaint(
                  size: _canvas,
                  painter: GraphPainter(
                    layout: _layout,
                    tokens: t,
                    pulse: _pulse,
                    selectedId: _selectedId,
                    scale: _scale,
                    repaint: _repaint,
                  ),
                ),
              ),
            ),
          ),
        ),
        Positioned(
          left: 12,
          right: 12,
          bottom: 12,
          child: SafeArea(
            top: false,
            child: selected != null
                ? _SelectionCard(
                    node: selected.data,
                    isRoot: selected.data.wordId == widget.rootId,
                    onOpen: () {
                      if (selected.data.wordId == widget.rootId) {
                        Navigator.of(context).pop();
                      } else {
                        openWord(context, selected.data.wordId);
                      }
                    },
                    onDismiss: () => setState(() => _selectedId = null),
                  )
                : _Legend(graph: widget.graph),
          ),
        ),
        Positioned(
          right: 12,
          bottom: 118,
          child: SafeArea(
            top: false,
            child: _ZoomControls(
              onFit: _frameGraph,
              onZoom: (factor) {
                final size = context.size;
                if (size == null) return;
                final focal = Offset(size.width / 2, size.height / 2);
                final point = _toCanvas(focal);
                final next = (_scale * factor).clamp(0.3, 3.2).toDouble();
                _viewer.value = Matrix4.identity()
                  ..translateByDouble(focal.dx, focal.dy, 0, 1)
                  ..scaleByDouble(next, next, next, 1)
                  ..translateByDouble(-point.dx, -point.dy, 0, 1);
                _scale = next;
                _repaint.value++;
              },
            ),
          ),
        ),
      ],
    );
  }
}

class _SelectionCard extends StatelessWidget {
  const _SelectionCard({
    required this.node,
    required this.isRoot,
    required this.onOpen,
    required this.onDismiss,
  });

  final GraphNode node;
  final bool isRoot;
  final VoidCallback onOpen;
  final VoidCallback onDismiss;

  @override
  Widget build(BuildContext context) {
    final t = tokensOf(context);
    final style = RelationStyle.of(node.relationType);

    return GlassCard(
      raised: true,
      blur: 20,
      padding: const EdgeInsets.fromLTRB(16, 14, 12, 14),
      child: Row(
        children: [
          Container(
            width: 12,
            height: 12,
            decoration: BoxDecoration(
              color: isRoot ? t.accentLight : style.color,
              shape: BoxShape.circle,
            ),
          ),
          const SizedBox(width: 12),
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              mainAxisSize: MainAxisSize.min,
              children: [
                Text(
                  node.label,
                  maxLines: 1,
                  overflow: TextOverflow.ellipsis,
                  style: TextStyle(
                    fontFamily: kFontFamily,
                    fontSize: 18,
                    fontWeight: FontWeight.w700,
                    color: t.text1,
                  ),
                ),
                const SizedBox(height: 2),
                Text(
                  isRoot
                      ? 'وشەی سەرەکی · ${node.weight} پەیوەندی'
                      : '${style.label} · ${node.weight} پەیوەندی',
                  style: TextStyle(
                      fontFamily: kFontFamily, fontSize: 12, color: t.text3),
                ),
              ],
            ),
          ),
          IconButton(
            tooltip: 'داخستن',
            icon: Icon(Icons.close_rounded, size: 18, color: t.text3),
            onPressed: onDismiss,
          ),
          FilledButton(
            onPressed: onOpen,
            style: FilledButton.styleFrom(
              padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 11),
            ),
            child: Text(isRoot ? 'گەڕانەوە' : 'کردنەوە'),
          ),
        ],
      ),
    );
  }
}

class _Legend extends StatelessWidget {
  const _Legend({required this.graph});

  final WordGraph graph;

  @override
  Widget build(BuildContext context) {
    // Only legend the relation types actually present in this graph.
    final types = <String>{
      for (final link in graph.links) link.relationType.toLowerCase(),
    }.toList();

    if (types.isEmpty) return const SizedBox.shrink();

    return GlassCard(
      blur: 14,
      padding: const EdgeInsets.symmetric(horizontal: 14, vertical: 11),
      child: Row(
        children: [
          Icon(Icons.touch_app_outlined,
              size: 15, color: tokensOf(context).text4),
          const SizedBox(width: 8),
          Expanded(
            child: Wrap(
              spacing: 7,
              runSpacing: 6,
              children: [
                for (final type in types)
                  RelationChip(relationType: type, dense: true),
              ],
            ),
          ),
        ],
      ),
    );
  }
}

class _ZoomControls extends StatelessWidget {
  const _ZoomControls({required this.onFit, required this.onZoom});

  final VoidCallback onFit;
  final void Function(double factor) onZoom;

  @override
  Widget build(BuildContext context) {
    final t = tokensOf(context);

    Widget button(IconData icon, String tooltip, VoidCallback onPressed) =>
        Tooltip(
          message: tooltip,
          child: InkWell(
            onTap: onPressed,
            borderRadius: BorderRadius.circular(12),
            child: SizedBox(
              width: 42,
              height: 42,
              child: Icon(icon, size: 19, color: t.text2),
            ),
          ),
        );

    return GlassCard(
      blur: 14,
      radius: 16,
      padding: EdgeInsets.zero,
      child: Column(
        mainAxisSize: MainAxisSize.min,
        children: [
          button(Icons.add, 'زووم', () => onZoom(1.35)),
          Divider(height: 1, color: t.borderSubtle),
          button(Icons.remove, 'دوورخستنەوە', () => onZoom(1 / 1.35)),
          Divider(height: 1, color: t.borderSubtle),
          button(Icons.center_focus_strong_outlined, 'هەمووی پیشان بدە', onFit),
        ],
      ),
    );
  }
}
