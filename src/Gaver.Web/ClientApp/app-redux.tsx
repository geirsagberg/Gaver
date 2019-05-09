import { CssBaseline } from '@material-ui/core'
import { ThemeProvider } from '@material-ui/styles'
import React from 'react'
import { render } from 'react-dom'
import Layout from './Layout'
import theme from './theme'
import { Provider } from 'react-redux'
import store from './redux/store'

render(
  <Provider store={store}>
    <ThemeProvider theme={theme}>
      <CssBaseline>
        <Layout />
      </CssBaseline>
    </ThemeProvider>
  </Provider>,
  document.getElementById('react-app')
)
