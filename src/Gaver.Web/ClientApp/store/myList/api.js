import { getJson, postJson, deleteJson } from 'utils/ajax'
import * as schemas from 'schemas'

export function fetchWishData () {
  return getJson('/api/WishList', schemas.wishList)
}

export function addWish (title) {
  return postJson('/api/Wish', {
    title
  }, schemas.wish)
}

export function deleteWish (id) {
  return deleteJson('/api/Wish/' + id)
}

export function shareList ({listId, emails}) {
  return postJson(`/api/WishList/{listId}/Share`, {emails})
}