import 'babel-polyfill'
import 'bootstrap'
import * as React from 'react'
import * as ReactDOM from 'react-dom'
import { browserHistory, Router } from 'react-router'
import { Provider } from 'react-redux'
import routes from './routes'
import configureStore from './configureStore'
import $script from 'scriptjs'
import setupProgress from 'utils/progress'
import 'bootswatch/darkly/bootstrap.css'
import 'toastr/build/toastr.css'
import 'nprogress/nprogress.css'
import './css/site.css'

// Setup SignalR
import 'ms-signalr-client'

setupProgress()

// Get the application-wide store instance, prepopulating with state from the server where available.
const initialState = window.initialReduxState
const store = configureStore(initialState)


// Ensure the dynamic SignalR script is loaded before rendering
$script('/signalr/hubs', () => {
  // This code starts up the React app when it runs in a browser. It sets up the routing configuration
  // and injects the app into a DOM element.
  ReactDOM.render(
    <Provider store={store}>
      <Router history={browserHistory} children={routes} />
    </Provider>, document.getElementById('react-app'))
})
