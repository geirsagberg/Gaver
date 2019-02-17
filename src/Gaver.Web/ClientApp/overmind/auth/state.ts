import { UserModel } from '~/types/data'

export interface AuthState {
  user?: UserModel
  isLoggedIn?: boolean
  isLoggingIn?: boolean
}

export const state: AuthState = {}
