/** @type {import('tailwindcss').Config} */
export default {
  content: ['./src/**/*.{html,js,svelte,ts}'],
  theme: {
    extend: {
      colors: {
        'jungle-green': {
          light: '#acde70',
          DEFAULT: '#7ab940',
          dark: '#69a336'
        },
        'jungle-bg': '#c1dea3'
      },
      fontFamily: {
        playwrite: ['"Playwrite GB S"', 'cursive']
      }
    }
  },
  plugins: []
}
