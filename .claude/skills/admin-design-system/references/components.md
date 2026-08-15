# Component recipes

Tailwind v3 classes, RTL-correct (logical utilities only). Copy these; don't reinvent
them. If you need a piece that isn't here and it will appear twice, build it as a Widget
in `frontend-blazor/Components/Widgets/` and add the recipe back to this file.

---

## 1. App shell

The signature of the template: one white rounded container holding *everything*.

```razor
<div class="min-h-screen bg-page p-5">
  <div class="mx-auto flex min-h-[calc(100vh-40px)] overflow-hidden
              rounded-shell bg-card shadow-shell">

    <aside class="hidden w-64 shrink-0 flex-col border-s border-line lg:flex">
      @* sidebar — recipe 2 *@
    </aside>

    <div class="flex min-w-0 flex-1 flex-col">
      @* top bar — recipe 3 *@
      <main class="flex-1 p-5">
        @Body
      </main>
    </div>
  </div>
</div>
```

- `border-s` on the aside = the right edge in RTL, i.e. the seam facing the content. The
  sidebar is `bg-card` like the content; the 1px line is the *only* separation.
- Nothing gets `bg-page` inside the shell except ghost buttons and the search field.
- On mobile the sidebar becomes an off-canvas drawer — keep the existing
  `#nav-toggle` checkbox pattern in `MainLayout.razor`. It must stay a **sibling** of the
  overlay and the `<aside>`; `peer-checked:*` compiles to a general-sibling selector, so
  nesting it silently kills the drawer. Drawer slides from the start edge:
  `translate-x-full` / `peer-checked:translate-x-0` with `fixed inset-y-0 start-0`.

## 2. Sidebar

```razor
@* Logo + wordmark *@
<div class="flex items-center gap-2.5 px-5 py-5">
  <span class="flex h-9 w-9 items-center justify-center rounded-card bg-green text-white">
    @* mark — our own, never the reference's *@
  </span>
  <span class="text-20 font-medium text-ink-1">فەرهەنگی کوردی</span>
</div>

@* Section label *@
<p class="px-5 pb-2 pt-4 text-11 tracking-[0.08em] text-ink-3">مێنیو</p>

@* Nav — see .nav-item below *@
<nav class="space-y-1 px-3">
  <NavItem Href="" Icon="…" Label="داشبۆرد" Match="NavLinkMatch.All" />
  <NavItem Href="words"    Icon="…" Label="وشەکان" />
  <NavItem Href="relations" Icon="…" Label="پەیوەندییەکان" />
  <NavItem Href="taxonomy" Icon="…" Label="پۆل و جۆرەکان" />
  <NavItem Href="queue"    Icon="…" Label="ڕیزی کار" Badge="@outstanding" />
  <NavItem Href="users"    Icon="…" Label="بەکارهێنەران" />
</nav>

<p class="px-5 pb-2 pt-5 text-11 tracking-[0.08em] text-ink-3">گشتی</p>
@* ڕێکخستن · دەرچوون *@
```

Section labels use **letter-spacing, not capitals** — Arabic script has no case and the
`uppercase` utility is banned.

`.nav-item` in `Styles/app.css` (Blazor's `NavLink` stamps `active` on the current route,
so both states live in CSS and the markup stays one class name):

```css
.nav-item {
  @apply flex items-center gap-3 rounded-chip px-3 py-2.5 text-15
         text-ink-2 transition-colors;
}
.nav-item:hover        { @apply bg-page text-ink-1; }
.nav-item.active       { @apply bg-green-tint font-medium text-ink-1
                                border-s-[3px] border-green rounded-s-none; }
.nav-item.active:hover { @apply bg-green-tint; }
```

`border-s` = the **right** edge in RTL, the outer edge of the sidebar. `rounded-s-none`
squares off that edge so the bar reads as a tab indicator, not a floating stripe.

Icons: 20px outline, `stroke-width="1.5"`, `stroke="currentColor"`, `fill="none"`.
Inline SVG only — the emoji currently in `MainLayout.razor` (📊 📖 🕸 🏷) are placeholders
and must go; they render in the OS emoji font and break the type system.

