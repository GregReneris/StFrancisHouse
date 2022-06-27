/** @type {import('tailwindcss').Config} */
module.exports = {
  purge: {
    enabled: true,
    content: [
      './Pages/**/*.cshtml',
      './Views/**/*.cshtml'],
  },
  darkMode: false,
  theme: {
    extend: {},
  },
  varients: {
    extend: {},
  },
  plugins: [require('daisyui')],
}
