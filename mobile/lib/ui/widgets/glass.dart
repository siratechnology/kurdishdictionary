import 'dart:ui';

import 'package:flutter/material.dart';

import '../../core/theme.dart';

AppTokens tokensOf(BuildContext context) =>
    Theme.of(context).extension<AppTokens>() ?? AppTokens.dark;

/// The `.glass-card` / `.glass-raised` utilities from `globals.css`: a
/// translucent fill over a real backdrop blur, a hairline border, and a soft
/// drop shadow.
///
/// `BackdropFilter` is not free — each one is a separate saveLayer — so cards
/// in long scrolling lists pass `blur: 0` and get the same look from the fill
/// alone, while the few chrome surfaces on screen at once keep the real blur.
class GlassCard extends StatelessWidget {
  const GlassCard({
    super.key,
    required this.child,
    this.padding = const EdgeInsets.all(16),
    this.margin,
    this.radius = 20,
    this.blur = 0,
    this.raised = false,
    this.borderColor,
    this.fill,
    this.onTap,
    this.onLongPress,
  });

  final Widget child;
  final EdgeInsetsGeometry padding;
  final EdgeInsetsGeometry? margin;
  final double radius;
  final double blur;
  final bool raised;
  final Color? borderColor;
  final Color? fill;
  final VoidCallback? onTap;
  final VoidCallback? onLongPress;

  @override
  Widget build(BuildContext context) {
    final t = tokensOf(context);
    final shape = BorderRadius.circular(radius);
    final background = fill ?? (raised ? t.surfaceRaised : t.surfaceCard);
    final border = borderColor ?? (raised ? t.borderStrong : t.border);

    Widget content = Padding(padding: padding, child: child);

    if (onTap != null || onLongPress != null) {
      content = Material(
        type: MaterialType.transparency,
        child: InkWell(
          onTap: onTap,
          onLongPress: onLongPress,
          borderRadius: shape,
          splashColor: t.accent.withValues(alpha: 0.10),
          highlightColor: t.accent.withValues(alpha: 0.06),
          child: content,
        ),
      );
    }

    Widget surface = DecoratedBox(
      decoration: BoxDecoration(
        color: background,
        borderRadius: shape,
        border: Border.all(color: border, width: 1),
        boxShadow: [
          BoxShadow(
            color: Colors.black.withValues(alpha: t.isDark ? 0.22 : 0.06),
            blurRadius: raised ? 34 : 18,
            offset: Offset(0, raised ? 10 : 5),
          ),
        ],
      ),
      child: content,
    );

    if (blur > 0) {
      surface = ClipRRect(
        borderRadius: shape,
        child: BackdropFilter(
          filter: ImageFilter.blur(sigmaX: blur, sigmaY: blur),
          child: surface,
        ),
      );
    } else {
      surface = ClipRRect(borderRadius: shape, child: surface);
    }

    return margin == null ? surface : Padding(padding: margin!, child: surface);
  }
}

/// A blurred bar for app-bar and bottom-nav backgrounds, where content really
/// does scroll underneath and the blur is the whole point.
class GlassBar extends StatelessWidget {
  const GlassBar({
    super.key,
    required this.child,
    this.blur = 22,
    this.border,
  });

  final Widget child;
  final double blur;
  final Border? border;

  @override
  Widget build(BuildContext context) {
    final t = tokensOf(context);
    return ClipRect(
      child: BackdropFilter(
        filter: ImageFilter.blur(sigmaX: blur, sigmaY: blur),
        child: DecoratedBox(
          decoration: BoxDecoration(
            color: t.background.withValues(alpha: t.isDark ? 0.72 : 0.68),
            border: border ??
                Border(bottom: BorderSide(color: t.borderSubtle, width: 1)),
          ),
          child: child,
        ),
      ),
    );
  }
}
