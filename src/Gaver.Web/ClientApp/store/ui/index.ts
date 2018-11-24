import produce from 'immer'

const namespace = 'gaver/ui/'

const LOADING_STARTED = namespace + 'LOADING_STARTED'
const LOADING_STOPPED = namespace + 'LOADING_STOPPED'
const TOGGLE_SHARED_LISTS = namespace + 'TOGGLE_SHARED_LISTS'
const SET_SHARED_LISTS_VISIBLE = namespace + 'SET_SHARED_LISTS_VISIBLE'

interface UIState {
  isShowingSharedLists?: boolean
}

const initialState = {}

const reducer = produce((draft: UIState = initialState, action) => {
  switch (action.type) {
    case TOGGLE_SHARED_LISTS:
      draft.isShowingSharedLists = !draft.isShowingSharedLists
      return
    case SET_SHARED_LISTS_VISIBLE:
      draft.isShowingSharedLists = action.visible
      return
  }
  return draft
})

export default reducer

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
