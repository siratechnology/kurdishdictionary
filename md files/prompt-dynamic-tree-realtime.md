# پڕۆمپت — درەختی بژاردەکان، ڕێکخستنی داینامیکی، ڕیاڵ تایم

ناو دەست لێ نەدرێت. ئەم پڕۆمپتە بۆ شەشی مایەوەیە و بۆ بەشی ڕێکخستن و ڕیاڵ تایم.

---

```
Read CLAUDE.md and docs/spec.md first.

## What is already correct — do not touch

ناو is finished: five parallel axes, the fifth carrying four values
(گشتی · تایبەت · بەرجەستە · واتای). Do not modify the noun's configuration, its data, or
its rendering. Anything you build here must leave it working exactly as it is now, and you
must prove that with a regression test before you finish.

## What is required and what is optional

- Sense.PartOfSpeechId is REQUIRED TO PUBLISH, but nullable in the database.
  A sense with no part of speech saves fine, sits in WorkflowState.Xam, appears at the top
  of the work queue, and NEVER appears on the public site. A teacher who does not know the
  part of speech must be able to save and move on — forcing a choice produces wrong data
  that looks finished.
- EVERY axis and EVERY sub-option is OPTIONAL, always, at every depth. Nothing below the
  part of speech may ever block a save. IsRequired may exist as a flag that drives the
  work queue and the completeness score, but it must not gate the save button.
- The only hard gate in the whole system is: no publish without a part of speech.

## The options tree — unlimited depth, no new schema

The behaviour wanted: choose a part of speech → a set of options appears → any one of
those options may itself open a further set → and so on, with no fixed limit.

This already exists in the schema. Do NOT add tables and do NOT migrate.
`PartOfSpeechAxis.RequiresValueId` IS the parent link: an axis whose RequiresValueId
points at a FeatureValue is a child group of that value. Chain it and you get depth 3, 4,
5 — as deep as the data goes.

What you must add is the constraint that keeps it a clean TREE rather than an arbitrary
graph, because an arbitrary graph is unmanageable in a settings UI:

- An axis's RequiresValue must belong to an axis assigned to the SAME part of speech.
- That parent axis must itself be either unconditional, or an ancestor of this one.
- No cycles. Reject on save with the offending path named.
- One axis has at most one parent value. Never two.

Resolve the visible set RECURSIVELY by walking the chain against the sense's current
answers. No special case for two levels, no hardcoded depth, no `if (level == 2)`.

Changing an answer at depth N removes every descendant group below it, clears their stored
values, and logs one FeatureCleared event per cleared answer. Nothing stale is ever left
behind.

## Settings: present the same data as a TREE

The data model stays. The presentation changes. Right now a conditional axis is configured
by picking a "required value" from a flat list, which is technically the same thing but
very hard to reason about. Replace that screen with a tree editor:

- Root of the tree: the part of speech.
- First level: its unconditional axes (5 for ناو, 1 for هاوەڵکار, 1 for ئامڕاز, 2 for
  چاوگ, and so on).
- Under each axis: its values.
- Under any value: a button "بژاردەی زیاتر زیاد بکە" that creates a child axis parented to
  that value. This is the whole interaction — one button, at any depth.
- Drag to reorder siblings. Inline rename. Deactivate rather than delete when in use.
- Every node shows its live usage count and, for a value, how many senses hold it.
- Collapse/expand, with the whole tree for one part of speech on one screen.

The person configuring this is a language teacher, not a developer. They must be able to
build کار's full cascade — جۆری کار → تەواو → تێپەڕی → تێپەڕ → whatever comes next —
without ever seeing the words "axis", "conditional", or "requires value".

## Selection count per axis — one, several, or a fixed maximum

Axes differ in how many values a sense may hold. ناو's axes are one-each. Others need two
or three. This must be CONFIGURABLE PER AXIS, not decided in code.

### Schema
Add to FeatureAxis:
  MinSelections  int   default 0    -- 0 means the axis may be left unanswered
  MaxSelections  int?  default 1    -- 1 = single choice · null = unlimited · 3 = up to three

MinSelections drives the work queue and the completeness score ONLY. It must NEVER block a
save — every sub-option stays optional at every depth, as specified above.

### The unique index must change
Today: UNIQUE(SenseId, AxisId) — one value per axis, enforced.
Change to: UNIQUE(SenseId, AxisId, ValueId) — several values per axis, but never the same
value twice on one sense.
This is a loosening, so no existing row can violate it and the migration is safe in that
direction. But:

CRITICAL: the migration MUST set MaxSelections = 1 on every axis that already exists.
ناو must behave exactly as it does today, with no visible change and no data change.
Assert this in a regression test before doing anything else.

### Not-applicable is exclusive
IsNotApplicable and any selected value are mutually exclusive at every axis, single or
multiple. Marking an axis not-applicable clears its selected values; selecting a value
clears the not-applicable flag. Enforce in the service layer, and make the UI show them as
one control, not two independent ones.

### Controls follow the config, not a hardcoded choice
MaxSelections == 1  → radio group / segmented control, choosing one replaces the other
MaxSelections != 1  → checkboxes, with a live "٢ لە ٣ هەڵبژێردراو" counter
At the cap, remaining boxes disable with a one-line reason rather than silently ignoring
clicks. Nothing here may be branched on part of speech or axis name.

### Multi-select changes the cascade — handle this explicitly
If a multi-select axis has children hanging off more than one of its values, and the
teacher selects two such values, TWO child groups open at once. That is correct and must
work, but it requires two things the single-select case never needed:

1. Each child group renders VISUALLY NESTED UNDER THE VALUE THAT OPENED IT — indented,
   labelled with the parent value's name. Never in a flat list. Without this, nobody can
   tell which sub-answer belongs to which parent, and the data becomes unreadable.
2. Deselecting one value clears ONLY that value's descendants. The other selected value's
   subtree and its stored answers are untouched. One FeatureCleared event per cleared
   answer, and not one event more.
Test both. This is the most likely place for a subtle data-loss bug.

### Settings control
On each axis node in the tree editor, one plain-language control:
  "چەند بژاردە دەکرێت هەڵبژێردرێت؟"  →  تەنها یەک · هەرچەند بێت · تا ژمارەیەکی دیاریکراو
Never show the words MinSelections, MaxSelections, or cardinality to the user.
Lowering MaxSelections when senses already hold more than the new limit: warn with the
exact count of senses affected, and do NOT silently truncate their answers — either block
the change or list the senses that need fixing first.

## Live preview beside the tree

The same screen renders the ACTUAL entry form that the current tree produces, with working
cascade behaviour, updating as they edit. Without this they configure blind and discover
the mistake after two hundred words.

## Real-time — what it means concretely

This is Blazor Server, so every user holds a SignalR circuit. Use it.

1. Taxonomy changes propagate to every open session immediately. Hold the taxonomy in a
   singleton cache with a change notification; on any settings write, publish the change
   and have each circuit re-render. No page refresh, no app restart, no logout.
2. Dashboard counters, the work-queue bucket counts, and the contributor list update live
   as work happens — pushed on event, not polled on a timer.
3. Presence and the sense lock are live (see spec prompt 9). Opening a sense someone else
   holds shows their name and current status immediately.
4. Two people editing different senses of the same word both see the other's saved changes
   appear without reloading.

### The hazard real-time creates — handle it explicitly

If a teacher is midway through filling an axis and an admin deactivates or reparents that
axis, naive real-time re-rendering destroys their unsaved input.

Required behaviour instead:
- Never discard a user's in-progress answer because of a remote configuration change.
- Keep the field on screen, mark it with an inline notice ("ئەم بژاردە لەلایەن ئەدمین
  گۆڕدرا"), and let them save what they have.
- Apply the structural change to their form only after they save or explicitly discard.
- If the axis was deactivated, their already-stored answer remains readable in history and
  on the sense — deactivation hides an option from NEW entries, it never erases past ones.
- Show a non-blocking toast, never a modal that steals focus mid-typing.

## Acceptance tests — show each one passing

1. ناو regression: five axes, fifth with four values, unchanged behaviour and data.
2. Save a sense with no part of speech → persists, top of work queue, absent from the
   public site. A second user sets it → they get SenseClassified, the creator keeps
   WordCreated, both in the word's history.
3. Save a sense that HAS a part of speech and NO axis answers at all → succeeds.
4. Build a four-level cascade for کار using only the tree editor: جۆری کار → تەواو →
   تێپەڕی → تێپەڕ → a new child group. It renders correctly in the entry form. No code
   change, no migration.
5. Answer at depth 3, then change the depth-1 answer → all descendants disappear, their
   values are cleared, one FeatureCleared event per cleared answer.
6. Attempt to make axis A conditional on a value under axis B while B is conditional on a
   value under A → rejected, cycle path shown.
7. Two browsers open. Add a value in settings in one → it appears in the other's entry
   form within a second, no refresh.
8. Two browsers open. One is mid-typing in an axis; the other deactivates that axis → the
   typing browser keeps its input, shows an inline notice, and can still save.
9. grep for hardcoded Kurdish taxonomy terms across the solution → zero hits outside seed
   data and test fixtures.
10. ناو regression AFTER the index change: every noun axis still MaxSelections = 1, the
    entry form is unchanged, and no existing sense gained or lost a value.
11. Set an axis to allow up to three values, hang a different child group off two of them,
    select both → both child groups render nested under their own parent value, labelled.
    Answer inside both. Deselect one parent value → only its subtree clears, with exactly
    one FeatureCleared event per cleared answer; the other subtree's answers survive.
12. Mark a multi-select axis not-applicable while it holds two values → the values clear
    and the note is required. Then select a value → the not-applicable flag clears.

## Do not

- Do not add tables or migrate for the tree; RequiresValueId is the parent link.
- Do not touch ناو.
- Do not make any sub-option required to save, at any depth, single or multi-select.
- Do not let the migration change any existing axis away from MaxSelections = 1.
- Do not render child groups of a multi-select axis in a flat list.
- Do not cap the cascade depth.
- Do not invent the values the source deck leaves blank (کار ڕۆنان، جێناوی لکاو's four
  groups، the sub-classification under تێپەڕ). Build the tree so the author adds them from
  settings.
- Do not use polling where SignalR will do.
- Do not let a remote config change destroy unsaved input.
```
