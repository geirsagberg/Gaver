import * as actions from './actions'
import Immutable from 'seamless-immutable'
import { combineReducers } from 'redux-seamless-immutable'

const initialState = Immutable({})

function wishes (state = initialState, action) {
  switch (action.type) {
    case actions.WISH_ADDED:
      return state.merge(action.data.entities.wishes, { deep: true })
    case actions.DATA_LOADED:
      return action.data.entities.wishes || initialState
    case actions.WISH_DELETED:
      return state.without(action.id)
    case actions.WISH_UPDATED:
      return state.merge(action.data.entities.wishes, { deep: true })
  }
  return state
}

function invitations (state = initialState, action) {
  switch (action.type) {
    case actions.DATA_LOADED:
      return action.data.entities.invitations || initialState
  }
  return state
}

const combinedReducer = combineReducers({
  wishes,
  invitations
})

function reducer (state = initialState, action) {
  state = combinedReducer(state, action)
  switch (action.type) {
    case actions.DATA_LOADED:
      return state.set('listId', action.data.result)
  }
  return state
}

export default reducer

export * from './actions'
