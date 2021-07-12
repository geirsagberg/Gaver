import { tryOrNotify } from '~/utils'
import AuthService from '~/utils/AuthService'
import { showError } from '~/utils/notifications'
import { Context } from '..'
import { RouteCallbackArgs } from '../routing/effects'

export const logOut = async ({ state }: Context) => {
  await AuthService.logout()
  state.auth = {}
}

export const logIn = async () => {
  await AuthService.login()
}

export const checkSession = ({
  state,
  effects,
  actions: {
    friends: { loadFriends },
    userGroups: { loadUserGroups },
  },
}: Context) =>
  tryOrNotify(async () => {
    if (state.auth.isLoggedIn) return

    if (AuthService.isAuthenticated()) {
      try {
        state.auth.isLoggingIn = true
        state.auth.user = await effects.api.getUserInfo()
        state.auth.isLoggedIn = true
        await Promise.all([loadFriends(), loadUserGroups()])
      } finally {
        state.auth.isLoggingIn = false
      }
    }
  })

export const handleAuthentication = ({ effects }: Context) => {
  AuthService.handleAuthentication(async ({ returnUrl, error }) => {
    if (error) {
      console.error(error)
      showError('Noe gikk galt ved innloggingen. Prøv igjen om litt.')
    } else {
      effects.routing.redirect(returnUrl || '')
    }
  })
}

export const redirectIfNotLoggedIn = (
  { actions }: Context,
  { next }: RouteCallbackArgs
) => {
  if (AuthService.isAuthenticated()) {
    next()
  } else {
    actions.auth.logIn()
  }
}
