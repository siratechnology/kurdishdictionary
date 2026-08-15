using backend.Data;
using backend.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services.Lexicon;

public enum IssueSeverity
{
    /// <summary>Blocks the save.</summary>
    Error = 0,

    /// <summary>Saves fine, but lands in the work queue. Most linguistic gaps are this.</summary>
    Warning = 1,
}

public record LexiconIssue(string Code, string Message, IssueSeverity Severity, string? Field = null);

public record ValidationResult(IReadOnlyList<LexiconIssue> Issues)
{
    public bool HasErrors => Issues.Any(i => i.Severity == IssueSeverity.Error);
    public static readonly ValidationResult Ok = new(Array.Empty<LexiconIssue>());
}

/// <summary>
/// The rules from پڕۆمپت ٦, in the SERVICE layer rather than the UI.
///
/// A rule that lives in a Razor component is enforced only for people who go through that
/// component: the API, an import, a bulk edit and next year's mobile client all bypass it. These
/// run wherever a save runs.
///
/// Two deliberate choices:
///
///  · Almost everything is a WARNING, not an error. A teacher who cannot save until the taxonomy is
///    satisfied will pick a wrong value to get past the form, which is worse than a blank — the
///    same reasoning as the escape hatch in پڕۆمپت ٧. Incomplete work goes to the queue instead.
///
///  · Every rule that depends on configuration checks whether that configuration exists first. The
///    axes ship empty; on day one no rule fires, and that is correct rather than a failure.
/// </summary>
public class LexiconValidator
{
    private readonly AppDbContext _db;
    private readonly OptionsTreeService _tree;

    public LexiconValidator(AppDbContext db, OptionsTreeService tree)
    {
        _db = db;
        _tree = tree;
    }

    // ── Sense ──────────────────────────────────────────────────────────────

    public async Task<ValidationResult> ValidateSenseAsync(int senseId, CancellationToken ct = default)
    {
        var sense = await _db.Senses
            .AsNoTracking()
            .Include(s => s.Features)
            .FirstOrDefaultAsync(s => s.Id == senseId, ct);

        if (sense is null) return ValidationResult.Ok;

        var issues = new List<LexiconIssue>();

        if (sense.PartOfSpeechId is null)
        {
            issues.Add(new("sense.no-part-of-speech", "مانا بەشی ئاخاوتنی نییە",
                IssueSeverity.Warning, nameof(Sense.PartOfSpeechId)));
        }

        // Deck slide 14: every sense needs a usage example in semantic context. Not optional.
        if (string.IsNullOrWhiteSpace(sense.ExampleUsage))
        {
            issues.Add(new("sense.no-example", "مانا نموونەی بەکارهێنانی نییە",
                IssueSeverity.Warning, nameof(Sense.ExampleUsage)));
        }

        issues.AddRange(await ValidateAxesAsync(sense, ct));

        return new ValidationResult(issues);
    }

    /// <summary>
    /// Required axes filled; sub-groups either satisfied or correctly absent.
    ///
    /// The visible set comes from <see cref="OptionsTreeService"/> — the same resolution the entry
    /// form uses, at every depth. A separate implementation here would eventually report a gap on a
    /// question the teacher was never shown, which is the most demoralising kind of queue item.
    ///
    /// Everything below the part of speech is a WARNING. Nothing here may become an error: a save
    /// blocked by a sub-option produces a wrong value that looks finished, and the whole point of
    /// the queue is that it can say "not yet" instead.
    /// </summary>
    private async Task<List<LexiconIssue>> ValidateAxesAsync(Sense sense, CancellationToken ct)
    {
        var issues = new List<LexiconIssue>();

        var visible = await _tree.ResolveAsync(sense, ct);

        var answered = sense.Features
            .Where(f => !f.IsDeleted)
            .GroupBy(f => f.AxisId)
            .ToDictionary(g => g.Key, g => g.ToList());

        // Nothing configured for this part of speech yet, or no part of speech at all. Day one, not
        // an error — the sense is asked nothing beyond its headword, definition and example.
        if (visible.Count == 0 && answered.Count == 0) return issues;

        foreach (var node in visible)
        {
            var axis = node.Axis;
            var rows = answered.GetValueOrDefault(axis.AxisId) ?? new List<SenseFeature>();

            var held = rows.Count(f => f.ValueId is not null);
            var notApplicable = rows.FirstOrDefault(f => f.IsNotApplicable);

            // «ئەم تەوەرە کار ناکات» is a complete answer, so it satisfies the axis outright.
            if (notApplicable is null)
            {
                // How many answers the axis wants: the assignment's own flag, plus the per-axis
                // minimum. Both feed the queue and the completeness score, and neither gates a save.
                var wanted = Math.Max(axis.IsRequired ? 1 : 0, axis.MinSelections);

                if (held < wanted)
                {
                    issues.Add(new("sense.missing-required-axis",
                        wanted <= 1
                            ? $"تەوەری «{axis.Name}» پێویستە"
                            : $"تەوەری «{axis.Name}» لانیکەم {wanted} بژاردەی دەوێت",
                        IssueSeverity.Warning, axis.Name));
                }
            }
            else if (string.IsNullOrWhiteSpace(notApplicable.Note))
            {
                // The escape hatch always costs a sentence of explanation.
                issues.Add(new("sense.not-applicable-without-note",
                    $"«{axis.Name}» وەک نەگونجاو دیاریکراوە بەڵام هۆکارەکەی نەنووسراوە",
                    IssueSeverity.Warning, axis.Name));
            }
        }

        // Anything answered that the tree does not ask for. The cascade clears these the moment a
        // sense is touched, so reaching here means data written before the rule changed.
        var visibleIds = visible.Select(v => v.Axis.AxisId).ToHashSet();

        foreach (var axisId in answered.Keys.Where(id => !visibleIds.Contains(id)))
        {
            var name = await _db.FeatureAxes
                .Where(a => a.Id == axisId)
                .Select(a => a.NameKu)
                .FirstOrDefaultAsync(ct) ?? "?";

            issues.Add(new("sense.stale-axis",
                $"وەڵامی «{name}» ماوەتەوە بەڵام ئەم تەوەرە چیتر پەیوەندیدار نییە",
                IssueSeverity.Warning, name));
        }

        return issues;
    }

