# Tokens

The green ramp is fixed by the client:

```
#328E6E   rgb(50, 142, 110)
#67AE6E   rgb(103, 174, 110)
#90C67C   rgb(144, 198, 124)
#E1EEBC   rgb(225, 238, 188)
```

These four are canonical — do not substitute, re-tint, or "adjust for contrast".
Two extra values are **derived** from them and marked as such below, because the ramp has
no colour dark enough to carry white text: white on `#328E6E` is 4.0:1, which fails AA for
anything under 18px. `--green-deep` is `#328E6E` taken to `hsl(159, 50%, 22%)`, same hue,
and gives 6.2:1 with white.

## Colour

| CSS var | Hex | Tailwind class | Used for |
| --- | --- | --- | --- |
| `--page` | `#F1F2F0` | `bg-page` | neutral paper behind the app shell; ghost icon buttons; search input fill |
| `--card` | `#FFFFFF` | `bg-card` | the shell, the sidebar, every card |
| `--border` | `#E6EAE6` | `border-line` | every 1px rule; the hatch stroke colour |
| `--green-deep` | `#1C5441` *(derived)* | `bg-green-deep` `text-green-deep` | hero card, primary buttons, dark tiles, text on `--green-tint` |
| `--green` | `#328E6E` | `bg-green` `text-green` | chart bars (primary series), gauge arc, active nav indicator |
| `--green-mid` | `#67AE6E` | `bg-green-mid` | second chart series, second gauge segment |
| `--green-soft` | `#90C67C` | `bg-green-soft` | third series, soft accents, presence dot |
| `--green-tint` | `#E1EEBC` | `bg-green-tint` | status pills, avatar backgrounds, active nav row, icon tiles |
| `--on-tint` | `#1C5441` *(= green-deep)* | `text-on-tint` | text sitting on `--green-tint` — 7.2:1 |
| `--on-deep` | `#E1EEBC` *(= green-tint)* | `text-on-deep` | label/footnote text sitting on `--green-deep` — 7.2:1 |
| `--text-1` | `#16211B` | `text-ink-1` | headings, numbers, primary label |
| `--text-2` | `#6A776E` | `text-ink-2` | body, subtitles, footnotes |
| `--text-3` | `#9AA69E` | `text-ink-3` | section labels, axis ticks, placeholders |
| `--warn` | `#B45309` | `text-warn` | warn number + footnote |
| `--warn-bg` | `#FDF3E3` | `bg-warn-tint` | warn card background, warn pill |
| `--danger` | `#B42318` | `text-danger` | danger number + footnote; the stop button |
| `--danger-bg` | `#FCEBEB` | `bg-danger-tint` | danger card background, danger pill |

There is no other colour. No greys from Tailwind's palette, no indigo, no gradient.

**Contrast rules that follow from this ramp — check them, they are easy to get wrong:**

- White text is legal on `--green-deep` only. Never on `--green`, `--green-mid`,
  `--green-soft`, or `--green-tint`.
- `--green` as *text* on white is 3.9:1 — allowed for 24px+ numerals and for icons, not
  for 12–15px body. Small text is always `--text-1` / `--text-2`.
- `--green-tint` (`#E1EEBC`) is a pale lime and sits close to `--warn-bg` (`#FDF3E3`) at a
  glance. Where a چالاک pill and a بێ‌چالاکی pill appear in the same list (the ئامادەکاران
  card), the text colour carries the difference — never the fill alone.

## Radius

| Token | Value | Class | Used for |
| --- | --- | --- | --- |
| shell | 24px | `rounded-shell` | the one outer app container |
| card | 16px | `rounded-card` | every card, tile, dark promo block |
| chip | 8px | `rounded-chip` | the ⌘K keycap, small chips |
| pill | 999px | `rounded-full` | buttons, status pills, search input, avatars |

## Spacing — plain Tailwind, no custom scale needed

| Purpose | Value | Class |
| --- | --- | --- |
| card padding | 20px | `p-5` |
| grid gap | 16px | `gap-4` |
| shell outer margin | 20px | `m-5` |

## Type scale

| px | Class | Used for |
| --- | --- | --- |
| 11 | `text-11` | section labels, counter pills, day initials, keycap, value chip |
| 12 | `text-xs` | footnotes, subtext lines, legends, gauge caption, email |
| 13 | `text-13` | metric card label |
| 14 | `text-sm` | nav-item label, page subtitle, row titles, user name |
| 15 | `text-15` | card titles, sidebar nav label, station field labels |
| 20 | `text-20` | wordmark |
| 28 | `text-28` | page title |
| 30 | `text-30` | gauge percentage, session timer |
| 34 | `text-34` | metric card number |

