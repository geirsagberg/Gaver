import Immutable from 'seamless-immutable'
import * as Api from './api'
import { showError, showSuccess } from 'utils/notifications'
import Cookies from 'js-cookie'
import { browserHistory } from 'react-router'
import $ from 'jquery'
// import Auth0 from 'auth0-js'
import AuthService from 'utils/authService'

const auth0ClientId = 'q57tZFsUo6359RyFzmzB0VYrmCeLVrBi'
const auth0Domain = 'sagberg.eu.auth0.com'

const namespace = 'gaver/user/'

const LOGGED_OUT = namespace + 'LOGGED_OUT'
const LOG_IN_SUCCESSFUL = namespace + 'LOG_IN_SUCCESSFUL'

const initialState = Immutable({})

export default function reducer (state = initialState, action = {}) {
  switch (action.type) {
    case LOG_IN_SUCCESSFUL:
      return action.user.merge({isLoggedIn: true})
    case LOGGED_OUT:
      return initialState
  }
  return state
}

function loggedOut () {
  return {
    type: LOGGED_OUT
  }
}

export const logOut = () => async dispatch => {
  Cookies.remove('user')
  await Api.logOut()
  dispatch(loggedOut())
  browserHistory.push('/login')
  return {
    type: LOGGED_OUT
  }
}

function logInSuccessful (user) {
  return {
    type: LOG_IN_SUCCESSFUL,
    user
  }
}

const authService = new AuthService({
  clientId: auth0ClientId,
  domain: auth0Domain
})

export const logIn = (email, redirect) => async dispatch => {
  try {
    authService.login()
    // const auth = new Auth0({
    //   domain: auth0Domain,
    //   clientID: auth0ClientId,
    //   responseType: 'token',
    //   callbackURL: '//' + window.location.host + '/login'
    // })

    // auth.requestMagicLink({
    //   email
    // }, error => {
    //   if (error) {
    //     showError(error)
    //   } else {
    //     showSuccess('En e-post har blitt sendt til ' + email + 'med lenke for innlogging')
    //   }
    // })
    // const lock = new Auth0LockPasswordless(auth0ClientId, auth0Domain)

    // lock.magiclink()

    // var user = await Api.logIn(name)
    // dispatch(logInSuccessful(user))
    // Cookies.set('user', user.name)
    // redirect()
  } catch (error) {
    showError(error)
  }
}
