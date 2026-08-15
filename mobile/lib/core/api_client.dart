import 'dart:async';
import 'dart:convert';
import 'dart:io';

import 'package:http/http.dart' as http;

/// Anything the UI is expected to render as a friendly failure. [kurdishMessage]
/// is what actually reaches the user; [message] keeps the technical detail for
/// the "details" expander on the error view.
class ApiException implements Exception {
  ApiException(this.message, {this.statusCode, String? kurdish})
      : kurdishMessage = kurdish ?? _kurdishFor(statusCode);

  final String message;
  final int? statusCode;
  final String kurdishMessage;

  bool get isUnauthorized => statusCode == 401 || statusCode == 403;
  bool get isNotFound => statusCode == 404;

  static String _kurdishFor(int? code) => switch (code) {
        401 => 'پێویستە بچیتە ژوورەوە',
        403 => 'دەسەڵاتت نییە بۆ ئەم کردارە',
        404 => 'هیچ نەدۆزرایەوە',
        null => 'پەیوەندی بە سێرڤەرەوە نەکرا',
        _ => 'هەڵەیەک ڕوویدا لە سێرڤەر',
      };

  @override
  String toString() => 'ApiException($statusCode): $message';
}

/// Thin JSON wrapper over `http`. Kept deliberately small — the repositories
/// own the endpoint knowledge, this only owns transport, auth headers and
/// turning failures into [ApiException].
class ApiClient {
  ApiClient({required this.baseUrl, this.token, http.Client? client})
      : _client = client ?? http.Client();

  final String baseUrl;
  final String? token;
  final http.Client _client;

  static const _timeout = Duration(seconds: 20);

  Uri _uri(String path, [Map<String, dynamic>? query]) {
    final base = baseUrl.endsWith('/')
        ? baseUrl.substring(0, baseUrl.length - 1)
        : baseUrl;
    final cleaned = <String, String>{};
    query?.forEach((k, v) {
      if (v == null) return;
      final s = '$v';
      if (s.isEmpty) return;
      cleaned[k] = s;
    });
    return Uri.parse('$base$path')
        .replace(queryParameters: cleaned.isEmpty ? null : cleaned);
  }

  Map<String, String> get _headers => {
        'Accept': 'application/json',
        'Content-Type': 'application/json; charset=utf-8',
        if (token != null && token!.isNotEmpty) 'Authorization': 'Bearer $token',
      };

  Future<dynamic> get(String path, {Map<String, dynamic>? query}) =>
      _send(() => _client.get(_uri(path, query), headers: _headers));

  Future<dynamic> post(String path, {Object? body}) => _send(
        () => _client.post(
          _uri(path),
          headers: _headers,
          body: body == null ? null : jsonEncode(body),
        ),
      );

  Future<dynamic> put(String path, {Object? body}) => _send(
        () => _client.put(
          _uri(path),
          headers: _headers,
          body: body == null ? null : jsonEncode(body),
        ),
      );

  Future<dynamic> delete(String path) =>
      _send(() => _client.delete(_uri(path), headers: _headers));

  Future<dynamic> _send(Future<http.Response> Function() run) async {
    late http.Response res;
    try {
      res = await run().timeout(_timeout);
    } on TimeoutException {
      throw ApiException(
        'Request timed out after ${_timeout.inSeconds}s',
        kurdish: 'سێرڤەر درەنگ وەڵامی دایەوە — دووبارە هەوڵ بدەوە',
      );
    } on SocketException catch (e) {
      throw ApiException(
        'Cannot reach $baseUrl — ${e.message}',
        kurdish: 'ناتوانرێت پەیوەندی بە سێرڤەرەوە بکرێت.\nناونیشانی API لە ڕێکخستنەکان بپشکنە.',
      );
    } on http.ClientException catch (e) {
      throw ApiException(
        'Cannot reach $baseUrl — ${e.message}',
        kurdish: 'ناتوانرێت پەیوەندی بە سێرڤەرەوە بکرێت.\nناونیشانی API لە ڕێکخستنەکان بپشکنە.',
      );
    }

    if (res.statusCode >= 200 && res.statusCode < 300) {
      if (res.bodyBytes.isEmpty) return null;
      // Sorani is multi-byte; decoding via `res.body` would use latin-1 whenever
      // the server omits a charset on the content type.
      return jsonDecode(utf8.decode(res.bodyBytes));
    }

    throw ApiException(
      _describe(res),
      statusCode: res.statusCode,
    );
  }

  /// ASP.NET returns either a ProblemDetails object or a bare string; surface
  /// whichever is present instead of a raw JSON blob.
  String _describe(http.Response res) {
    if (res.bodyBytes.isEmpty) return 'HTTP ${res.statusCode}';
    final raw = utf8.decode(res.bodyBytes, allowMalformed: true);
    try {
      final decoded = jsonDecode(raw);
      if (decoded is String) return decoded;
      if (decoded is Map) {
        for (final key in ['detail', 'title', 'message', 'error']) {
          final v = decoded[key];
          if (v is String && v.trim().isNotEmpty) return v;
        }
      }
    } catch (_) {
      // Not JSON — fall through to the raw body.
    }
    return raw.length > 300 ? '${raw.substring(0, 300)}…' : raw;
  }

  void close() => _client.close();
}
