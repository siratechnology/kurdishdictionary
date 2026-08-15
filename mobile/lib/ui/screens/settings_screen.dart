import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../core/api_client.dart';
import '../../core/config.dart';
import '../../core/theme.dart';
import '../../state/providers.dart';
import '../navigation.dart';
import '../widgets/glass.dart';

class SettingsScreen extends ConsumerStatefulWidget {
  const SettingsScreen({super.key});

  @override
  ConsumerState<SettingsScreen> createState() => _SettingsScreenState();
}

class _SettingsScreenState extends ConsumerState<SettingsScreen> {
  late final TextEditingController _apiController;
  late final TextEditingController _shareController;

  /// null = not tested yet.
  bool? _connectionOk;
  String? _connectionDetail;
  bool _testing = false;

  /// The server address is a development concern, not a user-facing setting —
  /// a shipped build points at the live API and nobody should be typing URLs
  /// into a dictionary. The section stays reachable for debugging by tapping
  /// the version line at the bottom of this screen [_unlockTaps] times.
  bool _devUnlocked = false;
  int _versionTaps = 0;
  static const _unlockTaps = 7;

  void _tapVersion() {
    if (_devUnlocked) return;
    setState(() {
      _versionTaps++;
      if (_versionTaps >= _unlockTaps) {
        _devUnlocked = true;
        ScaffoldMessenger.of(context)
          ..clearSnackBars()
          ..showSnackBar(const SnackBar(
            content: Text('ڕێکخستنی گەشەپێدەر کرایەوە'),
            duration: Duration(milliseconds: 1600),
          ));
      }
    });
  }

  @override
  void initState() {
    super.initState();
    final settings = ref.read(settingsProvider);
    _apiController = TextEditingController(text: settings.apiBase);
    _shareController = TextEditingController(text: settings.shareBase);
  }

  @override
  void dispose() {
    _apiController.dispose();
    _shareController.dispose();
    super.dispose();
  }

  Future<void> _saveApi(String value) async {
    await ref.read(settingsProvider.notifier).setApiBase(value);
    if (!mounted) return;
    _apiController.text = ref.read(settingsProvider).apiBase;
    setState(() {
      _connectionOk = null;
      _connectionDetail = null;
    });
    // The address changed, so every cached list is now from the wrong server.
    ref.invalidate(categoriesProvider);
    ref.invalidate(speechStatsProvider);
  }

  /// Probes every known address in parallel and adopts whichever replies — the
  /// user shouldn't have to know whether their phone reaches the dev machine at
  /// 192.168.1.x or 192.168.100.x.
  Future<void> _autoDetect() async {
    setState(() {
      _testing = true;
      _connectionOk = null;
      _connectionDetail = null;
    });

    final found = await ref.read(settingsProvider.notifier).autoDetectApi();

    if (!mounted) return;
    if (found == null) {
      setState(() {
        _testing = false;
        _connectionOk = false;
        _connectionDetail =
            'هیچ سێرڤەرێک نەدۆزرایەوە. دڵنیابە کە مۆبایل و کۆمپیوتەر لەسەر هەمان Wi-Fi ن.';
      });
      return;
    }

    _apiController.text = found;
    ref.invalidate(categoriesProvider);
    ref.invalidate(speechStatsProvider);
    setState(() => _testing = false);
    await _testConnection();
  }

  Future<void> _testConnection() async {
    setState(() {
      _testing = true;
      _connectionOk = null;
      _connectionDetail = null;
    });
    try {
      final words = await ref
          .read(wordsRepositoryProvider)
          .search(page: 1, pageSize: 1);
      if (!mounted) return;
      setState(() {
        _connectionOk = true;
        _connectionDetail = '${words.totalCount} وشە لە سێرڤەرەکەدا هەیە';
      });
    } on ApiException catch (e) {
      if (!mounted) return;
      setState(() {
        _connectionOk = false;
        _connectionDetail = e.message;
      });
    } finally {
      if (mounted) setState(() => _testing = false);
    }
  }