Counter pill (the reference's "12+"), on ڕیزی کار:

```razor
<span class="ms-auto rounded-full bg-green-tint px-2 py-0.5 text-11 text-on-tint">
  <bdi>@Ku.N(outstanding)</bdi>
</span>
```

Bottom promo slot — the reference's "Download our Mobile App", repurposed:

```razor
<div class="m-3 mt-auto rounded-card bg-green-deep p-4">
  <p class="text-sm font-medium text-white">ڕێنمایی فەرهەنگنووسی</p>
  <p class="mt-1 text-xs text-on-deep">چۆن وشە پۆلێن دەکرێت</p>
  <a href="/guide"
     class="mt-3 inline-flex rounded-full bg-green-tint px-3 py-1.5 text-11 text-on-tint">
    بیخوێنەوە
  </a>
</div>
```

## 3. Top bar

```razor
<header class="flex h-16 items-center gap-3 border-b border-line px-5">

  @* Search: magnifier at the START (right in RTL), keycap at the END (left) *@
  <div class="relative w-full max-w-md">
    <svg class="pointer-events-none absolute start-3 top-1/2 h-4 w-4 -translate-y-1/2 text-ink-3" …/>
    <input class="w-full rounded-full border-0 bg-page py-2.5 ps-10 pe-14 text-sm
                  text-ink-1 placeholder:text-ink-3 focus:outline-none focus:ring-1 focus:ring-green"
           placeholder="گەڕان بۆ وشە…" />
    <span class="absolute end-3 top-1/2 -translate-y-1/2 rounded-chip border border-line
                 bg-card px-1.5 py-0.5 text-11 text-ink-3">⌘K</span>
  </div>

  <div class="ms-auto flex items-center gap-2">
    @* Ghost icon button, 36px *@
    <button class="relative flex h-9 w-9 items-center justify-center rounded-full bg-page text-ink-2">
      <svg class="h-[18px] w-[18px]" …/>
      @* unread = a DOT, never a number *@
      <span class="absolute end-2 top-2 h-1.5 w-1.5 rounded-full bg-green"></span>
    </button>

    @* User block *@
    <div class="flex items-center gap-2.5 ps-2">
      <span class="flex h-9 w-9 items-center justify-center rounded-full bg-green-tint
                   text-sm font-medium text-on-tint">@Initials</span>
      <div class="hidden leading-tight sm:block">
        <p class="text-sm font-medium text-ink-1">@name</p>
        <p class="text-xs text-ink-2"><bdi>@email</bdi></p>
      </div>
    </div>
  </div>
</header>
```

## 4. Page header

```razor
<div class="mb-5 flex flex-wrap items-start gap-3">
  <div>
    <h1 class="text-28 font-medium text-ink-1">داشبۆرد</h1>
    <p class="mt-1 text-sm text-ink-2">@subtitle</p>
  </div>
  <div class="ms-auto flex items-center gap-2">
    <button class="btn-primary">
      <svg class="h-4 w-4" …/> وشەی نوێ
    </button>
    <button class="btn-secondary">هێنانی داتا</button>
  </div>
</div>
```

Buttons, in `Styles/app.css`:

```css
.btn-primary {
  @apply inline-flex items-center gap-2 rounded-full bg-green-deep px-4 py-2.5
         text-sm font-medium text-white transition-opacity hover:opacity-90;
}
.btn-secondary {
  @apply inline-flex items-center gap-2 rounded-full border border-line bg-card px-4 py-2.5
         text-sm font-medium text-ink-1 transition-colors hover:bg-page;
}
.btn-ghost-icon {
  @apply flex h-7 w-7 items-center justify-center rounded-full border border-line
         text-ink-2 transition-colors hover:bg-page;
}
```

Hover on the primary is **opacity**, not a second green. The ramp has no "green-deep
hover" value and inventing one splits the palette.

## 5. Metric cards — 4 across

```razor
<div class="grid grid-cols-1 gap-4 sm:grid-cols-2 xl:grid-cols-4">
  <MetricCard … />
</div>
```

`Widgets/MetricCard.razor`:

```razor
<div class="relative rounded-card p-5 @ShellClass">
  <a href="@Href" class="btn-ghost-icon absolute top-5 end-5 @ArrowClass" aria-label="@Label">
    <svg class="h-3.5 w-3.5 rtl:-scale-x-100" …/>   @* ↗ mirrors to ↖ *@
  </a>

  <p class="text-13 @LabelClass">@Label</p>
  <p class="mt-2 text-34 font-medium @NumberClass"><bdi>@Ku.N(Value)</bdi></p>

  <div class="mt-3 flex items-center gap-1.5">
    <svg class="h-4 w-4 @FootClass" …/>
    <span class="text-xs @FootClass">@Footnote</span>
  </div>
</div>
```

State classes — **computed from thresholds, never hardcoded per card**:

| State | shell | label | number | footnote |
| --- | --- | --- | --- | --- |
| `Hero` | `bg-green-deep` | `text-on-deep` | `text-white` | `text-on-deep` |
| `Normal` | `bg-card border border-line` | `text-ink-2` | `text-ink-1` | `text-ink-2` |
| `Warn` | `bg-warn-tint border border-line` | `text-ink-2` | `text-warn` | `text-warn` |
| `Danger` | `bg-danger-tint border border-line` | `text-ink-2` | `text-danger` | `text-danger` |

```csharp
// Threshold, not decoration. Hero is hierarchy — a hero card can also be in a warn state,
// in which case Warn wins for the number and footnote colour.
static MetricState StateFor(string key, double value) => key switch {
    "relations"  => value == 0            ? MetricState.Danger : MetricState.Normal,
    "categories" => value > CategoryBudget ? MetricState.Warn   : MetricState.Normal,
    _            => MetricState.Normal,
};
```

Dashboard assignment (پڕۆمپت ٨ §5): 1 وشەی تۆمارکراو = hero · 2 واتای تۆمارکراو ·
3 پەیوەندی (danger at 0) · 4 پۆلەکان (warn over budget).

## 6. The hatch — the template's key trick

Incomplete / projected / remaining data is **hatched, never grey**. 45° `--border` lines
on transparent.

One class, defined once in `Styles/app.css`:

```css
/* A hatch PATTERN, not a decorative gradient. This is the single permitted use of
   repeating-linear-gradient in the app; everything else is flat colour. */
.hatch {
  background-image: repeating-linear-gradient(
    45deg,
    var(--border) 0 2px,
    transparent 2px 7px
  );
}
```

For SVG strokes (the gauge) you need a real `<pattern>`. Put it in one shared widget,
`Widgets/HatchDefs.razor`, and render it once per page:

```razor
<svg width="0" height="0" class="absolute" aria-hidden="true">
  <defs>
    <pattern id="hatch" width="7" height="7" patternTransform="rotate(45)"
             patternUnits="userSpaceOnUse">
      <line x1="0" y1="0" x2="0" y2="7" stroke="var(--border)" stroke-width="2" />
    </pattern>
  </defs>
</svg>
```

Then `stroke="url(#hatch)"`.

## 7. Bar chart — چالاکی هەفتانە

7 bars, capsule shape, RTL order (today at the start = right).

```razor
<div class="flex h-40 items-end justify-between gap-2" dir="rtl">
  @foreach (var d in Days)
  {
    <div class="flex flex-1 flex-col items-center gap-2">
      <div class="relative flex w-full flex-1 items-end">
        @if (d.HasChip)
        {
          <span class="absolute -top-1 start-1/2 -translate-x-1/2 rounded-chip
                       bg-green-deep px-1.5 py-0.5 text-11 text-white"><bdi>@d.Chip</bdi></span>
        }
        <div class="w-full rounded-full @(d.IsComplete ? "bg-green" : "hatch")"
             style="height:@(d.Pct)%"></div>
      </div>
      <span class="text-11 text-ink-3">@d.Initial</span>
    </div>
  }
</div>
```

Complete days: solid `bg-green`. Projected / partial days: `.hatch` with no fill. Never
a grey bar — grey reads as "disabled", hatch reads as "not yet".

## 8. Semicircular gauge — کوالیتی داتا

```razor
<svg viewBox="0 0 200 112" class="mx-auto w-full max-w-[220px]">
  @* Track: hatched remainder. Arc drawn right→left so it fills from the start edge in RTL. *@
  <path d="M 180 100 A 80 80 0 0 0 20 100" fill="none" stroke="url(#hatch)"
        stroke-width="18" stroke-linecap="round" />
  <path d="M 180 100 A 80 80 0 0 0 20 100" fill="none" stroke="var(--green)"
        stroke-width="18" stroke-linecap="round"
        stroke-dasharray="251.33"
        stroke-dashoffset="@((251.33 * (1 - Pct)).ToString("0.##", Inv))" />
</svg>

<p class="-mt-8 text-center text-30 font-medium text-ink-1"><bdi>@Ku.P(Pct)</bdi></p>
<p class="text-center text-xs text-ink-2">وشەکان پەیوەندییان هەیە</p>

<div class="mt-4 flex items-center justify-center gap-4">
  <span class="flex items-center gap-1.5 text-xs text-ink-2">
    <span class="h-2 w-2 rounded-full bg-green"></span>تەواو</span>
  <span class="flex items-center gap-1.5 text-xs text-ink-2">
    <span class="h-2 w-2 rounded-full bg-green-mid"></span>لە کارکردندا</span>
  <span class="flex items-center gap-1.5 text-xs text-ink-2">
    <span class="hatch h-2 w-4 rounded-full border border-line"></span>ماوە</span>
</div>
```

`251.33` = π × 80, the semicircle's length. At 0% the ring is **entirely hatched with no
green at all** — that is the honest rendering of the empty relations table, and it is the
point of the card. Do not floor it at a visible minimum.

## 9. List rows

Recent words (نوێترین وشەکان):

```razor
<div class="flex items-center gap-3 py-2.5">
  <span class="flex h-8 w-8 shrink-0 items-center justify-center rounded-chip
               bg-green-tint text-on-tint">
    <svg class="h-4 w-4" …/>
  </span>
  <div class="min-w-0">
    <p class="truncate text-sm font-medium text-ink-1">@w.Headword</p>
    <p class="text-xs text-ink-2">زیادکرا @Relative(w.CreatedAt)</p>
  </div>
</div>
```

Contributors (ئامادەکاران) — presence comes from پڕۆمپت ٩:

```razor
<div class="flex items-center gap-3 py-2.5">
  <span class="relative shrink-0">
    <span class="flex h-9 w-9 items-center justify-center rounded-full bg-green-tint
                 text-sm font-medium text-on-tint">@Initials(u.Name)</span>
    <span class="absolute -bottom-0.5 -end-0.5 h-2.5 w-2.5 rounded-full border-2 border-card
                 @DotClass(u.Status)"></span>
  </span>
  <div class="min-w-0">
    <p class="truncate text-sm font-medium text-ink-1">@u.Name</p>
    <p class="truncate text-xs text-ink-2">
      لەسەر وشەی <span class="font-medium text-ink-1">«@u.CurrentWord»</span> کار دەکات
    </p>
  </div>
  <span class="ms-auto @PillClass(u.Status)">@StatusLabel(u.Status)</span>
</div>
```

Status pills — the fill alone does not distinguish چالاک from بێ‌چالاکی (see the contrast
note in `tokens.md`), so the text colour must differ too:

| Status | classes |
| --- | --- |
| چالاک | `rounded-full bg-green-tint px-2.5 py-1 text-11 text-on-tint` |
| بێ‌چالاکی | `rounded-full bg-warn-tint px-2.5 py-1 text-11 text-warn` |
| دەرچوو | `rounded-full bg-page px-2.5 py-1 text-11 text-ink-2` |

Presence dot: چالاک `bg-green` · بێ‌چالاکی `bg-warn` · دەرچوو `bg-ink-3`.

## 10. Dark tile — کاتی کارکردنی ئەمڕۆ

```razor
<div class="rounded-card bg-green-deep p-5">
  <p class="text-sm text-white">کاتی کارکردنی ئەمڕۆ</p>
  <p class="mt-3 font-mono text-30 text-white"><bdi>@Ku.Sub(elapsed.ToString(@"hh\:mm\:ss"))</bdi></p>
  <div class="mt-4 flex items-center gap-2">
    <button class="flex h-10 w-10 items-center justify-center rounded-full bg-white text-green-deep">…</button>
    <button class="flex h-10 w-10 items-center justify-center rounded-full bg-danger text-white">…</button>
  </div>
</div>
```

The timer is `font-mono` and `dir="ltr"` — see `rtl-kurdish.md`.

## 11. Card wrapper — everything in the content grid

```razor
<section class="rounded-card border border-line bg-card p-5">
  <div class="mb-4 flex items-center gap-2">
    <h2 class="text-15 font-medium text-ink-1">@Title</h2>
    @* optional header pill on the end side *@
    <button class="ms-auto rounded-full border border-line px-2.5 py-1 text-11 text-ink-2">+ نوێ</button>
  </div>
  @ChildContent
</section>
```

Grid: `grid gap-4 lg:grid-cols-[2fr_1.5fr_1.5fr]`.

## 12. Tables — not in the reference, defined here so they don't drift

The word list and taxonomy screens need tables; the reference has none, so this is the
house rule:

```razor
<table class="w-full text-start">
  <thead>
    <tr class="border-b border-line">
      <th class="px-4 py-3 text-start text-11 font-normal text-ink-3">وشە</th>
    </tr>
  </thead>
  <tbody>
    <tr class="border-b border-line transition-colors last:border-0 hover:bg-page">
      <td class="px-4 py-3 text-sm text-ink-1">…</td>
    </tr>
  </tbody>
</table>
```

No zebra striping, no vertical rules, no shadow. Numeric cells get `dir="ltr"` and
`text-start`. Wrap in the card of recipe 11 with `p-0` and `overflow-hidden`.

## 13. Form controls

```css
.field {
  @apply w-full rounded-chip border border-line bg-card px-3 py-2.5 text-sm text-ink-1
         placeholder:text-ink-3 focus:border-green focus:outline-none focus:ring-1 focus:ring-green;
}
.field-label { @apply mb-1.5 block text-13 text-ink-2; }
.field-error { @apply mt-1 text-xs text-danger; }
```

Segmented control — the axis picker for the station screen (پڕۆمپت ٧). Keyboard-first:
digits pick values, so the digit hint is part of the control.

```razor
<div class="flex flex-wrap gap-1.5">
  @foreach (var (v, i) in Values.Select((v, i) => (v, i + 1)))
  {
    <button type="button"
            class="@(v.Id == Selected
                     ? "rounded-full bg-green-deep px-3.5 py-2 text-sm font-medium text-white"
                     : "rounded-full bg-page px-3.5 py-2 text-sm text-ink-2 hover:bg-green-tint hover:text-on-tint")">
      <span class="me-1.5 text-11 opacity-60" dir="ltr">@i</span>@v.NameKu
    </button>
  }
</div>
```

A conditional axis is **absent or present** — never render a disabled or greyed one.
There is no `disabled:` state in this design system for axis controls.

## 14. Empty states

```razor
<div class="flex flex-col items-center py-10 text-center">
  <span class="hatch mb-3 h-12 w-12 rounded-full border border-line"></span>
  <p class="text-sm text-ink-1">@Title</p>
  <p class="mt-1 text-xs text-ink-2">@Hint</p>
</div>
```

Reuse the hatch: "nothing here yet" and "not yet complete" are the same visual idea, and
using it consistently is what makes the language read as one system.
