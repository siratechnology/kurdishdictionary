import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../core/config.dart';
import '../../core/theme.dart';
import '../../state/providers.dart';
import '../widgets/glass.dart';

/// Signing in is optional — the whole dictionary is readable anonymously. It
/// only unlocks the Admin/Editor write endpoints.
class LoginScreen extends ConsumerStatefulWidget {
  const LoginScreen({super.key});

  @override
  ConsumerState<LoginScreen> createState() => _LoginScreenState();
}

class _LoginScreenState extends ConsumerState<LoginScreen> {
  final _userController = TextEditingController();
  final _passwordController = TextEditingController();
  final _formKey = GlobalKey<FormState>();

  bool _obscure = true;
  bool _busy = false;
  String? _error;

  @override
  void dispose() {
    _userController.dispose();
    _passwordController.dispose();
    super.dispose();
  }

  Future<void> _submit() async {
    if (!(_formKey.currentState?.validate() ?? false)) return;
    setState(() {
      _busy = true;
      _error = null;
    });

    final error = await ref.read(authProvider.notifier).signIn(
          _userController.text,
          _passwordController.text,
        );

    if (!mounted) return;
    setState(() {
      _busy = false;
      _error = error;
    });
    if (error == null) Navigator.of(context).pop();
  }

  @override
  Widget build(BuildContext context) {
    final t = tokensOf(context);

    return Scaffold(
      extendBodyBehindAppBar: true,
      appBar: AppBar(
        title: const Text('چوونە ژوورەوە'),
        flexibleSpace: const GlassBar(child: SizedBox.expand()),
      ),
      body: Center(
        child: SingleChildScrollView(
          padding: const EdgeInsets.all(24),
          physics: const BouncingScrollPhysics(),
          child: GlassCard(
            raised: true,
            blur: 20,
            padding: const EdgeInsets.fromLTRB(24, 30, 24, 26),
            child: Form(
              key: _formKey,
              child: Column(
                mainAxisSize: MainAxisSize.min,
                crossAxisAlignment: CrossAxisAlignment.stretch,
                children: [
                  Center(
                    child: Container(
                      width: 66,
                      height: 66,
                      decoration: BoxDecoration(
                        color: t.accent.withValues(alpha: 0.14),
                        shape: BoxShape.circle,
                        border:
                            Border.all(color: t.accent.withValues(alpha: 0.28)),
                      ),
                      child: Icon(Icons.lock_outline,
                          size: 28, color: t.accentLight),
                    ),
                  ),
                  const SizedBox(height: 18),
                  Center(
                    child: Text(
                      AppConfig.appName,
                      style: TextStyle(
                        fontFamily: kFontFamily,
                        fontSize: 20,
                        fontWeight: FontWeight.w700,
                        color: t.text1,
                      ),
                    ),
                  ),
                  const SizedBox(height: 6),
                  Center(
                    child: Text(
                      'بۆ دەستکاریکردنی وشەکان بچۆ ژوورەوە',
                      textAlign: TextAlign.center,
                      style: TextStyle(
                          fontFamily: kFontFamily,
                          fontSize: 13,
                          color: t.text3),
                    ),
                  ),
                  const SizedBox(height: 26),
                  TextFormField(
                    controller: _userController,
                    autocorrect: false,
                    textInputAction: TextInputAction.next,
                    decoration: const InputDecoration(
                      labelText: 'ناوی بەکارهێنەر',
                      prefixIcon: Icon(Icons.person_outline),
                    ),
                    validator: (v) => (v ?? '').trim().isEmpty
                        ? 'ناوی بەکارهێنەر پێویستە'
                        : null,
                  ),
                  const SizedBox(height: 14),
                  TextFormField(
                    controller: _passwordController,
                    obscureText: _obscure,
                    textInputAction: TextInputAction.done,
                    onFieldSubmitted: (_) => _submit(),
                    decoration: InputDecoration(
                      labelText: 'وشەی نهێنی',
                      prefixIcon: const Icon(Icons.key_outlined),
                      suffixIcon: IconButton(
                        icon: Icon(_obscure
                            ? Icons.visibility_outlined
                            : Icons.visibility_off_outlined),
                        onPressed: () => setState(() => _obscure = !_obscure),
                      ),
                    ),
                    validator: (v) =>
                        (v ?? '').isEmpty ? 'وشەی نهێنی پێویستە' : null,
                  ),
                  if (_error != null) ...[
                    const SizedBox(height: 16),
                    Container(
                      padding: const EdgeInsets.all(12),
                      decoration: BoxDecoration(
                        color: const Color(0xFFF87171).withValues(alpha: 0.10),
                        borderRadius: BorderRadius.circular(12),
                        border: Border.all(
                            color: const Color(0xFFF87171)
                                .withValues(alpha: 0.28)),
                      ),
                      child: Row(
                        children: [
                          const Icon(Icons.error_outline,
                              size: 18, color: Color(0xFFF87171)),
                          const SizedBox(width: 10),
                          Expanded(
                            child: Text(
                              _error!,
                              style: TextStyle(
                                fontFamily: kFontFamily,
                                fontSize: 13,
                                color: t.text1,
                                height: 1.5,
                              ),
                            ),
                          ),
                        ],
                      ),
                    ),
                  ],
                  const SizedBox(height: 22),
                  FilledButton(
                    onPressed: _busy ? null : _submit,
                    child: _busy
                        ? const SizedBox(
                            width: 20,
                            height: 20,
                            child: CircularProgressIndicator(
                                strokeWidth: 2.2, color: Colors.white),
                          )
                        : const Text('چوونە ژوورەوە'),
                  ),
                ],
              ),
            ),
          ),
        ),
      ),
    );
  }
}
