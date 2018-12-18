import { AppStateWithRouting } from '.'

export const selectMyList = (state: AppStateWithRouting) => {
  const user = selectCurrentUser(state)
  return user ? state.data.wishLists[user.id] : null
}

export const selectAuth = (state: AppStateWithRouting) => state.auth

export const selectIsLoggedIn = (state: AppStateWithRouting) => state.auth.isLoggedIn
export const selectCurrentUser = (state: AppStateWithRouting) => state.auth.user
