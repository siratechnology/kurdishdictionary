# پڕۆمپت — دوو شێوەی فۆرمی داخڵکردن + ناونیشانە ئاسانەکان

هەموو ئەم فایلە بە یەک جار بدە بە Claude Code.

---

```
Read CLAUDE.md and docs/spec.md first.

## The whole feature in one picture

A single dropdown at the top: part of speech, seven options, single choice. That choice is
the ONLY required field. Below it, a set of dropdowns appears whose SHAPE depends on the
part of speech:

- ناو → FIVE separate dropdowns shown SIDE BY SIDE, all at once, each independent, each a
  single choice.
- everything else → ONE dropdown; choosing a value opens a CHILD dropdown nested beneath
  it, whose options depend on what was chosen; that child may open its own child; and so
  on with no depth limit. A list inside a list.

Both shapes are the SAME underlying data. The only thing that differs is whether a
dropdown is independent (shown in parallel) or conditional (shown nested under its
parent's chosen value). Do not build two systems. Build one, driven by whether a dropdown
has a parent value or not.

## This already exists in the schema — do not migrate

`PartOfSpeechAxis` is a dropdown. `PartOfSpeechAxis.RequiresValueId` is its parent link.
- RequiresValueId == null  → independent dropdown → rendered in the parallel row (ناو).
- RequiresValueId set       → conditional dropdown → rendered nested under that value.
Every dropdown is single-choice: MaxSelections stays 1. Do NOT add multi-select here.
Do NOT add tables. Resolve the nested chain recursively — no hardcoded depth.

## Two different screens

### Screen A — the admin builds the dropdowns (settings)
This is where I define the lists. The teacher never sees it.
- Pick a part of speech.
- Add dropdowns to it. For each, add its options.
- Under any option, an "add a dependent dropdown" button creates a child dropdown parented
  to that option (this sets RequiresValueId). One button, usable at any depth — that is how
  a list-inside-a-list is built.
- For ناو I add five dropdowns and give none of them a parent, so they render in parallel.
- For کار I add جۆری کار, then under its تەواو option I add تێپەڕی, and so on.
- Drag to reorder. Inline rename. Deactivate when in use rather than delete.
- Live usage count on every dropdown and option.
- Beside it, a live preview of the actual teacher form this config produces.

### Screen B — the teacher uses the dropdowns (word entry)
- Choose part of speech.
- ناو → five dropdowns appear side by side. Fill any, in any order. None required.
- کار etc. → one dropdown; on choosing a value its child dropdown appears beneath it,
  indented and clearly hanging off the parent; continue as deep as the config goes. None
  required.
- Changing a parent value removes its child dropdowns and clears their answers (one
  FeatureCleared event each).
- Every field optional. The ONLY gate is: no publish without a part of speech.

## THE LABEL PROBLEM — this is the specific thing to fix

Right now each dropdown shows a raw grammatical label like «جۆری کار» or «تێپەڕی» or
«لە ڕووی ڕەگەزەوە», and a teacher reading «جۆری کار» does not know what is being asked of
them. Fix it with THREE changes together:

1. Labels are editable data, and are meant to be rewritten in plain question form.
   Add to PartOfSpeechAxis (or the dropdown row):
     LabelKu   — the short technical name, kept for admin reference (e.g. جۆری کار)
     PromptKu  — the plain-language question shown to the TEACHER (e.g. کارەکە چ جۆرێکە؟)
   The teacher form shows PromptKu. If PromptKu is empty it falls back to LabelKu.
   The settings tree editor lets me set both, side by side, and shows me exactly what the
   teacher will read.

2. The dropdown itself opens EMPTY with a prompt option, never pre-selected on the first
   real value. The empty option reads "— هەڵبژێرە —". A pre-selected first value is how
   wrong data gets saved silently — the teacher never touched it but it looks answered.

3. Each option MAY carry a short example hint. Add OptionHintKu (nullable) to FeatureValue,
   shown in muted text after the option or beneath the dropdown, e.g. under تێپەڕ show a
   worked example. Never required.

Seed PromptKu for the known dropdowns with natural Kurdish questions rather than leaving
them as bare category names. Where you are unsure of the exact wording, leave PromptKu
null so it falls back to the label, and list those for me to fill — do NOT invent
linguistic phrasing and present it as final.

Suggested starting prompts (I will refine them in settings — mark them as needing review,
do not treat as authoritative):
  ناو ڕەگەز           →  ڕەگەزی وشەکە چییە؟
  ناو ژمارە            →  تاکە یان کۆ؟
  ناو ڕۆنان            →  پێکهاتەی وشەکە چۆنە؟
  ناو ناسراوی          →  ناسراوە یان نەناسراو؟
  ناو تایبەتمەندی/هەبوون →  گشتییە یان تایبەت، بەرجەستە یان واتایی؟
  کار جۆری کار         →  کارەکە چ جۆرێکە؟
  کار تێپەڕی           →  ئایا کارەکە تێپەڕە یان تێنەپەڕ؟

## Real-time

Blazor Server. Taxonomy edits in Screen A propagate to every open Screen B immediately via
the SignalR circuit — no refresh. BUT if a teacher is mid-selection and I change or
deactivate that dropdown, never destroy their unsaved input: keep the field, show an inline
notice, let them save, apply the structural change only after. A deactivated option stays
readable in existing data; it just stops appearing in new entries.

## Acceptance tests — show each passing

1. ناو → exactly five dropdowns render in parallel, each single-choice, none required, the
   fifth offering گشتی/تایبەت/بەرجەستە/واتای. Unchanged from today.
2. کار → one dropdown. Choose تەواو → a child dropdown (تێپەڕی) appears nested beneath it.
   Choose تێنەپەڕ elsewhere → no grandchild. Change تەواو to ناتەواو → the تێپەڕی child
   disappears and its answer is cleared with an event.
3. Using ONLY Screen A, build جۆری کار → تەواو → تێپەڕی → تێپەڕ → a new dependent dropdown
   (a fourth level). It renders nested correctly in Screen B. No code change, no migration.
4. Every dropdown shows PromptKu, not the raw label. Setting PromptKu empty falls back to
   the label. The teacher never sees the word «axis» or «تەوەر».
5. Every dropdown opens on "— هەڵبژێرە —", never pre-selected on a real value.
6. Save a word with a part of speech and zero dropdown answers → succeeds. Save with NO
   part of speech → succeeds, goes top of work queue, stays off the public site.
7. Two browsers: add an option in Screen A → appears in the other's Screen B within a
   second. One mid-typing while the other deactivates that dropdown → typing survives with
   an inline notice, still saves.
8. grep for hardcoded Kurdish taxonomy terms → zero hits outside seed data and fixtures.

## Do not

- Do not touch ناو's five-dropdown behaviour or data.
- Do not build two separate systems for parallel vs nested — one engine, parent-or-not.
- Do not add tables or migrate; RequiresValueId is the parent link.
- Do not make any dropdown below the part of speech required to save.
- Do not pre-select the first option in any dropdown.
- Do not invent final Kurdish prompt wording; leave unknowns null and list them for me.
- Do not cap nesting depth.
```

---

## ئەوەی خۆم دەبێت بیکەم دواتر

دوای ئەوەی ئەمە کاری کرد، دەبێت لە بەشی ڕێکخستنەوە دەقی هەموو درۆپداونەکان (PromptKu)
بگۆڕم بۆ پرسیاری ئاسان. ئەو حەوت نموونەی سەرەوە خاڵی دەستپێکن، بەڵام تۆ باشتر دەزانی
چۆن بۆ مامۆستا ڕوون دەبن.
