import * as actions from './actions'
import Immutable from 'seamless-immutable'
import { combineReducers } from 'redux'

const initialState = Immutable({})

function reducer (state = initialState, action) {
  switch (action.type) {
    case actions.WISH_ADDED:
      state = state.merge(action.data.entities)
      return state
    case actions.DATA_LOADED:
      state = state.merge(action.data.entities)
      state = state.set('listId', action.data.result)
      return state
    case actions.WISH_DELETED:
      return state.without(action.id)
  }
  return state
}

export default reducer

export * from './actions'