    // ── Word (morphology) ──────────────────────────────────────────────────

    public async Task<ValidationResult> ValidateWordAsync(int wordId, CancellationToken ct = default)
    {
        var word = await _db.Words
            .AsNoTracking()
            .Include(w => w.Senses).ThenInclude(s => s.Features).ThenInclude(f => f.Value)
            .Include(w => w.OutgoingWordRelations).ThenInclude(r => r.Type)
            .FirstOrDefaultAsync(w => w.Id == wordId, ct);

        if (word is null) return ValidationResult.Ok;

        var issues = new List<LexiconIssue>();

        var values = word.Senses
            .SelectMany(s => s.Features)
            .Where(f => !f.IsDeleted && f.Value is not null)
            .Select(f => f.Value!.Code)
            .Where(c => c is not null)
            .ToHashSet();

        var relations = word.OutgoingWordRelations
            .Where(r => !r.IsDeleted)
            .GroupBy(r => r.Type.Code)
            .ToDictionary(g => g.Key, g => g.Count());

        // ڕۆنان = داڕێژراو → the word MUST have at least one ڕەگ.
        if (values.Contains(TaxonomyCodes.Value.Derived)
            && relations.GetValueOrDefault(TaxonomyCodes.Relation.Root) == 0)
        {
            issues.Add(new("word.derived-without-root",
                "وشەی داڕێژراو دەبێت لانیکەم یەک ڕەگی هەبێت", IssueSeverity.Warning));
        }

        // ڕۆنان = لێکدراو → at least TWO پێکهاتە. One component is not a compound.
        if (values.Contains(TaxonomyCodes.Value.Compound)
            && relations.GetValueOrDefault(TaxonomyCodes.Relation.Component) < 2)
        {
            issues.Add(new("word.compound-without-components",
                "وشەی لێکدراو دەبێت لانیکەم دوو پێکهاتەی هەبێت", IssueSeverity.Warning));
        }

        // A comparative or superlative هاوەڵناو is a FORM of a چەسپاو headword, not its own Word.
        if (values.Contains(TaxonomyCodes.Value.Comparative) || values.Contains(TaxonomyCodes.Value.Superlative))
        {
            issues.Add(new("word.degree-should-be-form",
                "پلەی بەراورد و پلەی باڵا دەبێت وەک ڕەگەزی وشە (WordForm) تۆمار بکرێن، نەک وەک وشەیەکی سەربەخۆ",
                IssueSeverity.Warning));
        }

        // ژمارە = کۆ on a standalone word — a flag, not a verdict. Some plurals are genuinely
        // their own headword, so this asks rather than tells.
        if (values.Contains(TaxonomyCodes.Value.Plural))
        {
            issues.Add(new("word.plural-should-be-form",
                "ئایا ئەمە دەبێت ببێتە ڕەگەزی کۆی وشەی تاک؟", IssueSeverity.Warning));
        }

        return issues.Count == 0 ? ValidationResult.Ok : new ValidationResult(issues);
    }
}
