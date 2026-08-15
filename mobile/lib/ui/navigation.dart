import 'package:flutter/material.dart';

import '../models/word.dart';
import 'screens/category_words_screen.dart';
import 'screens/login_screen.dart';
import 'screens/mindmap_screen.dart';
import 'screens/settings_screen.dart';
import 'screens/word_detail_screen.dart';
import 'screens/word_edit_screen.dart';

/// Route helpers, kept in one file so screens don't import each other just to
/// push a page — and so every push shares the same transition.
///
/// The app is RTL, so pages slide in from the left edge.
Route<T> _slide<T>(Widget page) => PageRouteBuilder<T>(
      pageBuilder: (_, _, _) => page,
      transitionDuration: const Duration(milliseconds: 300),
      reverseTransitionDuration: const Duration(milliseconds: 240),
      transitionsBuilder: (context, animation, secondary, child) {
        final curved =
            CurvedAnimation(parent: animation, curve: Curves.easeOutCubic);
        return FadeTransition(
          opacity: curved,
          child: SlideTransition(
            position: Tween(begin: const Offset(-0.06, 0), end: Offset.zero)
                .animate(curved),
            child: child,
          ),
        );
      },
    );

Future<void> openWord(BuildContext context, int wordId) =>
    Navigator.of(context).push(_slide(WordDetailScreen(wordId: wordId)));

Future<void> openMindMap(BuildContext context, int wordId, String title) =>
    Navigator.of(context).push(_slide(MindMapScreen(wordId: wordId, title: title)));

Future<void> openCategory(BuildContext context, Category category) =>
    Navigator.of(context).push(_slide(CategoryWordsScreen(category: category)));

Future<void> openSpeechType(BuildContext context, SpeechPaneStat stat) =>
    Navigator.of(context).push(_slide(CategoryWordsScreen(speechType: stat)));

Future<void> openSettings(BuildContext context) =>
    Navigator.of(context).push(_slide(const SettingsScreen()));

Future<void> openLogin(BuildContext context) =>
    Navigator.of(context).push(_slide(const LoginScreen()));

/// Resolves to the saved word on success, or null if the user backed out.
Future<Word?> openWordEditor(BuildContext context, {Word? word}) =>
    Navigator.of(context).push<Word>(_slide(WordEditScreen(word: word)));
