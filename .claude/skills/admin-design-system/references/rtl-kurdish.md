# RTL, Kurdish typography, and numerals

## The font family: NRT

The house Kurdish font, and the same one the public site uses — the admin and the dictionary
should not look like two different products.

```
frontend-blazor/wwwroot/fonts/NRT-Reg.ttf
```

**One weight file.** Weight 500 is declared against the same file rather than left undeclared:
an undeclared 500 lets the browser synthesise a bold, and faux-bold on Arabic script thickens the
letter joins unevenly. So `font-medium` renders identically to `font-normal`, and emphasis is
carried by **size and colour** — which is how the reference dashboard does it anyway.

```css
@font-face { font-family:'NRT'; font-weight:400; font-style:normal; font-display:swap;
             src:url('/fonts/NRT-Reg.ttf') format('truetype'); }
@font-face { font-family:'NRT'; font-weight:500; font-style:normal; font-display:swap;
             src:url('/fonts/NRT-Reg.ttf') format('truetype'); }

:root {
  --font-main: 'NRT', system-ui, -apple-system, sans-serif;
  --font-mono: ui-monospace, 'Cascadia Mono', 'Segoe UI Mono', Menlo, monospace;
}

html { font-family: var(--font-main); }
input, select, textarea, button { font: inherit; }
```

One `font-family` on `html`, inherited; form controls do not inherit fonts by default, which is
the only reason the second rule exists. Preload the single file in `Components/App.razor`.

> UniSIRWAN Madani's nine weights were installed and then reverted at the client's request. The
> files remain in `wwwroot/fonts/` — if it comes back, register the weights and point 500 at
> `UniSirwanMadani-Medium.ttf`. Nothing else has to change.

### Where each weight goes

| Weight | Where |
| --- | --- |
| `font-normal` (400) | everything: body, labels, table cells, nav items, footnotes, subtitles |
| `font-medium` (500) | emphasis: page titles, card titles, metric numbers, active nav item, row titles, button labels. Visually identical under NRT — it marks intent, and starts rendering differently the moment a real medium weight is added. |

Never `font-semibold`, never `font-bold`. If a heading is not standing out, the fix is size or
colour, not weight.

## Direction

`dir="rtl"` is already on `<html>` in `Components/App.razor` — good, keep it there.
The `html { direction: rtl }` rule in `Styles/app.css` is a duplicate and should go: with
the attribute present, Tailwind's `rtl:` variants work; with only the CSS rule they don't,
because the variant compiles to `[dir="rtl"] &`.

Also in `App.razor`: `<body class="bg-gray-50">` is old-palette and must become
`bg-page`.

### Logical utilities — the whole rule

| Never | Always | In RTL that means |
| --- | --- | --- |
| `ml-*` `mr-*` | `ms-*` `me-*` | start = right, end = left |
| `pl-*` `pr-*` | `ps-*` `pe-*` | |
| `left-*` `right-*` | `start-*` `end-*` | |
| `border-l` `border-r` | `border-s` `border-e` | |
| `rounded-l-*` `rounded-r-*` | `rounded-s-*` `rounded-e-*` | |
| `text-left` `text-right` | `text-start` `text-end` | |

`MainLayout.razor` currently uses `mr-auto`, `lg:mr-64`, `inset-y-0 right-0`,
`border-l` — all of that converts.

`space-x-*` and `divide-x-*` **do not** flip. Use `gap-*` in a flex/grid container
instead; it is direction-agnostic. `flex-row-reverse` is almost never the answer — if you
reach for it, you have a physical utility somewhere upstream.

### What mirrors, and what must not

Mirrors: sidebar (to the right), active nav indicator (right edge of the item), card
corner buttons (to the left), search magnifier (right), keycap (left), chart category
order (right to left), directional arrows and chevrons.

Mirror an icon with `rtl:-scale-x-100`, not a separate asset.

Does **not** mirror: clocks, the play/pause/stop glyphs, logos, avatars, checkmarks, the
magnifier glass itself (only its position moves), and any icon whose meaning is not
directional. Mirroring a play triangle makes it a rewind button.

