import { CurrentUserDto } from '~/types/data'

export interface AuthState {
  user?: CurrentUserDto
  isLoggedIn?: boolean
  isLoggingIn?: boolean
}

export const state: AuthState = {}
