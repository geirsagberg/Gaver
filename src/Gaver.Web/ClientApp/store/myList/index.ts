import * as actions from './actions'
import { combineReducers } from 'redux'
import { merge, without, omit } from 'lodash-es'
import { Wish, Invitation, MyListModel } from '~/types/data'
import produce from 'immer'
import { createAction } from '~/utils/reduxUtils'

enum ActionType {
  MyListLoaded = 'MY_LIST:DATA_LOADED',
  WishAdded = 'MY_LIST:WISH_ADDED',
  WishDeleted = 'MY_LIST:WISH_DELETED',
  WishUpdated = 'MY_LIST:WISH_UPDATED'
}

export interface MyListState {
  wishes: Dictionary<Wish>
  invitations: Dictionary<Invitation>
  listId?: number
}

const wishes = produce((draft: Dictionary<Wish> = {}, action) => {
  if (!action) return
  switch (action.type) {
    case ActionType.WishAdded:
      return merge(draft, action.data.entities.wishes, { deep: true })
    case ActionType.MyListLoaded:
      return action.data.entities.wishes || {}
    case ActionType.WishDeleted:
      delete draft[action.id]
      return
    case ActionType.WishUpdated:
      return merge(draft, action.data.entities.wishes, { deep: true })
  }
  return draft
})

function invitations(state: Dictionary<Invitation> = {}, action): Dictionary<Invitation> {
  if (!action) return state
  switch (action.type) {
    case ActionType.MyListLoaded:
      return action.data.entities.invitations || {}
  }
  return state
}

const initialState: MyListState = {
  wishes: wishes(undefined, undefined),
  invitations: invitations(undefined, undefined)
}

const combinedReducer = combineReducers({
  wishes,
  invitations
})

const reducer = produce((state = initialState, action) => {
  state = { ...state, ...combinedReducer(state, action) }
  switch (action.type) {
    case ActionType.MyListLoaded:
      state.listId = action.data.result
      return
  }
  return state
})

export default reducer

export const wishUpdated = (wish: Wish) => createAction(ActionType.WishUpdated, wish)

export const wishAdded = (wish: Wish) => createAction(ActionType.WishAdded, wish)

export const myListLoaded = (data: MyListModel) => createAction(ActionType.MyListLoaded, data)

export const wishDeleted = (id: number) => createAction(ActionType.WishDeleted, id)
