using backend.Data;
using Microsoft.EntityFrameworkCore;
using Shared.Text;

namespace LexiconMigration;

/// <summary>
/// Backfills <c>Normalized</c> for rows that predate the column.
///
/// The interceptor keeps it correct from now on, but the 3,026 words already in the database were
/// saved before it existed and have an empty key — which means search would find none of them.
/// Idempotent: re-running it changes nothing once the values agree.
/// </summary>
public static class Normalize
{
    public static async Task RunAsync(AppDbContext db, bool dryRun)
    {
        Console.WriteLine(dryRun
            ? "DRY RUN — nothing will be written.\n"
            : "Backfilling Normalized…\n");

        var words = await db.Words
            .IgnoreQueryFilters()   // a hidden word still has to be findable when restoring it
            .Select(w => new { w.Id, w.Kurdish, w.Normalized })
            .ToListAsync();

        var stale = words
            .Select(w => new { w.Id, w.Kurdish, Current = w.Normalized, Expected = KurdishText.Normalize(w.Kurdish) })
            .Where(w => w.Current != w.Expected)
            .ToList();

        Console.WriteLine($"  words            : {words.Count,7:N0}");
        Console.WriteLine($"  needing backfill : {stale.Count,7:N0}");

        // The whole point of the column: how many distinct headwords does folding merge?
        var collisions = words
            .GroupBy(w => KurdishText.Normalize(w.Kurdish))
            .Where(g => g.Select(x => x.Kurdish).Distinct().Count() > 1)
            .ToList();

        if (collisions.Count > 0)
        {
            Console.WriteLine();
            Console.WriteLine($"── {collisions.Count} headwords that folding reveals as the SAME word ──");
            foreach (var g in collisions.Take(15))
                Console.WriteLine($"  {g.Key,-24} ← {string.Join(" · ", g.Select(x => x.Kurdish).Distinct())}");
        }

        if (dryRun || stale.Count == 0)
        {
            Console.WriteLine();
            Console.WriteLine(stale.Count == 0 ? "Already up to date." : "Dry run — nothing written.");
            return;
        }

        // ExecuteUpdate in batches: 3,000 tracked entities would each raise a ledger event, and a
        // derived column being computed for the first time is not a contribution by anyone.
        var done = 0;
        foreach (var chunk in stale.Chunk(500))
        {
            foreach (var w in chunk)
            {
                await db.Database.ExecuteSqlInterpolatedAsync(
                    $"UPDATE [Words] SET [Normalized] = {w.Expected} WHERE [Id] = {w.Id}");
            }

            done += chunk.Length;
            Console.WriteLine($"  {done,7:N0} / {stale.Count:N0}");
        }

        Console.WriteLine();
        Console.WriteLine($"Done. {done:N0} rows updated.");
    }
}
