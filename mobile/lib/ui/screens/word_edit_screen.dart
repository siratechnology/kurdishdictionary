import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../core/api_client.dart';
import '../../core/theme.dart';
import '../../data/words_repository.dart';
import '../../models/word.dart';
import '../../state/providers.dart';
import '../widgets/chips.dart';
import '../widgets/glass.dart';

/// Create a word, or edit an existing one. Requires an Admin or Editor token —
/// the calling screens only offer it when [AuthState.canEdit] is true.
///
/// Note that `PUT /api/words/{id}` replaces the word wholesale, relations
/// included, so the form seeds itself from the full word and sends everything
/// back — editing one meaning must not silently drop the word's links.
class WordEditScreen extends ConsumerStatefulWidget {
  const WordEditScreen({super.key, this.word});

  final Word? word;

  @override
  ConsumerState<WordEditScreen> createState() => _WordEditScreenState();
}

class _WordEditScreenState extends ConsumerState<WordEditScreen> {
  final _formKey = GlobalKey<FormState>();
  late final TextEditingController _kurdish;
  late final TextEditingController _description;

  final List<_MeaningRow> _meanings = [];
  late Set<int> _speechPanes;
  late Set<int> _categoryIds;
  late int _gender;

  bool _saving = false;
  String? _error;

  bool get _isEdit => widget.word != null;

  @override
  void initState() {
    super.initState();
    final w = widget.word;
    _kurdish = TextEditingController(text: w?.kurdish ?? '');
    _description = TextEditingController(text: w?.description ?? '');
    _speechPanes = {...?w?.speechPanes.map((p) => p.id)};
    _categoryIds = {...?w?.categories.map((c) => c.id)};
    _gender = w?.gender ?? 0;

    if (w != null && w.meanings.isNotEmpty) {
      for (final m in w.meanings) {
        _meanings.add(_MeaningRow.from(m));
      }
    } else {
      _meanings.add(_MeaningRow.empty());
    }
  }

  @override
  void dispose() {
    _kurdish.dispose();
    _description.dispose();
    for (final row in _meanings) {
      row.dispose();
    }
    super.dispose();
  }

  Future<void> _save() async {
    if (!(_formKey.currentState?.validate() ?? false)) return;
    if (_speechPanes.isEmpty) {
      setState(() => _error = 'لانیکەم یەک جۆری وشە هەڵبژێرە');
      return;
    }

    setState(() {
      _saving = true;
      _error = null;
    });

    final input = WordInput(
      kurdish: _kurdish.text,
      speechPanes: _speechPanes.toList(),
      categoryIds: _categoryIds.toList(),
      gender: _gender,
      description: _description.text,
      meanings: [
        for (final row in _meanings)
          if (row.meaning.text.trim().isNotEmpty)
            WordMeaning(
                meaning: row.meaning.text, locate: row.locate.text),
      ],
      // Preserved verbatim on edit; a new word starts with none.
      relatedWords: _isEdit
          ? WordInput.fromWord(widget.word!).relatedWords
          : const [],
    );

    try {
      final repository = ref.read(wordsRepositoryProvider);
      final saved = _isEdit
          ? await repository.update(widget.word!.id, input)
          : await repository.create(input);

      if (!mounted) return;
      HapticFeedback.mediumImpact();
      Navigator.of(context).pop(saved);
    } on ApiException catch (e) {
      if (!mounted) return;
      setState(() {
        _saving = false;
        _error = '${e.kurdishMessage}\n${e.message}';
      });
    }
  }

  Future<void> _delete() async {
    final word = widget.word;
    if (word == null) return;

    final confirmed = await showDialog<bool>(
      context: context,
      builder: (context) => AlertDialog(
        backgroundColor: tokensOf(context).surfaceCard,
        shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(20)),
        title: Text('سڕینەوەی «${word.kurdish}»؟'),
        content: const Text(
            'وشەکە و هەموو واتا و پەیوەندییەکانی دەسڕدرێنەوە. ئەم کردارە ناگەڕێتەوە.'),
        actions: [
          TextButton(
            onPressed: () => Navigator.of(context).pop(false),
            child: const Text('پاشگەزبوونەوە'),
          ),
          FilledButton(
            style:
                FilledButton.styleFrom(backgroundColor: const Color(0xFFF87171)),
            onPressed: () => Navigator.of(context).pop(true),
            child: const Text('سڕینەوە'),
          ),
        ],
      ),
    );
    if (confirmed != true) return;

