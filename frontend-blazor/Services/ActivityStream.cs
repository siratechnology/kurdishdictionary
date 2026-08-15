using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using Shared.Dtos;

namespace frontend_blazor.Services;

/// <summary>
/// Live feed of content changes, pushed from the backend's ActivityHub.
///
/// Scoped, so there is exactly one hub connection per circuit no matter how many components listen —
/// the bell, the dashboard and the words grid all share this one. The connection carries the signed-in
/// user's JWT, which is why it cannot be a singleton: the hub authorises per user.
/// </summary>
public sealed class ActivityStream : IAsyncDisposable
{
    /// <summary>Must match <c>ActivityHub.ActivityEvent</c> in the backend.</summary>
    private const string ActivityEvent = "ActivityHappened";

    /// <summary>Must match <c>ActivityHub.TaxonomyEvent</c> in the backend.</summary>
    private const string TaxonomyEvent = "TaxonomyChanged";

    private readonly IConfiguration _config;
    private readonly AuthenticationStateProvider _auth;
    private readonly ILogger<ActivityStream> _log;

    private HubConnection? _connection;
    private Task<bool>? _starting;

    public ActivityStream(IConfiguration config, AuthenticationStateProvider auth, ILogger<ActivityStream> log)
    {
        _config = config;
        _auth = auth;
        _log = log;
    }

    /// <summary>
    /// Raised for every batch of audit rows the backend commits.
    /// <para>
    /// Handlers run on a thread-pool thread, not the circuit's renderer — a component must therefore
    /// marshal back with <c>InvokeAsync</c> before touching state or calling <c>StateHasChanged</c>.
    /// </para>
    /// </summary>
    public event Func<IReadOnlyList<AuditLogDto>, Task>? Received;

    /// <summary>
    /// Raised after the connection comes back from a drop. Anything pushed while it was down was
    /// missed outright — SignalR does not replay — so listeners use this to re-pull and catch up.
    /// </summary>
    public event Func<Task>? Reconnected;

    /// <summary>
    /// Raised when an admin changes the taxonomy — a group added, renamed, re-parented, retired.
    ///
    /// Deliberately its own event rather than part of <see cref="Received"/>. A content change
    /// refreshes a list; a taxonomy change re-shapes the form somebody may be typing into, and
    /// that has to be handled without ever discarding their unsaved input. Merging the two would
    /// make it impossible to treat them differently, and the safe treatment is the whole point.
    /// <para>
    /// Handlers run on a thread-pool thread. Marshal back with <c>InvokeAsync</c> before touching
    /// component state.
    /// </para>
    /// </summary>
    public event Func<TaxonomyChangedDto, Task>? TaxonomyChanged;

    /// <summary>The latest taxonomy version this circuit has been told about. 0 until the first push.</summary>
    public long TaxonomyVersion { get; private set; }

    public bool IsConnected => _connection?.State == HubConnectionState.Connected;

    /// <summary>
    /// Connects on first call and is a no-op afterwards, so every component can call it from
    /// OnInitializedAsync without coordinating. Returns false if no connection could be made.
    /// </summary>
    public Task<bool> StartAsync() => _starting ??= ConnectAsync();

    private async Task<bool> ConnectAsync()
    {
        var state = await _auth.GetAuthenticationStateAsync();
        var token = state.User.FindFirst(ApiClient.TokenClaim)?.Value;

        // Anonymous circuits (the login page) have nothing to listen with, and the hub would
        // reject them anyway.
        if (string.IsNullOrEmpty(token)) return false;

        var apiUrl = _config["ApiUrl"] ?? "http://localhost:6000";

        _connection = new HubConnectionBuilder()
            .WithUrl($"{apiUrl.TrimEnd('/')}/hubs/activity", options =>
            {
                // Re-read per attempt rather than captured once: a reconnect after a long drop needs
                // whatever token is current, not the one this circuit started with.
                options.AccessTokenProvider = async () =>
                {
                    var current = await _auth.GetAuthenticationStateAsync();
                    return current.User.FindFirst(ApiClient.TokenClaim)?.Value;
                };
            })
            // Back off rather than hammer a backend that is restarting.
            .WithAutomaticReconnect(new[]
            {
                TimeSpan.Zero, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(15),
            })
            .Build();

        _connection.On<List<AuditLogDto>>(ActivityEvent, async entries =>
        {
            if (Received is { } handler)
                await handler.Invoke(entries);
        });

        _connection.On<TaxonomyChangedDto>(TaxonomyEvent, async change =>
        {
            TaxonomyVersion = change.Version;

            if (TaxonomyChanged is { } handler)
                await handler.Invoke(change);
        });

        _connection.Reconnected += async _ =>
        {
            if (Reconnected is { } handler)
                await handler.Invoke();
        };

        try
        {
            await _connection.StartAsync();
            return true;
        }
        catch (Exception ex)
        {
            // The backend may simply not be up yet. Callers fall back to their own refresh path;
            // a dead feed must not take the page down with it.
            _log.LogWarning(ex, "Could not open the activity hub connection");
            return false;
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_connection is not null)
            await _connection.DisposeAsync();
    }
}
