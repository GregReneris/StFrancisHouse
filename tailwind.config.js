/** @type {import('tailwindcss').Config} */
module.exports = {
  purge: {
    enabled: true,
    content: [
      './StFrancisHouse/Pages/**/*.cshtml',
      './StFrancisHouse/Views/**/*.cshtml'],
  },
  darkMode: 'media',
  theme: {
    extend: {},
  },
  varients: {
    extend: {},
  },
  plugins: [require('daisyui')],
}
