using backend.Data;
using backend.Data.Models;
using backend.Services;
using backend.Services.Lexicon;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace backend.Tests;

/// <summary>
/// The user the next SaveChanges will be attributed to. Swapping this between saves is how the
/// tests act as two different teachers against one context.
/// </summary>
public class FakeCurrentUser : ICurrentUser
{
    public Guid? UserId { get; set; }
    public string? UserName { get; set; }
    public string? IpAddress => "127.0.0.1";
    public string? UserAgent => "xunit";
    public string? Country => null;
    public string? City => null;
}

/// <summary>Swallows the SignalR broadcast the audit interceptor makes — no hub in a test.</summary>
public class NullBroadcaster : IActivityBroadcaster
{
    public Task BroadcastAsync(IReadOnlyList<Shared.Dtos.AuditLogDto> entries, CancellationToken ct = default) =>
        Task.CompletedTask;
}

/// <summary>
/// A real SQL Server database, migrated.
///
/// Deliberately NOT the in-memory provider: the two things this suite has to prove — that the
/// append-only trigger fires, and that vw_ContributionStats returns the right numbers — are a
/// trigger and a view. Neither exists in the in-memory provider, so a passing in-memory test would
/// prove nothing about the guarantee the ledger is supposed to give.
/// </summary>
public sealed class LedgerFixture : IAsyncLifetime
{
    private const string Master =
        "Server=localhost;Database=master;Trusted_Connection=True;TrustServerCertificate=True;";

    public string DatabaseName { get; } = $"KurdishDictionary_Test_{Guid.NewGuid():N}";

    public string ConnectionString =>
        $"Server=localhost;Database={DatabaseName};Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True;";

    public FakeCurrentUser CurrentUser { get; } = new();

    /// <summary>
    /// The process-wide options-tree cache, wired exactly as Program.cs wires it — including the
    /// interceptor that drops it after a taxonomy write commits.
    ///
    /// Sharing one instance across the whole collection is not a convenience: it is what the tests
    /// have to exercise. A cache that is only ever cold cannot prove that a settings change reaches
    /// an open session, which is the whole point of holding the taxonomy in one.
    /// </summary>
    public TaxonomyCache TaxonomyCache { get; } = new();

    public Guid SomaId { get; private set; }
    public Guid PerjinId { get; private set; }

    public async Task InitializeAsync()
    {
        await using (var db = NewContext())
        {
            await db.Database.MigrateAsync();
        }

        await using var seed = NewContext();
        SomaId = await AddUser(seed, "soma", "سۆما");
        PerjinId = await AddUser(seed, "perjin", "پەرژین");
    }

    public async Task DisposeAsync()
    {
        // SINGLE_USER ROLLBACK: the connection pool keeps sessions open and DROP blocks behind them.
        await using var master = new AppDbContext(
            new DbContextOptionsBuilder<AppDbContext>().UseSqlServer(Master).Options);

        await master.Database.ExecuteSqlRawAsync(
            $"IF DB_ID('{DatabaseName}') IS NOT NULL BEGIN " +
            $"ALTER DATABASE [{DatabaseName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; " +
            $"DROP DATABASE [{DatabaseName}]; END");
    }

    /// <summary>A fresh context with the ledger interceptors wired exactly as Program.cs wires them.</summary>
    public AppDbContext NewContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(ConnectionString)
            .AddInterceptors(
                new SoftDeleteInterceptor(CurrentUser),
                new ContributionEventInterceptor(CurrentUser),
                new AuditSaveChangesInterceptor(CurrentUser, new NullBroadcaster()),
                new TaxonomyChangeInterceptor(TaxonomyCache))
            .Options;

        return new AppDbContext(options);
    }

    // ── The lexicon services, assembled the way the container assembles them ──
    //
    // Constructing them by hand in each test drifts from Program.cs the moment a dependency is
    // added; these four helpers are the one place that has to be updated when it is.

    public OptionsTreeService Tree(AppDbContext db) => new(db, TaxonomyCache);

    public LexiconValidator Validator(AppDbContext db) => new(db, Tree(db));

    public StationService Station(AppDbContext db) =>
        new(db, new ClaimService(db), Validator(db), Tree(db));

    public ClassificationService Classification(AppDbContext db) => new(db);

    /// <summary>Runs <paramref name="work"/> as the given user, on their own context.</summary>
    public async Task As(Guid userId, string userName, Func<AppDbContext, Task> work)
    {
        CurrentUser.UserId = userId;
        CurrentUser.UserName = userName;

        await using var db = NewContext();
        await work(db);
    }

    private static async Task<Guid> AddUser(AppDbContext db, string userName, string fullName)
    {
        var user = new AppUser
        {
            Id = Guid.NewGuid(),
            UserName = userName,
            NormalizedUserName = userName.ToUpperInvariant(),
            Email = $"{userName}@test.local",
            NormalizedEmail = $"{userName}@test.local".ToUpperInvariant(),
            FullName = fullName,
            SecurityStamp = Guid.NewGuid().ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString(),
        };

        db.Users.Add(user);
        await db.SaveChangesAsync();
        return user.Id;
    }
}

[CollectionDefinition("ledger")]
public class LedgerCollection : ICollectionFixture<LedgerFixture>;
