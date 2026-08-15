using backend.Data;
using backend.Data.Models;
using backend.Services.Lexicon;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos;

namespace backend.Tests;

/// <summary>
/// The options tree — unlimited depth, per-axis selection counts, and the cascade.
///
/// Acceptance tests ٢–٨, ١١ and ١٢ from the prompt. ١ and ١٠ (the two ناو regressions) live in
/// <see cref="NounRegressionTests"/>; ٩ (the grep for hardcoded Kurdish taxonomy terms) lives in
/// <see cref="NoHardcodedTaxonomyTests"/> because it reads the source tree rather than the database.
///
/// Every configuration in this file is built through the same service calls the settings screen
/// makes. That is the point of several of the tests: کار's four-level cascade must be reachable with
/// no code change and no migration, and a test that reached into the tables directly would not prove
/// it.
/// </summary>
[Collection("ledger")]
public class OptionsTreeTests
{
    private readonly LedgerFixture _fx;

    public OptionsTreeTests(LedgerFixture fx) => _fx = fx;

    // ═══════════════════════════════════════════════════════════════════════
    // ٢. A sense with no part of speech saves, sits at the top of the work queue,
    //    and never appears on the public site. A second user classifying it earns
    //    SenseClassified while the creator keeps WordCreated — both in the history.
    // ═══════════════════════════════════════════════════════════════════════
    [Fact]
    public async Task A_sense_with_no_part_of_speech_saves_and_waits_in_the_queue()
    {
        var (wordId, senseId) = await NewSenseAsync(partOfSpeechId: null);

        await using (var read = _fx.NewContext())
        {
            var sense = await read.Senses.FirstAsync(s => s.Id == senseId);

            // It saved. A teacher who does not know the part of speech must be able to move on —
            // forcing a choice produces wrong data that looks finished.
            Assert.Null(sense.PartOfSpeechId);
            Assert.Equal(SenseWorkflowState.Raw, sense.WorkflowState);

            // Top of the work queue, in the bucket that is one click per sense.
            var queue = await new WorkQueueService(read).GetSummaryAsync();
            var bucket = queue.Buckets.First();

            Assert.Equal(WorkQueueBucket.MissingPartOfSpeech, bucket.Bucket);
            Assert.True(bucket.Count > 0);

            var items = await new WorkQueueService(read).GetItemsAsync(WorkQueueBucket.MissingPartOfSpeech, 500);
            Assert.Contains(items, i => i.SenseId == senseId);
        }

        // Submitting does NOT publish it. The one hard gate in the system is no publish without a
        // part of speech, and it is the only thing below the headword that refuses anything.
        await using (var db = _fx.NewContext())
        {
            var state = await new ClassificationService(db).SubmitAsync(senseId, _fx.SomaId);
            Assert.Equal(SenseWorkflowState.Raw, state);
        }

        await using (var read = _fx.NewContext())
        {
            Assert.False(await read.Senses
                .AnyAsync(s => s.Id == senseId && s.WorkflowState == SenseWorkflowState.Published));
        }

        // ── A second user classifies it ─────────────────────────────────────
        await _fx.As(_fx.PerjinId, "perjin", async db =>
        {
            var sense = await db.Senses.FirstAsync(s => s.Id == senseId);
            sense.PartOfSpeechId = 1;
            await db.SaveChangesAsync();
        });

        await using var ledger = _fx.NewContext();

        // The creator keeps WordCreated…
        var created = await ledger.ContributionEvents
            .Where(e => e.WordId == wordId && e.EventType == ContributionEventType.WordCreated)
            .ToListAsync();

        Assert.Single(created);
        Assert.Equal(_fx.SomaId, created[0].UserId);

        // …and the classifier gets their own event, on the same word's history.
        var classified = await ledger.ContributionEvents
            .Where(e => e.EntityType == nameof(Sense) &&
                        e.EntityId == senseId &&
                        e.FieldName == nameof(Sense.PartOfSpeechId))
            .ToListAsync();

        Assert.Single(classified);
        Assert.Equal(_fx.PerjinId, classified[0].UserId);
    }

