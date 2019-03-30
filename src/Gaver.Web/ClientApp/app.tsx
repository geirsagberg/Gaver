import '@babel/polyfill'
import { CssBaseline, MuiThemeProvider } from '@material-ui/core'
import React from 'react'
import { render } from 'react-dom'
import './bootstrap'
import Layout from './OvermindLayout'
import theme from './theme'

render(
  <MuiThemeProvider theme={theme}>
    <CssBaseline>
      <Layout />
    </CssBaseline>
  </MuiThemeProvider>,
  document.getElementById('react-app')
)
