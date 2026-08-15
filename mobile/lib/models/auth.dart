/// Mirrors the auth slice of `shared/Dtos/AuthDtos.cs`.
library;

class AuthUser {
  const AuthUser({
    required this.id,
    required this.userName,
    this.email,
    this.fullName,
    this.roles = const [],
    this.mustChangePassword = false,
    this.wordCount = 0,
  });

  final String id;
  final String userName;
  final String? email;
  final String? fullName;
  final List<String> roles;
  final bool mustChangePassword;
  final int wordCount;

  /// The API gates writes on these two roles (`Roles.AdminOrEditor`), so the UI
  /// hides the edit affordances for anyone else rather than letting them tap
  /// into a guaranteed 403.
  bool get canEdit =>
      roles.any((r) => r == 'Admin' || r == 'Editor');

  bool get isAdmin => roles.contains('Admin');

  String get displayName =>
      (fullName != null && fullName!.trim().isNotEmpty) ? fullName! : userName;

  factory AuthUser.fromJson(Map<String, dynamic> j) => AuthUser(
        id: '${j['id'] ?? ''}',
        userName: j['userName'] as String? ?? '',
        email: j['email'] as String?,
        fullName: j['fullName'] as String?,
        roles: (j['roles'] as List? ?? const []).map((e) => '$e').toList(),
        mustChangePassword: j['mustChangePassword'] as bool? ?? false,
        wordCount: j['wordCount'] is int ? j['wordCount'] as int : 0,
      );

  Map<String, dynamic> toJson() => {
        'id': id,
        'userName': userName,
        'email': email,
        'fullName': fullName,
        'roles': roles,
        'mustChangePassword': mustChangePassword,
        'wordCount': wordCount,
      };
}

class AuthResult {
  const AuthResult({
    required this.succeeded,
    this.error,
    this.token,
    this.expiresAt,
    this.user,
  });

  final bool succeeded;
  final String? error;
  final String? token;
  final DateTime? expiresAt;
  final AuthUser? user;

  factory AuthResult.fromJson(Map<String, dynamic> j) => AuthResult(
        succeeded: j['succeeded'] as bool? ?? false,
        error: j['error'] as String?,
        token: j['token'] as String?,
        expiresAt: DateTime.tryParse(j['expiresAt'] as String? ?? ''),
        user: j['user'] == null
            ? null
            : AuthUser.fromJson(j['user'] as Map<String, dynamic>),
      );
}
