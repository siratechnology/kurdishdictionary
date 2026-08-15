using System.Net.Http.Headers;
using System.Net.Http.Json;
using Shared.Dtos;

namespace frontend_blazor.Services;

public class AuthService
{
    private readonly ApiClient _api;

    public AuthService(ApiClient api) => _api = api;

    /// <summary>Verifies credentials against the backend. No token exists yet, so this call is anonymous.</summary>
    public async Task<AuthResultDto> LoginAsync(LoginDto dto)
    {
        try
        {
            var http = _api.CreateAnonymous();
            var response = await http.PostAsJsonAsync("api/auth/login", dto);

            if (!response.IsSuccessStatusCode)
                return new AuthResultDto { Succeeded = false, Error = "سەرکەوتوو نەبوو — هەوڵ بدەرەوە" };

            return await response.Content.ReadFromJsonAsync<AuthResultDto>()
                   ?? new AuthResultDto { Succeeded = false, Error = "وەڵامی نەناسراو" };
        }
        catch (HttpRequestException)
        {
            // The API being down should read as "the API is down", not "wrong password".
            return new AuthResultDto { Succeeded = false, Error = "ناتوانرێت پەیوەندی بە سێرڤەرەوە بکرێت" };
        }
    }

    /// <summary>Re-reads the user from the API — used to detect an account disabled mid-session.</summary>
    public async Task<UserDto?> GetCurrentAsync()
    {
        var http = await _api.CreateAsync();
        var response = await http.GetAsync("api/auth/me");
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<UserDto>();
    }

    public async Task<AuthResultDto> ChangePasswordAsync(ChangePasswordDto dto)
    {
        var http = await _api.CreateAsync();
        var response = await http.PostAsJsonAsync("api/auth/change-password", dto);
        response.EnsureAuthorizedAndSuccess();
        return await response.Content.ReadFromJsonAsync<AuthResultDto>()
               ?? new AuthResultDto { Succeeded = false };
    }

    /// <summary>
    /// Saves the signed-in user's own details. Roles and the active flag are not in
    /// <see cref="UpdateProfileDto"/> at all, so this call cannot carry a privilege change even
    /// if a caller tried — those stay on the admin-only user endpoint.
    /// </summary>
    public async Task<AuthResultDto> UpdateProfileAsync(UpdateProfileDto dto)
    {
        var http = await _api.CreateAsync();
        var response = await http.PutAsJsonAsync("api/auth/me", dto);
        response.EnsureAuthorizedAndSuccess();
        return await response.Content.ReadFromJsonAsync<AuthResultDto>()
               ?? new AuthResultDto { Succeeded = false };
    }

    /// <summary>
    /// Uploads a profile picture. The server validates by DECODING the bytes and re-encodes what
    /// it stores, so a rejection here is a real answer ("that is not an image", "too large") and
    /// nothing the browser claims about the file is trusted.
    /// </summary>
    public async Task<AuthResultDto> UploadAvatarAsync(byte[] content, string fileName)
    {
        var http = await _api.CreateAsync();

        using var form = AvatarForm(content, fileName);

        var response = await http.PostAsync("api/auth/me/avatar", form);
        response.EnsureAuthorizedAndSuccess();
        return await response.Content.ReadFromJsonAsync<AuthResultDto>()
               ?? new AuthResultDto { Succeeded = false };
    }

    public async Task<AuthResultDto> RemoveAvatarAsync()
    {
        var http = await _api.CreateAsync();
        var response = await http.DeleteAsync("api/auth/me/avatar");
        response.EnsureAuthorizedAndSuccess();
        return await response.Content.ReadFromJsonAsync<AuthResultDto>()
               ?? new AuthResultDto { Succeeded = false };
    }

