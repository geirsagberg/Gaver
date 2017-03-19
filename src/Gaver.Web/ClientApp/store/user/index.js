import Immutable from 'seamless-immutable'
import * as Api from './api'
import Auth0Lock from 'auth0-lock'
import * as auth from 'utils/auth'
import { tryOrNotify } from 'utils'

const auth0ClientId = 'q57tZFsUo6359RyFzmzB0VYrmCeLVrBi'
const auth0Domain = 'sagberg.eu.auth0.com'
const redirectUrl = `${window.location.protocol}//${window.location.host}/login`

const namespace = 'gaver/user/'

const LOGGED_OUT = namespace + 'LOGGED_OUT'
const LOG_IN_SUCCESSFUL = namespace + 'LOG_IN_SUCCESSFUL'
const AUTH_STARTED = namespace + 'AUTH_STARTED'

const initialState = Immutable({})

const lock = new Auth0Lock(auth0ClientId, auth0Domain, {
  auth: {
    redirectUrl,
    params: {
      scope: 'openid name email'
    },
    responseType: 'token'
  },
  theme: {
    logo: '/images/gift.png',
    primaryColor: '#375a7f'
  },
  languageDictionary: {
    title: 'Gaver'
  },
  language: 'nb'
})

export default function reducer(state = initialState, action = {}) {
  switch (action.type) {
    case LOG_IN_SUCCESSFUL:
      return state.merge(action.data).set('isLoggedIn', true).set('isLoggingIn', false)
    case LOGGED_OUT:
      return initialState
    case AUTH_STARTED:
      return state.set('isLoggingIn', true)
  }
  return state
}

function loggedOut() {
  return {
    type: LOGGED_OUT
  }
}

function logInSuccessful(data) {
  return {
    type: LOG_IN_SUCCESSFUL,
    data
  }
}

function authStarted() {
  return {
    type: AUTH_STARTED
  }
}

export const setUrlAfterLogin = url => () => {
  const a = document.createElement('a')
  a.href = url
  auth.saveUrlAfterLogin(`${a.pathname}${a.hash}${a.search}`)
}

export const logOut = () => async dispatch => {
  auth.clearTokens()
  dispatch(loggedOut())
  history.replace('/login')
}

const completeLogin = async (dispatch, accessToken) => tryOrNotify(async () => {
  const userInfo = await Api.loadUserInfo(accessToken)
  dispatch(logInSuccessful(userInfo))
})

export const initAuth = () => async dispatch => {
  dispatch(authStarted())
  lock.on('authenticated', async authResult => {
    auth.saveIdToken(authResult.idToken)
    auth.saveAccessToken(authResult.accessToken)
    await completeLogin(dispatch, authResult.accessToken)
  })
  const accessToken = auth.loadAccessToken()
  // Calling loadIdToken to check whether JWT is still valid
  if (accessToken && auth.loadIdToken()) {
    await completeLogin(dispatch, accessToken)
  }
}

export const logIn = () => async dispatch => {
  lock.show()
}