Weights: `font-normal` (400) and `font-medium` (500). Nothing else exists.

## Shadow

Exactly one, on the shell:

```
0 1px 3px rgba(0,0,0,0.04), 0 8px 24px rgba(0,0,0,0.04)
```

Class: `shadow-shell`. Cards inside the shell have **no shadow** — they are separated by
`border border-line` only.

---

## `frontend-blazor/tailwind.config.js`

```js
/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./**/*.razor", "./**/*.html", "./**/*.cshtml"],
  theme: {
    extend: {
      colors: {
        page:           'var(--page)',
        card:           'var(--card)',
        line:           'var(--border)',
        'green-deep':   'var(--green-deep)',
        green:          'var(--green)',
        'green-mid':    'var(--green-mid)',
        'green-soft':   'var(--green-soft)',
        'green-tint':   'var(--green-tint)',
        'on-tint':      'var(--on-tint)',
        'on-deep':      'var(--on-deep)',
        'ink-1':        'var(--text-1)',
        'ink-2':        'var(--text-2)',
        'ink-3':        'var(--text-3)',
        warn:           'var(--warn)',
        'warn-tint':    'var(--warn-bg)',
        danger:         'var(--danger)',
        'danger-tint':  'var(--danger-bg)',
      },
      borderRadius: { shell: '24px', card: '16px', chip: '8px' },
      boxShadow: {
        shell: '0 1px 3px rgba(0,0,0,0.04), 0 8px 24px rgba(0,0,0,0.04)',
      },
      fontSize: {
        '11': ['11px', '1.45'],
        '13': ['13px', '1.45'],
        '15': ['15px', '1.5'],
        '20': ['20px', '1.35'],
        '28': ['28px', '1.25'],
        '30': ['30px', '1.2'],
        '34': ['34px', '1.15'],
      },
      fontFamily: {
        sans: ['var(--font-main)'],
        mono: ['var(--font-mono)'],
      },
    },
  },
  plugins: [],
}
```

Declaring colours as `var(--x)` (not hex) means opacity modifiers like `bg-green/10`
will **not** work — Tailwind can't inject an alpha channel into an opaque var. If a
translucent green is genuinely needed, add an explicit token for it rather than reaching
for `/10`. In practice the design needs none: separation is done with borders and tints.

## `frontend-blazor/Styles/app.css` — the token block

```css
:root {
  --page:        #F1F2F0;
  --card:        #FFFFFF;
  --border:      #E6EAE6;

  /* Client palette — canonical, do not alter */
  --green:       #328E6E;   /* rgb(50, 142, 110)  */
  --green-mid:   #67AE6E;   /* rgb(103, 174, 110) */
  --green-soft:  #90C67C;   /* rgb(144, 198, 124) */
  --green-tint:  #E1EEBC;   /* rgb(225, 238, 188) */

  /* Derived: --green at hsl(159,50%,22%). The ramp has nothing dark enough to
     carry white text — white on #328E6E is only 4.0:1. This is 6.2:1. */
  --green-deep:  #1C5441;

  --on-tint:     #1C5441;   /* text on --green-tint  → 7.2:1 */
  --on-deep:     #E1EEBC;   /* text on --green-deep  → 7.2:1 */

  --text-1:      #16211B;
  --text-2:      #6A776E;
  --text-3:      #9AA69E;
  --warn:        #B45309;
  --warn-bg:     #FDF3E3;
  --danger:      #B42318;
  --danger-bg:   #FCEBEB;
}
```

There is **one theme**. Do not add a dark mode, a `prefers-color-scheme` block, or a
`.dark` variant — the reference has one look and the public Next.js site's dark tokens
are a separate system that must not leak in here.

## Chart series order

When a chart needs more than one green, go down the ramp in this order and stop:
`--green` → `--green-mid` → `--green-soft`. A fourth series does not get `--green-tint`
(it is a background token and will vanish against white) — a fourth series means the
chart is wrong, split it.

## Deleting the old system

`Styles/app.css` currently carries indigo scrollbars and a dark `.nav-item` (slate-800 /
indigo-600). Those are the previous design, not a base layer — replace them, don't
layer over them. Same for `MainLayout.razor`'s `bg-slate-900` sidebar and every
`bg-indigo-600` avatar across `Components/`.

Sweep for leftovers with:

```
grep -rnE "indigo|slate-|gray-|purple|magenta|gradient|font-(semi)?bold|font-\[?[67]00" frontend-blazor --include=*.razor --include=*.css
```

(`Styles/app.css` only — `wwwroot/css/app.css` is generated output and will show hits
until you rebuild.)
