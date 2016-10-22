import Immutable from 'seamless-immutable'
import * as api from './api'
import { isDevelopment, tryOrNotify } from 'utils'
import $ from 'jquery'
import { normalize } from 'normalizr'
import * as schemas from 'schemas'
import { loadMessages } from 'store/chat'
import { deepMerge } from 'utils/immutableExtensions'

const initialState = Immutable({})

const namespace = 'gaver/sharedList/'

const DATA_LOADED = namespace + 'DATA_LOADED'
const SET_USERS = namespace + 'SET_USERS'
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

export default function reducer(state = initialState, action) {
  switch (action.type) {
    case DATA_LOADED: {
      const wishListId = action.data.result
      return state::deepMerge(action.data.entities)
          .set('owner', action.data.entities.wishLists[wishListId].owner)
          .set('listId', wishListId)
    }
    case SET_BOUGHT_SUCCESS:
      return state.setIn(['wishes', action.wishId, 'boughtByUser'], action.isBought ? action.userId : null)
    case SET_USERS:
      return state::deepMerge(action.data.entities)
        .set('currentUsers', action.data.result)
  }
  return state
}

export const loadSharedList = listId => async dispatch => tryOrNotify(async () => {
  const data = await api.loadSharedList(listId)
  dispatch(dataLoaded(data))
})

export const setBought = ({listId, wishId, isBought}) => async (dispatch, getState) => tryOrNotify(async () => {
  await api.setBought({ listId, wishId, isBought })
  dispatch(setBoughtSuccess({ wishId, isBought, userId: getState().user.id }))
})

export const subscribeList = listId => async dispatch => {
  $.connection.hub.logging = isDevelopment
  const { server, client } = $.connection.listHub
  client.updateUsers = data => dispatch(setUsers(Immutable(normalize(data.currentUsers, schemas.users))))
  client.refresh = () => {
    dispatch(loadSharedList(listId))
    dispatch(loadMessages(listId))
  }
  await $.connection.hub.start()
  const users = await server.subscribe(listId)
  client.updateUsers(users)
}

export const unsubscribeList = listId => async dispatch => {
  const { server } = $.connection.listHub
  await server.unsubscribeList(listId)
  await $.connection.hub.stop()
}

export function setUsers(data) {
  return {
    type: SET_USERS,
    data
  }
}
