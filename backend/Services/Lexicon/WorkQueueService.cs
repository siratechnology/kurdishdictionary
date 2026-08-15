using backend.Data;
using backend.Data.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos;

namespace backend.Services.Lexicon;

/// <summary>
/// What is incomplete, cheapest to fix first (پڕۆمپت ٦).
///
/// The order is deliberate and is not a priority ranking: bucket 1 is one click per sense, bucket 6
/// needs a linguist to think. Putting the cheap work first means a teacher with ten spare minutes
/// can actually finish something.
///
/// Buckets whose taxonomy is not configured report IsConfigured=false rather than 0. A zero that
/// means "nothing to do" and a zero that means "nobody has set up the axes yet" look identical on a
/// dashboard, and the second one would hide the entire backlog.
/// </summary>
public class WorkQueueService
{
    private readonly AppDbContext _db;

    public WorkQueueService(AppDbContext db) => _db = db;

    public async Task<WorkQueueDto> GetSummaryAsync(CancellationToken ct = default)
    {
        var buckets = new List<WorkQueueBucketDto>
        {
            await MissingPartOfSpeech(ct),
            await MissingRequiredAxis(ct),
            await DerivedWithoutRoot(ct),
            await CompoundWithoutComponents(ct),
            await MissingExample(ct),
            await MissingSemanticRelation(ct),
        };

        return new WorkQueueDto
        {
            Buckets = buckets,
            Total = buckets.Where(b => b.IsConfigured).Sum(b => b.Count),
        };
    }

    public async Task<List<WorkQueueItemDto>> GetItemsAsync(
        WorkQueueBucket bucket, int take = 50, CancellationToken ct = default) => bucket switch
    {
        WorkQueueBucket.MissingPartOfSpeech =>
            await SenseItems(s => s.PartOfSpeechId == null, "بەشی ئاخاوتنی نییە", "pos", take, ct),

        WorkQueueBucket.MissingExample =>
            await SenseItems(s => s.ExampleUsage == "", "نموونەی بەکارهێنانی نییە", "example", take, ct),

        WorkQueueBucket.MissingSemanticRelation =>
            await SenseItems(
                s => !_db.SenseRelations.Any(r =>
                        (r.FromSenseId == s.Id || r.ToSenseId == s.Id) &&
                        (r.Type.Code == TaxonomyCodes.Relation.Synonym ||
                         r.Type.Code == TaxonomyCodes.Relation.Antonym)),
                "نە هاومانای هەیە نە پێچەوانە", "relations", take, ct),

        _ => await AxisOrMorphologyItems(bucket, take, ct),
    };

    /// <summary>
    /// A bucket's size in distinct WORDS, deduped by spelling.
    ///
    /// Every bucket used to count SENSES, so the queue reported 4,914 against a dictionary of
    /// 2,967 words and no two screens agreed on how much work was left. The unit people plan in is
    /// words — "we have three hundred left" — and a word whose three senses all need an example is
    /// one word to open, not three. The labels below say وشە for the same reason.
    /// </summary>
    private Task<int> CountWords(
        System.Linq.Expressions.Expression<Func<Sense, bool>> predicate, CancellationToken ct)
    {
        var wordIds = _db.Words.AsNoTracking().GroupBy(w => w.Kurdish).Select(g => g.Min(w => w.Id));

        return _db.Senses.AsNoTracking()
            .Where(predicate)
            .Where(s => wordIds.Contains(s.WordId))
            .Select(s => s.WordId)
            .Distinct()
            .CountAsync(ct);
    }

    // ── buckets ────────────────────────────────────────────────────────────

    private async Task<WorkQueueBucketDto> MissingPartOfSpeech(CancellationToken ct) => new()
    {
        Bucket = WorkQueueBucket.MissingPartOfSpeech,
        Label = "وشە بەبێ بەشی ئاخاوتن",
        Hint = "یەک کلیک بۆ هەر وشەیەک",
        Count = await CountWords(s => s.PartOfSpeechId == null, ct),
    };

