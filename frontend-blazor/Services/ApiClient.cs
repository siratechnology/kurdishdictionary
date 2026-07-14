using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace frontend_blazor.Services;

/// <summary>
/// Hands out an <see cref="HttpClient"/> already carrying the signed-in user's bearer token.
///
/// The token lives as a claim inside the encrypted auth cookie, which means it is only reachable
/// through the current <see cref="AuthenticationState"/>. That rules out the usual
/// DelegatingHandler approach: IHttpClientFactory pools handlers across scopes, so a handler cannot
/// safely capture a per-circuit scoped service. Resolving the token per request instead keeps it
/// correct for every user on every circuit.
/// </summary>
public class ApiClient
{
    public const string ClientName = "api";

    /// <summary>Claim type the JWT is stored under in the auth cookie.</summary>
    public const string TokenClaim = "jwt";

    private readonly IHttpClientFactory _factory;
    private readonly AuthenticationStateProvider _auth;

    public ApiClient(IHttpClientFactory factory, AuthenticationStateProvider auth)
    {
        _factory = factory;
        _auth = auth;
    }

    public async Task<HttpClient> CreateAsync()
    {
        var http = _factory.CreateClient(ClientName);

        var state = await _auth.GetAuthenticationStateAsync();
        var token = state.User.FindFirst(TokenClaim)?.Value;

        if (!string.IsNullOrEmpty(token))
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return http;
    }

    /// <summary>An unauthenticated client — for the login call, which has no token yet.</summary>
    public HttpClient CreateAnonymous() => _factory.CreateClient(ClientName);
}

/// <summary>
/// Thrown when the API rejects a call as unauthorised. The layout catches it and bounces the user
/// to the login page, which is what should happen if their account was disabled mid-session.
/// </summary>
public class ApiUnauthorizedException : Exception
{
    public ApiUnauthorizedException() : base("Not authorised") { }
}

public static class HttpResponseExtensions
{
    /// <summary>
    /// Distinguishes "you are not allowed" from a generic failure, so the UI can say something
    /// useful instead of a dead 500 page.
    /// </summary>
    public static void EnsureAuthorizedAndSuccess(this HttpResponseMessage response)
    {
        if (response.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden)
            throw new ApiUnauthorizedException();

        response.EnsureSuccessStatusCode();
    }
}