    // ═══════════════════════════════════════════════════════════════════════
    // ٣. A sense that HAS a part of speech and NO axis answers at all saves fine.
    // ═══════════════════════════════════════════════════════════════════════
    [Fact]
    public async Task A_sense_with_a_part_of_speech_and_no_answers_at_all_saves()
    {
        var pos = await NewPartOfSpeechAsync("کار");

        // Give it plenty to not answer, including a "required" one — IsRequired feeds the queue and
        // must not gate anything.
        var axis = await AddGroupAsync(pos, null, "جۆری کار");
        await AddValueAsync(axis, "تەواو");

        await using (var db = _fx.NewContext())
        {
            var assignment = await db.PartOfSpeechAxes
                .FirstAsync(a => a.PartOfSpeechId == pos && a.AxisId == axis);

            assignment.IsRequired = true;
            await db.SaveChangesAsync();
        }

        // Also a minimum, which is the other thing that must never block a save.
        await using (var db = _fx.NewContext())
            await Tree(db).SetSelectionModeAsync(axis, minSelections: 1, maxSelections: 1);

        var (_, senseId) = await NewSenseAsync(pos);

        await using var read = _fx.NewContext();

        Assert.Empty(await read.SenseFeatures.Where(f => f.SenseId == senseId).ToListAsync());

        // The validator reports the gap — as a WARNING. Nothing below the part of speech is ever an
        // error, because a save blocked by a sub-option produces a wrong value that looks finished.
        var result = await _fx.Validator(read).ValidateSenseAsync(senseId);

        Assert.False(result.HasErrors);
        Assert.Contains(result.Issues, i => i.Code == "sense.missing-required-axis");

        // And it saves, and submits.
        await using var db2 = _fx.NewContext();
        var state = await new ClassificationService(db2).SubmitAsync(senseId, _fx.SomaId);
        Assert.NotEqual(SenseWorkflowState.Raw, state);
    }

    // ═══════════════════════════════════════════════════════════════════════
    // ٤. A four-level cascade for کار, built with nothing but the tree editor's own
    //    calls: جۆری کار ← تەواو ← تێپەڕی ← تێپەڕ ← a new child group.
    //    It renders correctly in the entry form. No code change, no migration.
    // ═══════════════════════════════════════════════════════════════════════
    [Fact]
    public async Task A_four_level_cascade_is_built_from_settings_alone_and_renders()
    {
        var pos = await NewPartOfSpeechAsync("کار");

        // Level 1
        var kindOfVerb = await AddGroupAsync(pos, null, "جۆری کار");
        var complete = await AddValueAsync(kindOfVerb, "تەواو");
        await AddValueAsync(kindOfVerb, "ناتەواو");

        // Level 2 — hangs off تەواو
        var transitivity = await AddGroupAsync(pos, complete, "تێپەڕی");
        var transitive = await AddValueAsync(transitivity, "تێپەڕ");
        await AddValueAsync(transitivity, "نەتێپەڕ");

        // Level 3 — hangs off تێپەڕ
        var objectCount = await AddGroupAsync(pos, transitive, "ژمارەی بەرکار");
        var twoObjects = await AddValueAsync(objectCount, "دوو بەرکار");

        // Level 4 — the one the source deck leaves blank. Built from settings, not invented in code.
        var fourth = await AddGroupAsync(pos, twoObjects, "جۆری بەرکاری دووەم");
        await AddValueAsync(fourth, "ڕاستەوخۆ");

        var (_, senseId) = await NewSenseAsync(pos);

        // ── Nothing but the first level is offered yet ──────────────────────
        await using (var read = _fx.NewContext())
        {
            var dto = await _fx.Station(read).GetBySenseAsync(senseId, _fx.SomaId);

            Assert.Single(dto!.Axes);
            Assert.Equal("جۆری کار", dto.Axes[0].Name);
            Assert.Equal(0, dto.Axes[0].Depth);
        }

        // ── Answer down the chain; each answer opens exactly one more group ──
        await AnswerAsync(senseId, kindOfVerb, complete);
        await AssertVisibleAsync(senseId, "جۆری کار", "تێپەڕی");

        await AnswerAsync(senseId, transitivity, transitive);
        await AssertVisibleAsync(senseId, "جۆری کار", "تێپەڕی", "ژمارەی بەرکار");

        await AnswerAsync(senseId, objectCount, twoObjects);

        await using var final = _fx.NewContext();
        var form = await _fx.Station(final).GetBySenseAsync(senseId, _fx.SomaId);

        Assert.Equal(
            new[] { "جۆری کار", "تێپەڕی", "ژمارەی بەرکار", "جۆری بەرکاری دووەم" },
            form!.Axes.Select(a => a.Name).ToArray());

        // Depth-first, each level labelled with the value that opened it — that is what lets the
        // form nest it rather than draw four questions in a row.
        Assert.Equal(new[] { 0, 1, 2, 3 }, form.Axes.Select(a => a.Depth).ToArray());
        Assert.Equal("تەواو", form.Axes[1].ParentValueName);
        Assert.Equal("تێپەڕ", form.Axes[2].ParentValueName);
        Assert.Equal("دوو بەرکار", form.Axes[3].ParentValueName);
    }

