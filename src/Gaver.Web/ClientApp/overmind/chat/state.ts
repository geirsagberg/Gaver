import { ChatMessageDto } from '~/types/data'

export interface ChatState {
  messages: ChatMessageDto[]
  visible: boolean
}

export const state: ChatState = {
  messages: [],
  visible: false,
}
