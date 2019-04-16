import '@babel/polyfill'
import { CssBaseline } from '@material-ui/core'
import { ThemeProvider } from '@material-ui/styles'
import React from 'react'
import { render } from 'react-dom'
import './bootstrap'
import './initial'
import Layout from './Layout'
import theme from './theme'

render(
  <ThemeProvider theme={theme}>
    <CssBaseline>
      <Layout />
    </CssBaseline>
  </ThemeProvider>,
  document.getElementById('react-app')
)
