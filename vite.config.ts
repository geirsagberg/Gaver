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
      '/api': 'http://localhost:5000',
      '/hub': 'http://localhost:5000',
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
          mui: ['@material-ui/core'],
        },
      },
    },
  },
})
