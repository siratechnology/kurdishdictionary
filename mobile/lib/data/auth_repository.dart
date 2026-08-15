import '../core/api_client.dart';
import '../models/auth.dart';

class AuthRepository {
  AuthRepository(this._api);

  final ApiClient _api;

  /// The controller returns `AuthResultDto` with `succeeded: false` for bad
  /// credentials rather than a 401, so a failed login is a normal response we
  /// unwrap — only transport problems throw.
  Future<AuthResult> login(String userName, String password) async {
    final json = await _api.post('/api/auth/login', body: {
      'userName': userName.trim(),
      'password': password,
      'rememberMe': true,
    });
    return AuthResult.fromJson(
        json is Map<String, dynamic> ? json : <String, dynamic>{});
  }

  /// Validates a stored token against the server on launch — a token that has
  /// expired or been invalidated by a key rotation must not leave the app
  /// showing edit buttons that will 401 on tap.
  Future<AuthUser> me() async {
    final json = await _api.get('/api/auth/me');
    return AuthUser.fromJson(
        json is Map<String, dynamic> ? json : <String, dynamic>{});
  }
}
