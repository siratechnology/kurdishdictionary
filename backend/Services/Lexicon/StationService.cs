using backend.Data;
using backend.Data.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos;

namespace backend.Services.Lexicon;

/// <summary>
/// The station screen's data (پڕۆمپت ٧): one sense at a time, walked from the first to the last.
///
/// The single rule that shapes this class: <see cref="StationSenseDto.Axes"/> contains exactly the
/// axes that apply to this sense right now. A conditional axis whose condition is not satisfied is
/// ABSENT, not disabled. Sending it greyed-out would invite a teacher to wonder what they did
/// wrong, and sending all of them would ask questions the grammar does not.
/// </summary>
public class StationService
{
    private readonly AppDbContext _db;
    private readonly ClaimService _claims;
    private readonly LexiconValidator _validator;
    private readonly OptionsTreeService _tree;

    public StationService(
        AppDbContext db, ClaimService claims, LexiconValidator validator, OptionsTreeService tree)
    {
        _db = db;
        _claims = claims;
        _validator = validator;
        _tree = tree;
    }

    /// <summary>
    /// The sense at a position in the walk. Ordered by word then sense so the teacher moves through
    /// the dictionary the way it reads, not the way rows happen to be stored.
    /// </summary>
    public async Task<StationSenseDto?> GetAtAsync(
        int position, Guid userId, bool onlyUnclassified, CancellationToken ct = default)
    {
        var query = Ordered(onlyUnclassified);

        var total = await query.CountAsync(ct);
        if (total == 0) return null;

        position = Math.Clamp(position, 1, total);

        var senseId = await query.Skip(position - 1).Select(s => s.Id).FirstOrDefaultAsync(ct);
        if (senseId == 0) return null;

        return await BuildAsync(senseId, position, total, userId, ct);
    }

    public async Task<StationSenseDto?> GetBySenseAsync(int senseId, Guid userId, CancellationToken ct = default)
    {
        var query = Ordered(onlyUnclassified: false);
        var total = await query.CountAsync(ct);

        // Position is derived rather than stored: the walk order must survive senses being added
        // and removed underneath it.
        //
        // Counted, not indexed. Pulling all ~4,900 ids into memory to call IndexOf was a list
        // allocation on every single navigation, and it grows with the dictionary.
        var target = await _db.Senses.AsNoTracking()
            .Where(s => s.Id == senseId)
            .Select(s => new { s.Id, s.SortOrder, Kurdish = s.Word.Kurdish })
            .FirstOrDefaultAsync(ct);

        if (target is null) return null;

        var before = await query.CountAsync(s =>
            string.Compare(s.Word.Kurdish, target.Kurdish) < 0
            || (s.Word.Kurdish == target.Kurdish && s.SortOrder < target.SortOrder)
            || (s.Word.Kurdish == target.Kurdish && s.SortOrder == target.SortOrder && s.Id < target.Id), ct);

        return await BuildAsync(senseId, before + 1, total, userId, ct);
    }

    /// <summary>
    /// The senses this walk steps through, in reading order.
    ///
    /// DEDUPED BY SPELLING, the same rule the words list uses (one row per distinct headword,
    /// lowest id). Without it the station counted senses belonging to duplicate word rows that no
    /// other screen shows, so the dictionary was 2,967 words on one page and something else here.
    /// A count that changes depending on which screen you are looking at is not a count.
    /// </summary>
    private IQueryable<Sense> Ordered(bool onlyUnclassified)
    {
        var minIds = _db.Words.AsNoTracking()
            .GroupBy(w => w.Kurdish)
            .Select(g => g.Min(w => w.Id));

        var query = _db.Senses.AsNoTracking().Where(s => minIds.Contains(s.WordId));

        if (onlyUnclassified)
            query = query.Where(s => s.PartOfSpeechId == null);

        return query.OrderBy(s => s.Word.Kurdish).ThenBy(s => s.SortOrder).ThenBy(s => s.Id);
    }

