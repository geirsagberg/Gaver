import * as React from 'react'
import { browserHistory, Router, IndexRoute, Route } from 'react-router'
import Layout from './components/Layout'
import MyList from './components/MyList'
import Login from './components/Login'

function requireAuth (store) {
  return function (nextState, replace) {
    if (!store.getState().user.isLoggedIn) {
      replace({
        pathname: '/login',
        state: { nextPathname: nextState.location.pathname }
      })
    }
  }
}

export default function createRoutes (store) {
  return (
    <Router history={browserHistory}>
      <Route path='/' component={Layout}>
        <IndexRoute component={MyList} onEnter={requireAuth(store)} />
        <Route path='login' component={Login} />
      </Route>
    </Router>
  )
}
