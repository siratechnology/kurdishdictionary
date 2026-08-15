import 'dart:async';

import 'package:http/http.dart' as http;

import 'config.dart';

/// Finds a reachable dictionary API without making the user type an IP address.
///
/// A phone on Wi-Fi has to reach the dev machine by its LAN address, an
/// emulator by 10.0.2.2, and a desktop build by localhost — and which LAN
/// address is right depends on which adapter the machine is using. Rather than
/// guess, probe them all at once and keep whichever answers first.
class ServerDiscovery {
  ServerDiscovery({http.Client? client}) : _client = client ?? http.Client();

  final http.Client _client;

  /// Short on purpose: an unreachable host on the same subnet fails fast, and
  /// waiting longer than this on a wrong candidate just delays the right one.
  static const _probeTimeout = Duration(milliseconds: 2500);

  /// Returns the first candidate that responds, or null if none do.
  Future<String?> discover({List<String>? candidates}) async {
    final list = candidates ?? AppConfig.discoveryCandidates;
    if (list.isEmpty) return null;

    final completer = Completer<String?>();
    var remaining = list.length;

    for (final base in list) {
      unawaited(probe(base).then((ok) {
        if (ok && !completer.isCompleted) {
          completer.complete(base);
        } else {
          remaining--;
          // Only report failure once every candidate has been ruled out.
          if (remaining <= 0 && !completer.isCompleted) {
            completer.complete(null);
          }
        }
      }));
    }

    return completer.future;
  }

  /// True if [base] serves the dictionary API. Hitting a real endpoint rather
  /// than just opening a socket means a random web server on the same port
  /// won't be mistaken for the backend.
  Future<bool> probe(String base) async {
    try {
      final uri = Uri.parse('$base/api/words?page=1&pageSize=1');
      final res = await _client.get(uri).timeout(_probeTimeout);
      return res.statusCode == 200 && res.body.contains('totalCount');
    } catch (_) {
      return false;
    }
  }

  void close() => _client.close();
}
