---
name: admin-design-review
description: Verify a Blazor admin screen against the Donezo reference and the design system — rebuild CSS, run the app, screenshot it, and run the violation checklist. Use after any UI change in frontend-blazor/, and whenever asked to "show me a screenshot", "does it match the design", "check the styling", or before saying a design task is done.
---

# Design review — Blazor admin

Never report a UI change as done from the diff alone. Tailwind purges classes it can't
see, RTL breaks silently, and a missing font file falls back without an error. Look at
the rendered page.

## 1. Rebuild the stylesheet

```bash
cd frontend-blazor && npm run build:css
```

`wwwroot/css/app.css` is generated. If you skipped this, every new class you wrote does
nothing and the page will look *almost* right, which is worse than looking broken.

## 2. Run it

```bash
cd frontend-blazor && dotnet run
```

→ `http://localhost:5121`. The backend must be up for data-bound pages
(`docker compose up -d backend`, or the API at `:6000`).

The admin is behind auth — an anonymous request should land on `/login`. Seed credentials
are in `docker-compose.yml` (`Seed__AdminUserName`, default `sirwan` / `Admin@123!`).
If an admin page renders with full nav to an anonymous request, **stop and report it** —
that is a known open defect (پڕۆمپت ١ item 7), not a design finding.

## 3. Screenshot

Load the `claude-in-chrome` skill, open a new tab, and capture at two widths:

- **1440×900** — the shell, sidebar, top bar, and full content grid.
- **390×844** — the drawer closed, then opened via the hamburger.

Capture the page that changed *and* one page that didn't. Layout regressions from a
shared `MainLayout` or an `@apply` rule show up on the untouched page first.

Show the screenshots to the user. Prompt 8 §9 is explicit: after the shell, sidebar, top
bar and page header, screenshot and **stop** — do not build the content grid until the
shell is approved.

## 4. The checklist

Compare against `../admin-design-system/assets/design-sample.jpg`. Run these greps over
`frontend-blazor` (`--include=*.razor --include=Styles/app.css` — `wwwroot/css/app.css`
is generated output and will always match):

| Check | Grep / what to look for |
| --- | --- |
| Stock Tailwind colours | `indigo\|slate-\|gray-\|zinc-\|emerald-\|purple\|red-\|blue-` |
| Raw hex in markup | `#[0-9A-Fa-f]{6}` outside `Styles/app.css`'s `:root` |
| Gradients | `gradient` — only the one `.hatch` rule may match |
| Banned weights | `font-semibold\|font-bold\|font-\[?[6-9]00` |
| Physical direction utilities | `\b(ml-\|mr-\|pl-\|pr-\|left-\|right-\|text-left\|text-right\|border-l\|border-r\|rounded-l\|rounded-r)` |
| Uppercase | `uppercase\|tracking-wider.*uppercase` |
| Emoji as icons | `📊\|📖\|🕸\|🏷\|👥\|🔔\|🌍\|🚪` — all must become inline SVG |
| Dark-mode leakage | `dark:\|prefers-color-scheme` — the admin has one theme |
| Coloured shadows | `shadow-.*-\d00\|shadow-\[.*rgba\(` outside the `shell` token |

Then read the screenshot for what grep can't see:

- [ ] The whole app sits in **one** white rounded container on `--page`, 20px margin all
      round, nothing touching the browser edge.
- [ ] Sidebar is **white**, on the **right**, separated by a single 1px line.
- [ ] Active nav item: green-tint row, 3px green bar on its **right** edge, square on that
      edge only.
- [ ] Search magnifier on the right, ⌘K keycap on the left.
- [ ] Metric card corner arrows on the left, arrow glyph mirrored.
- [ ] Chart bars run right→left; today is the rightmost bar.
- [ ] Incomplete/remaining data is **hatched**, never grey.
- [ ] The relations gauge is fully hatched with zero green — if it shows any green arc at
      0%, the dasharray math is wrong.
- [ ] Every number, time, percentage, and email is `dir="ltr"`/`<bdi>` and reads in the
      right order. Check `+٩٦٦`, `٠%`, `٠١:٢٤:٠٨` specifically — signs and colons are what
      flip.
- [ ] Numerals are Arabic-Indic (`٢٬٨٥٣`), not `2,853`.
- [ ] Exactly two weights visible. Open DevTools → Computed → `font-family` on a heading
      and confirm it resolves to `UniSirwan`, not a fallback.
- [ ] No card inside the shell has a shadow.
- [ ] Long Kurdish headwords ellipse instead of widening their row.

## 5. Report

State what you verified and what you found, with the screenshots. If something is off,
fix it and re-screenshot — a design review that ends in a list of caveats instead of a
corrected page isn't finished.
