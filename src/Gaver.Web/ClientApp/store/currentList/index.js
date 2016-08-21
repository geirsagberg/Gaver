import * as actions from './actions'
import Immutable from 'seamless-immutable'
import { combineReducers } from 'redux'

function wishes (state = Immutable({}), action) {
  switch (action.type) {
    case actions.WISH_ADDED:
      return state.set(action.wish.id, action.wish)
    case actions.DATA_LOADED:
      return state.merge(action.data.entities.wishes || {})
    case actions.WISH_DELETED:
      return state.without(action.id)
  }
  return state
}

function users (state = Immutable({}), action) {
  switch (action.type) {
    case actions.SET_USERS:
      return state.merge(action.users)
  }
  return state
}

export const reducer = combineReducers({
  wishes,
  users
})

export * from './actions'