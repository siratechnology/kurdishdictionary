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
        nrt: ['NRT', 'system-ui', '-apple-system', 'sans-serif'],
      },
    },
  },
  plugins: [],
}
