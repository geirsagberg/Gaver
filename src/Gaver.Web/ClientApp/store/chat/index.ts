import produce from 'immer';
import { combineReducers } from 'redux';
import { ChatMessage } from '~/types/data';
import { ActionsUnion, createAction } from '~/utils/reduxUtils';

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

const users = produce(((draft: , action) => {
  switch (action.type) {
    case ActionType.MessagesLoaded:

      return action.entities.users || initialState
    case ActionType.MessageAdded:

      return state.merge(action.data.entities.users, { deep: true })
  }
  return state
}))

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
