import '@babel/polyfill'
import React from 'react'
import { render } from 'react-dom'
import { Provider } from 'react-redux'
import configureStore from './configureStore'
import 'nprogress/nprogress.css'
import './css/site.css'
import Layout from './Layout'
import { setupProgress } from './utils/progress'
import { MuiThemeProvider, CssBaseline } from '@material-ui/core'
import theme from './theme'
import { reducers } from './store'

setupProgress()

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
