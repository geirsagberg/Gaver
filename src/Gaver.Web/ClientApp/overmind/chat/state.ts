import { ChatMessageModel } from '~/types/data'

export interface ChatState {
  messages: ChatMessageModel[]
  visible: boolean
}

export const state: ChatState = {
  messages: [],
  visible: false
}
