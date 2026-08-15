using backend.Data;
using backend.Data.Models;
using backend.Services.Lexicon;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos;

namespace backend.Tests;

/// <summary>
/// What the TEACHER reads, and what the dropdown opens on.
///
/// Acceptance tests ٤ and ٥ of the two-shapes prompt:
///
///   ٤. Every dropdown shows PromptKu, not the raw label. Clearing PromptKu falls back to the label.
///      The teacher never meets the word «تەوەر».
///   ٥. Every dropdown opens on the empty prompt option, never pre-selected on a real value.
///
/// Both are about data that LOOKS like work and is not. A teacher who reads «جۆری کار» and does not
/// know what is being asked picks something to get past it; a dropdown that opens on its first real
/// value is answered before anybody touches it. Neither failure throws, and neither is distinguishable
/// afterwards from a judgement a named teacher made — which is why they are pinned here rather than
/// checked by eye on the entry form.
///
/// Every configuration goes through the settings service, the same calls the settings screen makes.
/// </summary>
[Collection("ledger")]
public class TeacherPromptTests
{
    private readonly LedgerFixture _fx;

    public TeacherPromptTests(LedgerFixture fx) => _fx = fx;

    // ═══════════════════════════════════════════════════════════════════════
    // ٤. The teacher reads the question. The label survives for the admin.
    // ═══════════════════════════════════════════════════════════════════════
    [Fact]
    public async Task Every_dropdown_shows_the_written_question_and_falls_back_to_the_label()
    {
        var pos = await NewPartOfSpeechAsync("کار");

        var kind = await AddGroupAsync(pos, null, "جۆری کار");
        var complete = await AddValueAsync(kind, "تەواو");
        await AddValueAsync(kind, "ناتەواو");

        // A nested group too: the fallback and the prompt must behave the same at every depth, since
        // depth is data and a rule that only holds at the first level is not a rule.
        var transitivity = await AddGroupAsync(pos, complete, "تێپەڕی");
        await AddValueAsync(transitivity, "تێپەڕ");

        await SetPromptAsync(kind, "کارەکە چ جۆرێکە؟");
        await SetPromptAsync(transitivity, "ئایا کارەکە تێپەڕە یان تێنەپەڕ؟");

        var (_, senseId) = await NewSenseAsync(pos);

        // ── The written question is what the form prints ────────────────────
        await using (var read = _fx.NewContext())
        {
            var dto = await _fx.Station(read).GetBySenseAsync(senseId, _fx.SomaId);
            var axis = dto!.Axes.Single();

            Assert.Equal("کارەکە چ جۆرێکە؟", axis.Ask);

            // The label is still there — the settings tree is navigated by it and history reads it
            // back. It is simply not what a teacher is shown.
            Assert.Equal("جۆری کار", axis.Name);
            Assert.Equal("کارەکە چ جۆرێکە؟", axis.Prompt);
        }

        // ── And at depth, once the parent answer opens the child ────────────
        await AnswerAsync(senseId, kind, complete);

        await using (var read = _fx.NewContext())
        {
            var dto = await _fx.Station(read).GetBySenseAsync(senseId, _fx.SomaId);

            Assert.Equal(2, dto!.Axes.Count);
            Assert.Equal("ئایا کارەکە تێپەڕە یان تێنەپەڕ؟", dto.Axes[1].Ask);
            Assert.Equal(1, dto.Axes[1].Depth);
        }

        // ── Clearing the prompt falls back to the label ─────────────────────
        //
        // Clearing is a supported answer, not a broken state: a bare category name reads as
        // unfinished and gets asked about, whereas an invented question reads as finished.
        await SetPromptAsync(kind, null);

        await using (var read = _fx.NewContext())
        {
            var dto = await _fx.Station(read).GetBySenseAsync(senseId, _fx.SomaId);
            var axis = dto!.Axes.First(a => a.Depth == 0);

            Assert.Null(axis.Prompt);
            Assert.Equal("جۆری کار", axis.Ask);
        }

        // ── Whitespace is not a question either ─────────────────────────────
        //
        // Saved as null rather than stored and rendered: a prompt of "   " would print an empty
        // heading above a live dropdown, which is worse than the label it replaced.
        await SetPromptAsync(kind, "   ");

        await using (var read = _fx.NewContext())
        {
            Assert.Null((await read.FeatureAxes.FirstAsync(a => a.Id == kind)).PromptKu);

            var dto = await _fx.Station(read).GetBySenseAsync(senseId, _fx.SomaId);
            Assert.Equal("جۆری کار", dto!.Axes.First(a => a.Depth == 0).Ask);
        }
    }

