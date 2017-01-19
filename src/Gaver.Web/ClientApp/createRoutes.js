import * as React from 'react'
import { browserHistory, Router, IndexRoute, Route } from 'react-router'
import Layout from './components/Layout'
import MyList from './components/MyList'
import Login from './components/Login'
import SharedList from './components/SharedList'
import { setUrlAfterLogin } from 'store/user'
import { AccessStatus } from './enums'
import { showError } from './utils/notifications'
import * as Api from './store/sharedList/api'

export default function createRoutes(store) {
  const {getState, dispatch} = store

  const checkAccess = async (nextState, replace, next) => {
    if (!getState().user.isLoggedIn) {
      dispatch(setUrlAfterLogin(window.location))
      replace('/login')
    }
    next()
  }

  const checkSharedListAccess = async (nextState, replace, next) => {
    if (!getState().user.isLoggedIn) {
      dispatch(setUrlAfterLogin(window.location))
      replace('/login')
    } else {
      const listId = parseInt(nextState.params.id)
      const token = nextState.location.query.token
      if (token) {
        try {
          await Api.registerToken(listId, token)
        } catch (error) {
          showError(error)
          replace('/')
          next()
          return
        }
      }
      const accessStatus = await Api.checkSharedListAccess(listId)
      switch (accessStatus) {
        case AccessStatus.NotInvited:
          showError('Du er ikke invitert til denne listen')
          // TODO: Egen liste for å be om tilgang
          replace('/')
          break
        case AccessStatus.Owner:
          replace('/')
          break
      }
    }
    next()
  }

  return (
    <Router history={browserHistory}>
      <Route path='/' component={Layout}>
        <IndexRoute component={MyList} onEnter={checkAccess} />
        <Route path='list/:id' component={SharedList} onEnter={checkSharedListAccess} />
        <Route path='login' component={Login} />
      </Route>
    </Router>
  )
}
