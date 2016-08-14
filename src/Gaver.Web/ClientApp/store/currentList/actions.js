const actionNamespace = 'gaver/currentList/'

export const ADD_WISH = actionNamespace + 'ADD_WISH'
export const LOAD_DATA = actionNamespace + 'LOAD_DATA'
export const DATA_LOADED = actionNamespace + 'DATA_LOADED'
export const WISH_ADDED = actionNamespace + 'WISH_ADDED'
export const DELETE_WISH = actionNamespace + 'DELETE_WISH'
export const WISH_DELETED = actionNamespace + 'WISH_DELETED'
export const FETCH_FAILED = actionNamespace + 'FETCH_FAILED'
export const SHARE_LIST = actionNamespace + 'SHARE_LIST'
export const INITIALIZE_LIST_UPDATES = actionNamespace + 'INITIALIZE_LIST_UPDATES'

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

export function deleteWish (id) {
  return {
    type: DELETE_WISH,
    id
  }
}

export function wishDeleted (id) {
  return {
    type: WISH_DELETED,
    id
  }
}

export function fetchFailed (error) {
  return {
    type: FETCH_FAILED,
    error: error.toString()
  }
}

export function shareList () {
  return {
    type: SHARE_LIST
  }
}

export function initializeListUpdates () {
  return {
    type: INITIALIZE_LIST_UPDATES
  }
}