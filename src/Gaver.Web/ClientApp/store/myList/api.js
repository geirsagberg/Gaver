import { getJson, postJson, deleteJson, putJson } from 'utils/ajax'
import * as schemas from 'schemas'

export function fetchWishData () {
  return getJson('/api/WishList', schemas.wishList)
}

export function addWish ({listId, title}) {
  return postJson('/api/WishList/' + listId, {
    title
  }, schemas.wish)
}

export function deleteWish ({listId, wishId}) {
  return deleteJson(`/api/WishList/${listId}/${wishId}`)
}

export function shareList ({listId, emails}) {
  return postJson(`/api/WishList/${listId}/Share`, {emails})
}

export function setUrl ({listId, wishId, url}) {
  return putJson(`/api/WishList/${listId}/${wishId}/SetUrl`, {url}, schemas.wish)
}