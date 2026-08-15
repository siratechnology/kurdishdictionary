using backend.Data.Models;
using backend.Services.Lexicon;
using Microsoft.EntityFrameworkCore;

namespace backend.Tests;

/// <summary>
/// پڕۆمپت ٦ — the rules and the queue.
///
/// The most important case here is the FIRST one: with an empty taxonomy every rule must stay
/// silent and every configurable bucket must say "not configured" rather than "0". That is the
/// state the app ships in, and a queue that reports a clean bill of health on day one would hide
/// the entire backlog the moment the team starts configuring.
/// </summary>
[Collection("ledger")]
public class WorkQueueTests
{
    private readonly LedgerFixture _fx;

    public WorkQueueTests(LedgerFixture fx) => _fx = fx;

    [Fact]
    public async Task An_empty_taxonomy_reports_not_configured_rather_than_zero()
    {
        await using var db = _fx.NewContext();
        var summary = await new WorkQueueService(db).GetSummaryAsync();

        var derived = summary.Buckets.Single(b => b.Bucket == Shared.Dtos.WorkQueueBucket.DerivedWithoutRoot);
        var compound = summary.Buckets.Single(b => b.Bucket == Shared.Dtos.WorkQueueBucket.CompoundWithoutComponents);
        var axes = summary.Buckets.Single(b => b.Bucket == Shared.Dtos.WorkQueueBucket.MissingRequiredAxis);

        Assert.False(derived.IsConfigured);
        Assert.False(compound.IsConfigured);
        Assert.False(axes.IsConfigured);

        // …while the buckets that need no configuration still work.
        var example = summary.Buckets.Single(b => b.Bucket == Shared.Dtos.WorkQueueBucket.MissingExample);
        Assert.True(example.IsConfigured);
    }

    [Fact]
    public async Task A_sense_with_no_example_lands_in_the_queue()
    {
        int senseId = 0;

        await _fx.As(_fx.SomaId, "soma", async db =>
        {
            var word = new Word { Kurdish = "ئەستێرە" };
            db.Words.Add(word);
            await db.SaveChangesAsync();

            var sense = new Sense
            {
                WordId = word.Id,
                Definition = "جەرگەیەکی گەشاوە لە ئاسمان",
                PartOfSpeechId = 1,          // ناو
                ExampleUsage = "",           // the gap
            };
            db.Senses.Add(sense);
            await db.SaveChangesAsync();
            senseId = sense.Id;
        });

        await using var read = _fx.NewContext();

        var issues = await _fx.Validator(read).ValidateSenseAsync(senseId);
        Assert.Contains(issues.Issues, i => i.Code == "sense.no-example");

        // A missing example is a warning, not an error: blocking the save makes a teacher invent
        // an example to get past the form, which is worse than an honest blank.
        Assert.False(issues.HasErrors);

        var items = await new WorkQueueService(read)
            .GetItemsAsync(Shared.Dtos.WorkQueueBucket.MissingExample, take: 200);

        Assert.Contains(items, i => i.SenseId == senseId);
        Assert.Contains(items, i => i.Href.Contains("focus=example"));
    }

    [Fact]
    public async Task Saving_a_root_relation_creates_its_inverse_automatically()
    {
        int rootWordId = 0, derivedWordId = 0;

        await _fx.As(_fx.SomaId, "soma", async db =>
        {
            var root = new Word { Kurdish = "خوێند" };
            var derived = new Word { Kurdish = "خوێندکار" };
            db.Words.AddRange(root, derived);
            await db.SaveChangesAsync();
            rootWordId = root.Id;
            derivedWordId = derived.Id;
        });

        _fx.CurrentUser.UserId = _fx.SomaId;
        _fx.CurrentUser.UserName = "soma";

        await using (var db = _fx.NewContext())
        {
            var rootType = await db.RelationTypes.FirstAsync(t => t.Code == "root");
            await new RelationService(db).AddWordRelationAsync(derivedWordId, rootWordId, rootType.Id);
        }

        await using var read = _fx.NewContext();

        // The edge the teacher entered…
        var forward = await read.WordRelations
            .Include(r => r.Type)
            .FirstOrDefaultAsync(r => r.FromWordId == derivedWordId && r.ToWordId == rootWordId);
        Assert.NotNull(forward);
        Assert.Equal("root", forward!.Type.Code);
        Assert.False(forward.IsAutoInverse);

        // …and the one it should never have had to enter.
        var back = await read.WordRelations
            .Include(r => r.Type)
            .FirstOrDefaultAsync(r => r.FromWordId == rootWordId && r.ToWordId == derivedWordId);
        Assert.NotNull(back);
        Assert.Equal("derived-from", back!.Type.Code);
        Assert.True(back.IsAutoInverse);

        // Both edges are that user's contribution, and both are in the ledger under their name.
        var events = await read.ContributionEvents
            .Where(e => e.EntityType == nameof(WordRelation) &&
                        e.EventType == ContributionEventType.RelationAdded)
            .ToListAsync();

        Assert.True(events.Count >= 2);
        Assert.All(events.TakeLast(2), e => Assert.Equal(_fx.SomaId, e.UserId));
    }

