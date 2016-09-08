import Immutable from 'seamless-immutable'
import * as Api from './api'
import showError from 'utils/dialogs'

const namespace = 'gaver/user/'

const LOG_OUT = namespace + 'LOG_OUT'
const LOG_IN = namespace + 'LOG_IN'

export function reducer (state = Immutable({}), action) {
  return state
}

export function logOut () {
  return {
    type: LOG_OUT
  }
}

export const logIn = name => async dispatch => {
  try {
    await Api.logIn(name)
  } catch (error) {
    showError(error)
  }
}