    /// <summary>
    /// The settings preview and the entry form resolve the same string.
    ///
    /// They are two screens reading one tree, and the fallback lives on the shared DTO precisely so
    /// they cannot each decide it — a preview that showed the question while the station showed the
    /// label would make the preview a confident lie, which is worse than having no preview.
    /// </summary>
    [Fact]
    public async Task The_settings_preview_and_the_entry_form_show_the_same_question()
    {
        var pos = await NewPartOfSpeechAsync("ناو");

        var gender = await AddGroupAsync(pos, null, "ڕەگەز");
        await AddValueAsync(gender, "نێر");

        var number = await AddGroupAsync(pos, null, "ژمارە");
        await AddValueAsync(number, "تاک");

        await SetPromptAsync(gender, "ڕەگەزی وشەکە چییە؟");
        // ژمارە deliberately left without one, so the pair covers both branches at once.

        var (_, senseId) = await NewSenseAsync(pos);

        await using var read = _fx.NewContext();

        var station = await _fx.Station(read).GetBySenseAsync(senseId, _fx.SomaId);

        // The preview path: the same tree, resolved with no sense behind it.
        var tree = await _fx.Tree(read).GetAsync(pos);
        var preview = tree.Resolve(new HashSet<int>());

        Assert.Equal(
            station!.Axes.Select(a => a.Ask).ToArray(),
            preview.Select(r => r.Axis.Ask).ToArray());

        Assert.Equal(new[] { "ڕەگەزی وشەکە چییە؟", "ژمارە" }, station.Axes.Select(a => a.Ask).ToArray());
    }

    /// <summary>
    /// The teacher never meets the schema's vocabulary. «تەوەر» and "axis" name a table, not a
    /// question, and a form that prints one is asking somebody to answer in a language they were
    /// never taught.
    /// </summary>
    [Fact]
    public async Task The_teacher_never_reads_the_word_axis()
    {
        var pos = await NewPartOfSpeechAsync("کار");

        var kind = await AddGroupAsync(pos, null, "جۆری کار");
        await AddValueAsync(kind, "تەواو");
        await SetPromptAsync(kind, "کارەکە چ جۆرێکە؟");

        var (_, senseId) = await NewSenseAsync(pos);

        await using var read = _fx.NewContext();
        var dto = await _fx.Station(read).GetBySenseAsync(senseId, _fx.SomaId);

        var teacherFacing = dto!.Axes
            .SelectMany(a => new[] { a.Ask, a.Description ?? "" }.Concat(a.Values.Select(v => v.Name)))
            .ToList();

        Assert.DoesNotContain(teacherFacing, s => s.Contains("تەوەر"));
        Assert.DoesNotContain(teacherFacing, s => s.Contains("axis", StringComparison.OrdinalIgnoreCase));
    }