  @override
  Widget build(BuildContext context) {
    final t = tokensOf(context);
    final settings = ref.watch(settingsProvider);
    final auth = ref.watch(authProvider);

    return Scaffold(
      extendBodyBehindAppBar: true,
      appBar: AppBar(
        title: const Text('ڕێکخستن'),
        flexibleSpace: const GlassBar(child: SizedBox.expand()),
      ),
      body: ListView(
        padding: EdgeInsets.fromLTRB(
            16, MediaQuery.paddingOf(context).top + kToolbarHeight + 16, 16, 60),
        physics: const BouncingScrollPhysics(),
        children: [
          _Group(
            icon: Icons.person_outline,
            title: 'هەژمار',
            children: [
              if (auth.isSignedIn)
                _AccountTile(
                  name: auth.user!.displayName,
                  roles: auth.user!.roles,
                  onSignOut: () => ref.read(authProvider.notifier).signOut(),
                )
              else
                ListTile(
                  contentPadding: EdgeInsets.zero,
                  leading: Icon(Icons.login, color: t.accentLight),
                  title: const Text('چوونە ژوورەوە'),
                  subtitle: Text(
                    'بۆ زیادکردن و دەستکاریکردنی وشەکان',
                    style: TextStyle(
                        fontFamily: kFontFamily, fontSize: 12.5, color: t.text3),
                  ),
                  trailing: Icon(Icons.chevron_left, color: t.text4),
                  onTap: () => openLogin(context),
                ),
            ],
          ),
          _Group(
            icon: Icons.palette_outlined,
            title: 'ڕووکار',
            children: [
              _Label('ڕەنگی ڕووکار'),
              const SizedBox(height: 8),
              SegmentedButton<ThemeMode>(
                segments: const [
                  ButtonSegment(
                    value: ThemeMode.dark,
                    icon: Icon(Icons.dark_mode_outlined, size: 17),
                    label: Text('شەو'),
                  ),
                  ButtonSegment(
                    value: ThemeMode.light,
                    icon: Icon(Icons.light_mode_outlined, size: 17),
                    label: Text('ڕۆژ'),
                  ),
                  ButtonSegment(
                    value: ThemeMode.system,
                    icon: Icon(Icons.smartphone, size: 17),
                    label: Text('سیستەم'),
                  ),
                ],
                selected: {settings.themeMode},
                showSelectedIcon: false,
                onSelectionChanged: (s) =>
                    ref.read(settingsProvider.notifier).setThemeMode(s.first),
              ),
              const SizedBox(height: 20),
              _Label('قەبارەی نووسین'),
              Row(
                children: [
                  Text('ک',
                      style: TextStyle(fontFamily: kFontFamily, fontSize: 13)),
                  Expanded(
                    child: Slider(
                      value: settings.textScale,
                      min: 0.85,
                      max: 1.35,
                      divisions: 10,
                      label: '${(settings.textScale * 100).round()}%',
                      onChanged: (v) =>
                          ref.read(settingsProvider.notifier).setTextScale(v),
                    ),
                  ),
                  Text('گ',
                      style: TextStyle(fontFamily: kFontFamily, fontSize: 21)),
                ],
              ),
            ],
          ),
          if (_devUnlocked)
          _Group(
            icon: Icons.dns_outlined,
            title: 'سێرڤەر (گەشەپێدەر)',
            children: [
              _Label('ناونیشانی API'),
              const SizedBox(height: 8),
              TextField(
                controller: _apiController,
                textDirection: TextDirection.ltr,
                keyboardType: TextInputType.url,
                autocorrect: false,
                decoration: const InputDecoration(
                    hintText: AppConfig.productionApi, isDense: true),
                onSubmitted: _saveApi,
              ),
              const SizedBox(height: 10),
              Wrap(
                spacing: 8,
                runSpacing: 8,
                children: [
                  for (final entry in AppConfig.suggestions.entries)
                    ActionChip(
                      label: Text(entry.value,
                          style: const TextStyle(
                              fontFamily: kFontFamily, fontSize: 12)),
                      onPressed: () {
                        _apiController.text = entry.key;
                        _saveApi(entry.key);
                      },
                    ),
                ],
              ),
              const SizedBox(height: 14),
              Wrap(
                spacing: 10,
                runSpacing: 10,
                children: [
                  FilledButton.icon(
                    onPressed: _testing
                        ? null
                        : () async {
                            await _saveApi(_apiController.text);
                            await _testConnection();
                          },
                    icon: _testing
                        ? const SizedBox(
                            width: 16,
                            height: 16,
                            child: CircularProgressIndicator(
                                strokeWidth: 2, color: Colors.white),
                          )
                        : const Icon(Icons.wifi_tethering, size: 18),
                    label: const Text('پاشەکەوت و تاقیکردنەوە'),
                  ),
                  OutlinedButton.icon(
                    onPressed: _testing ? null : _autoDetect,
                    icon: const Icon(Icons.radar, size: 18),
                    label: const Text('دۆزینەوەی خۆکار'),
                  ),
                ],
              ),
              if (_connectionOk != null) ...[
                const SizedBox(height: 12),
                _ConnectionResult(
                  ok: _connectionOk!,
                  detail: _connectionDetail,
                ),
              ],
              const SizedBox(height: 20),
              _Label('ناونیشانی ماڵپەڕ (بۆ هاوبەشکردن)'),
              const SizedBox(height: 8),
              TextField(
                controller: _shareController,
                textDirection: TextDirection.ltr,
                keyboardType: TextInputType.url,
                autocorrect: false,
                decoration: const InputDecoration(
                    hintText: AppConfig.productionSite, isDense: true),
                onSubmitted: (v) =>
                    ref.read(settingsProvider.notifier).setShareBase(v),
              ),
              const SizedBox(height: 6),
              Text(
                'بەستەری هاوبەشکراو لەم ناونیشانەوە دروست دەکرێت.',
                style: TextStyle(
                    fontFamily: kFontFamily, fontSize: 11.5, color: t.text4),
              ),
            ],
          ),
          _Group(
            icon: Icons.cleaning_services_outlined,
            title: 'داتای مۆبایل',
            children: [
              ListTile(
                contentPadding: EdgeInsets.zero,
                leading: Icon(Icons.history, color: t.text3),
                title: const Text('سڕینەوەی مێژووی بینین'),
                onTap: () => ref.read(historyProvider.notifier).clear(),
              ),
              ListTile(
                contentPadding: EdgeInsets.zero,
                leading: Icon(Icons.search_off, color: t.text3),
                title: const Text('سڕینەوەی گەڕانە دواییەکان'),
                onTap: () => ref.read(recentSearchesProvider.notifier).clear(),
              ),
            ],
          ),
          const SizedBox(height: 24),
          Center(
            child: GestureDetector(
              // Seven taps here reveal the server section — see [_tapVersion].
              behavior: HitTestBehavior.opaque,
              onTap: _tapVersion,
              child: Column(
                children: [
                  Text(
                    AppConfig.appName,
                    style: TextStyle(
                      fontFamily: kFontFamily,
                      fontSize: 15,
                      fontWeight: FontWeight.w600,
                      color: t.text2,
                    ),
                  ),
                  const SizedBox(height: 4),
                  Text(
                    'v1.0.0 · ${AppConfig.sponsor}',
                    style: TextStyle(
                        fontFamily: kFontFamily, fontSize: 12, color: t.text4),
                  ),
                ],
              ),
            ),
          ),
        ],
      ),
    );
  }
}