    /// <summary>
    /// Where this sense sits when the walk is counted in WORDS, which is the unit every other
    /// screen reports and therefore the only one a teacher can reconcile.
    ///
    /// The walk itself still steps sense by sense — part of speech belongs to the sense, and a word
    /// with three senses is three separate questions — so a multi-sense word simply holds the word
    /// counter still for a step or two while «مانای ٢ لە ٣» moves. That is honest about both units
    /// instead of picking one and being wrong about the other.
    /// </summary>
    private async Task<(int WordPosition, int WordTotal, int SenseIndex, int SenseCount)> WordCountsAsync(
        Sense sense, bool onlyUnclassified, CancellationToken ct)
    {
        var walk = Ordered(onlyUnclassified);
        var word = sense.Word;

        // Counted over EVERY deduped word, not only the ones that have senses.
        //
        // 111 words have no sense at all, so counting the walk's own words gave 2,856 while the
        // word list, the relation workspace and the dashboard all said 2,967. The number a teacher
        // is told the dictionary contains cannot depend on which screen they are looking at — and
        // a word with no sense is not "not in the dictionary", it is unfinished work that belongs
        // in the denominator. The walk simply steps over those, so the counter skips a number now
        // and then rather than lying about the size of the job.
        var words = _db.Words.AsNoTracking()
            .Where(w => _db.Words.GroupBy(x => x.Kurdish).Select(g => g.Min(x => x.Id)).Contains(w.Id));

        var wordTotal = await words.CountAsync(ct);

        // Words sorting before this one, on the same key the walk orders by, so the two can never
        // disagree about what "before" means.
        var wordPosition = await words
            .CountAsync(w => string.Compare(w.Kurdish, word.Kurdish) < 0
                          || (w.Kurdish == word.Kurdish && w.Id < word.Id), ct);

        var siblings = await walk
            .Where(s => s.WordId == sense.WordId)
            .Select(s => s.Id)
            .ToListAsync(ct);

        return (wordPosition + 1, wordTotal, siblings.IndexOf(sense.Id) + 1, siblings.Count);
    }

    private async Task<StationSenseDto?> BuildAsync(
        int senseId, int position, int total, Guid userId, CancellationToken ct)
    {
        var sense = await _db.Senses
            .AsNoTracking()
            .Include(s => s.Word)
            .Include(s => s.Domain)
            .Include(s => s.Features).ThenInclude(f => f.Value)
            .FirstOrDefaultAsync(s => s.Id == senseId, ct);

        if (sense is null) return null;

        var holder = await _claims.CurrentHolderAsync(senseId, ct);

        var dto = new StationSenseDto
        {
            SenseId = sense.Id,
            WordId = sense.WordId,
            Word = sense.Word.Kurdish,
            Definition = sense.Definition,
            ExampleUsage = sense.ExampleUsage,
            PartOfSpeechId = sense.PartOfSpeechId,
            DomainId = sense.DomainId,
            DomainName = sense.Domain?.NameKu,
            WorkflowState = sense.WorkflowState.ToString(),
            Position = position,
            Total = total,
            HeldBy = holder is not null && holder.UserId != userId
                ? holder.User.FullName ?? holder.User.UserName
                : null,
        };

        var (wordPosition, wordTotal, senseIndex, senseCount) =
            await WordCountsAsync(sense, onlyUnclassified: false, ct);

        dto.WordPosition = wordPosition;
        dto.WordTotal = wordTotal;
        dto.SenseIndex = senseIndex;
        dto.SenseCount = senseCount;

        dto.Axes = await ResolveVisibleAxesAsync(sense, ct);
        dto.Relations = await LoadRelationsAsync(sense.Id, ct);
        dto.Issues = (await _validator.ValidateSenseAsync(senseId, ct)).Issues
            .Select(i => i.Message)
            .ToList();

        return dto;
    }

