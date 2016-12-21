import Immutable from 'seamless-immutable'

const namespace = 'gaver/ui/'

const LOADING_STARTED = namespace + 'LOADING_STARTED'
const LOADING_STOPPED = namespace + 'LOADING_STOPPED'

const initialState = Immutable({})

export default function reducer (state = initialState, action = {}) {
  switch (action.type) {
    case LOADING_STARTED:
      return state.update('isLoading', x => (x || 0) + 1)
    case LOADING_STOPPED:
      return state.update('isLoading', x => (x - 1) || 0)
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