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
const SET_BOUGHT_SUCCESS = namespace + 'SET_BOUGHT_SUCCESS'

function dataLoaded(data) {
  return {
    type: DATA_LOADED,
    data
  }
}

function setBoughtSuccess({wishId, isBought, userId}) {
  return {
    type: SET_BOUGHT_SUCCESS,
    wishId,
    isBought,
    userId
  }
}

export default function reducer (state = initialState, action) {
  switch (action.type) {
    case DATA_LOADED:
      state = state.merge(action.data.entities)
      var wishListId = action.data.result
      state = state.set('owner', action.data.entities.wishLists[wishListId].owner)
      return state
    case SET_BOUGHT_SUCCESS:
      return state.setIn(['wishes', action.wishId, 'boughtByUser'], action.isBought ? action.userId : null)
  }
  return state
}

export const loadSharedList = listId => async dispatch => tryOrNotify(async () => {
  const data = await api.loadSharedList(listId)
  dispatch(dataLoaded(data))
})

export const setBought = ({listId, wishId, isBought}) => async (dispatch, getState) => tryOrNotify(async () => {
  await api.setBought({listId, wishId, isBought})
  dispatch(setBoughtSuccess({wishId, isBought, userId: getState().user.id}))
})

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
