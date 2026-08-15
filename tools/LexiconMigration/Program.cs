using backend.Data;
using backend.Data.Models;
using LexiconMigration;
using Microsoft.EntityFrameworkCore;

// ═══════════════════════════════════════════════════════════════════════════
// Lexicon migration — پڕۆمپت ٤
//
//   propose   phase 1: read the live lexicon, write CSVs to ./migration-review/, change NOTHING
//   apply     phase 2: read the REVIEWED CSVs back and write the new schema
//
// Phase 1 is read-only by construction: it never opens a write path. Phase 2 is a separate
// command precisely so that no run can slide from proposing into applying.
// ═══════════════════════════════════════════════════════════════════════════

var command = args.FirstOrDefault()?.ToLowerInvariant() ?? "help";

var connection = Environment.GetEnvironmentVariable("LEXICON_CONNECTION")
    ?? "Server=localhost;Database=KurdishDictionary;Trusted_Connection=True;TrustServerCertificate=True;";

var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlServer(connection).Options;

switch (command)
{
    case "propose":
        await Propose.RunAsync(new AppDbContext(options), connection);
        break;

    case "configure-axes":
    {
        // Attributed to the seeded admin so the ledger records who configured the taxonomy —
        // configuration is a contribution too, and پڕۆمپت ١١ wants every taxonomy edit trailed.
        await using var db = new AppDbContext(options);
        var admin = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions
            .FirstOrDefaultAsync(db.Users, u => u.UserName == "sirwan");

        await ConfigureAxes.RunAsync(db, admin?.Id ?? Guid.Empty, dryRun: args.Contains("--dry-run"));
        break;
    }

    case "normalize":
        await Normalize.RunAsync(new AppDbContext(options), dryRun: args.Contains("--dry-run"));
        break;

    case "apply":
        // The review happens IN THE APP, at the station screen, not in a spreadsheet: every word
        // is walked one at a time by the teachers who entered it. So this migrates all of them and
        // leaves the ambiguous ones unclassified rather than refusing to move them — an
        // unclassified sense in ڕیزی کار is reviewable; a row sitting in a CSV is not.
        await Apply.RunAsync(new AppDbContext(options), dryRun: args.Contains("--dry-run"));
        break;

    default:
        Console.WriteLine("""
            usage: dotnet run --project tools/LexiconMigration -- <command>

              propose     phase 1 — write ./migration-review/*.csv. Reads only; changes nothing.
              apply       phase 2 — write Senses, Domains and translations from the old tables.
                          Ambiguous rows arrive unclassified and go to ڕیزی کار.
                          Pass --dry-run to see the counts without writing.
              configure-axes  create ژمارە and ڕەگەز with their values, assign them, and carry
                              the old Word.Gender column onto the senses.

              normalize   backfill Word.Normalized for rows that predate the column.
                          Pass --dry-run to see what it would change.

            Connection comes from LEXICON_CONNECTION, defaulting to the local database.
            """);
        break;
}
