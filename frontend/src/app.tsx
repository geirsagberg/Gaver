import { CssBaseline } from '@mui/material'
import { StyledEngineProvider, ThemeProvider } from '@mui/material/styles'
import { createOvermind } from 'overmind'
import { Provider } from 'overmind-react'
import React, { ReactNode } from 'react'
import { render } from 'react-dom'
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

render(
  <Provider value={overmind}>
    <StyledEngineProvider injectFirst>
      <ThemeProvider theme={theme}>
        <CssBaseline>
          <ErrorBoundary>
            <Layout />
          </ErrorBoundary>
        </CssBaseline>
      </ThemeProvider>
    </StyledEngineProvider>
  </Provider>,
  document.getElementById('react-app')
)