    /// <summary>Admin-only: set another user's picture. Same validation as the self-service route.</summary>
    public async Task<AuthResultDto> UploadUserAvatarAsync(Guid userId, byte[] content, string fileName)
    {
        var http = await _api.CreateAsync();

        using var form = AvatarForm(content, fileName);

        var response = await http.PostAsync($"api/auth/users/{userId}/avatar", form);
        response.EnsureAuthorizedAndSuccess();
        return await response.Content.ReadFromJsonAsync<AuthResultDto>()
               ?? new AuthResultDto { Succeeded = false };
    }

    public async Task<AuthResultDto> RemoveUserAvatarAsync(Guid userId)
    {
        var http = await _api.CreateAsync();
        var response = await http.DeleteAsync($"api/auth/users/{userId}/avatar");
        response.EnsureAuthorizedAndSuccess();
        return await response.Content.ReadFromJsonAsync<AuthResultDto>()
               ?? new AuthResultDto { Succeeded = false };
    }

    /// <summary>
    /// The multipart body for either avatar route.
    ///
    /// The bytes arrive already in memory rather than as the browser's stream, and that is the
    /// point: a ByteArrayContent has a known length, so the request goes out with a Content-Length
    /// instead of chunked, and it can be sent a second time if the connection has to be retried.
    /// Handing HttpClient the live browser stream instead tied the upload to the API to the speed
    /// of the phone still sending it over the circuit, and a stall on that side truncated the body
    /// the API was reading — which the API could only report as a failure to parse the form.
    /// </summary>
    private static MultipartFormDataContent AvatarForm(byte[] content, string fileName)
    {
        var form = new MultipartFormDataContent();
        var file = new ByteArrayContent(content);

        // The server decodes the bytes to decide what this is, so the declared type is a
        // formality — but the multipart part needs one, and octet-stream is the honest answer
        // for something we have not inspected either.
        file.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

        // The name is sent because the multipart format wants one; the server discards it.
        form.Add(file, "file", fileName);

        return form;
    }

    /// <summary>Editor-only contributor ranking — readable by any signed-in user.</summary>
    public async Task<List<ContributorDto>> GetLeaderboardAsync()
    {
        var http = await _api.CreateAsync();
        var response = await http.GetAsync("api/auth/leaderboard");
        response.EnsureAuthorizedAndSuccess();
        return await response.Content.ReadFromJsonAsync<List<ContributorDto>>() ?? new();
    }

    public async Task<List<UserDto>> GetUsersAsync()
    {
        var http = await _api.CreateAsync();
        var response = await http.GetAsync("api/auth/users");
        response.EnsureAuthorizedAndSuccess();
        return await response.Content.ReadFromJsonAsync<List<UserDto>>() ?? new();
    }

    public async Task<List<RoleDto>> GetRolesAsync()
    {
        var http = await _api.CreateAsync();
        var response = await http.GetAsync("api/auth/roles");
        response.EnsureAuthorizedAndSuccess();
        return await response.Content.ReadFromJsonAsync<List<RoleDto>>() ?? new();
    }

    public async Task<AuthResultDto> CreateUserAsync(CreateUserDto dto)
    {
        var http = await _api.CreateAsync();
        var response = await http.PostAsJsonAsync("api/auth/users", dto);
        response.EnsureAuthorizedAndSuccess();
        return await response.Content.ReadFromJsonAsync<AuthResultDto>()
               ?? new AuthResultDto { Succeeded = false };
    }

    public async Task<AuthResultDto> UpdateUserAsync(Guid id, UpdateUserDto dto)
    {
        var http = await _api.CreateAsync();
        var response = await http.PutAsJsonAsync($"api/auth/users/{id}", dto);
        response.EnsureAuthorizedAndSuccess();
        return await response.Content.ReadFromJsonAsync<AuthResultDto>()
               ?? new AuthResultDto { Succeeded = false };
    }

    public async Task<AuthResultDto> DeleteUserAsync(Guid id)
    {
        var http = await _api.CreateAsync();
        var response = await http.DeleteAsync($"api/auth/users/{id}");
        response.EnsureAuthorizedAndSuccess();
        return await response.Content.ReadFromJsonAsync<AuthResultDto>()
               ?? new AuthResultDto { Succeeded = false };
    }
}
