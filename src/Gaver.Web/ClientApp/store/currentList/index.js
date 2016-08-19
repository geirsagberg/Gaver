import * as actions from './actions'
import Immutable from 'seamless-immutable'
import { combineReducers } from 'redux'

function wishReducer (state = Immutable({}), action) {
  switch (action.type) {
    case actions.WISH_ADDED:
      return state.set(action.wish.id, action.wish)
    case actions.DATA_LOADED:
      return state.merge(action.data.entities.wishes || {})
    case actions.WISH_DELETED:
      return state.without(action.id)
    case actions.SET_COUNT:
      return state.merge(action.count)
  }
  return state
}

export const reducer = combineReducers({
  wishes: wishReducer
})

export { default as saga } from './saga'

export * from './actions'