    /// <summary>
    /// The axes that apply to this sense, in the order the form renders them.
    ///
    /// The rule itself lives in <see cref="OptionsTree"/> — deliberately, because the validator, the
    /// work queue and the settings preview all have to answer the same question and a second
    /// implementation is how they start disagreeing. This method only turns the answer into a DTO.
    ///
    /// Depth is unlimited by design and the result is depth-first: each axis is immediately followed
    /// by the groups its answers opened, each carrying the parent value it hangs off so the form can
    /// nest and label it.
    /// </summary>
    private async Task<List<StationAxisDto>> ResolveVisibleAxesAsync(Sense sense, CancellationToken ct)
    {
        var resolved = await _tree.ResolveAsync(sense, ct);
        if (resolved.Count == 0) return new List<StationAxisDto>();

        var answers = sense.Features.Where(f => !f.IsDeleted).ToList();

        return resolved.Select(node =>
        {
            var axis = node.Axis;
            var onThisAxis = answers.Where(f => f.AxisId == axis.AxisId).ToList();
            var held = onThisAxis.Where(f => f.ValueId is not null).Select(f => f.ValueId!.Value).ToList();
            var notApplicable = onThisAxis.FirstOrDefault(f => f.IsNotApplicable);

            // A deactivated value stays on screen when this sense holds it — retiring a term must
            // not make a teacher's existing answer unreadable — but is never offered otherwise.
            var offered = axis.Values
                .Where(v => v.IsActive || held.Contains(v.Id))
                .ToList();

            return new StationAxisDto
            {
                AxisId = axis.AxisId,
                Name = axis.Name,
                // The teacher reads Prompt; Name rides along only so the admin preview can show the
                // label it belongs to. StationAxisDto.Ask applies the fallback.
                Prompt = axis.Prompt,
                Description = axis.Description,
                IsRequired = axis.IsRequired,
                AllowsNotApplicable = axis.AllowsNotApplicable,
                MinSelections = axis.MinSelections,
                MaxSelections = axis.MaxSelections,
                SelectedValueIds = held,
                IsNotApplicable = notApplicable is not null,
                Note = notApplicable?.Note,
                Depth = node.Depth,
                ParentValueId = node.ParentValue?.Id,
                ParentValueName = node.ParentValue?.Name,
                IsRetired = !axis.IsActive,
                Values = offered
                    .Select((v, i) => new StationValueDto
                    {
                        ValueId = v.Id,
                        Name = v.Name,
                        Hint = v.Hint,
                        // Digits 1-9 only. A tenth value would need a modifier and the keyboard
                        // shortcut stops being faster than the mouse.
                        Digit = i < 9 ? i + 1 : 0,
                        IsRetired = !v.IsActive,
                        OpensChildGroup = v.OpensChildGroup,
                    })
                    .ToList(),
            };
        }).ToList();
    }

    private Task<List<StationRelationDto>> LoadRelationsAsync(int senseId, CancellationToken ct) =>
        _db.SenseRelations
            .AsNoTracking()
            .Where(r => r.FromSenseId == senseId && !r.IsAutoInverse)
            .Select(r => new StationRelationDto
            {
                RelationId = r.Id,
                TypeName = r.Type.NameKu,
                TargetWord = r.ToSense.Word.Kurdish,
            })
            .ToListAsync(ct);

    /// <summary>
    /// Saves the sense-level fields. Deliberately does NOT validate-then-refuse: a teacher who
    /// cannot save until everything is perfect will type something wrong to escape the form.
    /// </summary>
    public async Task SaveAsync(SaveStationSenseDto dto, CancellationToken ct = default)
    {
        var sense = await _db.Senses.FirstAsync(s => s.Id == dto.SenseId, ct);

        if (sense.Definition != dto.Definition) sense.Definition = dto.Definition ?? "";
        if (sense.ExampleUsage != dto.ExampleUsage) sense.ExampleUsage = dto.ExampleUsage ?? "";
        if (sense.PartOfSpeechId != dto.PartOfSpeechId) sense.PartOfSpeechId = dto.PartOfSpeechId;
        if (sense.DomainId != dto.DomainId) sense.DomainId = dto.DomainId;

        await _db.SaveChangesAsync(ct);
    }

    /// <summary>
    /// Clears answers on axes the tree no longer asks for, after a part-of-speech or value change.
    ///
    /// Never leave a stale answer on an axis that is no longer asked: it would sit in the database
    /// asserting something about the word that the current classification does not support, and the
    /// validator would report it forever without the teacher ever seeing the control.
    ///
    /// The work is <see cref="OptionsTreeService.ClearStaleAnswersAsync"/> — one implementation, so
    /// what the form shows and what the cascade clears cannot drift apart.
    /// </summary>
    public Task<int> ClearStaleAnswersAsync(int senseId, CancellationToken ct = default) =>
        _tree.ClearStaleAnswersAsync(senseId, ct);
}