    setState(() {
      _saving = true;
      _error = null;
    });
    try {
      await ref.read(wordsRepositoryProvider).deleteWord(word.id);
      if (!mounted) return;
      Navigator.of(context).pop(null);
    } on ApiException catch (e) {
      if (!mounted) return;
      setState(() {
        _saving = false;
        _error = '${e.kurdishMessage}\n${e.message}';
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    final t = tokensOf(context);
    final speechTypes = ref.watch(speechTypesProvider);
    final genders = ref.watch(gendersProvider);
    final categories = ref.watch(categoriesProvider);
    final isAdmin = ref.watch(authProvider.select((a) => a.user?.isAdmin ?? false));

    return Scaffold(
      extendBodyBehindAppBar: true,
      appBar: AppBar(
        title: Text(_isEdit ? 'دەستکاری وشە' : 'وشەی نوێ'),
        flexibleSpace: const GlassBar(child: SizedBox.expand()),
        actions: [
          if (_isEdit && isAdmin)
            IconButton(
              tooltip: 'سڕینەوە',
              icon: const Icon(Icons.delete_outline, color: Color(0xFFF87171)),
              onPressed: _saving ? null : _delete,
            ),
        ],
      ),
      body: Form(
        key: _formKey,
        child: ListView(
          padding: EdgeInsets.fromLTRB(16,
              MediaQuery.paddingOf(context).top + kToolbarHeight + 16, 16, 120),
          physics: const BouncingScrollPhysics(),
          children: [
            GlassCard(
              padding: const EdgeInsets.all(16),
              child: TextFormField(
                controller: _kurdish,
                textDirection: TextDirection.rtl,
                style: TextStyle(
                  fontFamily: kFontFamily,
                  fontSize: 22,
                  fontWeight: FontWeight.w700,
                  color: t.text1,
                ),
                decoration: const InputDecoration(
                  labelText: 'وشە بە کوردی',
                  hintText: 'وشەکە بنووسە…',
                ),
                validator: (v) =>
                    (v ?? '').trim().isEmpty ? 'وشەکە پێویستە' : null,
              ),
            ),
            _EditGroup(
              title: 'جۆری وشە',
              subtitle: 'دەتوانیت زیاتر لە یەک هەڵبژێریت',
              child: speechTypes.when(
                loading: () => const _InlineLoader(),
                error: (e, _) => _InlineError('$e'),
                data: (list) => Wrap(
                  spacing: 8,
                  runSpacing: 8,
                  children: [
                    for (final type in list)
                      _SelectableChip(
                        label: type.kurdish,
                        color: SpeechStyle.of(type.id),
                        selected: _speechPanes.contains(type.id),
                        onTap: () => setState(() {
                          if (!_speechPanes.remove(type.id)) {
                            _speechPanes.add(type.id);
                          }
                        }),
                      ),
                  ],
                ),
              ),
            ),
            _EditGroup(
              title: 'ڕەگەز',
              child: genders.when(
                loading: () => const _InlineLoader(),
                error: (e, _) => _InlineError('$e'),
                data: (list) => Wrap(
                  spacing: 8,
                  runSpacing: 8,
                  children: [
                    for (final g in list)
                      _SelectableChip(
                        label: g.kurdish,
                        color: t.accentLight,
                        selected: _gender == g.id,
                        onTap: () => setState(() => _gender = g.id),
                      ),
                  ],
                ),
              ),
            ),
            _EditGroup(
              title: 'پۆلەکان',
              child: categories.when(
                loading: () => const _InlineLoader(),
                error: (e, _) => _InlineError('$e'),
                data: (list) => Wrap(
                  spacing: 8,
                  runSpacing: 8,
                  children: [
                    for (final c in list)
                      _SelectableChip(
                        label: c.name,
                        color: t.accentLight,
                        selected: _categoryIds.contains(c.id),
                        onTap: () => setState(() {
                          if (!_categoryIds.remove(c.id)) {
                            _categoryIds.add(c.id);
                          }
                        }),
                      ),
                  ],
                ),
              ),
            ),
            _EditGroup(
              title: 'واتاکان',
              subtitle: 'هەر واتایەک بە شوێنی خۆیەوە',
              child: Column(
                children: [
                  for (var i = 0; i < _meanings.length; i++)
                    _MeaningEditor(
                      row: _meanings[i],
                      index: i + 1,
                      canRemove: _meanings.length > 1,
                      onRemove: () => setState(() {
                        _meanings.removeAt(i).dispose();
                      }),
                    ),
                  const SizedBox(height: 8),
                  Align(
                    alignment: AlignmentDirectional.centerStart,
                    child: TextButton.icon(
                      onPressed: () =>
                          setState(() => _meanings.add(_MeaningRow.empty())),
                      icon: const Icon(Icons.add, size: 18),
                      label: const Text('زیادکردنی واتا'),
                    ),
                  ),
                ],
              ),
            ),
            _EditGroup(
              title: 'ڕوونکردنەوە',
              child: TextFormField(
                controller: _description,
                textDirection: TextDirection.rtl,
                maxLines: 4,
                minLines: 2,
                decoration: const InputDecoration(
                    hintText: 'ڕوونکردنەوەیەکی زیاتر (ئارەزوومەندانە)'),
              ),
            ),
            if (_error != null) ...[
              const SizedBox(height: 8),
              Container(
                padding: const EdgeInsets.all(14),
                decoration: BoxDecoration(
                  color: const Color(0xFFF87171).withValues(alpha: 0.10),
                  borderRadius: BorderRadius.circular(14),
                  border: Border.all(
                      color: const Color(0xFFF87171).withValues(alpha: 0.28)),
                ),
                child: Text(
                  _error!,
                  style: TextStyle(
                    fontFamily: kFontFamily,
                    fontSize: 13,
                    color: t.text1,
                    height: 1.6,
                  ),
                ),
              ),
            ],
          ],
        ),
      ),
      bottomNavigationBar: SafeArea(
        child: Padding(
          padding: const EdgeInsets.fromLTRB(16, 8, 16, 12),
          child: FilledButton.icon(
            onPressed: _saving ? null : _save,
            icon: _saving
                ? const SizedBox(
                    width: 18,
                    height: 18,
                    child: CircularProgressIndicator(
                        strokeWidth: 2.2, color: Colors.white),
                  )
                : const Icon(Icons.check, size: 20),
            label: Text(_isEdit ? 'پاشەکەوتکردن' : 'زیادکردن'),
            style: FilledButton.styleFrom(
                padding: const EdgeInsets.symmetric(vertical: 16)),
          ),
        ),
      ),
    );
  }
}

/// One editable meaning: the sense text plus its optional dialect label.
class _MeaningRow {
  _MeaningRow(this.meaning, this.locate);

  factory _MeaningRow.empty() =>
      _MeaningRow(TextEditingController(), TextEditingController());

  factory _MeaningRow.from(WordMeaning m) => _MeaningRow(
        TextEditingController(text: m.meaning),
        TextEditingController(text: m.locate ?? ''),
      );

  final TextEditingController meaning;
  final TextEditingController locate;

  void dispose() {
    meaning.dispose();
    locate.dispose();
  }
}

class _MeaningEditor extends ConsumerWidget {
  const _MeaningEditor({
    required this.row,
    required this.index,
    required this.canRemove,
    required this.onRemove,
  });

  final _MeaningRow row;
  final int index;
  final bool canRemove;
  final VoidCallback onRemove;

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final t = tokensOf(context);
    final locates = ref.watch(locatesProvider).value ?? const <String>[];

    return Container(
      margin: const EdgeInsets.only(bottom: 12),
      padding: const EdgeInsets.fromLTRB(12, 10, 12, 12),
      decoration: BoxDecoration(
        color: t.surfaceRaised,
        borderRadius: BorderRadius.circular(14),
        border: Border.all(color: t.borderSubtle),
      ),
      child: Column(
        children: [
          Row(
            children: [
              Container(
                width: 22,
                height: 22,
                alignment: Alignment.center,
                decoration: BoxDecoration(
                  color: t.accent.withValues(alpha: 0.14),
                  borderRadius: BorderRadius.circular(7),
                ),
                child: Text(
                  '$index',
                  style: TextStyle(
                    fontFamily: kFontFamily,
                    fontSize: 11,
                    fontWeight: FontWeight.w700,
                    color: t.accentLight,
                  ),
                ),
              ),
              const SizedBox(width: 10),
              Expanded(
                child: TextFormField(
                  controller: row.meaning,
                  textDirection: TextDirection.rtl,
                  maxLines: null,
                  decoration: const InputDecoration(
                    hintText: 'واتا…',
                    filled: false,
                    border: InputBorder.none,
                    enabledBorder: InputBorder.none,
                    focusedBorder: InputBorder.none,
                    isDense: true,
                    contentPadding: EdgeInsets.symmetric(vertical: 4),
                  ),
                ),
              ),
              if (canRemove)
                IconButton(
                  tooltip: 'لابردن',
                  icon: Icon(Icons.close_rounded, size: 18, color: t.text4),
                  visualDensity: VisualDensity.compact,
                  onPressed: onRemove,
                ),
            ],
          ),
          const SizedBox(height: 6),
          Row(
            children: [
              Icon(Icons.place_outlined, size: 15, color: t.text4),
              const SizedBox(width: 8),
              Expanded(
                child: TextFormField(
                  controller: row.locate,
                  textDirection: TextDirection.ltr,
                  style: TextStyle(
                      fontFamily: kFontFamily, fontSize: 13, color: t.text2),
                  decoration: const InputDecoration(
                    hintText: 'شێوەزار (ئارەزوومەندانە)',
                    filled: false,
                    border: InputBorder.none,
                    enabledBorder: InputBorder.none,
                    focusedBorder: InputBorder.none,
                    isDense: true,
                    contentPadding: EdgeInsets.symmetric(vertical: 2),
                  ),
                ),
              ),
              // Reuse an existing dialect label rather than inventing a new
              // spelling of one that already exists in the data.
              if (locates.isNotEmpty)
                PopupMenuButton<String>(
                  tooltip: 'شێوەزارە بەردەستەکان',
                  icon: Icon(Icons.arrow_drop_down, color: t.text4),
                  onSelected: (value) => row.locate.text = value,
                  itemBuilder: (_) => [
                    for (final l in locates.take(20))
                      PopupMenuItem(value: l, child: Text(l)),
                  ],
                ),
            ],
          ),
        ],
      ),
    );
  }
}

class _EditGroup extends StatelessWidget {
  const _EditGroup({required this.title, required this.child, this.subtitle});