    private async Task<WorkQueueBucketDto> MissingRequiredAxis(CancellationToken ct)
    {
        var configured = await _db.PartOfSpeechAxes.AnyAsync(a => a.IsRequired || a.Axis.MinSelections > 0, ct);

        var dto = new WorkQueueBucketDto
        {
            Bucket = WorkQueueBucket.MissingRequiredAxis,
            Label = "وشە بەبێ تەوەری پێویست",
            Hint = "تەوەرە ڕێزمانییە پێویستەکان",
            IsConfigured = configured,
        };

        if (!configured) return dto;

        dto.Count = await CountWords(ShortOfWhatAnAxisAsks, ct);
        return dto;
    }

    /// <summary>
    /// Senses holding fewer answers than an axis asks for.
    ///
    /// This is the ONLY thing IsRequired and MinSelections do — they populate this bucket and feed
    /// the completeness score. Neither ever gates a save, at any depth.
    ///
    /// The condition test is one level deep on purpose, and is still correct at any depth: the
    /// cascade clears a whole subtree the moment its parent answer changes, so a sense can only be
    /// holding a parent value if every ancestor above it is still answered. Resolving the full chain
    /// in memory would mean loading every sense to count one bucket.
    /// </summary>
    private System.Linq.Expressions.Expression<Func<Sense, bool>> ShortOfWhatAnAxisAsks =>
        s => _db.PartOfSpeechAxes.Any(a =>
            a.PartOfSpeechId == s.PartOfSpeechId &&
            (a.IsRequired || a.Axis.MinSelections > 0) &&
            (a.RequiresValueId == null || s.Features.Any(f => f.ValueId == a.RequiresValueId)) &&
            // A not-applicable row answers the axis outright, so it is counted as satisfying it.
            !s.Features.Any(f => f.AxisId == a.AxisId && f.IsNotApplicable) &&
            s.Features.Count(f => f.AxisId == a.AxisId && f.ValueId != null) <
                (a.Axis.MinSelections > 0 ? a.Axis.MinSelections : 1));


    private async Task<WorkQueueBucketDto> DerivedWithoutRoot(CancellationToken ct)
    {
        var valueId = await ValueIdFor(TaxonomyCodes.Value.Derived, ct);

        var dto = new WorkQueueBucketDto
        {
            Bucket = WorkQueueBucket.DerivedWithoutRoot,
            Label = "وشەی داڕێژراو بەبێ ڕەگ",
            Hint = "ڕەگی وشەکە دیاری بکە",
            IsConfigured = valueId is not null,
        };

        if (valueId is not { } derived) return dto;

        dto.Count = await _db.Words.CountAsync(w =>
            w.Senses.Any(s => s.Features.Any(f => f.ValueId == derived)) &&
            !w.OutgoingWordRelations.Any(r => r.Type.Code == TaxonomyCodes.Relation.Root), ct);

        return dto;
    }

    private async Task<WorkQueueBucketDto> CompoundWithoutComponents(CancellationToken ct)
    {
        var valueId = await ValueIdFor(TaxonomyCodes.Value.Compound, ct);

        var dto = new WorkQueueBucketDto
        {
            Bucket = WorkQueueBucket.CompoundWithoutComponents,
            Label = "وشەی لێکدراو بە کەمتر لە دوو پێکهاتە",
            Hint = "پێکهاتەکانی وشە زیاد بکە",
            IsConfigured = valueId is not null,
        };

        if (valueId is not { } compound) return dto;

        dto.Count = await _db.Words.CountAsync(w =>
            w.Senses.Any(s => s.Features.Any(f => f.ValueId == compound)) &&
            w.OutgoingWordRelations.Count(r => r.Type.Code == TaxonomyCodes.Relation.Component) < 2, ct);

        return dto;
    }

    private async Task<WorkQueueBucketDto> MissingExample(CancellationToken ct) => new()
    {
        Bucket = WorkQueueBucket.MissingExample,
        Label = "وشە بەبێ نموونەی بەکارهێنان",
        Hint = "دەق slide ١٤: هەموو مانایەک نموونەی دەوێت",
        Count = await CountWords(s => s.ExampleUsage == "", ct),
    };

