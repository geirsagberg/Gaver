import '@babel/polyfill'
import './bootstrap'
import React from 'react'
import { render } from 'react-dom'
import { Provider } from 'react-redux'
import configureStore from './configureStore'
import Layout from './Layout'
import { MuiThemeProvider, CssBaseline } from '@material-ui/core'
import theme from './theme'
import { reducers } from './store'

const { store, updateStore } = configureStore(reducers)

render(
  <Provider store={store}>
    <MuiThemeProvider theme={theme}>
      <CssBaseline>
        <Layout />
      </CssBaseline>
    </MuiThemeProvider>
  </Provider>,
  document.getElementById('react-app')
)

if (module && module.hot) {
  module.hot.accept('./store', () => updateStore(reducers))
}
