import 'babel-polyfill'
import 'bootstrap'
import * as React from 'react'
import * as ReactDOM from 'react-dom'
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
const initialState = window.initialReduxState
const store = configureStore(initialState)

ReactDOM.render(
  <Provider store={store}>
    <Layout />
  </Provider>, document.getElementById('react-app')
)