import * as actions from './actions'

export function reducer (state = [], action) {
  switch (action.type) {
    case actions.WISH_ADDED:
      return [
        ...state,
        action.wish
      ]
    case actions.DATA_LOADED:
      return action.data
  }
  return state
}

export { default as saga } from './saga'

export * from './actions'