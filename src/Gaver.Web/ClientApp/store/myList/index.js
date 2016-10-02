import * as actions from './actions'
import Immutable from 'seamless-immutable'
import { combineReducers } from 'redux'

const initialState = Immutable({})

function wishes (state = initialState, action) {
  switch (action.type) {
    case actions.WISH_ADDED:
      return state.merge(action.data.entities.wishes)
    case actions.DATA_LOADED:
      return action.data.entities.wishes || initialState
    case actions.WISH_DELETED:
      return state.without(action.id)
  }
  return state
}

const reducer = combineReducers({
  wishes
})

export default reducer

export * from './actions'