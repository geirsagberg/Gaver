import Auth0Lock from 'auth0-lock'
import { tryOrNotify } from '~/utils'
import * as auth from '~/utils/auth'
import { showConfirm } from '~/utils/dialogs'
import { authStarted, loggedOut, logInSuccessful } from '.'
import * as Api from './api'

const auth0ClientId = 'q57tZFsUo6359RyFzmzB0VYrmCeLVrBi'
const auth0Domain = 'sagberg.eu.auth0.com'
const redirectUrl = `${window.location.protocol}//${window.location.host}/login`

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

export const setUrlAfterLogin = url => () => {
  const a = document.createElement('a')
  a.href = url
  auth.saveUrlAfterLogin(`${a.pathname}${a.hash}${a.search}`)
}

export const logOut = () => async dispatch => {
  if (await showConfirm('Vil du logge ut?')) {
    auth.clearTokens()
    dispatch(loggedOut())
  }
}

const completeLogin = (dispatch, accessToken) =>
  tryOrNotify(async () => {
    const userInfo = await Api.loadUserInfo(accessToken)
    dispatch(logInSuccessful(userInfo))
  })

export const initAuth = () => async dispatch => {
  lock.on('authenticated', async authResult => {
    auth.saveIdToken(authResult.idToken)
    auth.saveAccessToken(authResult.accessToken)
    await completeLogin(dispatch, authResult.accessToken)
  })
  const accessToken = auth.loadAccessToken()
  // Calling loadIdToken to check whether JWT is still valid
  if (accessToken && auth.loadIdToken()) {
    dispatch(authStarted())
    await completeLogin(dispatch, accessToken)
  } else if (location.hash.includes('access_token')) {
    dispatch(authStarted())
  }
}

export const logIn = () => dispatch => {
  lock.show()
}

export const actionCreators = {
  logIn,
  initAuth,
  logOut,
  setUrlAfterLogin
}
