import Immutable from 'seamless-immutable'

const namespace = 'gaver/ui/'

const LOADING_STARTED = namespace + 'LOADING_STARTED'
const LOADING_STOPPED = namespace + 'LOADING_STOPPED'
const TOGGLE_SHARED_LISTS = namespace + 'TOGGLE_SHARED_LISTS'
const SET_SHARED_LISTS_VISIBLE = namespace + 'SET_SHARED_LISTS_VISIBLE'

const initialState = Immutable({})

export default function reducer (state = initialState, action = {}) {
  switch (action.type) {
    case LOADING_STARTED:
      return state.update('isLoading', x => (x || 0) + 1)
    case LOADING_STOPPED:
      return state.update('isLoading', x => (x - 1) || 0)
    case TOGGLE_SHARED_LISTS:
      return state.update('isShowingSharedLists', x => !x)
    case SET_SHARED_LISTS_VISIBLE:
      return state.set('isShowingSharedLists', action.visible)
  }
  return state
}

export function loadingStarted () {
  return {
    type: LOADING_STARTED
  }
}

export function loadingStopped () {
  return {
    type: LOADING_STOPPED
  }
}

export function toggleSharedLists() {
  return {
    type: TOGGLE_SHARED_LISTS
  }
}

export function setSharedListsVisible(visible) {
  return {
    type: SET_SHARED_LISTS_VISIBLE,
    visible
  }
}