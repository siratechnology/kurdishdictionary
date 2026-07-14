using System.Security.Claims;
using frontend_blazor.Components;
using frontend_blazor.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var apiUrl = builder.Configuration["ApiUrl"] ?? "http://localhost:6000";
builder.Services.AddHttpClient(ApiClient.ClientName, client =>
{
    client.BaseAddress = new Uri(apiUrl);
});

builder.Services.AddScoped<ApiClient>();
builder.Services.AddScoped<WordService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<AnalyticsService>();
builder.Services.AddScoped<ActivityService>();

// ── Authentication ────────────────────────────────────────────────────────
// The cookie is the session; the JWT it carries is what the API checks. Both last a week, so a
// signed-in editor is not asked for a password again for seven days.
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "kurdish_auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;

        options.LoginPath = "/login";
        options.LogoutPath = "/logout";
        options.AccessDeniedPath = "/denied";

        options.ExpireTimeSpan = TimeSpan.FromDays(7);

        // Sliding: an editor who uses the dashboard daily never gets logged out, while an
        // abandoned session still dies a week after it was last touched.
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found");
app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

// Sign-out has to happen outside the interactive circuit: by the time a Blazor component is
// running, the response headers are long gone and the cookie can no longer be cleared.
app.MapPost("/logout", async (HttpContext http) =>
{
    await http.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return Results.Redirect("/login");
}).RequireAuthorization();

// After a password change the cookie still carries the old token and a stale "must change password"
// claim, so the user has to sign in again to pick up a fresh one.
app.MapGet("/logout-and-return", async (HttpContext http) =>
{
    await http.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return Results.Redirect("/login");
}).RequireAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