## Numerals

The UI shows Arabic-Indic digits (٢٬٨٥٣، ٧٩، ٠%). Two separate problems:

**1. .NET does not substitute digits.** `value.ToString("N0", culture)` emits ASCII
digits regardless of `NumberFormatInfo.NativeDigits` — substitution is a rendering-layer
feature, not a formatting one. You must map them. One helper, used everywhere:

```csharp
// frontend-blazor/Services/Ku.cs
public static class Ku
{
    private static readonly string[] Digits =
        { "٠","١","٢","٣","٤","٥","٦","٧","٨","٩" };

    /// <summary>Grouped number in Arabic-Indic digits: 2853 → "٢٬٨٥٣".</summary>
    public static string N(long value)   => Sub(value.ToString("N0", CultureInfo.InvariantCulture));
    public static string P(double ratio) => Sub(ratio.ToString("P0", CultureInfo.InvariantCulture));

    private static string Sub(string s)
    {
        var b = new StringBuilder(s.Length);
        foreach (var c in s)
            b.Append(c switch
            {
                >= '0' and <= '9' => Digits[c - '0'],
                ','               => '٬',   // U+066C Arabic thousands separator
                '.'               => '٫',   // U+066B Arabic decimal separator
                _                 => c,
            });
        return b.ToString();
    }
}
```

Never inline a digit map at a call site; a second copy will drift.

**2. Bidi reordering.** A run of digits next to punctuation (`٠%`, `٢٤:١٥:٠٨`, `+٩٦٦`,
an email, `⌘K`) can be reordered by the bidi algorithm and render backwards or with the
sign on the wrong end. Isolate every such run:

```razor
<span dir="ltr">@Ku.N(count)</span>
```

or, for a run embedded in a Kurdish sentence, `<bdi>@Ku.N(count)</bdi>` — `<bdi>` isolates
without forcing a direction and is the safer default inside prose.

Rule of thumb: **any element whose entire text is a number, a time, a percentage, a
version, an email, or a keycap gets `<bdi>` around the text.** That covers every
`text-34`, `text-30`, counter pill, chip, axis tick, and table numeric cell in
`components.md`.

### The trap: `dir` on a positioned or block element

`dir="ltr"` does not only isolate text — it changes how **that element's own** logical
properties resolve. Put it on a box and:

- `start-*` / `end-*` (`inset-inline-start`/`end`) swap sides. A `dir="ltr"` chip with
  `end-3` lands on the **right**, not the left. This shipped once already: the ⌘K keycap
  stacked on top of the search magnifier because both resolved to the start edge.
- `text-align` flips to left, so a metric number set as `<p dir="ltr">` hugs the wrong
  side of its card.
- `ms-*`/`me-*`/`ps-*`/`pe-*` on the same element swap too.

So:

```razor
❌  <span class="absolute end-3" dir="ltr">⌘K</span>
✅  <span class="absolute end-3"><bdi>⌘K</bdi></span>

❌  <p class="text-34" dir="ltr">@Ku.N(total)</p>
✅  <p class="text-34"><bdi>@Ku.N(total)</bdi></p>
```

`<bdi>` isolates the bidi run without touching layout direction, which is exactly what is
wanted. Reserve `dir="ltr"` for a leaf `<span>` that carries text and no positioning, no
alignment, and no logical spacing — and prefer `<bdi>` even then.

The session timer additionally gets `font-mono` so the digits don't jitter as they tick —
NRT's digits are not tabular.

## Text that must not break

- `truncate` needs `min-w-0` on the flex child, otherwise a long Kurdish headword blows
  the row width instead of ellipsing. Every row recipe in `components.md` has it; keep it.
- `text-11` with `tracking-[0.08em]` is safe on Arabic script here because those labels
  (مێنیو، گشتی) are short. Do not apply letter-spacing to running Kurdish text — it
  breaks the cursive joins.
- No `uppercase`. Arabic script has no case; the utility does nothing on Kurdish and
  mangles any Latin string that slips through.
