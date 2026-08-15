import 'package:flutter/material.dart';

import '../../core/theme.dart';
import 'glass.dart';

class SearchField extends StatefulWidget {
  const SearchField({
    super.key,
    required this.controller,
    required this.onChanged,
    required this.onSubmitted,
    this.hint = 'بگەڕێ بۆ وشەیەک…',
    this.autofocus = false,
    this.trailing,
  });

  final TextEditingController controller;
  final ValueChanged<String> onChanged;
  final ValueChanged<String> onSubmitted;
  final String hint;
  final bool autofocus;
  final Widget? trailing;

  @override
  State<SearchField> createState() => _SearchFieldState();
}

class _SearchFieldState extends State<SearchField> {
  final _focus = FocusNode();

  @override
  void initState() {
    super.initState();
    _focus.addListener(() => setState(() {}));
    widget.controller.addListener(_onTextChanged);
  }

  @override
  void dispose() {
    widget.controller.removeListener(_onTextChanged);
    _focus.dispose();
    super.dispose();
  }

  // Rebuilds only to show/hide the clear button.
  void _onTextChanged() => setState(() {});

  @override
  Widget build(BuildContext context) {
    final t = tokensOf(context);
    final focused = _focus.hasFocus;
    final hasText = widget.controller.text.isNotEmpty;

    return AnimatedContainer(
      duration: const Duration(milliseconds: 180),
      curve: Curves.easeOut,
      decoration: BoxDecoration(
        color: t.inputFill,
        borderRadius: BorderRadius.circular(18),
        border: Border.all(
          color: focused ? t.accent.withValues(alpha: 0.65) : t.border,
          width: focused ? 1.6 : 1,
        ),
        boxShadow: focused
            ? [BoxShadow(color: t.accentGlow, blurRadius: 22, spreadRadius: -4)]
            : null,
      ),
      child: Row(
        children: [
          const SizedBox(width: 14),
          Icon(
            Icons.search_rounded,
            size: 21,
            color: focused ? t.accentLight : t.text3,
          ),
          Expanded(
            child: TextField(
              controller: widget.controller,
              focusNode: _focus,
              autofocus: widget.autofocus,
              onChanged: widget.onChanged,
              onSubmitted: widget.onSubmitted,
              textInputAction: TextInputAction.search,
              textDirection: TextDirection.rtl,
              style: TextStyle(
                fontFamily: kFontFamily,
                fontSize: 16,
                color: t.text1,
                height: 1.4,
              ),
              decoration: InputDecoration(
                hintText: widget.hint,
                hintStyle: TextStyle(
                  fontFamily: kFontFamily,
                  fontSize: 15,
                  color: t.text4,
                ),
                filled: false,
                border: InputBorder.none,
                enabledBorder: InputBorder.none,
                focusedBorder: InputBorder.none,
                contentPadding:
                    const EdgeInsets.symmetric(horizontal: 10, vertical: 15),
                isDense: true,
              ),
            ),
          ),
          if (hasText)
            IconButton(
              icon: Icon(Icons.close_rounded, size: 19, color: t.text3),
              visualDensity: VisualDensity.compact,
              tooltip: 'سڕینەوە',
              onPressed: () {
                widget.controller.clear();
                widget.onChanged('');
              },
            ),
          if (widget.trailing != null) widget.trailing!,
          const SizedBox(width: 6),
        ],
      ),
    );
  }
}

/// Pins the search bar to the top of a [CustomScrollView] with a blurred
/// backdrop, so words scroll underneath it rather than pushing it away.
class PinnedSearchHeader extends SliverPersistentHeaderDelegate {
  PinnedSearchHeader({
    required this.child,
    required this.height,
    this.topPadding = 0,
  });

  final Widget child;
  final double height;
  final double topPadding;

  @override
  double get minExtent => height + topPadding;

  @override
  double get maxExtent => height + topPadding;

  @override
  Widget build(BuildContext context, double shrinkOffset, bool overlaps) =>
      GlassBar(
        border: Border(
          bottom: BorderSide(
            // The divider only earns its keep once content is behind the bar.
            color: shrinkOffset > 4
                ? tokensOf(context).borderSubtle
                : Colors.transparent,
          ),
        ),
        child: Padding(
          padding: EdgeInsets.only(top: topPadding),
          child: SizedBox(height: height, child: child),
        ),
      );

  @override
  bool shouldRebuild(PinnedSearchHeader old) =>
      old.height != height || old.topPadding != topPadding || old.child != child;
}
