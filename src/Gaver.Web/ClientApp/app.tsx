import '@babel/polyfill'
import './bootstrap'
import React from 'react'
import { render } from 'react-dom'
import Layout from './Layout'
import theme from './theme'
import { ThemeProvider } from '@material-ui/styles'
import { CssBaseline } from '@material-ui/core'

render(
  <ThemeProvider theme={theme}>
    <CssBaseline>
      <Layout />
    </CssBaseline>
  </ThemeProvider>,
  document.getElementById('react-app')
)
