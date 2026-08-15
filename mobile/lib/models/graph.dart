/// Mirrors `shared/Dtos/GraphDto.cs` — the payload the web mind map renders
/// with D3. The mobile app runs its own force simulation over the same data.
library;

int _asInt(Object? v) => v is int ? v : int.tryParse('${v ?? ''}') ?? 0;

class GraphNode {
  const GraphNode({
    required this.id,
    required this.label,
    this.category,
    this.isCenter = false,
    this.weight = 1,
    this.color,
    this.relationType,
    this.speechPane = 0,
  });

  final String id;
  final String label;
  final String? category;
  final bool isCenter;

  /// Relation count — drives the node radius.
  final int weight;
  final String? color;

  /// How this node hangs off the centre node; null for the centre itself.
  final String? relationType;
  final int speechPane;

  /// Node ids are word ids stringified by the API.
  int get wordId => int.tryParse(id) ?? 0;

  factory GraphNode.fromJson(Map<String, dynamic> j) => GraphNode(
        id: '${j['id'] ?? ''}',
        label: j['label'] as String? ?? '',
        category: j['category'] as String?,
        isCenter: j['isCenter'] as bool? ?? false,
        weight: _asInt(j['weight']),
        color: j['color'] as String?,
        relationType: j['relationType'] as String?,
        speechPane: _asInt(j['speechPane']),
      );
}

class GraphLink {
  const GraphLink({
    required this.source,
    required this.target,
    required this.relationType,
    this.weight = 1,
    this.isIncoming = false,
  });

  final String source;
  final String target;
  final String relationType;
  final int weight;
  final bool isIncoming;

  factory GraphLink.fromJson(Map<String, dynamic> j) => GraphLink(
        source: '${j['source'] ?? ''}',
        target: '${j['target'] ?? ''}',
        relationType: j['relationType'] as String? ?? 'Related',
        weight: _asInt(j['weight']),
        isIncoming: j['isIncoming'] as bool? ?? false,
      );
}

class WordGraph {
  const WordGraph({required this.nodes, required this.links});

  final List<GraphNode> nodes;
  final List<GraphLink> links;

  bool get isEmpty => nodes.isEmpty;

  /// True when the centre word has no neighbours — the mind map shows a
  /// dedicated "no connections yet" state instead of a lone dot.
  bool get isolated => nodes.length <= 1;

  factory WordGraph.fromJson(Map<String, dynamic> j) => WordGraph(
        nodes: (j['nodes'] as List? ?? const [])
            .map((e) => GraphNode.fromJson(e as Map<String, dynamic>))
            .toList(),
        links: (j['links'] as List? ?? const [])
            .map((e) => GraphLink.fromJson(e as Map<String, dynamic>))
            .toList(),
      );
}
