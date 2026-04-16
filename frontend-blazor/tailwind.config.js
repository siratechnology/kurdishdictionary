/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./**/*.razor",
    "./**/*.html",
    "./**/*.cshtml",
  ],
  theme: {
    extend: {
      fontFamily: {
        sans: ['nrt-reg', 'system-ui', '-apple-system', 'sans-serif'],
        nrt: ['nrt-reg', 'system-ui', '-apple-system', 'sans-serif'],
      },
    },
  },
  plugins: [],
}
