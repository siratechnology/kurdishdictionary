using backend.Data.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace backend.Tests;

/// <summary>
/// The guarantee this suite exists to prove: one contributor's record never moves because another
/// contributor did something. That is the whole basis for putting these teachers' names in the
/// published dictionary's credits, so it is tested rather than assumed.
/// </summary>
[Collection("ledger")]
public class ContributionLedgerTests
{
    private readonly LedgerFixture _fx;

    public ContributionLedgerTests(LedgerFixture fx) => _fx = fx;

    // ═══════════════════════════════════════════════════════════════════════
    // The test the handoff gates on:
    //   Soma creates word W → Perjin edits W → A still has WordCreated=1 and
    //   B has WordEdited=1, and neither number moves when the other acts.
    // ═══════════════════════════════════════════════════════════════════════
    [Fact]
    public async Task An_editor_never_displaces_the_original_author()
    {
        // Deltas, not absolutes: the whole suite shares one database, and the claim being tested is
        // that a number does not MOVE when someone else acts — which is a delta by definition.
        var somaBefore = await StatsFor(_fx.SomaId);
        var perjinBefore = await StatsFor(_fx.PerjinId);
        var somaEditsBefore = await CountEvents(_fx.SomaId, ContributionEventType.WordEdited);

        int wordId = 0;

        await _fx.As(_fx.SomaId, "soma", async db =>
        {
            var word = new Word { Kurdish = "جوان" };
            db.Words.Add(word);
            await db.SaveChangesAsync();
            wordId = word.Id;
        });

        Assert.Equal(somaBefore.WordsCreated + 1, (await StatsFor(_fx.SomaId)).WordsCreated);

        var perjinEditsBefore = await CountEvents(_fx.PerjinId, ContributionEventType.WordEdited);

        await _fx.As(_fx.PerjinId, "perjin", async db =>
        {
            var word = await db.Words.FirstAsync(w => w.Id == wordId);
            word.Description = "ڕوونکردنەوەیەکی نوێ";
            await db.SaveChangesAsync();
        });

        // Soma's created count did not move when Perjin edited her word.
        Assert.Equal(somaBefore.WordsCreated + 1, (await StatsFor(_fx.SomaId)).WordsCreated);

        // Perjin edited, and created nothing.
        Assert.Equal(perjinBefore.WordsCreated, (await StatsFor(_fx.PerjinId)).WordsCreated);
        Assert.Equal(perjinEditsBefore + 1, await CountEvents(_fx.PerjinId, ContributionEventType.WordEdited));

        // And Soma did not acquire an edit she never made.
        Assert.Equal(somaEditsBefore, await CountEvents(_fx.SomaId, ContributionEventType.WordEdited));

        // The word's own history shows both names, creator first.
        await using var read = _fx.NewContext();
        var history = await read.ContributionEvents
            .Where(e => e.WordId == wordId)
            .OrderBy(e => e.Id)
            .ToListAsync();

        Assert.Equal(_fx.SomaId, history.First().UserId);
        Assert.Equal(ContributionEventType.WordCreated, history.First().EventType);
        Assert.Contains(history, e => e.UserId == _fx.PerjinId);
    }

    [Fact]
    public async Task Soft_deleting_a_word_does_not_reduce_its_authors_count()
    {
        int wordId = 0;

        await _fx.As(_fx.SomaId, "soma", async db =>
        {
            var word = new Word { Kurdish = "ڕەنگ" };
            db.Words.Add(word);
            await db.SaveChangesAsync();
            wordId = word.Id;
        });

        var before = (await StatsFor(_fx.SomaId)).WordsCreated;

        await _fx.As(_fx.PerjinId, "perjin", async db =>
        {
            var word = await db.Words.FirstAsync(w => w.Id == wordId);
            db.Words.Remove(word);
            await db.SaveChangesAsync();
        });

        // The row is hidden, not gone — deleting it would delete Soma's evidence.
        await using var read = _fx.NewContext();
        Assert.False(await read.Words.AnyAsync(w => w.Id == wordId));

        var still = await read.Words.IgnoreQueryFilters().FirstAsync(w => w.Id == wordId);
        Assert.True(still.IsDeleted);
        Assert.Equal(_fx.PerjinId, still.DeletedByUserId);

        Assert.Equal(before, (await StatsFor(_fx.SomaId)).WordsCreated);
    }