  final String title;
  final String? subtitle;
  final Widget child;

  @override
  Widget build(BuildContext context) {
    final t = tokensOf(context);
    return Padding(
      padding: const EdgeInsets.only(top: 16),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Padding(
            padding: const EdgeInsets.fromLTRB(4, 0, 4, 10),
            child: Row(
              children: [
                Text(
                  title,
                  style: TextStyle(
                    fontFamily: kFontFamily,
                    fontSize: 15,
                    fontWeight: FontWeight.w600,
                    color: t.text2,
                  ),
                ),
                if (subtitle != null) ...[
                  const SizedBox(width: 8),
                  Expanded(
                    child: Text(
                      subtitle!,
                      style: TextStyle(
                          fontFamily: kFontFamily,
                          fontSize: 11.5,
                          color: t.text4),
                    ),
                  ),
                ],
              ],
            ),
          ),
          GlassCard(padding: const EdgeInsets.all(16), child: child),
        ],
      ),
    );
  }
}

class _SelectableChip extends StatelessWidget {
  const _SelectableChip({
    required this.label,
    required this.color,
    required this.selected,
    required this.onTap,
  });

  final String label;
  final Color color;
  final bool selected;
  final VoidCallback onTap;

  @override
  Widget build(BuildContext context) => TintedChip(
        label: label,
        color: selected ? color : tokensOf(context).text4,
        icon: selected ? Icons.check : null,
        onTap: onTap,
      );
}

class _InlineLoader extends StatelessWidget {
  const _InlineLoader();

  @override
  Widget build(BuildContext context) => const Padding(
        padding: EdgeInsets.symmetric(vertical: 10),
        child: Center(
          child: SizedBox(
            width: 20,
            height: 20,
            child: CircularProgressIndicator(strokeWidth: 2),
          ),
        ),
      );
}

class _InlineError extends StatelessWidget {
  const _InlineError(this.message);

  final String message;

  @override
  Widget build(BuildContext context) => Text(
        message,
        style: const TextStyle(fontSize: 12, color: Color(0xFFF87171)),
      );
}
