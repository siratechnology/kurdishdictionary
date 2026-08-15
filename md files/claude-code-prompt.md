# Claude Code handoff — فەرهەنگی کوردی

**تاکسۆنۆمی لە کۆدەکەدا نییە.** هیچ ناوی بەشی ئاخاوتن، تەوەر، یان نرخ seed ناکرێت —
تیمەکە هەموویان لە بەشی ڕێکخستنەوە داخڵ دەکەن. بۆیە پڕۆمپت ١١ (ڕێکخستن) ئێستا
ڕەهەندێکی سەرەکییە، نەک لاوەکی: ئەگەر ئەو بەشە باش نەبێت، هیچ داتایەک ناچێتە ناوی.

ئەو حەوت پرسیارەی فایلی «هونەری فەرهەنگنووسی» هێشتا دەبێت وەڵام بدرێنەوە — بەڵام ئێستا
وەڵامەکان ڕاستەوخۆ لە ڕووکارەکەدا داخڵ دەکرێن، نەک لە کۆدەکەدا. کۆدەکە پێویستی بەوان نییە
بۆ ئەوەی بەڕێ بکەوێت.

ڕیزبەندی پڕۆمپتەکان زۆر گرنگە. **پرۆڤێنانس (٢) دەبێت پێش کۆچ (٤) بێت** — ئەگەرنا ٢,٨٥٣
وشەی ئێستا بێ‌خاوەن تۆمار دەکرێن.

---

# بەشی ١ — CLAUDE.md

```markdown
# Kurdish (Sorani) Monolingual Dictionary

## Apps
- `jinzar.krd` — public front end. Search, word counter, "نەخشەی مێشک".
- `manage.jinzar.krd` — ASP.NET Core Blazor Server admin.

## Current data (read live, do not trust these — they move weekly)
2,853 words · 7,237 senses · 79 categories · **0 relations** · 11 contributors
Category count went 26 → 79 in one month. The سیفەت/ئاوەڵناو gap went 16 → 117.
Both defects are accelerating.

## Known defects — do not reintroduce
1. Part of speech stored TWICE (`پۆل` and `جۆری وشە`), drifting fast:
   سیفەت 484 vs ئاوەڵناو 601 · ئەنجامدانی کارێک 495 vs کار+چاوگ 542.
2. `relations` table empty, yet every mind-map button reads from it.
3. Public `/about` and `/privacy` return 404 — missing SPA fallback route.
4. Admin returned HTTP 200 with full nav to an anonymous request. Verify auth.

## Linguistic model — SINGLE SOURCE OF TRUTH
Source: "هونەری فەرهەنگنووسی" — موختار جاف، قوتابخانەی فام/سلێمانی.
**Nothing may be added to the taxonomy that is not in that deck.** The 26 categories
currently in the DB are NOT authoritative — they are the defect being repaired.

Three INDEPENDENT dimensions. Never collapse them into one column:
- **A. بەشی ئاخاوتن** — CLOSED list of exactly 7. Never user-extensible.
- **B. تەوەرەکانی ڕێزمانی** — one value per axis per sense. A noun carries SIX at once.
- **C. بواری زانستی** — subject field, hierarchical, orthogonal to A and B.

## Hard rules
- Part of speech belongs to the **sense**, never the word.
  Proof: زۆر and کەم are both هاوەڵناوی نادیار AND هاوەڵکاری چەندێتی.
- Inflected forms (جوانتر، جوانترین، کۆ) are **WordForms**, never separate Word rows.
  Search must match forms and return the headword.
- Morphological relations (ڕەگ، پێکهاتە) sit on **Word**. Semantic relations
  (هاومانا، پێچەوانە) sit on **Sense**. Two tables. Never merge.
- Terminology: **هاوەڵناو / هاوەڵکار / داڕێژراو**. The DB's سیفەت / ئاوەڵناو are wrong.
  The deck itself has typos (هەوەڵناو، هەوەڵکار، داڕێژڕاو) — normalise on seed.
- Every sense requires a usage example in semantic context (deck slide 14). Not optional.

## Provenance rules — absolute
- Contribution counts are NEVER read from columns on the word or sense row.
  They are ALWAYS derived from the append-only `ContributionEvent` table.
- `ContributionEvent` receives INSERT only. No UPDATE, no DELETE, ever.
  Enforce with a DB trigger, not convention.
- No hard deletes anywhere in the lexicon. `IsDeleted` only. Deleting a row would
  delete its contributor's credit.
- A later editor never displaces the original creator. Both events stand forever.
- The ledger exists for ATTRIBUTION and TRACEABILITY, not policing. Contributors are
  expert language teachers whose names go in the published dictionary's credits.
  No XP, no leaderboard, no accuracy score anywhere in the UI.
```

---

# بەشی ٢ — پڕۆمپتەکان

## پڕۆمپت ١ — ناسینەوە، بێ گۆڕانکاری

