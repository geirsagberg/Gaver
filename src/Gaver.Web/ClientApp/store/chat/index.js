import Immutable from 'seamless-immutable'
import { combineReducers } from 'redux-seamless-immutable'
import { tryOrNotify } from 'utils'
import * as api from './api'

const initialState = Immutable({})

const namespace = 'gaver/chat/'

const MESSAGES_LOADED = namespace + 'MESSAGES_LOADED'
const MESSAGE_ADDED = namespace + 'MESSAGE_ADDED'

function users (state = initialState, action) {
  switch (action.type) {
    case MESSAGES_LOADED:
      return action.data.entities.users || initialState
    case MESSAGE_ADDED:
      return state.merge(action.data.entities.users, { deep: true })
  }
  return state
}

function messages (state = initialState, action) {
  switch (action.type) {
    case MESSAGES_LOADED:
      return action.data.entities.messages || initialState
    case MESSAGE_ADDED:
      return state.merge(action.data.entities.messages, { deep: true })
  }
  return state
}

export default combineReducers({
  messages,
  users
})

function messagesLoaded (data) {
  return {
    type: MESSAGES_LOADED,
    data
  }
}

function messageAdded (data) {
  return {
    type: MESSAGE_ADDED,
    data
  }
}

export const addMessage = ({ listId, text }) => async (dispatch) =>
  tryOrNotify(async () => {
    const data = await api.addMessage({ listId, text })
    dispatch(messageAdded(data))
  })

export const loadMessages = (listId) => async (dispatch) =>
  tryOrNotify(async () => {
    const data = await api.loadMessages(listId)
    dispatch(messagesLoaded(data))
  })
