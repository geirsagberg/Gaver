import { getJson, postJson, deleteJson } from 'utils/ajax'
import * as schemas from 'schemas'

export function fetchWishData () {
  return getJson('/api/Wish', schemas.wishes)
}

export function addWish (wish) {
  return postJson('/api/Wish', wish, schemas.wish)
}

export function deleteWish (id) {
  return deleteJson('/api/Wish/' + id)
}

export function shareList ({emails}) {
  return postJson('/api/Wish/Share', {emails})
}