    // ═══════════════════════════════════════════════════════════════════════
    // ٥. Nothing is ever pre-selected.
    // ═══════════════════════════════════════════════════════════════════════
    [Fact]
    public async Task A_dropdown_never_opens_pre_selected_on_a_real_value()
    {
        var pos = await NewPartOfSpeechAsync("ناو");

        // Five parallel dropdowns, the shape ناو has — every one must open empty.
        var axisIds = new List<int>();

        for (var i = 1; i <= 5; i++)
        {
            var axis = await AddGroupAsync(pos, null, $"پرسیار {i} — {Code()[..4]}");
            await AddValueAsync(axis, $"أ{i}");
            await AddValueAsync(axis, $"ب{i}");
            axisIds.Add(axis);
        }

        var (_, senseId) = await NewSenseAsync(pos);

        await using (var read = _fx.NewContext())
        {
            var dto = await _fx.Station(read).GetBySenseAsync(senseId, _fx.SomaId);

            Assert.Equal(5, dto!.Axes.Count);

            // Not one of them arrives holding a value. The blank «— هەڵبژێرە —» entry is what the
            // control renders as selected, and it can only do that if nothing else claims to be.
            Assert.All(dto.Axes, a => Assert.Null(a.SelectedValueId));
            Assert.All(dto.Axes, a => Assert.Empty(a.SelectedValueIds));
            Assert.All(dto.Axes, a => Assert.False(a.IsNotApplicable));
        }

        // And no row was written for a question nobody answered. A pre-selected control that also
        // persisted its default would be indistinguishable from finished work.
        await using (var read = _fx.NewContext())
        {
            Assert.Equal(0, await read.SenseFeatures.CountAsync(f => f.SenseId == senseId));
        }

        // Answering one leaves the other four empty — a choice is a choice about ONE question.
        await AnswerAsync(senseId, axisIds[2], await FirstValueIdAsync(axisIds[2]));

        await using (var read = _fx.NewContext())
        {
            var dto = await _fx.Station(read).GetBySenseAsync(senseId, _fx.SomaId);

            var answered = Assert.Single(dto!.Axes, a => a.SelectedValueId is not null);
            Assert.Equal(axisIds[2], answered.AxisId);
        }
    }

    /// <summary>
    /// A child dropdown appears empty too. It is the easier one to get wrong: the group was just
    /// revealed by an answer, so "something was picked" is true of the parent and must not leak down.
    /// </summary>
    [Fact]
    public async Task A_child_dropdown_also_opens_empty()
    {
        var pos = await NewPartOfSpeechAsync("کار");

        var kind = await AddGroupAsync(pos, null, "جۆری کار");
        var complete = await AddValueAsync(kind, "تەواو");

        var transitivity = await AddGroupAsync(pos, complete, "تێپەڕی");
        await AddValueAsync(transitivity, "تێپەڕ");
        await AddValueAsync(transitivity, "تێنەپەڕ");

        var (_, senseId) = await NewSenseAsync(pos);
        await AnswerAsync(senseId, kind, complete);

        await using var read = _fx.NewContext();
        var dto = await _fx.Station(read).GetBySenseAsync(senseId, _fx.SomaId);

        var child = dto!.Axes.Single(a => a.AxisId == transitivity);

        Assert.Equal(1, child.Depth);
        Assert.Null(child.SelectedValueId);
        Assert.Empty(child.SelectedValueIds);
    }

    // ═══════════════════════════════════════════════════════════════════════
    // Suggested prompts are flagged, and the flag clears when a human decides.
    // ═══════════════════════════════════════════════════════════════════════
    [Fact]
    public async Task A_suggested_prompt_is_flagged_until_somebody_edits_it()
    {
        var pos = await NewPartOfSpeechAsync("کار");
        var kind = await AddGroupAsync(pos, null, "جۆری کار");
        await AddValueAsync(kind, "تەواو");

        // The migration's one-time pass leaves suggestions in exactly this state.
        await using (var db = _fx.NewContext())
        {
            var axis = await db.FeatureAxes.FirstAsync(a => a.Id == kind);
            axis.PromptKu = "کارەکە چ جۆرێکە؟";
            axis.PromptNeedsReview = true;
            await db.SaveChangesAsync();
        }

        await using (var read = _fx.NewContext())
        {
            var node = await FindNodeAsync(read, pos, kind);

            Assert.True(node.PromptNeedsReview);
            Assert.Equal("کارەکە چ جۆرێکە؟", node.Ask);
        }

        // A human writes their own wording. The suggestion is no longer what is on screen, so the
        // flag has nothing left to warn about.
        await SetPromptAsync(kind, "ئەم کارە چ جۆرە کارێکە؟");

        await using (var read = _fx.NewContext())
        {
            var node = await FindNodeAsync(read, pos, kind);

            Assert.False(node.PromptNeedsReview);
            Assert.Equal("ئەم کارە چ جۆرە کارێکە؟", node.PromptKu);
        }
    }

