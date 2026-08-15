---
name: admin-design-system
description: The visual design system for the Blazor admin (manage.jinzar.krd) — Donezo-style layout language, green palette, RTL Kurdish. Use BEFORE writing or editing any .razor markup, Tailwind classes, or CSS in frontend-blazor/. Triggers on dashboard, sidebar, card, chart, button, table, form, modal, colour, spacing, font, icon, layout, "make it look", "restyle", "design".
---

# Admin design system — فەرهەنگی کوردی

Scope: **`frontend-blazor/` only.** The public Next.js site and the Flutter app have their
own (dark, indigo) system — never let tokens cross between them.

Four references, in `assets/`. **Read the relevant one before building anything.**

- `sidebar-reference.jpg` — **the chrome.** Light rail, grey pill for the active item,
  profile at the foot of the rail, and the three widths that are also our responsive story.
- `design-sample-2.jpg` — **the content.** Card anatomy, the hero dial, sparkline tiles,
  the alert rail, the tab strip, the table. Its *dark* chrome is superseded by the sidebar
  reference — take the layout from this one, not the colour.
- `soft-gradients.png` — the client palette sheet. Two of its ten swatches are in use
  (AMETHYST 08, ROYAL BLUE 05); the rest are not. See "Colour" below.
- `design-sample.jpg` — the original Donezo template. Superseded, kept because the hatch
  idea and the metric-card states come from it.

We reproduce their *layout language*, mirrored for RTL, with the client's green ramp. We do
not copy any logo, wordmark, illustration or asset from any of them.

Font: **NRT** — the house Kurdish font, shared with the public site. It ships one weight
file, so `font-medium` is mapped to it explicitly and emphasis is carried by size and
colour rather than weight. See `references/rtl-kurdish.md`.

> UniSIRWAN Madani (9 weights) was installed and then reverted at the client's request.
> The files are still in `wwwroot/fonts/` if it is ever wanted back.

## The five rules that break the design if violated

1. **Tokens only.** Never write a hex value, `bg-indigo-600`, `text-slate-400`, or any
   stock Tailwind colour in markup. Only the token classes in `references/tokens.md`.
   The existing dark-slate/indigo admin is the thing being replaced — treat every
   `slate-*`/`indigo-*`/`gray-*` class you meet as a bug to convert.
2. **Gradients on details only — never on a surface.** They belong to badges, progress
   fills, chart bars, primary buttons, the nav dots and the tier rings — and nothing else.
   Backgrounds, cards, sidebars, headers and table rows are FLAT.
   The test is size: if the gradient is more than about 40px across it has stopped being a
   detail. Still banned outright: coloured shadows, per-user colour coding, accent stripes
   under headings.
3. **Two font weights: `font-normal` and `font-medium`.** NRT has one weight file and both
   map to it, so `font-medium` is a semantic marker rather than a visual change — never
   `font-semibold`, never `font-bold`, and never let the browser synthesise one.
   **Digits are Latin and set in Calibri**, via a `unicode-range` @font-face — never a
   class on the number, and never Arabic-Indic.
4. **Logical properties, not physical.** `ms-*`/`me-*`/`ps-*`/`pe-*`/`start-*`/`end-*`,
   never `ml-*`/`mr-*`/`left-*`/`right-*`. Physical utilities are how RTL layouts rot.
5. **Sentence case.** No ALL CAPS, no `uppercase` utility. The one exception is the
   11px section labels in the sidebar, which use letter-spacing, not capitals.

## Colour

**Light everywhere. Colour is a detail, never a surface.**

- Page is `--page` (white smoke), cards and chrome are `--card` (white).
**Three families, and each one means something.** Colour is never chosen for variety:

| Family | Means | Where |
| --- | --- | --- |
| **Green** (client ramp) | state of the work — done, progress, primary action | logo, active-nav dot, card badges, progress fills, primary buttons, the gauge arc, `pill-ok` |
| **Amethyst** `#4A1478→#7A34B4` | people | contributor word counts (`.num-users`), the ئامادەکاران badge |
| **Royal blue** `#16276E→#2B4AAE` | time | every chart bar (`.chart-bar`), the week-chart peak label |

So a screen reads *green = state of the work, purple = who, blue = when*. Adding a fourth
family means finding a fourth kind of thing — not a fourth mood. The palette sheet has a
purple for every occasion and using six of them turns the dashboard into a swatch page.

- All gradients are **135°**, so two side by side read as one material — and so RTL cannot
  mirror one and not the other.
- The `-ink` token of a borrowed family is its dark stop, safe for text on white and on its
  own tint. Never set text in the light stop.
- Contributor tiers are a fourth set (five metals) and the documented exception: a medal the
  same colour as every other medal is not a medal. See `Services/Tier.cs`.
- Cards have **no shadow**. The border carries the whole structure, which is why
  `--border` is a real grey (`#D9DCD9`) rather than the hint of one it used to be.

> Three reversals are recorded here so they are not undone by accident:
>
> 1. The Donezo shell floated on paper with a 20px margin and a 24px radius. The client
>    asked for it removed — on a working screen that margin is a strip of nothing wrapped
>    around a dashboard someone is trying to read. **Do not put it back.**
> 2. **The chrome was near-black for one revision and is light again.** The client asked
>    for light, so the sidebar and top bar are white with a 1px seam. `--chrome*` and
>    `--on-chrome*` still exist as tokens but **nothing uses them** — reaching for one is
>    almost certainly a mistake.
> 3. The active nav item was a solid green bar. It is a pale pill with a hairline and a
>    small gradient dot. An accent that fills a 200px block on every navigation is not an
>    accent.

## The shell

