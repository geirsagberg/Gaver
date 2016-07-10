import { getJson, postJson, deleteJson } from 'utils/ajax'

export function fetchWishData () {
  return getJson('/api/Wish')
}

export function addWish (wish) {
  return postJson('/api/Wish', wish)
}

export function deleteWish (id) {
  return deleteJson('/api/Wish/' + id)
}