    // ═══════════════════════════════════════════════════════════════════════
    // An option's worked example reaches the form, and gates nothing.
    // ═══════════════════════════════════════════════════════════════════════
    [Fact]
    public async Task An_option_hint_reaches_the_form_and_is_never_required()
    {
        var pos = await NewPartOfSpeechAsync("کار");

        var transitivity = await AddGroupAsync(pos, null, "تێپەڕی");
        var transitive = await AddValueAsync(transitivity, "تێپەڕ");
        var intransitive = await AddValueAsync(transitivity, "تێنەپەڕ");

        await SetOptionHintAsync(transitive, "بەرکار دەگرێت — «نانەکەی خوارد»");
        // تێنەپەڕ deliberately has none. A hint is help, so most options will never carry one.

        var (_, senseId) = await NewSenseAsync(pos);

        await using (var read = _fx.NewContext())
        {
            var dto = await _fx.Station(read).GetBySenseAsync(senseId, _fx.SomaId);
            var axis = dto!.Axes.Single();

            Assert.Equal("بەرکار دەگرێت — «نانەکەی خوارد»",
                axis.Values.Single(v => v.ValueId == transitive).Hint);

            Assert.Null(axis.Values.Single(v => v.ValueId == intransitive).Hint);
        }

        // The hint changes nothing about what may be saved: a sense with a part of speech and zero
        // answers is complete enough to save, hinted options or not.
        await using (var read = _fx.NewContext())
        {
            var issues = await _fx.Validator(read).ValidateSenseAsync(senseId);
            Assert.DoesNotContain(issues.Issues, i => i.Message.Contains("نموونە:"));
        }

        // Blank clears it, and clearing is not an error.
        await SetOptionHintAsync(transitive, "");

        await using (var read = _fx.NewContext())
        {
            Assert.Null((await read.FeatureValues.FirstAsync(v => v.Id == transitive)).OptionHintKu);
        }
    }

    /// <summary>
    /// The settings tree carries both strings and the hint, so the editor can show the label, the
    /// question and the example without a second round trip per node.
    /// </summary>
    [Fact]
    public async Task The_settings_tree_carries_the_label_the_question_and_the_hint()
    {
        var pos = await NewPartOfSpeechAsync("ناو");

        var gender = await AddGroupAsync(pos, null, "ڕەگەز");
        var masculine = await AddValueAsync(gender, "نێر");

        await SetPromptAsync(gender, "ڕەگەزی وشەکە چییە؟");
        await SetOptionHintAsync(masculine, "وەک «کوڕ»");

        await using var read = _fx.NewContext();
        var node = await FindNodeAsync(read, pos, gender);

        Assert.Equal("ڕەگەز", node.NameKu);
        Assert.Equal("ڕەگەزی وشەکە چییە؟", node.PromptKu);
        Assert.Equal("ڕەگەزی وشەکە چییە؟", node.Ask);
        Assert.Equal("وەک «کوڕ»", node.Values.Single(v => v.ValueId == masculine).OptionHintKu);
    }

