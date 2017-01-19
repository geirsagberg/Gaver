import Immutable from 'seamless-immutable'
import * as Api from './api'
import { browserHistory } from 'react-router'
import Auth0Lock from 'auth0-lock'
import * as auth from 'utils/auth'
import { tryOrNotify } from 'utils'

const auth0ClientId = 'q57tZFsUo6359RyFzmzB0VYrmCeLVrBi'
const auth0Domain = 'sagberg.eu.auth0.com'
const redirectUrl = `${window.location.protocol}//${window.location.host}/login`

const namespace = 'gaver/user/'

const LOGGED_OUT = namespace + 'LOGGED_OUT'
const LOG_IN_SUCCESSFUL = namespace + 'LOG_IN_SUCCESSFUL'
const USER_INFO_LOADED = namespace + 'USER_INFO_LOADED'

const initialState = Immutable({})

const lock = new Auth0Lock(auth0ClientId, auth0Domain, {
  auth: {
    redirectUrl,
    params: {
      scope: 'openid name email'
    },
    responseType: 'token'
  }
})

export default function reducer(state = initialState, action = {}) {
  switch (action.type) {
    case LOG_IN_SUCCESSFUL:
      return state.set('isLoggedIn', true)
    case LOGGED_OUT:
      return initialState
    case USER_INFO_LOADED:
      return state.merge(action.data)
  }
  return state
}

function userInfoLoaded(data) {
  return {
    type: USER_INFO_LOADED,
    data
  }
}

function loggedOut() {
  return {
    type: LOGGED_OUT
  }
}

function logInSuccessful(user) {
  return {
    type: LOG_IN_SUCCESSFUL
  }
}

function redirectAfterLogin() {
  const urlAfterLogin = auth.loadUrlAfterLogin()
  if (urlAfterLogin) {
    auth.clearUrlAfterLogin()
    browserHistory.push(urlAfterLogin)
  }
}

export const loadUserInfo = () => dispatch => tryOrNotify(async () => {
  const accessToken = auth.loadAccessToken()
  if (!accessToken) {
    throw new Error('Access token is missing. Please refresh and try again.')
  }
  const userInfo = await Api.loadUserInfo(accessToken)
  dispatch(userInfoLoaded(userInfo))
})

export const setUrlAfterLogin = url => () => {
  const a = document.createElement('a')
  a.href = url
  auth.saveUrlAfterLogin(`${a.pathname}${a.hash}${a.search}`)
}

export const logOut = () => async dispatch => {
  auth.clearTokens()
  dispatch(loggedOut())
  browserHistory.replace('/login')
}

export const initAuth = () => dispatch => {
  lock.on('authenticated', authResult => {
    auth.saveIdToken(authResult.idToken)
    auth.saveAccessToken(authResult.accessToken)
    dispatch(logInSuccessful())
    redirectAfterLogin()
  })
  if (auth.loadIdToken()) {
    dispatch(logInSuccessful())
    redirectAfterLogin()
  }
}

export const logIn = () => async dispatch => {
  lock.show()
}