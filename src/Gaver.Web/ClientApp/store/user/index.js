import Immutable from 'seamless-immutable'
import * as Api from './api'
import { showError } from 'utils/notifications'

const namespace = 'gaver/user/'

const LOG_OUT = namespace + 'LOG_OUT'
const LOG_IN_SUCCESSFUL = namespace + 'LOG_IN_SUCCESSFUL'

export function reducer (state = Immutable({}), action) {
  switch (action.type) {
    case LOG_IN_SUCCESSFUL:
      return action.user.merge({isLoggedIn: true})
  }
  return state
}

export function logOut () {
  return {
    type: LOG_OUT
  }
}

function logInSuccessful (user) {
  return {
    type: LOG_IN_SUCCESSFUL,
    user
  }
}

export const logIn = (name, redirect) => async dispatch => {
  try {
    var user = await Api.logIn(name)
    dispatch(logInSuccessful(user))
    redirect()
  } catch (error) {
    showError(error)
  }
}
