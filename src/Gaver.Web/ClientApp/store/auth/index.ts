import produce from 'immer'
import { createAction, ActionsUnion } from '~/utils/reduxUtils'
import { UserModel } from '~/types/data'

export interface AuthState {
  user?: UserModel
  isLoggedIn?: boolean
  isLoggingIn?: boolean
}

enum ActionType {
  LogInSuccessful = 'AUTH:LOG_IN_SUCCESSFUL',
  LogInStarted = 'AUTH:LOG_IN_STARTED',
  LogInCompleted = 'AUTH:LOG_IN_COMPLETED',
  LogOut = 'AUTH:LOG_OUT'
}

export const logInSuccessful = (user: UserModel) => createAction(ActionType.LogInSuccessful, user)
export const logInStarted = () => createAction(ActionType.LogInStarted)
export const logInCompleted = () => createAction(ActionType.LogInCompleted)
export const logOut = () => createAction(ActionType.LogOut)

export const authActionCreators = {
  logInSuccessful,
  logInStarted,
  logInCompleted,
  logOut
}

type Action = ActionsUnion<typeof authActionCreators>

const initialState: AuthState = {}

export const reducer = produce((draft: AuthState, action: Action) => {
  switch (action.type) {
    case ActionType.LogInStarted: {
      draft.isLoggingIn = true
      return
    }
    case ActionType.LogInSuccessful: {
      draft.user = action.payload
      draft.isLoggedIn = true
      draft.isLoggingIn = false
      return
    }
    case ActionType.LogInCompleted: {
      draft.isLoggingIn = false
      return
    }
    case ActionType.LogOut: {
      return initialState
    }
  }
}, initialState)
