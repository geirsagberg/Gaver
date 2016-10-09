import Immutable from 'seamless-immutable'
import * as api from './api'
import { isDevelopment, tryOrNotify } from 'utils'
import { compose } from 'redux'
import $ from 'jquery'
import { normalize } from 'normalizr'
import * as schemas from 'schemas'

const initialState = Immutable({})

const namespace = 'gaver/sharedList/'
const DATA_LOADED = namespace + 'DATA_LOADED'
const SET_USERS = namespace + 'SET_COUNT'

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
  tryOrNotify(async () => {
    const data = await api.loadSharedList(listId)
    dispatch(dataLoaded(data))
  })
}

export const setBought = ({listId, wishId}) => async dispatch => {

}

const createCaller = dispatch =>
  (action, schema) =>
      compose(dispatch, action, Immutable, data => schema ? normalize(data, schema) : data)

export const initializeListUpdates = listId => async dispatch => {
  $.connection.hub.logging = isDevelopment
  const call = createCaller(dispatch)
  const { client } = $.connection.listHub
  client.updateUsers = call(setUsers)
  client.refresh = call(() => loadSharedList(listId), schemas.wishes)
}

export function setUsers(users) {
  return {
    type: SET_USERS,
    users
  }
}
