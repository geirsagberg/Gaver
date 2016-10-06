import Immutable from 'seamless-immutable'
import * as api from './api'
import {showError} from 'utils/notifications'

const initialState = Immutable({})

const namespace = 'gaver/sharedList/'
const DATA_LOADED = namespace + 'DATA_LOADED'

function dataLoaded(data) {
  return {
    type: DATA_LOADED,
    data
  }
}

export default function reducer (state = initialState, action) {
  switch (action.type) {
    case DATA_LOADED:
      state = state.merge(action.data.entities)
      var wishListId = action.data.result
      state = state.set('owner', action.data.entities.wishLists[wishListId].owner)
      return state
  }
  return state
}

export const loadSharedList = listId => async dispatch => {
  try {
    const data = await api.loadSharedList(listId)
    dispatch(dataLoaded(data))
  } catch (error) {
    showError(error)
  }
}