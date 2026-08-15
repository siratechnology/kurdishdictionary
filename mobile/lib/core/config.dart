/// Where the app looks for the API, and how a word turns into a shareable link.
class AppConfig {
  /// The live dictionary. This is what a shipped build talks to — the server
  /// address is not something a normal user should ever have to think about,
  /// so it is the default and the settings UI for changing it is hidden behind
  /// the developer unlock (see [SettingsScreen]).
  static const productionApi = 'https://jinzar.krd';

  /// Public Next.js site — the target of a shared word link. It renders
  /// `/word/{id}` with an OG image, so a shared link unfurls nicely.
  static const productionSite = 'https://jinzar.krd';

  // ── Development targets ─────────────────────────────────────────────────
  // Only reachable through the hidden developer settings; never used unless
  // someone deliberately selects one.

  /// An Android emulator cannot see the host's `localhost` — 10.0.2.2 is the
  /// alias the emulator maps to the host loopback.
  static const emulatorApi = 'http://10.0.2.2:6000';

  /// LAN addresses of the dev machine as seen at build time.
  static const lanApi = 'http://192.168.1.14:6000';
  static const lanApiAlt = 'http://192.168.100.15:6000';

  /// Works on desktop, and on a USB phone after `adb reverse tcp:6000 tcp:6000`.
  static const desktopApi = 'http://localhost:6000';

  /// Probed in parallel by `ServerDiscovery`; first to answer wins. Production
  /// leads so a normal install settles on the live API even if a dev server
  /// happens to be reachable too.
  static const discoveryCandidates = [
    productionApi,
    desktopApi,
    emulatorApi,
    lanApi,
    lanApiAlt,
  ];

  static const defaultApiBase = productionApi;
  static const defaultShareBase = productionSite;

  /// One-tap options in the (hidden) developer server settings.
  static const suggestions = <String, String>{
    productionApi: 'ڕاستەقینە (ئینتەرنێت)',
    desktopApi: 'کۆمپیوتەر / USB',
    emulatorApi: 'ئێمولەیتەری ئەندرۆید',
    lanApi: 'مۆبایل (LAN 1)',
    lanApiAlt: 'مۆبایل (LAN 2)',
  };

  static const appName = 'فەرهەنگی کوردی';
  static const appTagline = 'فەرهەنگێکی زیندووی کوردی';
  static const sponsor = 'Sira Technology';
}
