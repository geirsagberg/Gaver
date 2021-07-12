import { ChatMessageDto } from '~/types/data'
import { getJson, postJson } from '~/utils/ajax'
import { Context } from '..'

export const toggleChat = async ({
  state: { chat },
  effects: {
    chat: { scrollChat },
  },
}: Context) => {
  chat.visible = !chat.visible
  if (chat.visible) {
    scrollChat()
  }
}

export const showChat = async ({
  state: { chat },
  effects: {
    chat: { scrollChat },
  },
}: Context) => {
  chat.visible = true
  scrollChat()
}

export const hideChat = async ({ state: { chat } }: Context) => {
  chat.visible = false
}

export const addMessage = async ({ state }: Context, text: string) => {
  const wishListId = state.routing.currentSharedListId
  const messageDto = await postJson(`/api/chat/${wishListId}`, { text })
  state.chat.messages.push(messageDto)
}

export const clearMessages = async ({ state }: Context) => {
  state.chat.messages = []
}

export const loadMessages = async ({ state }: Context, listId: number) => {
  const { messages } = await getJson(`/api/chat/${listId}`)
  state.chat.messages = messages
}

export const onMessageAdded = (
  {
    state,
    effects: {
      chat: { scrollChat },
    },
  }: Context,
  chatMessage: ChatMessageDto
) => {
  state.chat.messages.push(chatMessage)
  scrollChat()
}