    private async Task<WorkQueueBucketDto> MissingSemanticRelation(CancellationToken ct) => new()
    {
        Bucket = WorkQueueBucket.MissingSemanticRelation,
        Label = "وشە بەبێ هاومانا و پێچەوانە",
        Hint = "پێویستی بە بیرکردنەوەیە — دواتر",
        Count = await CountWords(s =>
            !_db.SenseRelations.Any(r =>
                (r.FromSenseId == s.Id || r.ToSenseId == s.Id) &&
                (r.Type.Code == TaxonomyCodes.Relation.Synonym ||
                 r.Type.Code == TaxonomyCodes.Relation.Antonym)), ct),
    };

    // ── items ──────────────────────────────────────────────────────────────

    private async Task<List<WorkQueueItemDto>> SenseItems(
        System.Linq.Expressions.Expression<Func<Sense, bool>> predicate,
        string reason, string focus, int take, CancellationToken ct)
    {
        return await _db.Senses
            .AsNoTracking()
            .Where(predicate)
            .OrderBy(s => s.WordId)
            .Take(take)
            .Select(s => new WorkQueueItemDto
            {
                SenseId = s.Id,
                WordId = s.WordId,
                Word = s.Word.Kurdish,
                Definition = s.Definition,
                Reason = reason,
                // Deep link to the field, not just the page: the queue is only useful if a row
                // lands you on the thing that needs typing.
                Href = $"/words/{s.WordId}/senses/{s.Id}?focus={focus}",
            })
            .ToListAsync(ct);
    }

    private async Task<List<WorkQueueItemDto>> AxisOrMorphologyItems(
        WorkQueueBucket bucket, int take, CancellationToken ct)
    {
        var code = bucket switch
        {
            WorkQueueBucket.DerivedWithoutRoot => TaxonomyCodes.Value.Derived,
            WorkQueueBucket.CompoundWithoutComponents => TaxonomyCodes.Value.Compound,
            _ => null,
        };

        if (code is null)
        {
            // MissingRequiredAxis — the reason is per-sense, so it is resolved per row.
            return await _db.Senses
                .AsNoTracking()
                .Where(ShortOfWhatAnAxisAsks)
                .OrderBy(s => s.WordId)
                .Take(take)
                .Select(s => new WorkQueueItemDto
                {
                    SenseId = s.Id,
                    WordId = s.WordId,
                    Word = s.Word.Kurdish,
                    Definition = s.Definition,
                    Reason = "تەوەری پێویست پڕنەکراوەتەوە",
                    Href = $"/words/{s.WordId}/senses/{s.Id}?focus=axes",
                })
                .ToListAsync(ct);
        }

        var valueId = await ValueIdFor(code, ct);
        if (valueId is not { } value) return new List<WorkQueueItemDto>();

        var relationCode = bucket == WorkQueueBucket.DerivedWithoutRoot
            ? TaxonomyCodes.Relation.Root
            : TaxonomyCodes.Relation.Component;

        var minimum = bucket == WorkQueueBucket.DerivedWithoutRoot ? 1 : 2;

        return await _db.Words
            .AsNoTracking()
            .Where(w => w.Senses.Any(s => s.Features.Any(f => f.ValueId == value)) &&
                        w.OutgoingWordRelations.Count(r => r.Type.Code == relationCode) < minimum)
            .OrderBy(w => w.Kurdish)
            .Take(take)
            .Select(w => new WorkQueueItemDto
            {
                SenseId = 0,
                WordId = w.Id,
                Word = w.Kurdish,
                Reason = bucket == WorkQueueBucket.DerivedWithoutRoot
                    ? "داڕێژراوە بەڵام ڕەگی نییە"
                    : "لێکدراوە بەڵام کەمتر لە دوو پێکهاتەی هەیە",
                Href = $"/words/{w.Id}?focus=relations",
            })
            .ToListAsync(ct);
    }

    /// <summary>Null when the team has not created a value with this code yet.</summary>
    private Task<int?> ValueIdFor(string code, CancellationToken ct) =>
        _db.FeatureValues
            .Where(v => v.Code == code && v.IsActive)
            .Select(v => (int?)v.Id)
            .FirstOrDefaultAsync(ct);
}
