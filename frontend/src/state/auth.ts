import { Context } from 'page'
import { proxy } from 'valtio'
import { RouteCallbackArgs } from '~/overmind/routing/effects'
import { CurrentUserDto } from '~/types/data'
import { tryOrNotify } from '~/utils'
import AuthService from '~/utils/AuthService'
import { showError } from '~/utils/notifications'

export interface AuthState {
  user?: CurrentUserDto
  isLoggedIn?: boolean
  isLoggingIn?: boolean
}

const initialState: AuthState = {
  user: undefined,
  isLoggedIn: false,
  isLoggingIn: false,
}

export const authState = proxy<AuthState>()

export const logOut = async () => {
  await AuthService.logout()
  Object.assign(authState, initialState)
}

export const logIn = async () => {
  await AuthService.login()
}

export const checkSession = () =>
  tryOrNotify(async () => {
    if (authState.isLoggedIn) return

    if (AuthService.isAuthenticated()) {
      try {
        authState.isLoggingIn = true
        authState.user = await effects.api.getUserInfo()
        authState.isLoggedIn = true
        await Promise.all([loadFriends(), loadUserGroups()])
      } finally {
        authState.isLoggingIn = false
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

export const redirectIfNotLoggedIn = ({ actions }: Context, { next }: RouteCallbackArgs) => {
  if (AuthService.isAuthenticated()) {
    next()
  } else {
    actions.auth.logIn()
  }
}
