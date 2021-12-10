import { CssBaseline } from '@mui/material';
import withStyles from '@mui/styles/withStyles';
import createStyles from '@mui/styles/createStyles';
import React from 'react'
import { render } from 'react-dom'
import Layout from './Layout'
import theme from './theme'
import { Provider } from 'overmind-react'
import { createOvermind } from 'overmind'
import { config } from './overmind'
import ErrorView from './components/ErrorView'
import { ThemeProvider, Theme, StyledEngineProvider } from '@mui/material/styles';

import { WithStyles } from '@mui/styles';


declare module '@mui/styles/defaultTheme' {
  // eslint-disable-next-line @typescript-eslint/no-empty-interface
  interface DefaultTheme extends Theme {}
}


const overmind = createOvermind(config)

const styles = createStyles({
  root: {
    height: '100%',
    position: 'relative',
    zIndex: 0,
    display: 'flex',
    justifyContent: 'center',
  },
})

class ErrorBoundaryInner extends React.Component<WithStyles<typeof styles>, { hasError: boolean }> {
  state = {
    hasError: false,
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