    /// <summary>
    /// An answer to a question the classification does not ask is dropped, not stored.
    ///
    /// The word dialog used to render every axis of a part of speech flat and unconditional, so it
    /// could send تێپەڕی for a ناتەواو verb — a question that does not apply. The dialog no longer
    /// resolves the tree itself, but the endpoint is where such a row would LAND, and a payload is
    /// whatever a client sent: an old build, a retried request, or a direct API call.
    ///
    /// So the guarantee is pinned at the resolution layer both write paths share. A stale answer must
    /// not survive, because it is invisible in any form that resolves the tree properly while the work
    /// queue keeps reporting it.
    /// </summary>
    [Fact]
    public async Task An_answer_to_a_question_that_does_not_apply_does_not_survive()
    {
        var pos = await NewPartOfSpeechAsync("کار");

        var kind = await AddGroupAsync(pos, null, "جۆری کار");
        var complete = await AddValueAsync(kind, "تەواو");
        var incomplete = await AddValueAsync(kind, "ناتەواو");

        // تێپەڕی hangs off تەواو only.
        var transitivity = await AddGroupAsync(pos, complete, "تێپەڕی");
        var transitive = await AddValueAsync(transitivity, "تێپەڕ");

        var (_, senseId) = await NewSenseAsync(pos);

        // A client writes BOTH: ناتەواو, and a تێپەڕی answer that ناتەواو does not open. This is
        // exactly the pair the old flat grid could produce.
        await using (var db = _fx.NewContext())
        {
            var classification = new ClassificationService(db);
            await classification.SetFeatureAsync(senseId, kind, incomplete, _fx.SomaId);
            await classification.SetFeatureAsync(senseId, transitivity, transitive, _fx.SomaId);
        }

        await using (var read = _fx.NewContext())
            Assert.Equal(2, await read.SenseFeatures.CountAsync(f => f.SenseId == senseId && !f.IsDeleted));

        // The resolution both write paths run.
        await using (var db = _fx.NewContext())
            Assert.Equal(1, await _fx.Tree(db).ClearStaleAnswersAsync(senseId));

        await using (var read = _fx.NewContext())
        {
            var held = await read.SenseFeatures
                .Where(f => f.SenseId == senseId && !f.IsDeleted)
                .Select(f => new { f.AxisId, f.ValueId })
                .ToListAsync();

            // Only the answer that was actually asked for.
            var only = Assert.Single(held);
            Assert.Equal(kind, only.AxisId);
            Assert.Equal(incomplete, only.ValueId);

            // And the form asks one question, not two.
            var dto = await _fx.Station(read).GetBySenseAsync(senseId, _fx.SomaId);
            var axis = Assert.Single(dto!.Axes);
            Assert.Equal(kind, axis.AxisId);
        }
    }

    /// <summary>
    /// A value is only marked as opening a further question when it really does.
    ///
    /// Half-built is the normal state of a cascade being configured: you add the child question, then
    /// its options. Between those two steps the group is linked but unrenderable, and the form used to
    /// count raw children — so the option was flagged «opens another question», the teacher picked it,
    /// and nothing appeared. The marker has to mean what the resolver does.
    /// </summary>
    [Fact]
    public async Task A_value_is_only_marked_as_opening_a_question_that_can_actually_be_asked()
    {
        var pos = await NewPartOfSpeechAsync("کار");

        var kind = await AddGroupAsync(pos, null, "جۆری کار");
        var complete = await AddValueAsync(kind, "تەواو");

        var (_, senseId) = await NewSenseAsync(pos);

        async Task<StationValueDto> OptionAsync()
        {
            await using var read = _fx.NewContext();
            var dto = await _fx.Station(read).GetBySenseAsync(senseId, _fx.SomaId);
            return dto!.Axes.Single(a => a.AxisId == kind).Values.Single(v => v.ValueId == complete);
        }

        Assert.False((await OptionAsync()).OpensChildGroup);

        // The child question exists but has no options. Linked, unrenderable — and so still not
        // something to promise the teacher.
        var transitivity = await AddGroupAsync(pos, complete, "تێپەڕی");
        Assert.False((await OptionAsync()).OpensChildGroup);

        await using (var read = _fx.NewContext())
        {
            var dto = await _fx.Station(read).GetBySenseAsync(senseId, _fx.SomaId);
            Assert.Single(dto!.Axes);
        }

        // Give it an option and the promise becomes true — and answering the parent now reveals it.
        await AddValueAsync(transitivity, "تێپەڕ");
        Assert.True((await OptionAsync()).OpensChildGroup);

        await AnswerAsync(senseId, kind, complete);

        await using (var read = _fx.NewContext())
        {
            var dto = await _fx.Station(read).GetBySenseAsync(senseId, _fx.SomaId);
            Assert.Equal(2, dto!.Axes.Count);
            Assert.Equal(transitivity, dto.Axes[1].AxisId);
        }

        // Deactivating the child retires the promise too.
        await using (var db = _fx.NewContext())
            await Tree(db).SetAxisActiveAsync(transitivity, false);

        Assert.False((await OptionAsync()).OpensChildGroup);
    }

