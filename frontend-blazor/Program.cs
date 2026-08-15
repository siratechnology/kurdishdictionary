using System.Security.Claims;
using frontend_blazor.Components;
using frontend_blazor.Services;
using frontend_blazor.Services.Presence;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(options =>
    {
        // A dropped connection (phone sleeping, laptop lid, flaky wifi) keeps its circuit warm for
        // three minutes, so reconnecting restores the page state instead of forcing a full reload.
        options.DisconnectedCircuitRetentionPeriod = TimeSpan.FromMinutes(3);
        options.DisconnectedCircuitMaxRetained = 200;
        options.JSInteropDefaultCallTimeout = TimeSpan.FromMinutes(1);
    })
    .AddHubOptions(options =>
    {
        // Ping often enough that a dead connection is noticed in seconds rather than after the
        // 30s default — the client reconnects that much sooner.
        options.KeepAliveInterval = TimeSpan.FromSeconds(10);
        options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
        options.HandshakeTimeout = TimeSpan.FromSeconds(15);
        // The word editor posts large payloads (meanings + relations) in a single interop call.
        options.MaximumReceiveMessageSize = 512 * 1024;
        // Let render batches pipeline instead of waiting for each ack — noticeably smoother typing
        // and grid updates on a high-latency link.
        options.MaximumParallelInvocationsPerClient = 4;
    });

// nginx terminates TLS and forwards plain HTTP, so without this every request looks insecure to the
// app: auth cookies never get the Secure flag and generated URLs come out as http://.
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    // The proxy's address is the docker bridge gateway, which is not stable across recreates, so the
    // default known-proxy allow-list would silently drop the headers. Only nginx can reach this port.
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

// Data Protection encrypts both the auth cookie and the antiforgery tokens. Left unconfigured the key
// ring lives inside the container and dies with it, so every redeploy signs everyone out and leaves
// them holding antiforgery cookies this app can no longer read. Keep the keys on a mounted volume.
var dataProtection = builder.Services
    .AddDataProtection()
    // Pinned so the ring survives a rename of the app/assembly.
    .SetApplicationName("kurdish-dictionary-admin");

// Only in a container. Locally the default per-user store already persists across runs, and pointing
// at an absolute "/keys" on Windows would strand the ring at the root of whatever drive we launched from.
var keysPath = builder.Configuration["DataProtectionKeysPath"]
    ?? (builder.Environment.IsDevelopment() ? null : "/keys");

if (!string.IsNullOrWhiteSpace(keysPath))
    dataProtection.PersistKeysToFileSystem(Directory.CreateDirectory(keysPath));

// A fixed name means a stale token can actually be deleted below; the default name carries a hash we
// would have to recompute to target it.
builder.Services.AddAntiforgery(options => options.Cookie.Name = ".kurdish_antiforgery");

var apiUrl = builder.Configuration["ApiUrl"] ?? "http://localhost:6000";
builder.Services.AddHttpClient(ApiClient.ClientName, client =>
{
    client.BaseAddress = new Uri(apiUrl);
});

// Telerik UI for Blazor — grids, dialogs, buttons and the notification host.
builder.Services.AddTelerikBlazor();

builder.Services.AddMemoryCache();
builder.Services.AddScoped<ApiClient>();
builder.Services.AddScoped<WordService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<AnalyticsService>();
builder.Services.AddScoped<ActivityService>();
builder.Services.AddScoped<WorkQueueService>();
builder.Services.AddScoped<StationService>();
builder.Services.AddScoped<RelationService>();
builder.Services.AddScoped<TaxonomyAdminService>();
builder.Services.AddScoped<Toast>();

// ── Presence (پڕۆمپت ٩) ────────────────────────────────────────────────────
// Singleton store: every circuit on this instance shares one view. Behind an interface because the
// in-memory implementation is wrong the moment this runs on two instances — swap in Redis there.
builder.Services.AddSingleton<IPresenceStore, InMemoryPresenceStore>();
builder.Services.AddScoped<CircuitHandler, PresenceCircuitHandler>();
builder.Services.AddScoped<PresenceService>();
builder.Services.AddScoped<PresenceApi>();
builder.Services.AddHostedService<PresenceFlushService>();
// Scoped: one hub connection per circuit, shared by every component on it.
builder.Services.AddScoped<ActivityStream>();

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

