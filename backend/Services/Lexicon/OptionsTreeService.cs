using backend.Data;
using backend.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services.Lexicon;

/// <summary>
/// The one way to ask "what is this sense being asked right now" and "what stopped being asked".
///
/// Everything that needs the options tree goes through here — the entry form, the validator, the
/// work queue, the settings preview — so they cannot disagree. A second implementation of the
/// visibility rule is the bug this class exists to prevent: the form would show a question the
/// validator did not know about, and the work queue would report a gap nobody could see.
/// </summary>
public class OptionsTreeService
{
    private readonly AppDbContext _db;
    private readonly TaxonomyCache _cache;

    public OptionsTreeService(AppDbContext db, TaxonomyCache cache)
    {
        _db = db;
        _cache = cache;
    }

    public Task<OptionsTree> GetAsync(int partOfSpeechId, CancellationToken ct = default) =>
        _cache.GetOrLoadAsync(partOfSpeechId, () => OptionsTree.LoadAsync(_db, partOfSpeechId, ct));

    /// <summary>
    /// The axes that apply to a sense right now, in render order, each carrying the value that
    /// opened it. A sense with no part of speech is asked nothing.
    /// </summary>
    public async Task<List<ResolvedAxis>> ResolveAsync(Sense sense, CancellationToken ct = default)
    {
        if (sense.PartOfSpeechId is not { } posId) return new List<ResolvedAxis>();

        var tree = await GetAsync(posId, ct);
        var answers = Answers(sense);

        return tree.Resolve(answers.HeldValueIds, answers.AnsweredAxisIds);
    }

    /// <summary>
    /// Removes the answers the tree no longer asks for, one FeatureCleared event each.
    ///
    /// Called after every part-of-speech change and every feature change, because both can retire a
    /// whole subtree. A stale answer left behind would sit in the database asserting something the
    /// current classification does not support, with no control on screen to correct it — and the
    /// validator would report it forever.
    ///
    /// One pass is enough at any depth. <see cref="OptionsTree.Resolve"/> asks whether the PARENT
    /// AXIS is visible as well as whether the parent value is held, so a change at depth ١ hides
    /// depth ٢ and depth ٣ together even though depth ٣'s own parent value is still stored.
    /// </summary>
    public async Task<int> ClearStaleAnswersAsync(int senseId, CancellationToken ct = default)
    {
        var sense = await _db.Senses
            .Include(s => s.Features)
            .FirstAsync(s => s.Id == senseId, ct);

        var answered = sense.Features.Where(f => !f.IsDeleted).ToList();
        if (answered.Count == 0) return 0;

        // No part of speech means no axes apply at all — the tree has no root to walk from.
        if (sense.PartOfSpeechId is not { } posId)
            return await ClearAsync(answered, ct);

        var tree = await GetAsync(posId, ct);
        var answers = Answers(sense);

        var staleAxisIds = tree.StaleAxisIds(answers.HeldValueIds, answers.AnsweredAxisIds);
        if (staleAxisIds.Count == 0) return 0;

        return await ClearAsync(answered.Where(f => staleAxisIds.Contains(f.AxisId)).ToList(), ct);
    }

    /// <summary>
    /// Marks each row so the ledger records a CLEARING rather than an edit, then removes them.
    ///
    /// «سۆما گۆڕی تێپەڕی» and «تێپەڕی چیتر پەیوەندیدار نییە، وەڵامەکەی لابرا» are different facts
    /// about a word. Per-word history is unreadable if a cascade looks like a person retyping.
    /// </summary>
    private async Task<int> ClearAsync(List<SenseFeature> rows, CancellationToken ct)
    {
        if (rows.Count == 0) return 0;

        foreach (var row in rows)
            row.ClearedByCascade = true;

        _db.SenseFeatures.RemoveRange(rows);

        // One SaveChanges, but the ledger interceptor walks the change tracker entry by entry — so
        // this is one FeatureCleared per cleared answer, and not one event more.
        await _db.SaveChangesAsync(ct);

        return rows.Count;
    }

    private static (HashSet<int> HeldValueIds, HashSet<int> AnsweredAxisIds) Answers(Sense sense)
    {
        var live = sense.Features.Where(f => !f.IsDeleted).ToList();

        return (
            live.Where(f => f.ValueId is not null).Select(f => f.ValueId!.Value).ToHashSet(),

            // A not-applicable row counts as an ANSWER on its axis but holds no value, so it keeps
            // the axis on screen without opening any child group. That is the point of it: «ئەم
            // تەوەرە کار ناکات» is a complete answer, not a blank.
            live.Select(f => f.AxisId).ToHashSet());
    }
}