    /// <summary>
    /// A group with no values is not rendered — and its parent still is.
    ///
    /// Building a group and then filling in its values is the normal order of work, so a
    /// half-built group is a state the entry form meets constantly. Rendering it produces a
    /// dropdown with nothing in it: a control that cannot be answered and cannot be dismissed,
    /// which reads as the cascade being broken rather than as work in progress.
    /// </summary>
    [Fact]
    public async Task A_group_with_no_values_yet_does_not_render_as_an_empty_dropdown()
    {
        var pos = await NewPartOfSpeechAsync("کار");

        var tense = await AddGroupAsync(pos, null, "دەمژمێر");
        var past = await AddValueAsync(tense, "ڕابردوو");

        // Added, not yet filled in — exactly what the settings screen leaves behind mid-edit.
        var halfBuilt = await AddGroupAsync(pos, past, "تێپەڕی");

        var (_, senseId) = await NewSenseAsync(pos);
        await AnswerAsync(senseId, tense, past);

        await using (var read = _fx.NewContext())
        {
            var form = await _fx.Station(read).GetBySenseAsync(senseId, _fx.SomaId);

            // The parent renders and holds its answer; the empty child is simply absent.
            var only = Assert.Single(form!.Axes);
            Assert.Equal(tense, only.AxisId);
            Assert.Equal(past, only.SelectedValueId);
        }

        // Give it a value and it appears, nested under the value that opens it. Nothing else moved.
        var transitive = await AddValueAsync(halfBuilt, "تێپەڕ");

        await using var after = _fx.NewContext();
        var form2 = await _fx.Station(after).GetBySenseAsync(senseId, _fx.SomaId);

        Assert.Equal(2, form2!.Axes.Count);
        Assert.Equal(halfBuilt, form2.Axes[1].AxisId);
        Assert.Equal("ڕابردوو", form2.Axes[1].ParentValueName);
        Assert.Equal(transitive, form2.Axes[1].Values.Single().ValueId);

        // And the answer already stored on the parent was not disturbed by any of it.
        Assert.Equal(past, form2.Axes[0].SelectedValueId);
    }

