import { CssBaseline } from '@mui/material'
import { ThemeProvider } from '@mui/material/styles'
import { createRoot } from 'react-dom/client'
import Layout from './Layout'
import theme from './theme'
import ErrorBoundary from './components/ErrorBoundary'

const container = document.getElementById('app')!
const root = createRoot(container)
root.render(
  <ThemeProvider theme={theme}>
    <CssBaseline>
      <ErrorBoundary>
        <Layout />
      </ErrorBoundary>
    </CssBaseline>
  </ThemeProvider>
)
