import { tryOrNotify } from '~/utils'
import AuthService from '~/utils/AuthService'
import { showError } from '~/utils/notifications'
import { Action } from '..'

export const logOut: Action = ({ state, effects }) => {
  AuthService.logout()
  state.auth = {}
  effects.routing.show('start')
}

export const logIn: Action = () => {
  AuthService.login()
}

export const checkSession: Action = ({ state, effects }) =>
  tryOrNotify(async () => {
    if (state.auth.isLoggedIn) return

    if (AuthService.isAuthenticated()) {
      try {
        state.auth.isLoggingIn = true
        state.auth.user = await effects.auth.getUserInfo()
        state.auth.isLoggedIn = true
      } finally {
        state.auth.isLoggingIn = false
      }
    }
  })

export const handleAuthentication: Action = ({ effects, actions }) => {
  AuthService.handleAuthentication(({ returnUrl, error }) => {
    if (error) {
      console.error(error)
      showError('Noe gikk galt ved innloggingen. Prøv igjen om litt.')
    } else {
      actions.auth.checkSession()
      effects.routing.redirect(returnUrl)
    }
  })
}

export const handleInvitation: Action = ({effects, actions }) => {

}