import { defineConfig } from 'vite'
import { svelte } from '@sveltejs/vite-plugin-svelte'

export default defineConfig({
  plugins: [
    svelte({
      compilerOptions: {
        dev: true
      }
    })
  ],
  server: {
    proxy: {
      '/api': {
        target: 'http://localhost:5071',
        changeOrigin: true
      }
    }
  }
})