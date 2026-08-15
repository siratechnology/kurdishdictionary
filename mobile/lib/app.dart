import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';

import 'core/config.dart';
import 'core/theme.dart';
import 'state/providers.dart';
import 'ui/shell.dart';
import 'ui/widgets/aurora_background.dart';

class KurdishDictionaryApp extends ConsumerWidget {
  const KurdishDictionaryApp({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final settings = ref.watch(settingsProvider);

    return MaterialApp(
      title: AppConfig.appName,
      debugShowCheckedModeBanner: false,
      themeMode: settings.themeMode,
      theme: buildAppTheme(AppTokens.light),
      darkTheme: buildAppTheme(AppTokens.dark),
      builder: (context, child) {
        final isDark = Theme.of(context).brightness == Brightness.dark;
        return AnnotatedRegion<SystemUiOverlayStyle>(
          // Edge-to-edge: the aurora runs under the status and nav bars.
          value: SystemUiOverlayStyle(
            statusBarColor: Colors.transparent,
            statusBarIconBrightness:
                isDark ? Brightness.light : Brightness.dark,
            systemNavigationBarColor: Colors.transparent,
            systemNavigationBarIconBrightness:
                isDark ? Brightness.light : Brightness.dark,
          ),
          child: Directionality(
            // Sorani Kurdish is written right-to-left; this is the app's
            // baseline, not a per-widget decision.
            textDirection: TextDirection.rtl,
            child: MediaQuery.withClampedTextScaling(
              // The in-app text-size slider composes with the OS setting, but
              // an extreme OS scale must not shatter the layout.
              minScaleFactor: settings.textScale * 0.9,
              maxScaleFactor: settings.textScale * 1.15,
              child: AuroraBackground(child: child ?? const SizedBox.shrink()),
            ),
          ),
        );
      },
      home: const AppShell(),
    );
  }
}
