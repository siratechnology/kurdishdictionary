/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./**/*.razor",
    "./**/*.html",
    "./**/*.cshtml",
  ],
  theme: {
    extend: {
      // Every colour is a CSS var declared in Styles/app.css. Nothing else may appear in
      // markup — see .claude/skills/admin-design-system/references/tokens.md.
      // Note: because these are vars and not hex, opacity modifiers (bg-green/10) do NOT
      // work. The design needs none; separation is done with borders and tints.
      colors: {
        page:          'var(--page)',
        card:          'var(--card)',
        line:          'var(--border)',
        'line-soft':   'var(--border-soft)',
        chrome:        'var(--chrome)',
        'chrome-soft': 'var(--chrome-soft)',
        'on-chrome':   'var(--on-chrome)',
        'on-chrome-2': 'var(--on-chrome-2)',
        'green-deep':  'var(--green-deep)',
        green:         'var(--green)',
        'green-mid':   'var(--green-mid)',
        'green-soft':  'var(--green-soft)',
        'green-tint':  'var(--green-tint)',
        'on-tint':     'var(--on-tint)',
        'on-deep':     'var(--on-deep)',
        'ink-1':       'var(--text-1)',
        'ink-2':       'var(--text-2)',
        'ink-3':       'var(--text-3)',
        warn:          'var(--warn)',
        'warn-tint':   'var(--warn-bg)',
        danger:        'var(--danger)',
        'danger-tint': 'var(--danger-bg)',
        overlay:       'var(--overlay)',
      },
      borderRadius: {
        shell: '24px',
        card:  '18px',
        chip:  '10px',
      },
      boxShadow: {
        // The only shadow in the app, and it belongs to the shell alone.
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
