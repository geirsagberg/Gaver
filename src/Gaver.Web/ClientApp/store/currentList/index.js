import * as actions from './actions'
import { List } from 'immutable'

export function reducer (state = List(), action) {
  switch (action.type) {
    case actions.WISH_ADDED:
      return state.push(action.wish)
    case actions.DATA_LOADED:
      return action.data
    case actions.WISH_DELETED:
      return state
  }
  return state
}

export { default as saga } from './saga'

export * from './actions'