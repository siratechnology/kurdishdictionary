import '../core/api_client.dart';
import '../models/graph.dart';
import '../models/word.dart';

/// Every dictionary endpoint the app touches, in one place. Read endpoints are
/// anonymous; the write endpoints below require an Admin/Editor bearer token,
/// which [ApiClient] attaches when the user is signed in.
class WordsRepository {
  WordsRepository(this._api);

  final ApiClient _api;

  Map<String, dynamic> _obj(dynamic v) =>
      v is Map<String, dynamic> ? v : <String, dynamic>{};

  List<dynamic> _list(dynamic v) => v is List ? v : const [];

  // ── Reads ───────────────────────────────────────────────────────────────

  /// The main feed. `category` filters by category *name* — that is what the
  /// controller compares against, not the id.
  Future<Paged<Word>> search({
    int page = 1,
    int pageSize = 20,
    String? search,
    String? category,
  }) async {
    final json = await _api.get('/api/words', query: {
      'page': page,
      'pageSize': pageSize,
      'search': search,
      'category': category,
    });
    return Paged.fromJson(_obj(json), Word.fromJson);
  }

  Future<Word> byId(int id) async =>
      Word.fromJson(_obj(await _api.get('/api/words/$id')));

  Future<WordMeta> meta(int id) async =>
      WordMeta.fromJson(_obj(await _api.get('/api/words/$id/meta')));

  Future<WordGraph> graph(int id) async =>
      WordGraph.fromJson(_obj(await _api.get('/api/words/$id/graph')));

  Future<List<Category>> categories() async =>
      _list(await _api.get('/api/words/categories'))
          .map((e) => Category.fromJson(e as Map<String, dynamic>))
          .toList();

  Future<Paged<Word>> categoryWords(
    int categoryId, {
    int page = 1,
    int pageSize = 20,
    String? search,
  }) async {
    final json = await _api.get('/api/words/categories/$categoryId/words',
        query: {'page': page, 'pageSize': pageSize, 'search': search});
    return Paged.fromJson(_obj(json), Word.fromJson);
  }

  Future<List<SpeechPaneStat>> speechTypeStats() async =>
      _list(await _api.get('/api/words/speech-types/stats'))
          .map((e) => SpeechPaneStat.fromJson(e as Map<String, dynamic>))
          .toList();

  Future<Paged<Word>> speechTypeWords(
    int typeId, {
    int page = 1,
    int pageSize = 20,
    String? search,
  }) async {
    final json = await _api.get('/api/words/speech-types/$typeId/words',
        query: {'page': page, 'pageSize': pageSize, 'search': search});
    return Paged.fromJson(_obj(json), Word.fromJson);
  }

  Future<List<NamedOption>> speechTypes() async =>
      _list(await _api.get('/api/words/speech-types'))
          .map((e) => NamedOption.fromJson(e as Map<String, dynamic>))
          .toList();

  Future<List<NamedOption>> genders() async =>
      _list(await _api.get('/api/words/genders'))
          .map((e) => NamedOption.fromJson(e as Map<String, dynamic>))
          .toList();

  Future<List<String>> locates() async =>
      _list(await _api.get('/api/words/locates')).map((e) => '$e').toList();

  // ── Writes (Admin/Editor only) ──────────────────────────────────────────

  Future<Word> create(WordInput input) async =>
      Word.fromJson(_obj(await _api.post('/api/words', body: input.toJson())));

  Future<Word> update(int id, WordInput input) async => Word.fromJson(
      _obj(await _api.put('/api/words/$id', body: input.toJson())));

  Future<void> deleteWord(int id) => _api.delete('/api/words/$id');

  Future<Category> createCategory(String name) async =>
      Category.fromJson(_obj(await _api.post('/api/words/categories', body: name)));
}

/// Shared body for create and update — `CreateWordDto` and `UpdateWordDto` are
/// field-for-field identical on the wire.
class WordInput {
  WordInput({
    required this.kurdish,
    required this.speechPanes,
    required this.categoryIds,
    required this.gender,
    this.description,
    required this.meanings,
    this.relatedWords = const [],
  });

  final String kurdish;
  final List<int> speechPanes;
  final List<int> categoryIds;
  final int gender;
  final String? description;
  final List<WordMeaning> meanings;
  final List<RelatedWordInput> relatedWords;

  /// Seeds the edit form from an existing word. Relations are carried over
  /// verbatim because `PUT /api/words/{id}` is a full replace — omitting them
  /// would silently delete every link the word has.
  factory WordInput.fromWord(Word w) => WordInput(
        kurdish: w.kurdish,
        speechPanes: w.speechPanes.map((p) => p.id).toList(),
        categoryIds: w.categories.map((c) => c.id).toList(),
        gender: w.gender,
        description: w.description,
        meanings: List.of(w.meanings),
        relatedWords: w.outgoingRelations
            .map((r) => RelatedWordInput(
                  targetWordId: r.relatedWordId,
                  relationType: r.relationType,
                  weight: r.weight == 0 ? 1 : r.weight,
                ))
            .toList(),
      );

  Map<String, dynamic> toJson() => {
        'kurdish': kurdish.trim(),
        'speechPanes': speechPanes,
        'categoryIds': categoryIds,
        'gender': gender,
        'description':
            (description == null || description!.trim().isEmpty) ? null : description!.trim(),
        'meanings': [
          for (final m in meanings)
            if (m.meaning.trim().isNotEmpty)
              {
                'id': 0,
                'meaning': m.meaning.trim(),
                'locate': (m.locate == null || m.locate!.trim().isEmpty)
                    ? null
                    : m.locate!.trim(),
              }
        ],
        'relatedWords': [for (final r in relatedWords) r.toJson()],
      };
}

class RelatedWordInput {
  const RelatedWordInput({
    required this.targetWordId,
    required this.relationType,
    this.weight = 1,
  });

  final int targetWordId;
  final String relationType;
  final int weight;

  Map<String, dynamic> toJson() => {
        'targetWordId': targetWordId,
        'relationType': relationType,
        'weight': weight,
      };
}