    [Fact]
    public async Task A_symmetric_relation_mirrors_itself()
    {
        int a = 0, b = 0;

        await _fx.As(_fx.SomaId, "soma", async db =>
        {
            var w1 = new Word { Kurdish = "نان" };
            var w2 = new Word { Kurdish = "سەموون" };
            db.Words.AddRange(w1, w2);
            await db.SaveChangesAsync();
            a = w1.Id; b = w2.Id;
        });

        _fx.CurrentUser.UserId = _fx.SomaId;

        await using (var db = _fx.NewContext())
        {
            var regional = await db.RelationTypes.FirstAsync(t => t.Code == "regional");
            Assert.True(regional.IsSymmetric);
            await new RelationService(db).AddWordRelationAsync(a, b, regional.Id);
        }

        await using var read = _fx.NewContext();
        var regionalId = (await read.RelationTypes.FirstAsync(t => t.Code == "regional")).Id;

        Assert.True(await read.WordRelations.AnyAsync(r => r.FromWordId == a && r.ToWordId == b && r.TypeId == regionalId));
        Assert.True(await read.WordRelations.AnyAsync(r => r.FromWordId == b && r.ToWordId == a && r.TypeId == regionalId));
    }

    [Fact]
    public async Task Removing_a_relation_removes_its_inverse_too()
    {
        int from = 0, to = 0;

        await _fx.As(_fx.SomaId, "soma", async db =>
        {
            var w1 = new Word { Kurdish = "دەست" };
            var w2 = new Word { Kurdish = "دەستەواژە" };
            db.Words.AddRange(w1, w2);
            await db.SaveChangesAsync();
            from = w2.Id; to = w1.Id;
        });

        _fx.CurrentUser.UserId = _fx.SomaId;

        int edgeId;
        await using (var db = _fx.NewContext())
        {
            var rootType = await db.RelationTypes.FirstAsync(t => t.Code == "root");
            var edge = await new RelationService(db).AddWordRelationAsync(from, to, rootType.Id);
            edgeId = edge.Id;
        }

        await using (var db = _fx.NewContext())
        {
            await new RelationService(db).RemoveWordRelationAsync(edgeId);
        }

        await using var read = _fx.NewContext();

        // Leaving the inverse behind would make the graph keep asserting what was just retracted.
        Assert.False(await read.WordRelations.AnyAsync(r => r.FromWordId == from && r.ToWordId == to));
        Assert.False(await read.WordRelations.AnyAsync(r => r.FromWordId == to && r.ToWordId == from));
    }

    [Fact]
    public async Task A_word_cannot_relate_to_itself()
    {
        int wordId = 0;

        await _fx.As(_fx.SomaId, "soma", async db =>
        {
            var w = new Word { Kurdish = "ئاسمان" };
            db.Words.Add(w);
            await db.SaveChangesAsync();
            wordId = w.Id;
        });

        await using var db2 = _fx.NewContext();
        var rootType = await db2.RelationTypes.FirstAsync(t => t.Code == "root");

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            new RelationService(db2).AddWordRelationAsync(wordId, wordId, rootType.Id));
    }

    [Fact]
    public async Task A_sense_relation_type_is_refused_on_a_word_relation()
    {
        int a = 0, b = 0;

        await _fx.As(_fx.SomaId, "soma", async db =>
        {
            var w1 = new Word { Kurdish = "ڕۆژ" };
            var w2 = new Word { Kurdish = "شەو" };
            db.Words.AddRange(w1, w2);
            await db.SaveChangesAsync();
            a = w1.Id; b = w2.Id;
        });

        await using var db2 = _fx.NewContext();

        // هاومانا is semantic — it belongs to senses. Morphology and semantics are separate tables
        // on purpose, and the service refuses to blur them.
        var synonym = await db2.RelationTypes.FirstAsync(t => t.Code == "synonym");

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            new RelationService(db2).AddWordRelationAsync(a, b, synonym.Id));
    }
}
