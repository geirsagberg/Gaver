import { getJson, postJson, putJson } from 'utils/ajax'
import * as schemas from 'schemas'

export function loadSharedList (listId) {
  return getJson('/api/WishList/' + listId, schemas.wishList)
}

export function setBought ({listId, wishId, isBought}) {
  return putJson(`/api/WishList/${listId}/${wishId}/SetBought`, {isBought})
}

export function registerToken (listId, token) {
  return postJson(`/api/WishList/${listId}/RegisterToken`, {token})
}