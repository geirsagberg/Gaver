import * as actions from './actions'
import Immutable from 'seamless-immutable'
import { deepMerge } from 'utils/immutableExtensions'

const initialState = Immutable({})

function reducer (state = initialState, action) {
  switch (action.type) {
    case actions.WISH_ADDED:
      state = state::deepMerge(action.data.entities)
      return state
    case actions.DATA_LOADED:
      state = state.merge(action.data.entities)
      state = state.set('listId', action.data.result)
      return state
    case actions.WISH_DELETED:
      return state.update('wishes', wishes => wishes.without(action.id))
  }
  return state
}

export default reducer

export * from './actions'