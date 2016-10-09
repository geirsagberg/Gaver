import * as React from 'react'
import { browserHistory, Router, IndexRoute, Route } from 'react-router'
import Layout from './components/Layout'
import MyList from './components/MyList'
import Login from './components/Login'
import SharedList from './components/SharedList'

export default function createRoutes(store) {
  const requireAuth = (nextState, replace) => {
    if (!store.getState().user.isLoggedIn) {
      replace({
        pathname: '/login',
        state: { nextPathname: nextState.location.pathname }
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
      <Route path='/' component={Layout}>
        <IndexRoute component={MyList} onEnter={requireAuth} />
        <Route path='list/:id' component={SharedList} onEnter={redirectIfOwner} />
        <Route path='login' component={Login} />
      </Route>
    </Router>
  )
}
