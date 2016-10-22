import Immutable from 'seamless-immutable'
import * as Api from './api'
import { showError } from 'utils/notifications'
import Cookies from 'js-cookie'
import { browserHistory } from 'react-router'
import $ from 'jquery'

const namespace = 'gaver/user/'

const LOGGED_OUT = namespace + 'LOGGED_OUT'
const LOG_IN_SUCCESSFUL = namespace + 'LOG_IN_SUCCESSFUL'

const initialState = Immutable({})

export default function reducer (state = initialState, action = {}) {
  switch (action.type) {
    case LOG_IN_SUCCESSFUL:
      return action.user.merge({isLoggedIn: true})
    case LOGGED_OUT:
      return initialState
  }
  return state
}

function loggedOut () {
  return {
    type: LOGGED_OUT
  }
}

export const logOut = () => async dispatch => {
  Cookies.remove('user')
  await Api.logOut()
  dispatch(loggedOut())
  browserHistory.push('/login')
  return {
    type: LOGGED_OUT
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
    Cookies.set('user', user.name)
    redirect()
  } catch (error) {
    showError(error)
  }
}
