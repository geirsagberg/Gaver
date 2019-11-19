import { CssBaseline, withStyles, createStyles } from '@material-ui/core'
import React from 'react'
import { render } from 'react-dom'
import Layout from './Layout'
import theme from './theme'
import { Provider } from 'overmind-react'
import { createOvermind } from 'overmind'
import { config } from './overmind'
import ErrorView from './components/ErrorView'
import { ThemeProvider, WithStyles } from '@material-ui/core/styles'

const overmind = createOvermind(config)

const styles = createStyles({
  root: {
    height: '100%',
    position: 'relative',
    zIndex: 0,
    display: 'flex',
    justifyContent: 'center'
  }
})

class ErrorBoundaryInner extends React.Component<WithStyles<typeof styles>, { hasError: boolean }> {
  state = {
    hasError: false
  }
  static getDerivedStateFromError() {
    return { hasError: true }
  }
  render() {
    const { classes } = this.props
    return this.state.hasError ? (
      <div className={classes.root}>
        <ErrorView onBackClicked={() => this.setState({ hasError: false })}>Noe gikk galt!</ErrorView>
      </div>
    ) : (
      this.props.children
    )
  }
}

const ErrorBoundary = withStyles(styles)(ErrorBoundaryInner)

render(
  <Provider value={overmind}>
    <ThemeProvider theme={theme}>
      <CssBaseline>
        <ErrorBoundary>
          <Layout />
        </ErrorBoundary>
      </CssBaseline>
    </ThemeProvider>
  </Provider>,
  document.getElementById('react-app')
)
