import * as React from 'react'
import { browserHistory, Router, IndexRoute, Route } from 'react-router'
import Layout from './components/Layout'
import MyList from './components/MyList'
import Login from './components/Login'
import SharedList from './components/SharedList'
import AuthService from 'utils/authService'
import { loadingStarted, loadingStopped } from './store/ui'

const auth0ClientId = 'q57tZFsUo6359RyFzmzB0VYrmCeLVrBi'
const auth0Domain = 'sagberg.eu.auth0.com'

const auth = new AuthService({
  clientId: auth0ClientId,
  domain: auth0Domain,
  redirectUrl: `${window.location.protocol}//${window.location.host}/login`
})

export default function createRoutes(store) {
  auth.on('loadingStarted', () => {
    store.dispatch(loadingStarted())
  })

  auth.on('loadingStopped', () => {
    store.dispatch(loadingStopped())
  })

  const requireAuth = (nextState, replace) => {
    if (!auth.loggedIn()) {
      auth.setUrlAfterLogin(window.location)
      replace({
        pathname: '/login'
      })
      return false
    }
    return true
  }

  const redirectIfOwner = (nextState, replace) => {
    if (requireAuth(nextState, replace)) {
      var listId = parseInt(nextState.params.id)
      if (store.getState().user.wishListId === listId) {
        replace('/')
      }
    }
  }

  return (
    <Router history={browserHistory}>
      <Route path='/' component={Layout} auth={auth}>
        <IndexRoute component={MyList} onEnter={requireAuth} />
        <Route path='list/:id' component={SharedList} onEnter={redirectIfOwner} />
        <Route path='login' component={Login} />
      </Route>
    </Router>
  )
}
