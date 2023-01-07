import react from '@vitejs/plugin-react'
import { resolve } from 'path'
import { defineConfig } from 'vite'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react()],
  root: 'frontend',
  resolve: {
    alias: {
      '~': resolve(__dirname, 'frontend/src'),
    },
  },
  server: {
    proxy: {
      '/api': 'http://0.0.0.0:5000',
      '/hub': 'http://0.0.0.0:5000',
    },
    port: 8080,
  },
  build: {
    minify: false,
    sourcemap: true,
    outDir: '../src/Gaver.Web/wwwroot',
    emptyOutDir: true,
    assetsInlineLimit: 0,
    rollupOptions: {
      output: {
        manualChunks: {
          mui: ['@mui/material', '@emotion/react', '@emotion/styled'],
        },
      },
    },
  },
})
