import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../core/theme.dart';
import '../state/providers.dart';
import 'navigation.dart';
import 'screens/browse_screen.dart';
import 'screens/saved_screen.dart';
import 'screens/search_screen.dart';
import 'widgets/glass.dart';

/// The three top-level destinations, kept alive in an [IndexedStack] so
/// switching tabs never loses your place in a feed you've scrolled far into.
class AppShell extends ConsumerStatefulWidget {
  const AppShell({super.key});

  @override
  ConsumerState<AppShell> createState() => _AppShellState();
}

class _AppShellState extends ConsumerState<AppShell> {
  int _index = 0;

  static const _destinations = [
    (icon: Icons.search_rounded, label: 'گەڕان'),
    (icon: Icons.grid_view_rounded, label: 'پۆلەکان'),
    (icon: Icons.bookmark_border_rounded, label: 'پاشەکەوت'),
  ];

  void _select(int index) {
    if (index == _index) return;
    HapticFeedback.selectionClick();
    setState(() => _index = index);
  }

  @override
  Widget build(BuildContext context) {
    final canEdit = ref.watch(authProvider.select((a) => a.canEdit));

    return Scaffold(
      extendBody: true,
      body: IndexedStack(
        index: _index,
        children: const [
          SearchScreen(),
          BrowseScreen(),
          SavedScreen(),
        ],
      ),
      floatingActionButton: canEdit && _index == 0
          ? FloatingActionButton(
              onPressed: () => openWordEditor(context),
              tooltip: 'وشەی نوێ',
              child: const Icon(Icons.add),
            )
          : null,
      bottomNavigationBar: _BottomBar(
        index: _index,
        destinations: _destinations,
        onSelected: _select,
        onSettings: () => openSettings(context),
      ),
    );
  }
}

class _BottomBar extends StatelessWidget {
  const _BottomBar({
    required this.index,
    required this.destinations,
    required this.onSelected,
    required this.onSettings,
  });

  final int index;
  final List<({IconData icon, String label})> destinations;
  final ValueChanged<int> onSelected;
  final VoidCallback onSettings;

  @override
  Widget build(BuildContext context) {
    final t = tokensOf(context);

    return GlassBar(
      border: Border(top: BorderSide(color: t.borderSubtle)),
      child: SafeArea(
        top: false,
        child: SizedBox(
          height: 62,
          child: Row(
            children: [
              for (var i = 0; i < destinations.length; i++)
                Expanded(
                  child: _NavItem(
                    icon: destinations[i].icon,
                    label: destinations[i].label,
                    selected: index == i,
                    onTap: () => onSelected(i),
                  ),
                ),
              SizedBox(
                width: 62,
                child: _NavItem(
                  icon: Icons.settings_outlined,
                  label: 'ڕێکخستن',
                  selected: false,
                  onTap: onSettings,
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}

class _NavItem extends StatelessWidget {
  const _NavItem({
    required this.icon,
    required this.label,
    required this.selected,
    required this.onTap,
  });

  final IconData icon;
  final String label;
  final bool selected;
  final VoidCallback onTap;

  @override
  Widget build(BuildContext context) {
    final t = tokensOf(context);
    final color = selected ? t.accentLight : t.text4;

    return InkWell(
      onTap: onTap,
      borderRadius: BorderRadius.circular(16),
      child: Column(
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          // The pill grows behind the active icon instead of the icon jumping.
          AnimatedContainer(
            duration: const Duration(milliseconds: 220),
            curve: Curves.easeOut,
            padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 4),
            decoration: BoxDecoration(
              color: selected
                  ? t.accent.withValues(alpha: 0.16)
                  : Colors.transparent,
              borderRadius: BorderRadius.circular(999),
            ),
            child: Icon(icon, size: 21, color: color),
          ),
          const SizedBox(height: 3),
          Text(
            label,
            style: TextStyle(
              fontFamily: kFontFamily,
              fontSize: 10.5,
              fontWeight: selected ? FontWeight.w600 : FontWeight.w400,
              color: color,
            ),
          ),
        ],
      ),
    );
  }
}
