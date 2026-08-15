/// Dart mirrors of `shared/Dtos/WordDto.cs`. The backend serialises with the
/// ASP.NET default camelCase policy, so the JSON keys here match the C# property
/// names with a lower-cased first letter.
library;

int _asInt(Object? v) => v is int ? v : int.tryParse('${v ?? ''}') ?? 0;

class SpeechPane {
  const SpeechPane({required this.id, required this.kurdish});

  final int id;
  final String kurdish;

  factory SpeechPane.fromJson(Map<String, dynamic> j) =>
      SpeechPane(id: _asInt(j['id']), kurdish: j['kurdish'] as String? ?? '');

  @override
  bool operator ==(Object other) => other is SpeechPane && other.id == id;

  @override
  int get hashCode => id.hashCode;
}

class Category {
  const Category({required this.id, required this.name, this.wordCount = 0});

  final int id;
  final String name;
  final int wordCount;

  factory Category.fromJson(Map<String, dynamic> j) => Category(
        id: _asInt(j['id']),
        name: j['name'] as String? ?? '',
        wordCount: _asInt(j['wordCount']),
      );

  @override
  bool operator ==(Object other) => other is Category && other.id == id;

  @override
  int get hashCode => id.hashCode;
}

/// `speech-types/stats` returns the same shape as a pane plus a count.
class SpeechPaneStat {
  const SpeechPaneStat({
    required this.id,
    required this.kurdish,
    required this.wordCount,
  });

  final int id;
  final String kurdish;
  final int wordCount;

  factory SpeechPaneStat.fromJson(Map<String, dynamic> j) => SpeechPaneStat(
        id: _asInt(j['id']),
        kurdish: j['kurdish'] as String? ?? '',
        wordCount: _asInt(j['wordCount']),
      );
}

class WordMeaning {
  const WordMeaning({this.id = 0, required this.meaning, this.locate});

  final int id;
  final String meaning;

  /// Dialect / region the sense belongs to, e.g. "Kurdish Sorani". Often null.
  final String? locate;

  factory WordMeaning.fromJson(Map<String, dynamic> j) => WordMeaning(
        id: _asInt(j['id']),
        meaning: j['meaning'] as String? ?? '',
        locate: j['locate'] as String?,
      );

  Map<String, dynamic> toJson() => {
        'id': id,
        'meaning': meaning,
        'locate': (locate == null || locate!.trim().isEmpty) ? null : locate,
      };

  WordMeaning copyWith({String? meaning, String? locate}) => WordMeaning(
        id: id,
        meaning: meaning ?? this.meaning,
        locate: locate ?? this.locate,
      );
}

class RelatedWord {
  const RelatedWord({
    this.id = 0,
    required this.relatedWordId,
    this.relatedKurdish,
    required this.relationType,
    required this.isIncoming,
    this.weight = 1,
  });

  final int id;
  final int relatedWordId;
  final String? relatedKurdish;

  /// One of `Synonym` / `Antonym` / `Related` / `Example` as spelled by the API.
  final String relationType;
  final bool isIncoming;
  final int weight;

  factory RelatedWord.fromJson(Map<String, dynamic> j) => RelatedWord(
        id: _asInt(j['id']),
        relatedWordId: _asInt(j['relatedWordId']),
        relatedKurdish: j['relatedKurdish'] as String?,
        relationType: j['relationType'] as String? ?? 'Related',
        isIncoming: j['isIncoming'] as bool? ?? false,
        weight: _asInt(j['weight']),
      );
}

class Word {
  const Word({
    required this.id,
    required this.kurdish,
    this.speechPanes = const [],
    this.categories = const [],
    this.gender = 0,
    this.genderKurdish,
    this.description,
    required this.createdAt,
    this.totalRelations = 0,
    this.meanings = const [],
    this.outgoingRelations = const [],
    this.incomingRelations = const [],
  });

  final int id;
  final String kurdish;
  final List<SpeechPane> speechPanes;
  final List<Category> categories;
  final int gender;
  final String? genderKurdish;
  final String? description;
  final DateTime createdAt;
  final int totalRelations;
  final List<WordMeaning> meanings;
  final List<RelatedWord> outgoingRelations;
  final List<RelatedWord> incomingRelations;

  /// Both directions merged — the detail screen groups them by relation type
  /// rather than by which side of the join the row happened to live on.
  List<RelatedWord> get allRelations => [
        ...outgoingRelations,
        ...incomingRelations,
      ];

  String? get firstMeaning =>
      meanings.isEmpty ? description : meanings.first.meaning;

