import { tryOrNotify } from '~/utils'
import * as api from './api'
import { ChatMessage } from '~/types/data'
import { createAction, ActionsUnion } from '~/utils/reduxUtils'
import { combineReducers } from 'redux'

type ChatState = Dictionary<ChatMessage>

const initialState: ChatState = {}

enum ActionType {
  MessagesLoaded = 'CHAT:MESSAGES_LOADED',
  MessageAdded = 'CHAT:MESSAGE_ADDED'
}

const messagesLoaded = (payload: { entities: { users; messages } }) => createAction(ActionType.MessagesLoaded, payload)
const messageAdded = (payload: { entities: { users; message } }) => createAction(ActionType.MessageAdded, payload)

const actionCreators = {
  messagesLoaded,
  messageAdded
}

type Action = ActionsUnion<typeof actionCreators>

function users(state: ChatState = initialState, action: Action): ChatState {
  switch (action.type) {
    case ActionType.MessagesLoaded:
      return action.entities.users || initialState
    case ActionType.MessageAdded:
      return state.merge(action.data.entities.users, { deep: true })
  }
  return state
}

function messages(state = initialState, action) {
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