```
Read CLAUDE.md first, then explore this repo and report back BEFORE changing anything.

1. The exact current EF Core entities for words, senses/meanings, categories, word
   types, and relations. Paste the class definitions.
2. Which column holds `پۆل`, which holds `جۆری وشە`, and how each is populated in the
   admin UI.
3. Is "meanings" (950 rows) a real separate table or a column on the word?
4. Does ANY table currently record who created or edited a row? Names, timestamps,
   user ids — anything. This determines whether the existing 513 words can be
   attributed to their real authors or are already anonymous.
5. What identity system is in use (ASP.NET Identity? external?) and are the current
   contributors' accounts already in it?
6. Does search normalise Kurdish characters at all, and where?
7. Is auth actually enforced on the admin (fallback policy, [Authorize], AuthorizeView)?

Report only. Write no code.
```

## پڕۆمپت ٢ — پرۆڤێنانس (دەبێت پێش کۆچ بێت)

```
Build the contribution ledger FIRST, before any schema or data change. Everything after
this must write to it.

ContributionEvent — append-only, INSERT only
  Id (bigint, identity)
  UserId                  -- never null; migrations use the real original author
  OccurredAt (UTC)
  EventType               -- enum, see below
  EntityType              -- Word | Sense | WordForm | WordRelation | SenseRelation
  EntityId
  WordId                  -- denormalised, so per-word history is one indexed lookup
  FieldName   (nullable)  -- for edits
  OldValue    (nullable)  -- text snapshot
  NewValue    (nullable)
  Note        (nullable)  -- reviewer's reason on reject
  SourceKind              -- Human | Migration | Import

EventType:
  WordCreated · WordEdited · WordDeleted · WordRestored
  SenseCreated · SenseEdited · SenseClassified · SenseReclassified
  FeatureSet · FeatureChanged
  RelationAdded · RelationRemoved
  FormAdded · FormRemoved
  SubmittedForReview · Approved · Rejected · Reopened

Requirements:
- A DB trigger that raises on UPDATE or DELETE against ContributionEvent. Not a
  convention, not an EF interceptor that can be bypassed — a trigger.
- Index: (UserId, EventType), (WordId, OccurredAt), (OccurredAt).
- An EF Core SaveChangesInterceptor that emits events automatically from the change
  tracker, so no service can forget to log. Every changed property becomes one
  FieldName/OldValue/NewValue event.
- Add `IsDeleted` + a global query filter to every lexicon entity. Remove any existing
  hard-delete code path. If you find a `.Remove(` on a lexicon entity, replace it.
- A `ContributionStats` read model (view or materialised, your call) computing per user:
  words created, senses classified, features set, relations added, reviews done,
  approval rate. Derived ENTIRELY from ContributionEvent — no counter columns anywhere.

Test to prove it works, and show me it passing:
  user A creates word W → user B edits W → assert A still has WordCreated=1 and
  B has WordEdited=1, and neither number moves when the other acts.
```

## پڕۆمپت ٣ — سکیمای تاکسۆنۆمی

```
Add the new schema alongside the old. Do NOT drop or alter existing columns yet.

PartOfSpeech        Id, Code, NameKu, SortOrder — seed exactly 7, closed, no admin CRUD:
                    ناو، کار، هاوەڵناو، هاوەڵکار، جێناو، ئامڕاز، چاوگ
FeatureAxis         Id, Code, NameKu, SortOrder
FeatureValue        Id, AxisId, NameKu, SortOrder
PartOfSpeechAxis    PartOfSpeechId, AxisId, IsRequired, RequiresValueId (nullable)
                    RequiresValueId = CONDITIONAL axis: only offered when the sense
                    already holds that value on another axis.
SenseFeature        SenseId, AxisId, ValueId
                    UNIQUE(SenseId, AxisId) enforced in the DB.
                    Service-layer validator: (Sense.PartOfSpeechId, AxisId) must exist
                    in PartOfSpeechAxis.
Domain              Id, NameKu, ParentId (nullable), SortOrder
WordForm            Id, WordId, Form, FormTypeId, Normalized
WordFormType        Id, NameKu — پلەی بەراورد، پلەی باڵا، کۆ
WordRelation        Id, FromWordId, ToWordId, TypeId    -- morphological
SenseRelation       Id, FromSenseId, ToSenseId, TypeId  -- semantic
RelationType        Id, Code, NameKu, Scope (Word|Sense), IsSymmetric, InverseId

Sense gains: PartOfSpeechId (required), DomainId (nullable), ExampleUsage (required,
non-empty), WorkflowState, Completeness (computed).
Word gains: Normalized.
Every one of these tables gets IsDeleted and is covered by the interceptor from
prompt 2.

### NO SEED DATA — this is a configuration engine

Do NOT hardcode any part-of-speech names, axis names, or feature values. The team enters
all of it through the settings area (prompt 11). Your job is the engine and the UI; the
linguistic content is theirs.

Seed exactly two things and nothing else:
- The seven PartOfSpeech rows, as empty named records they can rename. Nothing else about
  them is fixed.
- The RelationType rows below, because relation semantics are structural, not editorial.

Everything else ships EMPTY. The app must work correctly with a completely empty
taxonomy: the entry form for a part of speech with zero configured axes shows the
headword, definition and example fields and saves fine. No crash, no blank panel, no
"loading" that never resolves. Test this state explicitly — it is day one.

### Cascading is N-level, not two

The flow the team described: pick one of the seven, and the axes for that type appear.
But depth goes further than two levels. جێناو → سەربەخۆ → کەسی/پرس/نیشانە is three, and
there is no reason a fourth won't appear later.

`PartOfSpeechAxis.RequiresValueId` already gives unlimited depth without a schema change:
axis C requires a value on axis B, which itself required a value on axis A. Resolve the
visible axis set by walking the chain against the sense's current answers, recursively.
Do not build a two-level special case — it will need replacing.

Rendering rules:
- On selecting a part of speech, load its axes and render every unconditional one at once.
  A noun may well end up with six parallel axes; they are answered together, not in
  sequence.
- A conditional axis appears the moment its required value is chosen, and is REMOVED
  (with its stored answer cleared, logged as an event) if that value is changed away.
  Never leave a stale answer on an axis that no longer applies.
- Never render a disabled or greyed-out inapplicable axis. It is absent or it is present.

### Configuration validation — enforce in the service layer

The team is entering this config, so the engine must refuse invalid states:
- No cycles in RequiresValueId. Reject on save with the cycle path shown.
- A RequiresValueId must reference a value on an axis assigned to the SAME part of speech.
- An axis marked required whose condition can never be satisfied is a contradiction —
  reject it.
- A value being used as another axis's condition cannot be deleted, only deactivated, and
  the UI must name what depends on it.
- Two axes on one part of speech may not share a name.

### Live preview in settings — required

After configuring a part of speech, the settings screen renders the ACTUAL entry form
that configuration produces, with working conditional behaviour, on the same page. Without
it the team configures blind and finds out after two hundred words.

### Changing the config after data exists

Any change to axes, values, or assignments shows the count of already-classified senses
affected before it commits, and routes through the deactivate/merge rules in prompt 11.
Never silently orphan an answer.

### RELATIONTYPE SEED
Word scope:  ڕەگ (inverse: داڕێژراو لێی) · پێکهاتە (inverse: بەشێکە لە) ·
             چاوگی کارەکە (inverse: کاری چاوگەکە) · زاراوەی هەرێمی (symmetric)
Sense scope: هاومانا (symmetric) · پێچەوانە (symmetric) ·
             مانای گشتیتر (inverse: مانای وردتر)
These are renameable but the Scope and Inverse wiring is not editable — a symmetric
relation cannot be made directional without breaking the auto-inverse logic in prompt 6.

### Domains ship empty too
The ten dictionaries from deck slide 13 are entered by the team in settings (prompt 12),
not seeded. The Domain table and its hierarchy are yours to build; the names are theirs.

Generate the migration. Do not apply it. Show me the file.
```

## پڕۆمپت ٤ — کۆچی داتا، دوو قۆناغ

```
Migrate the existing 513 words / 950 senses / 26 categories. Two phases, and every
write goes through the ledger from prompt 2 with SourceKind=Migration.

ATTRIBUTION: use the real original author for each row wherever prompt 1 found one.
Only where no author exists may UserId fall back to a dedicated `legacy-import`
account — and then log how many rows that was, loudly, in the summary.

Phase 1 — PROPOSE. Emit CSV to ./migration-review/:
  sense_id, word, current_پۆل, current_جۆری_وشە, proposed_PartOfSpeech,
  proposed_Domain, proposed_axis_values, confidence, conflict_reason

Rules:
- جۆری وشە → PartOfSpeech: ناو→ناو · ئاوەڵناو|سیفەت→هاوەڵناو · کار→کار ·
  چاوگ→چاوگ · ئاوەڵکار→هاوەڵکار
- پۆل → Domain, NOT PartOfSpeech. کەرەستە، کەس، شوێن، ڕووەک، نەخۆشی are subject
  fields. Nest under the deck's 10: نەخۆشی under پزیشکی, ڕووەک under ڕووەکناسی, etc.
- Where a category is a part of speech in disguise (سیفەت، ئەنجامدانی کارێک), flag it
  as a conflict. Do not silently route it to Domain.
- Where پۆل and جۆری وشە disagree, confidence=LOW plus conflict_reason. Do not guess.
- Propose axis values ONLY where mechanically derivable. Leave the rest blank.
  Never invent grammatical features.
- Any existing category value with no home in the deck's taxonomy goes in a separate
  `unmapped.csv`. Do not force it. Do not create a new axis for it.

Phase 2 — APPLY, a separate command reading the reviewed CSV back. It must refuse any
row where confidence=LOW and no human value was supplied.

Run phase 1 only. I review the CSV by hand.
```

## پڕۆمپت ٥ — گەڕانی کوردی و فۆرمەکان

```
1. One Kurdish normalisation function, used at BOTH write time (Word.Normalized,
   WordForm.Normalized) and query time. Fold:
   ي/ی/ى → ی · ك/ک → ک · final ه / ھ / ە → ە · ر/ڕ collapsed for matching ·
   و/وو collapsed · strip ZWNJ (U+200C), tatweel, Arabic diacritics.
   Unit-test with: جوان/جوانی · ڕەگ/رەگ · کورد/كورد.

2. Forms searchable. A query for جوانترین must match WordForm.Normalized and return the
   parent Word's entry — never a separate result row, never zero results. Show which
   form matched: "جوانترین — پلەی باڵای جوان".

Index Normalized on both tables.
```

## پڕۆمپت ٦ — ڕێساکان و ڕیزی کار

```
Service-layer validators (not UI-only):
- ڕۆنان = داڕێژراو → the word MUST have ≥1 ڕەگ WordRelation.
- ڕۆنان = لێکدراو → ≥2 پێکهاتە WordRelations.
- A هاوەڵناو at پلەی بەراورد or پلەی باڵا must exist as a WordForm of a چەسپاو
  headword, not as its own Word.
- ژمارە = کۆ on a standalone Word → flag: should this be a WordForm of the تاک?
- Every sense: PartOfSpeechId set · all required axes for that POS filled · every
  conditional axis either satisfied or correctly inapplicable · ExampleUsage non-empty.

Saving a ڕەگ relation auto-creates its inverse via RelationType.InverseId; same for
symmetric types. Never make anyone enter an edge twice. The inverse is one
RelationAdded event, attributed to the same user.

Add a "ڕیزی کار" page listing what is incomplete, cheapest-to-fix first:
  1. Senses missing PartOfSpeech
  2. Senses missing a required axis value
  3. داڕێژراو words with no ڕەگ link
  4. لێکدراو words with <2 پێکهاتە links
  5. Senses with no ExampleUsage
  6. Senses with no هاومانا or پێچەوانە
Each row deep-links to the edit form with that field focused. Counts per bucket on the
dashboard, REPLACING the current "جۆرەکانی پەیوەندی" chart that reads an empty table.
```

## پڕۆمپت ٧ — ژووری ئۆپەڕاسیۆن

```
Build the operations room. Read this framing first, because it changes the design:

The people using this are EXPERT LANGUAGE TEACHERS, not crowd workers. The ledger from
prompt 2 exists for ATTRIBUTION and TRACEABILITY, not policing. Their names will appear
in the published dictionary's contributor credits, attached to the sections they built —
that is the motivation, and the ledger is its evidence. Design accordingly: no XP, no
leaderboard, no accuracy score. Do not build anything that reads as surveillance of a
professor.

### Workflow states
Sense.WorkflowState: خام → پۆلێنکراو → بڵاوکراو  (plus کێشەدار, see disputes)

Trust levels on the user: Contributor | Senior.
- Senior publishes directly: پۆلێنکراو and بڵاوکراو in one action. No waiting on anyone.
- Contributor's work goes to a Senior's queue.
- A background sampler flags ~5% of Senior-published senses for a consistency read —
  checking terminology uniformity across contributors, NOT correctness. Surface it as
  "یەکڕێزی زاراوە", never as review of the teacher.

### Claim lock
`SenseClaim`: SenseId, UserId, ClaimedAt, ExpiresAt (+30 min). Taking the next word
claims it; expired claims are reclaimable. Show the holder's name if someone opens a
claimed word. This is concurrency control, not oversight — two experts opening the same
word simultaneously loses one of their contributions.

### The station screen — single sense
Headword and definition large. Axes for that part of speech as segmented controls, in
deck order. Example-usage field. Relation quick-add for ڕەگ / هاومانا / پێچەوانە with
type-ahead over existing words.
Keyboard-first: digits pick axis values, Enter saves and advances, Esc releases the claim.
Conditional axes appear ONLY when their RequiresValue is satisfied — never render a
disabled تێپەڕی on a ناتەواو verb. Never show an axis outside that part of speech.

### Escape hatch — required, do not omit
A teacher will hit words the taxonomy does not fit. If the form forces a value to save,
they will pick a WRONG one, which is worse than blank. So:
  "ئەم تەوەرە بۆ ئەم وشە کار ناکات" — sets the axis to Inapplicable, requires a free-text
  note, and routes the sense to a کێشەدار queue. Saving is never blocked by it.
The کێشەدار queue is the input for revising the source taxonomy itself. Give it its own
page grouped by axis, so a pattern across 20 words is visible at a glance.

### Disagreement, not error
When a second teacher changes another's classification, do NOT mark the first wrong.
Record it as a disagreement on that sense: both judgements, both names, both notes,
visible together. زۆر is هاوەڵناو AND هاوەڵکار in the source deck itself — legitimate
disagreement is linguistic data, and losing it loses information.

### Bulk operations — required for expert speed
Filter senses (e.g. "all ناو senses with no ڕەگەز value") and apply one axis value to
the whole filtered set in one action, with a confirm step showing the exact count.
Each affected sense still emits its OWN FeatureSet event, so per-word history stays
complete. A bulk action must never collapse into a single log entry.

### Contributor credit page
Per teacher: words created, senses classified, relations added, forms added, broken down
BY DOMAIN (بواری زانستی) — because that is what goes in the printed credits
("بەشی پزیشکی: ...", "بەشی ڕووەکناسی: ..."). All derived from ContributionEvent, no
counter columns. Add an export of this as a formatted contributor list.

### Per-word history page
Every event on a word: who, when, old → new, and any note. This is the proof that a
teacher's work still exists. It must never be able to show a gap.

### Sanity test, and show it passing
Soma creates 1000 words. Perjin reclassifies 300. Assert:
  Soma.WordsCreated == 1000 (unchanged)
  Perjin.SensesClassified == 300 and Perjin.WordsCreated == 0
  all 300 histories show BOTH names in order
Then soft-delete one word and assert Soma.WordsCreated is still 1000.
Then bulk-set ڕەگەز on 50 senses and assert 50 separate FeatureSet events exist.
```

## پڕۆمپت ٨ — دیزاینی نوێ: تێمپلەیتی «Donezo»، سەوز، RTL

```
Rebuild the admin's visual layer to match the reference dashboard I'm giving you
(a Donezo-style admin template). Current state: purple/magenta gradient hero, four
gradient stat cards in red/orange/green/blue, random per-user colour bars. All of that
goes. Reproduce the reference's LAYOUT LANGUAGE and green palette, mirrored for RTL
Kurdish. Build the layout and CSS yourself — do not copy the reference's logo, wordmark,
illustrations, or any asset from it.

### 0. Tokens — define once, use nowhere else
--page:        #F1F2F0     /* neutral page behind the app shell */
--card:        #FFFFFF
--border:      #E6EAE6
--green-deep:  #103B2C     /* filled hero cards, primary buttons, dark tiles */
--green:       #1F6E4C     /* charts, active indicator, gauge arc */
--green-soft:  #A9D5BE     /* secondary chart series */
--green-tint:  #DCF3E5     /* status pills, avatar backgrounds */
--on-tint:     #10402C     /* text on --green-tint */
--text-1:      #16211B
--text-2:      #6A776E
--text-3:      #9AA69E
--warn:        #B45309  on  #FDF3E3
--danger:      #B42318  on  #FCEBEB
Radii: shell 24px · card 16px · pills 999px · chips 8px
Spacing: card padding 20px · grid gap 16px · shell outer margin 20px

### 1. App shell — the signature of this template
The page background is --page. The ENTIRE app (sidebar + content) sits inside one white
container with 24px radius, a 20px margin all round, and a very soft shadow
(0 1px 3px rgba(0,0,0,0.04), 0 8px 24px rgba(0,0,0,0.04)). It reads as a window floating
on neutral paper. Nothing bleeds to the browser edge.

### 2. Sidebar — WHITE, not dark
Same white as the content, separated only by a 1px --border. Contents top to bottom:
- Logo mark (green) + wordmark "فەرهەنگی کوردی", 20px, weight 500.
- Section label "مێنیو" — 11px, --text-3, letter-spacing 0.08em.
- Nav items: 20px outline icon + 15px label, --text-2, 10px vertical padding.
  ACTIVE item: --text-1 at weight 500, background --green-tint, and a 3px --green bar on
  the RIGHT edge (RTL mirror of the reference's left bar), radius 0 on that edge.
  Items: داشبۆرد · وشەکان · پەیوەندییەکان · پۆل و جۆرەکان · ڕیزی کار · بەکارهێنەران
- Counter pill on ڕیزی کار showing outstanding items, e.g. "٢٣٧" — --green-tint bg,
  --on-tint text, 11px, pill radius. This is the template's "12+" badge.
- Section label "گشتی" then: ڕێکخستن · دەرچوون
- Bottom: a --green-deep filled card, 16px radius, 16px padding, white text —
  "ڕێنمایی فەرهەنگنووسی" with a one-line subtitle and a --green-tint pill button
  "بیخوێنەوە". This is the reference's mobile-app promo slot, repurposed.

### 3. Top bar
- Search: pill input, full radius, --page background, no border, magnifier icon on the
  RIGHT (RTL). A keycap chip "⌘K" on the LEFT end — 11px, --card bg, 1px --border, 6px
  radius.
- Two circular ghost icon buttons (36px, --page bg): bell, and one more.
  Notification count as a small dot, not a number.
- User block: avatar circle, then name (14px/500) above email (12px/--text-2).

### 4. Page header
Title "داشبۆرد" at 28px weight 500 --text-1, subtitle 14px --text-2 beneath.
On the opposite side, two pill buttons:
  primary   — --green-deep fill, white text, plus icon: "وشەی نوێ"
  secondary — --card bg, 1px --border, --text-1: "هێنانی داتا"

### 5. Metric cards — 4 across, gap 16px
Every card: 16px radius, 20px padding, and a 28px circular ghost button in the corner
with an arrow icon (mirrored ↖ for RTL) that deep-links to that section.
Anatomy: 13px label · 34px/500 number · footnote row of a 16px icon + 12px --text-2.

Card 1 is the HERO: --green-deep fill, white number, --green-soft label and footnote.
Cards 2-4: white, 1px --border.
Assign them:
  1 وشەی تۆمارکراو   2,853   hero, footnote "+٩٦٦ لەم حەفتەیە"
  2 واتای تۆمارکراو   7,237   footnote "٩٦% وشەکان واتایان هەیە"
  3 پەیوەندی          0       number and footnote in --danger, card bg --danger tint
  4 پۆلەکان           79      number and footnote in --warn, card bg --warn tint
Compute states 3 and 4 from thresholds — never hardcode a colour per card. The hero fill
is visual hierarchy, NOT a "good" signal; that is why states still need warn/danger.

### 6. Content grid — 3 columns, 2fr / 1.5fr / 1.5fr, gap 16px
All cards white, 16px radius, 1px --border, 20px padding, 15px/500 titles.

a) چالاکی هەفتانە — bar chart, words added per day, 7 bars.
   THE TEMPLATE'S KEY TRICK: bars for real/completed data are solid --green with fully
   rounded tops; bars for projected or incomplete data use a DIAGONAL HATCH pattern
   (SVG <pattern> of 45° --border lines on transparent), not grey. One bar carries a
   floating value chip (--green-deep bg, white 11px text) as a tooltip.
   Day initials beneath in 11px --text-3, RTL order.

b) ڕیزی کار — the next actionable item: 16px/500 title, a --text-2 line beneath, then a
   full-width --green-deep pill button with an icon: "دەستپێبکە".

c) نوێترین وشەکان — rows of: a 32px rounded-square icon tile in --green-tint, then the
   word at 14px/500 with "زیادکرا ٢ خولەک لەمەوبەر" at 12px --text-2 beneath.
   A small "+ نوێ" pill button in the card header.

d) ئامادەکاران — rows of avatar + name (14px/500) + a subtext line that reads
   "لەسەر وشەی «X» کار دەکات" with the word portion in --text-1/500 and the rest in
   --text-2 (the reference's "Working on ..." pattern exactly). A status pill on the far
   side: چالاک = --green-tint/--on-tint · بێ‌چالاکی = --warn tint · دەرچوو = --page bg
   with --text-2. A presence dot on the avatar's corner. Header carries a
   "+ زیادکردنی مامۆستا" pill.
   This card's data comes from prompt 9.

e) کوالیتی داتا — a semicircular gauge, thick stroke, rounded caps. The completed arc is
   --green; the remainder uses the SAME diagonal hatch as the bar chart. Large centred
   percentage at 30px/500 with a 12px --text-2 caption beneath. Show the relations
   coverage: "٠%" / "وشەکان پەیوەندییان هەیە" — an entirely hatched ring with no green
   at all. Legend beneath: three dots with 12px labels.

f) کاتی کارکردنی ئەمڕۆ — a --green-deep filled tile. White label, then the elapsed
   session time at 30px in --font-mono (keep digits LTR inside an RTL page), then two
   circular buttons: white pause, --danger stop.

### 7. RTL rules
dir="rtl" at the root. Everything mirrors: sidebar to the right, active indicator on the
right edge of the nav item, card corner buttons to the left, search icon to the right,
chart categories right-to-left.
Numerals, times, and percentages stay LTR — wrap them so they don't reorder.
Pick ONE Arabic-script family globally with a Latin fallback for digits: Noto Kufi
Arabic, Vazirmatn, or Rabar. The current build is inheriting a mismatched fallback for
numerals and Latin labels.

### 8. Hard prohibitions
No gradients anywhere — the reference has none. No coloured drop shadows. No per-user
colour coding. No decorative accent stripes under headings. Two font weights only,
400 and 500 — never 600 or 700. Sentence case, never ALL CAPS.

### 9. Order of work
Build the shell, sidebar, top bar and page header first and show me a screenshot before
touching the content grid. If the shell is wrong, everything inside it is wrong.
```

## پڕۆمپت ٩ — دۆخی چالاکی و دوایین بینین

```
Add presence. This app is Blazor Server, so every user already holds a SignalR circuit —
do NOT build polling or a second WebSocket. Use CircuitHandler.

### States — three, not two
چالاک      input within the last 2 minutes
بێ‌چالاکی   circuit open, no input for 2+ minutes
دەرچوو     circuit closed  → show "دوایین جار X پێش ئێستا"

An open circuit does NOT mean active. A teacher leaves the tab open and walks away.
Derive presence from LastActivityAt, not from circuit state alone.

### Implementation
- `CircuitHandler`: OnCircuitOpenedAsync / OnConnectionUpAsync mark online;
  OnConnectionDownAsync / OnCircuitClosedAsync mark offline.
- Client-side activity: a small JS listener on pointermove/keydown/visibilitychange,
  throttled to at most one call per 30s, invoking a `Heartbeat()` hub method that bumps
  LastActivityAt. Throttling matters — do not send an event per keystroke.
- Store live state in a singleton ConcurrentDictionary keyed by UserId; flush
  LastActivityAt to the DB every 60s and on disconnect, so "دوایین جار" survives a
  restart. Do NOT write to the DB on every heartbeat.
- SignalR keepalive means a closed browser takes ~15-30s to register. Show
  "بێ‌چالاکی" during that gap rather than flipping straight to دەرچوو — never claim
  someone is offline before you know it.
- If the app ever runs on more than one instance, the in-memory dictionary breaks. Add a
  Redis backplane behind an interface now, even if it uses the in-memory impl today.

### UserPresence
  UserId · Status · LastActivityAt · LastSeenAt · CurrentPage · CurrentSenseId (nullable)

### Where it surfaces
- Contributor list: a status dot on the avatar, and one line of subtext —
  "لەسەر وشەی «X» کار دەکات" when CurrentSenseId is set, otherwise
  "بێ‌چالاکی ٧ خولەک" or "دوایین جار ٣ کاتژمێر پێش ئێستا".
- Header: a live count of who is currently چالاک.
- THIS IS ALSO THE CLAIM LOCK from prompt 7. CurrentSenseId and SenseClaim are the same
  fact — implement presence and the lock as one feature, not two. Opening a sense someone
  else holds shows their name and their live status.

### Restraint
Show exact last-seen durations and per-page location on the users admin page only. To
peers, show the dot and "چالاک / بێ‌چالاکی / دەرچوو" without precise durations. These are
teachers, not agents on a queue — presence is for coordination, not monitoring.
```

## پڕۆمپت ١٠ — ئەو باگی ١٠ وشە

```
The dashboard contradicts itself. The "وشەی تۆمارکراو" card reads 2,853. The contributor
list sums to 2,863 across its 11 rows. A 10-word gap.

Find both queries. They are counting different sets — likely one filters something
(soft-deleted rows, a null owner, an unpublished state) that the other does not, or one
counts senses where the other counts words.

Then make the leaderboard read from the SAME source as the card. Until these agree, no
contributor's number can be trusted, and the whole point of the credit system collapses.

Add a test asserting card total == sum of the contributor list, and have it fail loudly
in CI if they ever diverge again.
```

## پڕۆمپت ١١ — بەشی ڕێکخستنی تاکسۆنۆمی

```
Build a settings area where the taxonomy itself is managed: parts of speech, feature
axes, feature values, the axis-to-POS mapping, conditional rules, and domains.

READ THIS FIRST. The disease in this project is category sprawl — 26 categories became
79 in one month because creating one was easy. If this settings page makes adding easy
for everyone, we rebuild that same bug in a new table. So EDITING and EXTENDING are
separate permissions, deliberately.

### Three tiers
Tier 1 — any admin:
  rename · reorder (SortOrder) · deactivate/reactivate · edit description
  These are the operations needed right now: ئاوەڵناو→هاوەڵناو، هەوەڵکار→هاوەڵکار،
  داڕێژڕاو→داڕێژراو، and the «واتای»/«واتایی» decision.
Tier 2 — a single designated linguistic owner role:
  add or remove a VALUE inside an existing axis · merge values · edit a conditional rule
Tier 3 — elevated role AND a mandatory free-text reason, stored on the event:
  add or remove an AXIS · add or remove a PART OF SPEECH · add a top-level domain
  A part of speech is a linguistic claim, not a data-entry convenience. The seven are
  closed by default; opening that list must feel deliberate and must be explained.

### Rename is UPDATE, never delete-and-insert
A rename keeps the same primary key. If the implementation inserts a new row and
repoints, every historical reference and every ContributionEvent pointing at the old id
breaks. Write a test that renames a value in use by 400 senses and asserts all 400 still
resolve, and that the row's Id is unchanged.

### Never hard-delete a value that is in use
Deletion is permitted ONLY at usage count zero. Otherwise the action is Deactivate:
IsActive=false, so the value stops appearing in entry dropdowns but existing data still
renders it and history stays intact. Show deactivated values in a collapsed section, not
hidden.

### MERGE — the operation they actually need
This is the feature that repairs the 79 categories. Do not ship the settings page
without it.
  Pick a source value and a target value → preview screen showing the exact count of
  rows that will move ("٤٨٤ مانا لە سیفەت دەچنە هاوەڵناو") and any row that would end up
  with a duplicate value on the same axis → confirm → repoint all references in one
  transaction → deactivate the source.
Each moved row emits its OWN event (see prompt 2) so per-word history stays complete. A
merge must never collapse into a single log line.
Merges must be reversible for 30 days: record the source id on each moved row so an
undo can put them back.

### Usage counts everywhere
Every value, axis, and domain row in this settings area shows its live usage count and
the date it was last used. Sort by count ascending by default — that surfaces the long
tail immediately. Right now nobody can see that most of the 71 remaining categories
probably hold two or three words each. Once the count is visible, the sprawl becomes
self-correcting.
Add a filter: "بەکارنەهاتووەکان" (count = 0) with a bulk deactivate.

### Axis-to-POS mapping is editable, not hardcoded
A screen showing the 7 parts of speech down one side and the axes across, with
checkboxes for assignment plus an IsRequired toggle per pairing. Changing an assignment
must warn with the count of senses affected.

### Conditional rules are editable, not hardcoded
The rule "تێپەڕی appears only when جۆری کار = تەواو" is data (PartOfSpeechAxis
.RequiresValueId), configurable in the UI. Same for "جۆری سەربەخۆ appears only when
جۆر = سەربەخۆ". No developer should be needed to wire a new conditional rule.
Validate on save: a rule may not reference a value on an axis that isn't assigned to the
same part of speech, and rules may not form a cycle.

### Empty vs not-applicable — keep them distinct
A sense with no ڕەگەز value is not the same as a ناتەواو verb having no تێپەڕی. The
first is unfinished work; the second is a complete answer. Store them differently
(null vs an explicit NotApplicable marker) or the work queue will list finished senses
forever with no way to clear them. The settings area is where a NotApplicable marker per
axis gets enabled.

### Audit
Every action here writes to ContributionEvent. Taxonomy edits are the highest-impact
changes in the system — one rename touches thousands of rows — so they need the trail
most. Tier 3 actions store the mandatory reason in the event's Note.

### Order of work
Ship rename + reorder + usage counts first, in one pass. That alone lets the team fix
the terminology today. Merge second. Tier 3 last.
```

## پڕۆمپت ١٢ — چوارچێوەی فەرهەنگ (بواری کارکردن)

```
Every sense belongs to one of the deck's ten dictionaries (slide 13). A user picks one as
their working scope, and that choice PERSISTS across sessions, browsers, and devices
until they change it.

### The ten — store without the «فەرهەنگی» prefix, render it in the UI
کولتووری · زانستیی پزیشکی · زیندەوەرزانی · کیمیا · فیزیا · ڕووەکناسی · ڕامیاری ·
ناو · کشتوکاڵ · ئاژەڵداری
Copy the labels character-for-character from the slide — note #2 is «زانستیی پزیشکی»,
not «پزیشکی».
[CONFIRM: #8 «فەرهەنگی ناو» sits among the sciences but isn't one. Proper nouns —
people and places?]
The 79 existing categories nest UNDER these as children (see prompt 4).

### Persistence — on the user row, not the browser
`User.ActiveDomainId`, nullable. Nullable means "هەموو فەرهەنگەکان".
Do NOT use a cookie, localStorage, or session state — the requirement is that the choice
survives coming back to the program later, including from another machine. Write it to
the DB the moment it changes.

### Scope filters everything
Word list · work queue · dashboard metrics · search results · charts · the default
domain on a newly created sense.
A "هەموو فەرهەنگەکان" option must always be available. Without it, a sense filed under
the wrong dictionary becomes invisible and unrecoverable by the person who filed it.

### Make the scope impossible to miss
This is the single biggest data-integrity risk in the app: a teacher enters fifty words
into the wrong dictionary and never notices.
- Persistent pill in the top bar showing the active dictionary, always visible, one click
  to change. Never bury it in a settings page.
- The new-word form shows the target dictionary inline, above the fields, not as a
  pre-selected dropdown the eye skips.
- On switching scope, a brief inline confirmation naming the dictionary just entered.

### Domain sits on the SENSE, not the word
The user's mental model is "one word, one dictionary", and for most words that holds. But
چاو is anatomy (زانستیی پزیشکی) and also carries cultural idioms; باڵ is a bird's wing
and an aircraft's; گوڵ is botany and culture. A word appears in whichever dictionaries
its senses belong to.
Preserve the simple model in the UI: a new sense inherits the active scope's domain
automatically, so the common case needs no thought. The field stays editable for the
words that genuinely span two dictionaries.
Word list rows show the set of dictionaries the word touches, deduplicated.

### Moving between dictionaries
A sense's domain must be changeable after the fact, individually and in bulk from a
filtered list. Every move emits its own event (prompt 2) recording old → new domain.
This is how fifty misfiled words get repaired without re-entering them.

### Per-dictionary progress
Dashboard: a row per dictionary with word count, sense count, and completeness. This
doubles as the breakdown for the contributor credit page in prompt 7, where credit is
reported by domain because that is what goes in the printed acknowledgements.
```

---

## دوای ئەمانە

دوو خاڵی وەستان:
- **پڕۆمپت ٢** — تێستی سۆما/پەرژین دەبێت تێپەڕ بێت پێش ئەوەی بەرەو ٣ بڕۆی.
- **پڕۆمپت ٤** — فایلی CSV بۆم بنێرە پێش ئەوەی قۆناغی ٢ کار بکات.

دوو کێشەی جیاواز کە لە دەرەوەی ئەمانەن: SPA fallback route (`/about`، `/privacy` ٤٠٤
دەدەن) و سەلماندنی auth لەسەر پانێلی ئەدمین.
