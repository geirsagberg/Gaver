import 'babel-polyfill'
import 'bootstrap'
import * as React from 'react'
import { render } from 'react-dom'
import { Provider } from 'react-redux'
import configureStore from './configureStore'
import setupProgress from 'utils/progress'
import 'bootswatch/darkly/bootstrap.css'
import 'toastr/build/toastr.css'
import 'nprogress/nprogress.css'
import './css/site.css'
import Layout from './components/Layout'

setupProgress()

// Get the application-wide store instance, prepopulating with state from the server where available.
const store = configureStore()

render(
  <Provider store={store}>
    <Layout />
  </Provider>,
  document.getElementById('app')
)
