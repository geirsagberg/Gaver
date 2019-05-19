import { CssBaseline } from '@material-ui/core'
import { ThemeProvider } from '@material-ui/styles'
import React from 'react'
import { render } from 'react-dom'
import Layout from './Layout'
import theme from './theme'
import { Provider } from 'overmind-react'
import { createOvermind } from 'overmind'
import { config } from './overmind'

const overmind = createOvermind(config)

render(
  <Provider value={overmind}>
    <ThemeProvider theme={theme}>
      <CssBaseline>
        <Layout />
      </CssBaseline>
    </ThemeProvider>
  </Provider>,
  document.getElementById('react-app')
)
