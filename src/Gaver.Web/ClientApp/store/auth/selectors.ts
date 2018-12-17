import { AppStateWithRouting } from '..'

export const selectAuth = (state: AppStateWithRouting) => state.auth

export const selectIsLoggedIn = (state: AppStateWithRouting) => state.auth.isLoggedIn
export const selectCurrentUser = (state: AppStateWithRouting) => state.auth.user