Three widths, which are the sidebar reference's own three panels:

| Width | Sidebar |
| --- | --- |
| `< lg` | off-canvas drawer, 256px, labels shown |
| `lg`–`xl` | collapsed rail, 64px, icons only, `title` carries the name, badge → dot |
| `≥ xl` | 256px, icons and labels |

- The scroll lives on the **nav wrapper inside** the aside, never on the aside itself: an
  overflow container clips absolutely-positioned descendants, and the profile menu is
  deliberately wider than the collapsed rail.
- The drawer toggle and the profile menu are **CSS peer-checkboxes**, not Blazor state.
  This layout renders statically, so anything driven by C# is dead until the circuit
  connects.
- **Desktop is viewport-locked.** At `lg` and up the shell is exactly one screen tall, the
  sidebar and top bar never move, and `<main>` is the only scroll container. `lg:min-h-0` on
  the content column is load-bearing — without it the flex child will not shrink below its
  content and the overflow silently returns to the page. Mobile keeps ordinary page scroll:
  locking a phone viewport fights the address-bar collapse and breaks scroll-into-view.
- The sidebar must **fit one view**. Its `overflow-y-auto` is a fallback for short windows,
  not the normal state; adding a nav section means re-measuring it.
- The profile block lives at the **foot of the sidebar**, not in the top bar, and opens the
  account menu (هەژمارەکەم / ڕێکخستن / دەرچوون). Logout is a POST with an antiforgery
  token, so it can never be a link.

## Telerik UI for Blazor

Grids, dialogs, buttons, dropdowns and notifications are **Telerik**, not hand-rolled.

- **Every interactive page wraps its markup in `<TelerikScope>`** (which is a
  `TelerikRootComponent` with `EnableRtl="true"`). It is NOT in `MainLayout`: the layout
  renders statically, each `@rendermode` island is its own render root, and a root
  component in a static layout is invisible to the interactive pages inside it. Forgetting
  the scope throws *"A Telerik component on the requested view requires a
  TelerikRootComponent"* and the page renders as blank chrome.
- Without `EnableRtl` every grid header, dialog action row and popup anchors left-to-right.
- **Grids have no border radius.** Cards keep 18px; grids get 0, and the reset has to name
  the header, content, toolbar, pager and first/last header cells, because Kendo rounds
  several inner parts itself and clearing `.k-grid` alone leaves a curved header in a square
  box. Do not pass `Class="rounded-card"` to a grid.
- Server-paged grids use **`OnRead`**, and external filters (a search box, a category pill)
  call `grid.Rebind()`. Telerik's own filter row only filters the page it was handed —
  twenty rows out of three thousand — so on this data it lies.
- The theme CSS loads **before** `app.css` so our tokens win every collision. The skin
  block at the bottom of `app.css` maps Telerik's chrome onto our tokens; keep it small,
  because restyling internal `.k-*` classes breaks on upgrade.
- Errors and successes go through **`Toast`** (`Services/Toast.cs`), never a silent
  failure. A refused save that changes nothing on screen is indistinguishable from one
  that worked — and the server already writes a good message.
- Package comes from a **local commercial feed** (`C:\Telerik Lasted Version`), not
  nuget.org. A machine without that NuGet source cannot restore the project.

## Icons

**Solid**, one filled path per glyph, in `Widgets/Icon.razor`. At 16-18px an outline icon
beside Kurdish text reads as a faint sketch; a filled one reads as a peer. Never emoji.
Only arrows and chevrons take `Mirror="true"`.

## Where things live

| What | Path |
| --- | --- |
| Tailwind source (edit this) | `frontend-blazor/Styles/app.css` |
| Tailwind config | `frontend-blazor/tailwind.config.js` |
| **Generated CSS — never edit by hand** | `frontend-blazor/wwwroot/css/app.css` |
| Shell / sidebar / top bar | `frontend-blazor/Components/Layout/MainLayout.razor` |
| Reusable pieces | `frontend-blazor/Components/Widgets/*.razor` |
| Pages | `frontend-blazor/Components/Pages/*.razor` |

**After any change to `Styles/app.css` or `tailwind.config.js`, or after adding a class
that has never appeared in the repo before, rebuild:**

```bash
cd frontend-blazor && npm run build:css
```

Tailwind scans `./**/*.razor` — a class that exists only in a C# string or a variable
will be purged out of the bundle and silently do nothing. If a style "isn't applying",
check that first.

## Reference files — read the one you need

- `references/tokens.md` — every colour, radius, size, shadow; the `tailwind.config.js`
  and `Styles/app.css` blocks that define them; the token → class mapping table.
- `references/components.md` — copy-ready recipes: app shell, sidebar, nav item, top bar,
  page header, metric cards (incl. the hero card and threshold-driven warn/danger states),
  the hatched bar chart, the semicircular gauge, list rows, pills, buttons, the dark tiles.
- `references/rtl-kurdish.md` — mirroring rules, keeping numerals LTR inside RTL text,
  the font stack, and the Tailwind RTL traps in this specific codebase.

## Building a new screen

1. Reread the relevant section of `md files/claude-code-prompt.md` (پڕۆمپت ٨) — it is the
   spec; this skill is how to execute it.
2. Compose from `references/components.md`. If the piece you need isn't there and it will
   appear twice, make it a Widget and add the recipe back to that file.
3. Semantic colour is computed from thresholds, never hardcoded per card. A metric is
   `danger` because its value crosses a threshold, not because it's the third card.
4. The hero panel (`.card-feature`) is **visual hierarchy, not a "good" signal.** A hero
   card can still be in a warn state.
5. Rebuild CSS, then verify with the `admin-design-review` skill.
