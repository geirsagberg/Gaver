import produce from 'immer'

enum ActionType {}

export interface User {
  id: number
  name: string
}

export type UserState = {
  isLoggedIn?: boolean
  isLoggingIn?: boolean
  user?: User
}

const initialState: UserState = {}

export const reducer = produce((draft: UserState = initialState, action) => {
  switch (action.type) {
    case LOG_IN_SUCCESSFUL:
      draft.user = action.data
      draft.isLoggedIn = true
      draft.isLoggingIn = false
      return
    case LOGGED_OUT:
      return initialState
    case AUTH_STARTED:
      draft.isLoggingIn = true
      return
  }
  return
})

export function loggedOut() {
  return {
    type: LOGGED_OUT
  }
}

export function logInSuccessful(data) {
  return {
    type: LOG_IN_SUCCESSFUL,
    data
  }
}

export function authStarted() {
  return {
    type: AUTH_STARTED
  }
}

export * from './thunks'
