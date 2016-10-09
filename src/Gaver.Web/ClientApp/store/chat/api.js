import { getJson, postJson } from 'utils/ajax'
import * as schemas from 'schemas'

export function addMessage ({listId, text}) {
  return postJson(`/api/Chat/${listId}`, { text }, schemas.message)
}

export function loadMessages (listId) {
  return getJson(`/api/Chat/${listId}`, schemas.chat)
}