class _Group extends StatelessWidget {
  const _Group({
    required this.icon,
    required this.title,
    required this.children,
  });

  final IconData icon;
  final String title;
  final List<Widget> children;

  @override
  Widget build(BuildContext context) {
    final t = tokensOf(context);
    return Padding(
      padding: const EdgeInsets.only(bottom: 16),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Padding(
            padding: const EdgeInsets.fromLTRB(4, 4, 4, 10),
            child: Row(
              children: [
                Icon(icon, size: 17, color: t.accentLight),
                const SizedBox(width: 8),
                Text(
                  title,
                  style: TextStyle(
                    fontFamily: kFontFamily,
                    fontSize: 15,
                    fontWeight: FontWeight.w600,
                    color: t.text2,
                  ),
                ),
              ],
            ),
          ),
          GlassCard(
            padding: const EdgeInsets.fromLTRB(16, 14, 16, 16),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.stretch,
              children: children,
            ),
          ),
        ],
      ),
    );
  }
}

class _Label extends StatelessWidget {
  const _Label(this.text);

  final String text;

  @override
  Widget build(BuildContext context) => Text(
        text,
        style: TextStyle(
          fontFamily: kFontFamily,
          fontSize: 13,
          fontWeight: FontWeight.w600,
          color: tokensOf(context).text3,
        ),
      );
}

