using backend.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace backend.Data;

/// <summary>
/// Creates the roles and the initial accounts, then assigns ownership of the pre-existing words.
///
/// Everything here is idempotent and strictly additive: it is safe to run on every boot against a
/// database that already holds real data. It never deletes, and it never overwrites a value that is
/// already set — a word that already has an owner is left exactly as it is.
/// </summary>
public static class DbSeeder
{
    /// <summary>
    /// Which editor owns the words starting with which Kurdish letter, as specified by the project owner.
    /// A word whose first letter isn't listed keeps a null owner rather than being guessed at.
    /// </summary>
    private static readonly (string UserName, string[] Letters)[] LetterOwners =
    {
        ("ahmadaziz", new[] { "ئ" }),
        ("barzihiwa", new[] { "ب" }),
        ("shkoskur",  new[] { "ح", "خ" }),
        ("somaali",   new[] { "ت" }),
    };

    public static async Task SeedAsync(IServiceProvider services, IConfiguration config, ILogger logger)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var users = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        var roles = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

        await SeedRolesAsync(roles);
        await SeedAdminAsync(users, config, logger);
        await SeedEditorsAsync(users, config, logger);
        await BackfillWordOwnersAsync(db, users, logger);
    }

    private static async Task SeedRolesAsync(RoleManager<AppRole> roles)
    {
        foreach (var (name, description) in Roles.All)
        {
            if (await roles.RoleExistsAsync(name)) continue;

            await roles.CreateAsync(new AppRole
            {
                Id = Guid.NewGuid(),
                Name = name,
                NormalizedName = name.ToUpperInvariant(),
                Description = description,
            });
        }
    }

    private static async Task SeedAdminAsync(UserManager<AppUser> users, IConfiguration config, ILogger logger)
    {
        var email = config["Seed:AdminEmail"] ?? "sirwanjaff999@gmail.com";
        var userName = config["Seed:AdminUserName"] ?? "sirwan";
        var password = config["Seed:AdminPassword"] ?? "Admin@123!";

        if (await users.FindByNameAsync(userName) is not null) return;

        var admin = new AppUser
        {
            Id = Guid.NewGuid(),
            UserName = userName,
            Email = email,
            FullName = "Sirwan Jaff",
            EmailConfirmed = true,
            IsActive = true,
            // Seeded credentials are in source control; nag until it's replaced.
            MustChangePassword = true,
        };

        var result = await users.CreateAsync(admin, password);
        if (!result.Succeeded)
        {
            logger.LogError("Failed to seed admin: {Errors}",
                string.Join("; ", result.Errors.Select(e => e.Description)));
            return;
        }

        await users.AddToRoleAsync(admin, Roles.Admin);
        logger.LogWarning("Seeded admin '{UserName}' with the default password — change it after first login", userName);
    }

    private static async Task SeedEditorsAsync(UserManager<AppUser> users, IConfiguration config, ILogger logger)
    {
        var defaultPassword = config["Seed:EditorPassword"] ?? "Editor@123!";

        foreach (var (userName, _) in LetterOwners)
        {
            if (await users.FindByNameAsync(userName) is not null) continue;

            var user = new AppUser
            {
                Id = Guid.NewGuid(),
                UserName = userName,
                Email = $"{userName}@kurdishdictionary.local",
                FullName = userName,
                EmailConfirmed = true,
                IsActive = true,
                MustChangePassword = true,
            };

            var result = await users.CreateAsync(user, defaultPassword);
            if (!result.Succeeded)
            {
                logger.LogError("Failed to seed editor {UserName}: {Errors}", userName,
                    string.Join("; ", result.Errors.Select(e => e.Description)));
                continue;
            }

            await users.AddToRoleAsync(user, Roles.Editor);
            logger.LogInformation("Seeded editor '{UserName}'", userName);
        }
    }

    /// <summary>
    /// Assigns the existing (pre-authentication) words to their owner by first letter.
    ///
    /// Only fills rows where CreatedByUserId IS NULL, so it cannot clobber an assignment made later
    /// through the UI, and re-running it on an already-backfilled database is a no-op. Uses
    /// ExecuteUpdate: a bulk UPDATE that deliberately bypasses the audit interceptor, because
    /// backfilling a thousand words is one migration step, not a thousand human edits.
    /// </summary>
    private static async Task BackfillWordOwnersAsync(AppDbContext db, UserManager<AppUser> users, ILogger logger)
    {
        foreach (var (userName, letters) in LetterOwners)
        {
            var user = await users.FindByNameAsync(userName);
            if (user is null)
            {
                logger.LogWarning("Cannot backfill words for '{UserName}': user not found", userName);
                continue;
            }

            foreach (var letter in letters)
            {
                var updated = await db.Words
                    .Where(w => w.CreatedByUserId == null && w.Kurdish.StartsWith(letter))
                    .ExecuteUpdateAsync(s => s.SetProperty(w => w.CreatedByUserId, user.Id));

                if (updated > 0)
                    logger.LogInformation("Assigned {Count} words starting with '{Letter}' to {UserName}",
                        updated, letter, userName);
            }
        }

        // The relations and meanings hanging off a word are work done on that word, so they belong
        // to whoever owns it. Derived from the word rather than re-matched by letter, so a word that
        // is reassigned by hand later keeps its children consistent on the next boot.
        //
        // Written as raw UPDATE...FROM because ExecuteUpdate cannot translate a navigation property
        // in SetProperty ("The LINQ expression could not be translated"). One set-based statement
        // per table, rather than loading thousands of rows to write them straight back.
        var relations = await db.Database.ExecuteSqlRawAsync("""
            UPDATE r
            SET    r.CreatedByUserId = w.CreatedByUserId
            FROM   RelatedWords r
            JOIN   Words w ON w.Id = r.WordId
            WHERE  r.CreatedByUserId IS NULL
              AND  w.CreatedByUserId IS NOT NULL
            """);

        var meanings = await db.Database.ExecuteSqlRawAsync("""
            UPDATE m
            SET    m.CreatedByUserId = w.CreatedByUserId
            FROM   WordMeans m
            JOIN   Words w ON w.Id = m.WordId
            WHERE  m.CreatedByUserId IS NULL
              AND  w.CreatedByUserId IS NOT NULL
            """);

        if (relations > 0 || meanings > 0)
            logger.LogInformation("Assigned {Relations} relations and {Meanings} meanings to their words' owners",
                relations, meanings);
    }
}