    // ═══════════════════════════════════════════════════════════════════════
    // ٥. Answer at depth 3, then change the depth-1 answer → every descendant
    //    disappears, its values are cleared, one FeatureCleared per cleared answer.
    // ═══════════════════════════════════════════════════════════════════════
    [Fact]
    public async Task Changing_a_depth_one_answer_clears_the_whole_subtree_one_event_each()
    {
        var pos = await NewPartOfSpeechAsync("کار");

        var kindOfVerb = await AddGroupAsync(pos, null, "جۆری کار");
        var complete = await AddValueAsync(kindOfVerb, "تەواو");
        var incomplete = await AddValueAsync(kindOfVerb, "ناتەواو");

        var transitivity = await AddGroupAsync(pos, complete, "تێپەڕی");
        var transitive = await AddValueAsync(transitivity, "تێپەڕ");

        var objectCount = await AddGroupAsync(pos, transitive, "ژمارەی بەرکار");
        var twoObjects = await AddValueAsync(objectCount, "دوو بەرکار");

        var (_, senseId) = await NewSenseAsync(pos);

        await AnswerAsync(senseId, kindOfVerb, complete);
        await AnswerAsync(senseId, transitivity, transitive);
        await AnswerAsync(senseId, objectCount, twoObjects);

        await using (var read = _fx.NewContext())
            Assert.Equal(3, await read.SenseFeatures.CountAsync(f => f.SenseId == senseId));

        var clearedBefore = await CountClearedAsync();

        // ── Change the depth-1 answer ───────────────────────────────────────
        await AnswerAsync(senseId, kindOfVerb, incomplete);

        await using var final = _fx.NewContext();

        // Depth 2 AND depth 3 are gone. Depth 3's own parent value was still stored when the change
        // was made — it is the recursive rule that takes both, not a second pass.
        var remaining = await final.SenseFeatures
            .Where(f => f.SenseId == senseId)
            .Select(f => new { f.AxisId, f.ValueId })
            .ToListAsync();

        Assert.Single(remaining);
        Assert.Equal(kindOfVerb, remaining[0].AxisId);
        Assert.Equal(incomplete, remaining[0].ValueId);

        // Exactly one FeatureCleared per cleared answer — two — and not one event more.
        Assert.Equal(clearedBefore + 2, await CountClearedAsync());

        // The form agrees: only the first level is asked again.
        var form = await _fx.Station(final).GetBySenseAsync(senseId, _fx.SomaId);
        Assert.Single(form!.Axes);
    }

    // ═══════════════════════════════════════════════════════════════════════
    // ٦. A rule that closes a cycle is refused, and the offending path is named.
    // ═══════════════════════════════════════════════════════════════════════
    [Fact]
    public async Task A_cycle_is_refused_and_the_path_is_named()
    {
        var pos = await NewPartOfSpeechAsync("کار");

        var axisA = await AddGroupAsync(pos, null, "تەوەری أ");
        var valueA = await AddValueAsync(axisA, "نرخی أ");

        var axisB = await AddGroupAsync(pos, valueA, "تەوەری ب");   // B already hangs off A
        var valueB = await AddValueAsync(axisB, "نرخی ب");

        int assignmentA;
        await using (var read = _fx.NewContext())
        {
            assignmentA = await read.PartOfSpeechAxes
                .Where(a => a.PartOfSpeechId == pos && a.AxisId == axisA)
                .Select(a => a.Id)
                .FirstAsync();
        }

        await using var db = _fx.NewContext();

        // Now try to hang A off a value of B. The form would never be able to decide what to show.
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            Tree(db).ReparentAsync(assignmentA, valueB));

