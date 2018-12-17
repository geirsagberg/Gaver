import { replace } from 'redux-first-router'
import { routeActions } from '~/routing'
import { UserModel } from '~/types/data'
import { tryOrNotify } from '~/utils'
import { getJson } from '~/utils/ajax'
import AuthService from '~/utils/AuthService'
import { showError } from '~/utils/notifications'
import { logInCompleted, logInStarted, logInSuccessful } from '.'
import { GaverThunk } from '..'

export const login = (): GaverThunk => () => AuthService.login()

export const logout = (): GaverThunk => dispatch => {
  AuthService.logout()
  dispatch(routeActions.showFrontPage())
}
export const handleAuthentication = (): GaverThunk => () => {
  AuthService.handleAuthentication(({ returnUrl, error }) => {
    if (error) {
      console.error(error)
      showError('Noe gikk galt ved innloggingen. Prøv igjen om litt.')
    } else {
      replace(returnUrl)
    }
  })
}
export const checkSession = () => (dispatch, getState) =>
  tryOrNotify(
    async () => {
      dispatch(logInStarted())
      if (AuthService.isAuthenticated()) {
        const userInfo = await getJson<UserModel>('/api/user')
        dispatch(logInSuccessful(userInfo))
      }
    },
    () => {
      dispatch(logInCompleted())
    }
  )