    // ═══════════════════════════════════════════════════════════════════════
    // Helpers — every configuration goes through the settings service.
    // ═══════════════════════════════════════════════════════════════════════

    private TaxonomyTreeService Tree(AppDbContext db) =>
        new(db, new TaxonomyAdminService(db), _fx.TaxonomyCache);

    private static string Code() => Guid.NewGuid().ToString("N")[..12];

    private async Task<int> NewPartOfSpeechAsync(string name)
    {
        // A row of its own per test: the collection shares one database.
        await using var db = _fx.NewContext();

        var pos = new PartOfSpeech { Code = Code(), NameKu = name };
        db.PartsOfSpeech.Add(pos);
        await db.SaveChangesAsync();

        return pos.Id;
    }

    private async Task<int> AddGroupAsync(int partOfSpeechId, int? parentValueId, string name)
    {
        await using var db = _fx.NewContext();
        return await Tree(db).AddChildAxisAsync(partOfSpeechId, parentValueId, name);
    }

    private async Task<int> AddValueAsync(int axisId, string name)
    {
        await using var db = _fx.NewContext();
        return await Tree(db).AddValueAsync(axisId, name, null);
    }

    private async Task SetPromptAsync(int axisId, string? prompt)
    {
        await using var db = _fx.NewContext();
        await Tree(db).SetPromptAsync(axisId, prompt);
    }

    private async Task SetOptionHintAsync(int valueId, string? hint)
    {
        await using var db = _fx.NewContext();
        await Tree(db).SetOptionHintAsync(valueId, hint);
    }

    private async Task<int> FirstValueIdAsync(int axisId)
    {
        await using var read = _fx.NewContext();

        return await read.FeatureValues
            .Where(v => v.AxisId == axisId)
            .OrderBy(v => v.SortOrder).ThenBy(v => v.Id)
            .Select(v => v.Id)
            .FirstAsync();
    }

    /// <summary>Finds one group in the settings tree at any depth.</summary>
    private async Task<Shared.Dtos.TaxonomyTreeAxisDto> FindNodeAsync(AppDbContext db, int posId, int axisId)
    {
        var tree = await Tree(db).GetTreeAsync(posId);
        Assert.NotNull(tree);

        var node = Flatten(tree!.Axes).FirstOrDefault(a => a.AxisId == axisId);
        Assert.NotNull(node);

        return node!;
    }

    private static IEnumerable<Shared.Dtos.TaxonomyTreeAxisDto> Flatten(
        List<Shared.Dtos.TaxonomyTreeAxisDto> axes)
    {
        foreach (var axis in axes)
        {
            yield return axis;

            foreach (var child in axis.Values.SelectMany(v => Flatten(v.Children)))
                yield return child;
        }
    }

    private async Task AnswerAsync(int senseId, int axisId, int valueId)
    {
        await using var db = _fx.NewContext();

        await new ClassificationService(db).SetFeatureAsync(senseId, axisId, valueId, _fx.SomaId);
        await _fx.Tree(db).ClearStaleAnswersAsync(senseId);
    }

    private async Task<(int WordId, int SenseId)> NewSenseAsync(int? partOfSpeechId)
    {
        var wordId = 0;
        var senseId = 0;

        await _fx.As(_fx.SomaId, "soma", async db =>
        {
            var word = new Word { Kurdish = $"و-{Guid.NewGuid():N}"[..14] };
            db.Words.Add(word);
            await db.SaveChangesAsync();

            var sense = new Sense
            {
                WordId = word.Id,
                Definition = "مانا",
                ExampleUsage = "نموونە",
                PartOfSpeechId = partOfSpeechId,
            };

            db.Senses.Add(sense);
            await db.SaveChangesAsync();

            wordId = word.Id;
            senseId = sense.Id;
        });

        return (wordId, senseId);
    }
}
