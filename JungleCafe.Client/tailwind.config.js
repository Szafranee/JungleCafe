/** @type {import('tailwindcss').Config} */
export default {
  content: ['./src/**/*.{html,js,svelte,ts}'],
  theme: {
    extend: {
      colors: {
        'jungle': {
          primary: '#2D5A27',    // Dark green
          secondary: '#4A8B3B',  // Medium green
          accent: '#82C341',     // Light green
          brown: '#634832',      // Warm brown
          sand: '#D4B59D',       // Sand accent
        }
      },
      fontFamily: {
        sans: ['"Poppins"', 'sans-serif'],
        display: ['"Playfair Display"', 'serif']
      },
      backgroundImage: {
        'leaves-pattern': "url('/img/leaves_pattern.png')",
      }
    }
  },
  plugins: []
}