    [Fact]
    public async Task Remove_is_rewritten_into_a_soft_delete_even_for_child_rows()
    {
        int wordId = 0;

        await _fx.As(_fx.SomaId, "soma", async db =>
        {
            var word = new Word { Kurdish = "ئاو" };
            word.Meanings.Add(new WordMeans { Meaning = "شلەی بێ ڕەنگ" });
            db.Words.Add(word);
            await db.SaveChangesAsync();
            wordId = word.Id;
        });

        await _fx.As(_fx.SomaId, "soma", async db =>
        {
            var meaning = await db.WordMeans.FirstAsync(m => m.WordId == wordId);
            db.WordMeans.Remove(meaning);
            await db.SaveChangesAsync();
        });

        await using var read = _fx.NewContext();
        Assert.False(await read.WordMeans.AnyAsync(m => m.WordId == wordId));
        Assert.True(await read.WordMeans.IgnoreQueryFilters().AnyAsync(m => m.WordId == wordId && m.IsDeleted));
    }

    [Fact]
    public async Task Every_changed_field_becomes_its_own_event()
    {
        int wordId = 0;

        await _fx.As(_fx.SomaId, "soma", async db =>
        {
            var word = new Word { Kurdish = "کتێب", Description = "یەکەم" };
            db.Words.Add(word);
            await db.SaveChangesAsync();
            wordId = word.Id;
        });

        await _fx.As(_fx.PerjinId, "perjin", async db =>
        {
            var word = await db.Words.FirstAsync(w => w.Id == wordId);
            word.Kurdish = "پەرتووک";
            word.Description = "دووەم";
            await db.SaveChangesAsync();
        });

        await using var read = _fx.NewContext();
        var edits = await read.ContributionEvents
            .Where(e => e.WordId == wordId && e.EventType == ContributionEventType.WordEdited)
            .ToListAsync();

        // Two properties changed in one save, so two events — per-field history is what lets a
        // word's page say "who changed the definition" rather than just "somebody edited this".
        Assert.Equal(2, edits.Count);

        var headword = Assert.Single(edits, e => e.FieldName == nameof(Word.Kurdish));
        Assert.Equal("کتێب", headword.OldValue);
        Assert.Equal("پەرتووک", headword.NewValue);
    }

    // ═══════════════════════════════════════════════════════════════════════
    // Append-only, enforced by the database rather than by convention.
    // ═══════════════════════════════════════════════════════════════════════

    [Fact]
    public async Task The_ledger_rejects_updates()
    {
        await _fx.As(_fx.SomaId, "soma", async db =>
        {
            db.Words.Add(new Word { Kurdish = "خۆر" });
            await db.SaveChangesAsync();
        });

        await using var db2 = _fx.NewContext();

        var ex = await Assert.ThrowsAsync<SqlException>(() =>
            db2.Database.ExecuteSqlRawAsync(
                "UPDATE ContributionEvents SET NewValue = 'tampered' WHERE Id = (SELECT MIN(Id) FROM ContributionEvents)"));

        Assert.Equal(50001, ex.Number);
        Assert.Contains("append-only", ex.Message);
    }

    [Fact]
    public async Task The_ledger_rejects_deletes()
    {
        await _fx.As(_fx.SomaId, "soma", async db =>
        {
            db.Words.Add(new Word { Kurdish = "مانگ" });
            await db.SaveChangesAsync();
        });

        await using var db2 = _fx.NewContext();

        var ex = await Assert.ThrowsAsync<SqlException>(() =>
            db2.Database.ExecuteSqlRawAsync(
                "DELETE FROM ContributionEvents WHERE Id = (SELECT MIN(Id) FROM ContributionEvents)"));

        Assert.Equal(50001, ex.Number);
    }

    // ── helpers ────────────────────────────────────────────────────────────

    private async Task<ContributionStats> StatsFor(Guid userId)
    {
        await using var db = _fx.NewContext();
        return await db.ContributionStats.FirstAsync(s => s.UserId == userId);
    }

    private async Task<int> CountEvents(Guid userId, ContributionEventType type)
    {
        await using var db = _fx.NewContext();
        return await db.ContributionEvents.CountAsync(e => e.UserId == userId && e.EventType == type);
    }
}
