import { tryOrNotify } from '~/utils'
import AuthService from '~/utils/AuthService'
import { showError } from '~/utils/notifications'
import { Action } from '..'
import { RouteCallbackArgs } from '../routing/effects'

export const logOut: Action = ({ state }) => {
  AuthService.logout()
  state.auth = {}
}

export const logIn: Action = () => {
  AuthService.login()
}

export const checkSession: Action = ({ state, effects, actions }) =>
  tryOrNotify(async () => {
    if (state.auth.isLoggedIn) return

    if (AuthService.isAuthenticated()) {
      try {
        state.auth.isLoggingIn = true
        state.auth.user = await effects.api.getUserInfo()
        state.auth.isLoggedIn = true
        await actions.app.loadSharedLists()
      } finally {
        state.auth.isLoggingIn = false
      }
    }
  })

export const handleAuthentication: Action = ({ effects, actions }) => {
  AuthService.handleAuthentication(async ({ returnUrl, error }) => {
    if (error) {
      console.error(error)
      showError('Noe gikk galt ved innloggingen. Prøv igjen om litt.')
    } else {
      effects.routing.redirect(returnUrl)
    }
  })
}

export const redirectIfNotLoggedIn: Action<RouteCallbackArgs> = (
  {
    actions: {
      auth: { logIn }
    }
  },
  { next }
) => {
  if (AuthService.isAuthenticated()) {
    next()
  } else {
    logIn()
  }
}
