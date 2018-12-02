import { combineReducers } from 'redux'
import * as users from './users'
import * as wishLists from './wishLists'

export interface DataState {
  users: users.UserData
  wishLists: wishLists.WishListData
}

export const reducer = combineReducers<DataState>({
  users: users.reducer,
  wishLists: wishLists.reducer
})
