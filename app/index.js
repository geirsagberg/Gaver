import 'babel-polyfill'
import { hashHistory, Route, Router } from 'react-router'
import { render } from 'react-dom'
import React from 'react'
import { Provider } from 'redux'

const Root = () => (
  <Provider>
    <Router history={hashHistory}>
      <Route path="/" component={App} />
    </Router>
  </Provider>
)

render(<Root store={store} history={hashHistory} />, document.getElementById('root'))