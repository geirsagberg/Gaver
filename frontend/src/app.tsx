import { CssBaseline } from '@mui/material'
import { ThemeProvider } from '@mui/material/styles'
import { createOvermind } from 'overmind'
import { Provider } from 'overmind-react'
import React, { ReactNode } from 'react'
import { createRoot } from 'react-dom/client'
import ErrorView from './components/ErrorView'
import Layout from './Layout'
import { config } from './overmind'
import theme from './theme'

const overmind = createOvermind(config)

class ErrorBoundary extends React.Component<{ children: ReactNode }, { hasError: boolean }> {
  state = {
    hasError: false,
  }
  static getDerivedStateFromError() {
    return { hasError: true }
  }
  render() {
    return this.state.hasError ? (
      <ErrorView onBackClicked={() => this.setState({ hasError: false })}>Noe gikk galt!</ErrorView>
    ) : (
      this.props.children
    )
  }
}

const container = document.getElementById('app')!
const root = createRoot(container)
root.render(
  <Provider value={overmind}>
    <ThemeProvider theme={theme}>
      <CssBaseline>
        <ErrorBoundary>
          <Layout />
        </ErrorBoundary>
      </CssBaseline>
    </ThemeProvider>
  </Provider>
)
