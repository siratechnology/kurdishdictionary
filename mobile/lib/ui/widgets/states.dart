import 'package:flutter/material.dart';

import '../../core/theme.dart';
import 'glass.dart';

class EmptyState extends StatelessWidget {
  const EmptyState({
    super.key,
    required this.icon,
    required this.title,
    this.message,
    this.action,
  });

  final IconData icon;
  final String title;
  final String? message;
  final Widget? action;

  @override
  Widget build(BuildContext context) {
    final t = tokensOf(context);
    return Center(
      child: Padding(
        padding: const EdgeInsets.symmetric(horizontal: 36, vertical: 40),
        child: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            Container(
              width: 84,
              height: 84,
              decoration: BoxDecoration(
                color: t.accent.withValues(alpha: 0.10),
                shape: BoxShape.circle,
                border: Border.all(color: t.accent.withValues(alpha: 0.18)),
              ),
              child: Icon(icon, size: 36, color: t.accent.withValues(alpha: 0.75)),
            ),
            const SizedBox(height: 20),
            Text(
              title,
              textAlign: TextAlign.center,
              style: TextStyle(
                fontFamily: kFontFamily,
                fontSize: 17,
                fontWeight: FontWeight.w600,
                color: t.text1,
              ),
            ),
            if (message != null) ...[
              const SizedBox(height: 8),
              Text(
                message!,
                textAlign: TextAlign.center,
                style: TextStyle(
                  fontFamily: kFontFamily,
                  fontSize: 14,
                  color: t.text3,
                  height: 1.7,
                ),
              ),
            ],
            if (action != null) ...[const SizedBox(height: 22), action!],
          ],
        ),
      ),
    );
  }
}

/// Failure state. [details] carries the technical message, hidden behind an
/// expander so a normal user sees only the Kurdish sentence.
class ErrorState extends StatelessWidget {
  const ErrorState({
    super.key,
    required this.message,
    this.details,
    this.onRetry,
    this.onOpenSettings,
  });

  final String message;
  final String? details;
  final VoidCallback? onRetry;
  final VoidCallback? onOpenSettings;

  @override
  Widget build(BuildContext context) {
    final t = tokensOf(context);
    return Center(
      child: SingleChildScrollView(
        padding: const EdgeInsets.symmetric(horizontal: 24, vertical: 40),
        child: GlassCard(
          padding: const EdgeInsets.all(22),
          child: Column(
            mainAxisSize: MainAxisSize.min,
            children: [
              Container(
                width: 64,
                height: 64,
                decoration: BoxDecoration(
                  color: const Color(0xFFF87171).withValues(alpha: 0.12),
                  shape: BoxShape.circle,
                ),
                child: const Icon(Icons.cloud_off_rounded,
                    size: 28, color: Color(0xFFF87171)),
              ),
              const SizedBox(height: 18),
              Text(
                message,
                textAlign: TextAlign.center,
                style: TextStyle(
                  fontFamily: kFontFamily,
                  fontSize: 15,
                  color: t.text1,
                  height: 1.75,
                ),
              ),
              const SizedBox(height: 20),
              Row(
                mainAxisAlignment: MainAxisAlignment.center,
                children: [
                  if (onRetry != null)
                    FilledButton.icon(
                      onPressed: onRetry,
                      icon: const Icon(Icons.refresh, size: 18),
                      label: const Text('دووبارە هەوڵ بدە'),
                    ),
                  if (onRetry != null && onOpenSettings != null)
                    const SizedBox(width: 10),
                  if (onOpenSettings != null)
                    TextButton.icon(
                      onPressed: onOpenSettings,
                      icon: const Icon(Icons.settings_outlined, size: 18),
                      label: const Text('ڕێکخستن'),
                    ),
                ],
              ),
              if (details != null && details!.trim().isNotEmpty) ...[
                const SizedBox(height: 6),
                Theme(
                  data: Theme.of(context)
                      .copyWith(dividerColor: Colors.transparent),
                  child: ExpansionTile(
                    tilePadding: EdgeInsets.zero,
                    childrenPadding: const EdgeInsets.only(bottom: 8),
                    title: Text(
                      'وردەکاری تەکنیکی',
                      style: TextStyle(
                        fontFamily: kFontFamily,
                        fontSize: 12.5,
                        color: t.text4,
                      ),
                    ),
                    children: [
                      SelectableText(
                        details!,
                        textDirection: TextDirection.ltr,
                        style: TextStyle(
                          fontFamily: 'monospace',
                          fontSize: 11.5,
                          color: t.text3,
                          height: 1.6,
                        ),
                      ),
                    ],
                  ),
                ),
              ],
            ],
          ),
        ),
      ),
    );
  }
}

/// The inline spinner shown at the tail of an infinite list while the next page
/// loads. Sized so appending it doesn't shift the rows above.
class FooterLoader extends StatelessWidget {
  const FooterLoader({super.key, this.label});

  final String? label;

  @override
  Widget build(BuildContext context) {
    final t = tokensOf(context);
    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 22),
      child: Column(
        children: [
          SizedBox(
            width: 22,
            height: 22,
            child: CircularProgressIndicator(strokeWidth: 2.2, color: t.accent),
          ),
          if (label != null) ...[
            const SizedBox(height: 10),
            Text(
              label!,
              style: TextStyle(
                  fontFamily: kFontFamily, fontSize: 12.5, color: t.text4),
            ),
          ],
        ],
      ),
    );
  }
}

/// The end-of-list marker — a quiet full stop so the feed has a bottom.
class EndOfFeed extends StatelessWidget {
  const EndOfFeed({super.key, required this.total});

  final int total;

  @override
  Widget build(BuildContext context) {
    final t = tokensOf(context);
    return Padding(
      padding: const EdgeInsets.fromLTRB(32, 22, 32, 34),
      child: Row(
        children: [
          Expanded(child: Divider(color: t.borderSubtle)),
          Padding(
            padding: const EdgeInsets.symmetric(horizontal: 12),
            child: Text(
              total > 0 ? '$total وشە' : 'کۆتایی',
              style: TextStyle(
                  fontFamily: kFontFamily, fontSize: 12, color: t.text4),
            ),
          ),
          Expanded(child: Divider(color: t.borderSubtle)),
        ],
      ),
    );
  }
}
