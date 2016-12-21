import * as React from 'react'
import { browserHistory, Router, IndexRoute, Route } from 'react-router'
import Layout from './components/Layout'
import MyList from './components/MyList'
import Login from './components/Login'
import SharedList from './components/SharedList'
import { setUrlAfterLogin, loadUserInfo } from 'store/user'

export default function createRoutes(store) {
  const {getState, dispatch} = store

  const requireAuth = async (nextState, replace, next) => {
    if (!getState().user.isLoggedIn) {
      dispatch(setUrlAfterLogin(window.location))
      replace('/login')
    } else if (!getState().user.wishListId) {
      await dispatch(loadUserInfo())
    }
    next()
  }

  const redirectIfOwner = async (nextState, replace, next) => {
    if (!getState().user.isLoggedIn) {
      dispatch(setUrlAfterLogin(window.location))
      replace('/login')
    } else {
      if (!getState().user.wishListId) {
        await dispatch(loadUserInfo())
      }
      const listId = parseInt(nextState.params.id)
      if (getState().user.wishListId === listId) {
        replace('/')
      }
    }
    next()
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