  factory Word.fromJson(Map<String, dynamic> j) => Word(
        id: _asInt(j['id']),
        kurdish: j['kurdish'] as String? ?? '',
        speechPanes: (j['speechPanes'] as List? ?? const [])
            .map((e) => SpeechPane.fromJson(e as Map<String, dynamic>))
            .toList(),
        categories: (j['categories'] as List? ?? const [])
            .map((e) => Category.fromJson(e as Map<String, dynamic>))
            .toList(),
        gender: _asInt(j['gender']),
        genderKurdish: j['genderKurdish'] as String?,
        description: j['description'] as String?,
        createdAt:
            DateTime.tryParse(j['createdAt'] as String? ?? '') ?? DateTime(2000),
        totalRelations: _asInt(j['totalRelations']),
        meanings: (j['meanings'] as List? ?? const [])
            .map((e) => WordMeaning.fromJson(e as Map<String, dynamic>))
            .toList(),
        outgoingRelations: (j['outgoingRelations'] as List? ?? const [])
            .map((e) => RelatedWord.fromJson(e as Map<String, dynamic>))
            .toList(),
        incomingRelations: (j['incomingRelations'] as List? ?? const [])
            .map((e) => RelatedWord.fromJson(e as Map<String, dynamic>))
            .toList(),
      );

  /// Trimmed-down JSON used to cache favourites and history offline, so a saved
  /// word still renders as a card with no network.
  Map<String, dynamic> toCacheJson() => {
        'id': id,
        'kurdish': kurdish,
        'speechPanes': [
          for (final p in speechPanes) {'id': p.id, 'kurdish': p.kurdish}
        ],
        'categories': [
          for (final c in categories) {'id': c.id, 'name': c.name}
        ],
        'gender': gender,
        'genderKurdish': genderKurdish,
        'description': description,
        'createdAt': createdAt.toIso8601String(),
        'totalRelations': totalRelations,
        'meanings': [for (final m in meanings) m.toJson()],
      };
}

/// `GET /api/words/{id}/meta` — the light payload used for relation previews.
class WordMeta {
  const WordMeta({
    required this.id,
    required this.kurdish,
    this.speechPanes = const [],
    this.categories = const [],
    this.genderKurdish,
    this.firstMeaning,
    this.description,
  });

  final int id;
  final String kurdish;
  final List<SpeechPane> speechPanes;
  final List<Category> categories;
  final String? genderKurdish;
  final String? firstMeaning;
  final String? description;

  factory WordMeta.fromJson(Map<String, dynamic> j) => WordMeta(
        id: _asInt(j['id']),
        kurdish: j['kurdish'] as String? ?? '',
        speechPanes: (j['speechPanes'] as List? ?? const [])
            .map((e) => SpeechPane.fromJson(e as Map<String, dynamic>))
            .toList(),
        categories: (j['categories'] as List? ?? const [])
            .map((e) => Category.fromJson(e as Map<String, dynamic>))
            .toList(),
        genderKurdish: j['genderKurdish'] as String?,
        firstMeaning: j['firstMeaning'] as String?,
        description: j['description'] as String?,
      );
}

/// Matches `PagedResultDto<T>`; `totalPages`/`hasNext` are computed server-side
/// but recomputed here so a partial payload still paginates correctly.
class Paged<T> {
  const Paged({
    required this.items,
    required this.totalCount,
    required this.page,
    required this.pageSize,
  });

  final List<T> items;
  final int totalCount;
  final int page;
  final int pageSize;

  int get totalPages => pageSize == 0 ? 0 : (totalCount / pageSize).ceil();
  bool get hasNext => page < totalPages;

  factory Paged.fromJson(
    Map<String, dynamic> j,
    T Function(Map<String, dynamic>) item,
  ) =>
      Paged(
        items: (j['items'] as List? ?? const [])
            .map((e) => item(e as Map<String, dynamic>))
            .toList(),
        totalCount: _asInt(j['totalCount']),
        page: _asInt(j['page']),
        pageSize: _asInt(j['pageSize']),
      );

  static Paged<T> empty<T>() =>
      Paged<T>(items: const [], totalCount: 0, page: 1, pageSize: 20);
}

/// A `{ id, kurdish }` pair — used by `speech-types` and `genders`.
class NamedOption {
  const NamedOption({required this.id, required this.kurdish});

  final int id;
  final String kurdish;

  factory NamedOption.fromJson(Map<String, dynamic> j) =>
      NamedOption(id: _asInt(j['id']), kurdish: j['kurdish'] as String? ?? '');
}