        Assert.Contains("←", ex.Message);
        Assert.Contains("تەوەری أ", ex.Message);
        Assert.Contains("تەوەری ب", ex.Message);
    }

    // ═══════════════════════════════════════════════════════════════════════
    // ٧. Two sessions. A value added in settings appears in the other's entry form
    //    immediately — no refresh, no restart.
    //
    // What is asserted here is the SERVER half: the shared cache is invalidated by the
    // write itself and the change notification fires. The transport on top of it is
    // TaxonomyChangeBroadcaster, which needs a live hub and belongs to a browser test.
    // ═══════════════════════════════════════════════════════════════════════
    [Fact]
    public async Task A_value_added_in_one_session_reaches_another_without_a_refresh()
    {
        var pos = await NewPartOfSpeechAsync("ئامڕاز");
        var axis = await AddGroupAsync(pos, null, "جۆر");
        await AddValueAsync(axis, "یەکەم");

        var (_, senseId) = await NewSenseAsync(pos);

        // ── Session one draws the form, which warms the shared cache ────────
        await using var sessionOne = _fx.NewContext();

        var before = await _fx.Station(sessionOne).GetBySenseAsync(senseId, _fx.SomaId);
        Assert.Single(before!.Axes[0].Values);

        var notified = 0;
        TaxonomyChange? seen = null;

        void Handler(TaxonomyChange change) { notified++; seen = change; }
        _fx.TaxonomyCache.Changed += Handler;

        try
        {
            // ── Session two adds a value in settings ────────────────────────
            await using (var sessionTwo = _fx.NewContext())
                await Tree(sessionTwo).AddValueAsync(axis, "دووەم", null);

            // The write itself announced the change — no settings endpoint has to remember to.
            Assert.True(notified > 0);
            Assert.NotNull(seen);

            // ── Session one, on the SAME long-lived cache, sees it ──────────
            var after = await _fx.Station(sessionOne).GetBySenseAsync(senseId, _fx.SomaId);

            Assert.Equal(2, after!.Axes[0].Values.Count);
            Assert.Contains(after.Axes[0].Values, v => v.Name == "دووەم");
        }
        finally
        {
            _fx.TaxonomyCache.Changed -= Handler;
        }
    }

    // ═══════════════════════════════════════════════════════════════════════
    // ٨. One session is mid-typing; the other deactivates that group. The typing
    //    session keeps its input, and its already-stored answer stays readable.
    //
    // The "keeps its input" half is a Blazor component rule (Station.razor never
    // replaces the form on a remote change). What is asserted here is the half that
    // makes it POSSIBLE: deactivation must not clear a stored answer, and the axis
    // must keep rendering on the sense that answered it.
    // ═══════════════════════════════════════════════════════════════════════
    [Fact]
    public async Task Deactivating_a_group_never_erases_an_answer_that_already_exists()
    {
        var pos = await NewPartOfSpeechAsync("هاوەڵکار");
        var axis = await AddGroupAsync(pos, null, "چەندێتی");
        var value = await AddValueAsync(axis, "زۆر");

        var (_, senseId) = await NewSenseAsync(pos);
        await AnswerAsync(senseId, axis, value);

        // ── The admin retires it while somebody is working ──────────────────
        await using (var admin = _fx.NewContext())
            await Tree(admin).SetAxisActiveAsync(axis, false);

        await using var read = _fx.NewContext();

        // The answer is untouched. Deactivation hides an option from NEW entries; it never rewrites
        // the past, and it is not a cascade — nothing was cleared.
        var stored = await read.SenseFeatures
            .Where(f => f.SenseId == senseId && f.AxisId == axis)
            .ToListAsync();

        Assert.Single(stored);
        Assert.Equal(value, stored[0].ValueId);

        // And it still renders on THIS sense, flagged as retired, so the teacher can still save.
        var form = await _fx.Station(read).GetBySenseAsync(senseId, _fx.SomaId);

        var rendered = Assert.Single(form!.Axes);
        Assert.True(rendered.IsRetired);
        Assert.Equal(value, rendered.SelectedValueId);

        // A sense that never answered it is not asked — the group is gone from new entries.
        var (_, freshSenseId) = await NewSenseAsync(pos);
        var freshForm = await _fx.Station(read).GetBySenseAsync(freshSenseId, _fx.SomaId);

        Assert.Empty(freshForm!.Axes);
    }

    // ═══════════════════════════════════════════════════════════════════════
    // ١١. A multi-select axis with a DIFFERENT child group under two of its values.
    //     Select both → both groups render, nested under their own parent, labelled.
    //     Answer inside both. Deselect one → only its subtree clears, one
    //     FeatureCleared per cleared answer; the other subtree survives.
    //
    // The prompt calls this the most likely place for a subtle data-loss bug.
    // ═══════════════════════════════════════════════════════════════════════
    [Fact]
    public async Task Deselecting_one_value_clears_only_that_values_subtree()
    {
        var pos = await NewPartOfSpeechAsync("جێناو");

        var kind = await AddGroupAsync(pos, null, "جۆری جێناو");
        var independent = await AddValueAsync(kind, "سەربەخۆ");
        var attached = await AddValueAsync(kind, "لکاو");
        await AddValueAsync(kind, "سێیەم");

        // Up to three, so two can be held at once.
        await using (var db = _fx.NewContext())
            await Tree(db).SetSelectionModeAsync(kind, minSelections: 0, maxSelections: 3);

        // A different child group under each of two values.
        var person = await AddGroupAsync(pos, independent, "کەس");
        var first = await AddValueAsync(person, "یەکەم کەس");

        var position = await AddGroupAsync(pos, attached, "شوێن");
        var suffix = await AddValueAsync(position, "پاشگر");

        var (_, senseId) = await NewSenseAsync(pos);

        // ── Select both parents ─────────────────────────────────────────────
        await AnswerAsync(senseId, kind, independent);
        await AnswerAsync(senseId, kind, attached);

        await using (var read = _fx.NewContext())
        {
            var form = await _fx.Station(read).GetBySenseAsync(senseId, _fx.SomaId);

            // Both groups are open at once, each nested under the value that opened it and labelled
            // with that value's name. In a flat list nobody could tell which is which.
            Assert.Equal(3, form!.Axes.Count);

            var kindAxis = form.Axes[0];
            Assert.Equal(2, kindAxis.SelectedValueIds.Count);
            Assert.Equal(3, kindAxis.MaxSelections);

            var personAxis = form.Axes.First(a => a.AxisId == person);
            var positionAxis = form.Axes.First(a => a.AxisId == position);

            Assert.Equal(1, personAxis.Depth);
            Assert.Equal("سەربەخۆ", personAxis.ParentValueName);
            Assert.Equal(independent, personAxis.ParentValueId);

            Assert.Equal(1, positionAxis.Depth);
            Assert.Equal("لکاو", positionAxis.ParentValueName);
            Assert.Equal(attached, positionAxis.ParentValueId);
        }

        // ── Answer inside both ──────────────────────────────────────────────
        await AnswerAsync(senseId, person, first);
        await AnswerAsync(senseId, position, suffix);

        await using (var read = _fx.NewContext())
            Assert.Equal(4, await read.SenseFeatures.CountAsync(f => f.SenseId == senseId));

        var clearedBefore = await CountClearedAsync();

        // ── Deselect ONE parent value ───────────────────────────────────────
        await AnswerAsync(senseId, kind, independent);   // multi-select: picking again toggles off

        await using var final = _fx.NewContext();

        var remaining = await final.SenseFeatures
            .Where(f => f.SenseId == senseId)
            .Select(f => new { f.AxisId, f.ValueId })
            .ToListAsync();

        // سەربەخۆ is gone and so is its ONE descendant answer. لکاو and its subtree are untouched.
        Assert.DoesNotContain(remaining, r => r.ValueId == independent);
        Assert.DoesNotContain(remaining, r => r.AxisId == person);

        Assert.Contains(remaining, r => r.ValueId == attached);
        Assert.Contains(remaining, r => r.AxisId == position && r.ValueId == suffix);

        // Exactly one FeatureCleared, for the one cleared answer. Not one more.
        Assert.Equal(clearedBefore + 1, await CountClearedAsync());

        // And the form shows the surviving subtree, still nested under its own parent.
        var form2 = await _fx.Station(final).GetBySenseAsync(senseId, _fx.SomaId);

        Assert.Equal(2, form2!.Axes.Count);
        Assert.Equal("لکاو", form2.Axes[1].ParentValueName);
        Assert.Equal(suffix, form2.Axes[1].SelectedValueId);
    }

    // ═══════════════════════════════════════════════════════════════════════
    // ١٢. Not-applicable is exclusive, both ways, on a multi-select axis.
    // ═══════════════════════════════════════════════════════════════════════
    [Fact]
    public async Task Not_applicable_and_selected_values_are_mutually_exclusive()
    {
        var pos = await NewPartOfSpeechAsync("چاوگ");
        var axis = await AddGroupAsync(pos, null, "ڕەگەز");

        var a = await AddValueAsync(axis, "یەکەم");
        var b = await AddValueAsync(axis, "دووەم");

        await using (var db = _fx.NewContext())
            await Tree(db).SetSelectionModeAsync(axis, minSelections: 0, maxSelections: null);

        var (_, senseId) = await NewSenseAsync(pos);

        await AnswerAsync(senseId, axis, a);
        await AnswerAsync(senseId, axis, b);

        await using (var read = _fx.NewContext())
            Assert.Equal(2, await read.SenseFeatures.CountAsync(f => f.SenseId == senseId && f.ValueId != null));

        // ── Mark it not-applicable while it holds two values ────────────────
        await using (var db = _fx.NewContext())
        {
            // The reason is not optional — the escape hatch always costs a sentence.
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                new ClassificationService(db).MarkNotApplicableAsync(senseId, axis, "  ", _fx.SomaId));
        }

        await using (var db = _fx.NewContext())
            await new ClassificationService(db).MarkNotApplicableAsync(
                senseId, axis, "ئەم تەوەرە بۆ چاوگ کار ناکات", _fx.SomaId);

        await using (var read = _fx.NewContext())
        {
            var rows = await read.SenseFeatures.Where(f => f.SenseId == senseId).ToListAsync();

            // Both values cleared, one not-applicable row left, with its reason.
            var row = Assert.Single(rows);
            Assert.True(row.IsNotApplicable);
            Assert.Null(row.ValueId);
            Assert.Equal("ئەم تەوەرە بۆ چاوگ کار ناکات", row.Note);

            // کێشەدار is the input for revising the taxonomy, not a rejection.
            var sense = await read.Senses.FirstAsync(s => s.Id == senseId);
            Assert.Equal(SenseWorkflowState.Disputed, sense.WorkflowState);
        }

        // ── Selecting a value clears the not-applicable flag ────────────────
        await AnswerAsync(senseId, axis, a);

        await using var final = _fx.NewContext();
        var after = await final.SenseFeatures.Where(f => f.SenseId == senseId).ToListAsync();

        var only = Assert.Single(after);
        Assert.False(only.IsNotApplicable);
        Assert.Equal(a, only.ValueId);
        Assert.Null(only.Note);
    }

    /// <summary>
    /// The cap says no rather than silently ignoring the click — a control that does nothing reads
    /// as broken, and the teacher clicks it again.
    /// </summary>
    [Fact]
    public async Task Selecting_past_the_cap_is_refused_with_the_limit_named()
    {
        var pos = await NewPartOfSpeechAsync("هاوەڵناو");
        var axis = await AddGroupAsync(pos, null, "پلە");

        var a = await AddValueAsync(axis, "یەکەم");
        var b = await AddValueAsync(axis, "دووەم");
        var c = await AddValueAsync(axis, "سێیەم");

        await using (var db = _fx.NewContext())
            await Tree(db).SetSelectionModeAsync(axis, minSelections: 0, maxSelections: 2);

        var (_, senseId) = await NewSenseAsync(pos);

        await AnswerAsync(senseId, axis, a);
        await AnswerAsync(senseId, axis, b);

        await using var db2 = _fx.NewContext();
        var result = await new ClassificationService(db2).SetFeatureAsync(senseId, axis, c, _fx.SomaId);

        Assert.NotNull(result.Refusal);
        Assert.Contains("2", result.Refusal);

        await using var read = _fx.NewContext();
        Assert.Equal(2, await read.SenseFeatures.CountAsync(f => f.SenseId == senseId));
    }

    /// <summary>
    /// Lowering a cap below what senses already hold is refused with the exact count, and nothing is
    /// truncated. Silently dropping somebody's third answer is a data loss in a place nobody would
    /// think to look for it.
    /// </summary>
    [Fact]
    public async Task Lowering_the_cap_below_stored_answers_is_refused_and_truncates_nothing()
    {
        var pos = await NewPartOfSpeechAsync("ناو");
        var axis = await AddGroupAsync(pos, null, "بوار");

        var a = await AddValueAsync(axis, "یەکەم");
        var b = await AddValueAsync(axis, "دووەم");
        var c = await AddValueAsync(axis, "سێیەم");

        await using (var db = _fx.NewContext())
            await Tree(db).SetSelectionModeAsync(axis, 0, null);

        var (_, senseId) = await NewSenseAsync(pos);

        await AnswerAsync(senseId, axis, a);
        await AnswerAsync(senseId, axis, b);
        await AnswerAsync(senseId, axis, c);

        await using (var db = _fx.NewContext())
        {
            var preview = await Tree(db).PreviewSelectionCapAsync(axis, 1);

            Assert.False(preview.IsSafe);
            Assert.Equal(1, preview.AffectedSenseCount);
            Assert.Equal(3, preview.Affected[0].HeldCount);

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                Tree(db).SetSelectionModeAsync(axis, 0, 1));

            Assert.Contains("1", ex.Message);
        }

        await using var read = _fx.NewContext();

        // Nothing was truncated, and the axis kept its old setting.
        Assert.Equal(3, await read.SenseFeatures.CountAsync(f => f.SenseId == senseId));
        Assert.Null((await read.FeatureAxes.FirstAsync(a2 => a2.Id == axis)).MaxSelections);
    }

    // ═══════════════════════════════════════════════════════════════════════
    // Helpers — every configuration goes through the settings service, never the
    // tables, so these tests prove the tree is reachable without a developer.
    // ═══════════════════════════════════════════════════════════════════════

    private TaxonomyTreeService Tree(AppDbContext db) =>
        new(db, new TaxonomyAdminService(db), _fx.TaxonomyCache);

    private static string Code() => Guid.NewGuid().ToString("N")[..12];

    private async Task<int> NewPartOfSpeechAsync(string name)
    {
        // A row of its own per test: the collection shares one database, so building onto a seeded
        // part of speech would let one test's tree leak into the next one's assertions.
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

    /// <summary>
    /// Answers one axis exactly as the station endpoint does: set the feature, then re-resolve the
    /// tree and clear whatever stopped applying.
    /// </summary>
    private async Task AnswerAsync(int senseId, int axisId, int valueId)
    {
        await using var db = _fx.NewContext();

        await new ClassificationService(db).SetFeatureAsync(senseId, axisId, valueId, _fx.SomaId);
        await _fx.Tree(db).ClearStaleAnswersAsync(senseId);
    }

    private async Task AssertVisibleAsync(int senseId, params string[] expected)
    {
        await using var read = _fx.NewContext();
        var dto = await _fx.Station(read).GetBySenseAsync(senseId, _fx.SomaId);

        Assert.Equal(expected, dto!.Axes.Select(a => a.Name).ToArray());
    }

    private async Task<int> CountClearedAsync()
    {
        await using var read = _fx.NewContext();
        return await read.ContributionEvents
            .CountAsync(e => e.EventType == ContributionEventType.FeatureCleared);
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
