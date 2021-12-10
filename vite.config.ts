import reactRefresh from '@vitejs/plugin-react-refresh'
import { resolve } from 'path'
import { defineConfig } from 'vite'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [reactRefresh()],
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
    outDir: '../src/Gaver.Web/wwwroot',
    emptyOutDir: true,
    assetsInlineLimit: 0,
    rollupOptions: {
      output: {
        manualChunks: {
          mui: ['@mui/material', '@mui/styles', '@emotion/react', '@emotion/styled'],
        },
      },
    },
  },
})
