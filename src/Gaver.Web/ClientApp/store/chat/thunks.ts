import { tryOrNotify } from '~/utils'

export const addMessage = ({ listId, text }) => async dispatch =>
  tryOrNotify(async () => {
    const data = await api.addMessage({ listId, text })
    dispatch(messageAdded(data))
  })

export const loadMessages = listId => async dispatch =>
  tryOrNotify(async () => {
    const data = await api.loadMessages(listId)
    dispatch(messagesLoaded(data))
  })
