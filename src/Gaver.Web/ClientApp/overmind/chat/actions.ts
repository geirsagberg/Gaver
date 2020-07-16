import { getJson, postJson } from '~/utils/ajax'
import { Action } from '..'
import { ChatMessageDto } from '~/types/data'

export const toggleChat: Action = async ({
  state: { chat },
  effects: {
    chat: { scrollChat },
  },
}) => {
  chat.visible = !chat.visible
  if (chat.visible) {
    scrollChat()
  }
}

export const showChat: Action = async ({
  state: { chat },
  effects: {
    chat: { scrollChat },
  },
}) => {
  chat.visible = true
  scrollChat()
}

export const hideChat: Action = async ({ state: { chat } }) => {
  chat.visible = false
}

export const addMessage: Action<string> = async ({ state }, text) => {
  const wishListId = state.routing.currentSharedListId
  const messageDto = await postJson(`/api/chat/${wishListId}`, { text })
  state.chat.messages.push(messageDto)
}

export const clearMessages: Action = async ({ state }) => {
  state.chat.messages = []
}

export const loadMessages: Action<number> = async ({ state }, listId) => {
  const { messages } = await getJson(`/api/chat/${listId}`)
  state.chat.messages = messages
}

export const onMessageAdded: Action<ChatMessageDto> = (
  {
    state,
    effects: {
      chat: { scrollChat },
    },
  },
  chatMessage
) => {
  state.chat.messages.push(chatMessage)
  scrollChat()
}