// Must run before anything that reads the scheme or the client IP.
app.UseForwardedHeaders();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found");
app.UseStaticFiles();

// ── Avatar proxy ───────────────────────────────────────────────────────────
// The API is NOT reachable from a browser. nginx publishes only this app and the public site;
// the backend sits on the internal docker network. So a profile picture stored by the API
// cannot be linked to directly, and this relays it under our own origin.
//
// The file name is checked against the exact shape AvatarService produces — 32 hex characters
// and ".jpg". That is not decoration: without it this becomes an open proxy into the API's
// file system, and "/avatars/../appsettings.json" is the first thing anyone would try.
app.MapGet("/avatars/{file}", async (string file, IHttpClientFactory factory, CancellationToken ct) =>
{
    if (!System.Text.RegularExpressions.Regex.IsMatch(file, @"^[a-f0-9]{32}\.jpg$"))
        return Results.NotFound();

    var http = factory.CreateClient(ApiClient.ClientName);

    var response = await http.GetAsync($"avatars/{file}", HttpCompletionOption.ResponseHeadersRead, ct);
    if (!response.IsSuccessStatusCode) return Results.NotFound();

    var stream = await response.Content.ReadAsStreamAsync(ct);

    // Names are content-addressed — a new upload gets a new name — so this can cache hard.
    return Results.Stream(stream, "image/jpeg", enableRangeProcessing: false);
}).AllowAnonymous();

// ── Order is load-bearing: authentication, then authorization, then antiforgery ──────────
// An antiforgery token embeds the identity of the user it was rendered for, and validation compares
// that against HttpContext.User. Validating before authentication runs means comparing a token minted
// for a signed-in user against an anonymous principal — every form POST then fails with a bare 400.
// Calling UseAuthentication() explicitly also stops WebApplication from auto-inserting it earlier, so
// getting this order wrong leaves nothing to populate the user in time.
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

// A stale token is a routine event — the sign-in page sat open past the cookie's week — and it must
// not dead-end on a blank 400. Drop the unreadable cookie and send the user back for a fresh pair.
app.Use(async (context, next) =>
{
    var antiforgery = context.Features.Get<IAntiforgeryValidationFeature>();

    if (antiforgery is { IsValid: false } && HttpMethods.IsPost(context.Request.Method))
    {
        context.Response.Cookies.Delete(".kurdish_antiforgery");
        context.Response.Redirect("/login?expired=1");
        return;
    }

    await next();
});

// Sign-out has to happen outside the interactive circuit: by the time a Blazor component is
// running, the response headers are long gone and the cookie can no longer be cleared.
// Presence has to end HERE, not when the circuit eventually closes.
//
// Blazor retains a disconnected circuit for three minutes so a flaky network can reconnect to
// it, which means OnCircuitClosedAsync can arrive minutes after someone pressed «دەرچوون» —
// and until then they were still being shown to their colleagues as working. Signing out is an
// explicit statement that they have finished, so it is recorded the moment it is made.
app.MapPost("/logout", async (HttpContext http, IPresenceStore presence) =>
{
    EndPresence(http, presence);
    await http.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return Results.Redirect("/login");
}).RequireAuthorization();

// After a password change the cookie still carries the old token and a stale "must change password"
// claim, so the user has to sign in again to pick up a fresh one.
app.MapGet("/logout-and-return", async (HttpContext http, IPresenceStore presence) =>
{
    EndPresence(http, presence);
    await http.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return Results.Redirect("/login");
}).RequireAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

// Top-level statements only allow local functions at the very end of the file.
static void EndPresence(HttpContext http, IPresenceStore presence)
{
    if (Guid.TryParse(http.User.FindFirstValue(ClaimTypes.NameIdentifier), out var id))
        presence.SignOut(id);
}