class _ConnectionResult extends StatelessWidget {
  const _ConnectionResult({required this.ok, required this.detail});

  final bool ok;
  final String? detail;

  @override
  Widget build(BuildContext context) {
    final color = ok ? const Color(0xFF22D36D) : const Color(0xFFF87171);
    return Container(
      padding: const EdgeInsets.all(12),
      decoration: BoxDecoration(
        color: color.withValues(alpha: 0.10),
        borderRadius: BorderRadius.circular(12),
        border: Border.all(color: color.withValues(alpha: 0.28)),
      ),
      child: Row(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Icon(ok ? Icons.check_circle_outline : Icons.error_outline,
              size: 18, color: color),
          const SizedBox(width: 10),
          Expanded(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(
                  ok ? 'پەیوەندی سەرکەوتوو بوو' : 'پەیوەندی نەکرا',
                  style: TextStyle(
                    fontFamily: kFontFamily,
                    fontSize: 13.5,
                    fontWeight: FontWeight.w600,
                    color: color,
                  ),
                ),
                if (detail != null && detail!.isNotEmpty)
                  Padding(
                    padding: const EdgeInsets.only(top: 4),
                    child: Text(
                      detail!,
                      style: TextStyle(
                        fontFamily: kFontFamily,
                        fontSize: 11.5,
                        color: tokensOf(context).text3,
                        height: 1.5,
                      ),
                    ),
                  ),
              ],
            ),
          ),
        ],
      ),
    );
  }
}

class _AccountTile extends StatelessWidget {
  const _AccountTile({
    required this.name,
    required this.roles,
    required this.onSignOut,
  });

  final String name;
  final List<String> roles;
  final VoidCallback onSignOut;

  @override
  Widget build(BuildContext context) {
    final t = tokensOf(context);
    return Row(
      children: [
        CircleAvatar(
          radius: 22,
          backgroundColor: t.accent.withValues(alpha: 0.18),
          child: Text(
            name.isNotEmpty ? name.characters.first : '?',
            style: TextStyle(
              fontFamily: kFontFamily,
              fontSize: 18,
              fontWeight: FontWeight.w700,
              color: t.accentLight,
            ),
          ),
        ),
        const SizedBox(width: 14),
        Expanded(
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Text(
                name,
                style: TextStyle(
                  fontFamily: kFontFamily,
                  fontSize: 16,
                  fontWeight: FontWeight.w600,
                  color: t.text1,
                ),
              ),
              Text(
                roles.isEmpty ? 'بەکارهێنەر' : roles.join(' · '),
                style: TextStyle(
                    fontFamily: kFontFamily, fontSize: 12.5, color: t.text3),
              ),
            ],
          ),
        ),
        TextButton(onPressed: onSignOut, child: const Text('دەرچوون')),
      ],
    );
  }
}
