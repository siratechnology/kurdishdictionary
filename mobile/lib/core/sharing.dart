import 'package:flutter/widgets.dart';
import 'package:share_plus/share_plus.dart';

import '../models/word.dart';

/// Turns a word into shareable text. The body is readable on its own — someone
/// receiving it in a chat gets the definition without opening anything — and it
/// ends with a link to the public site, which serves an OG image for `/word/{id}`.
class WordSharing {
  const WordSharing(this.shareBase);

  final String shareBase;

  String linkFor(int wordId) {
    final base = shareBase.endsWith('/')
        ? shareBase.substring(0, shareBase.length - 1)
        : shareBase;
    return '$base/word/$wordId';
  }

  String textFor(Word word) {
    final b = StringBuffer()..writeln('📖 ${word.kurdish}');

    final panes = word.speechPanes.map((p) => p.kurdish).join(' · ');
    final gender = word.genderKurdish;
    // Sorani grammar has a "none" gender; printing it adds noise, not meaning.
    final showGender = gender != null && gender.isNotEmpty && gender != 'نییە';
    if (panes.isNotEmpty || showGender) {
      b.writeln([if (panes.isNotEmpty) panes, if (showGender) gender].join(' · '));
    }

    if (word.meanings.isNotEmpty) {
      b.writeln();
      for (var i = 0; i < word.meanings.length; i++) {
        final m = word.meanings[i];
        final locate = (m.locate != null && m.locate!.trim().isNotEmpty)
            ? ' (${m.locate})'
            : '';
        b.writeln('${i + 1}. ${m.meaning}$locate');
      }
    } else if ((word.description ?? '').trim().isNotEmpty) {
      b
        ..writeln()
        ..writeln(word.description!.trim());
    }

    if (word.categories.isNotEmpty) {
      b
        ..writeln()
        ..writeln('🏷 ${word.categories.map((c) => c.name).join('، ')}');
    }

    b
      ..writeln()
      ..writeln(linkFor(word.id));

    return b.toString().trimRight();
  }

  /// [origin] positions the iPad share popover; harmless elsewhere, and cheap
  /// to compute, so we always pass it when we have a render box.
  Future<void> shareWord(Word word, {Rect? origin}) => SharePlus.instance.share(
        ShareParams(
          text: textFor(word),
          subject: word.kurdish,
          sharePositionOrigin: origin,
        ),
      );

  Future<void> shareLink(Word word, {Rect? origin}) => SharePlus.instance.share(
        ShareParams(
          text: linkFor(word.id),
          subject: word.kurdish,
          sharePositionOrigin: origin,
        ),
      );
}

/// The share sheet needs the *global* position of the widget that triggered it.
Rect? originOf(BuildContext context) {
  final box = context.findRenderObject();
  if (box is! RenderBox || !box.hasSize) return null;
  return box.localToGlobal(Offset.zero) & box.size;
}
