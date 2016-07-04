const actionNamespace = 'gaver/currentList/'

export const ADD_WISH = actionNamespace + 'ADD_WISH'
export const LOAD_DATA = actionNamespace + 'LOAD_DATA'
export const DATA_LOADED = actionNamespace + 'DATA_LOADED'
export const WISH_ADDED = actionNamespace + 'WISH_ADDED'

export default (state = [], action) => {
  switch (action.type) {
    case WISH_ADDED:
      return [
        ...state,
        action.wish
      ]
  }
  return state
}

export function addWish (wish) {
  return {
    type: ADD_WISH,
    wish
  }
}

export function wishAdded (wish) {
  return {
    type: WISH_ADDED,
    wish
  }
}

export function loadData () {
  return {
    type: LOAD_DATA
  }
}

export function fetchDataSuccess (data) {
  return {
    type: DATA_LOADED,
    data
  }
}