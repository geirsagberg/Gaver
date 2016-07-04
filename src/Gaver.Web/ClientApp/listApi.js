import { getJson, postJson } from './ajax'

export function fetchWishData () {
  return getJson('/api/Wish')
}

export function addWish (wish) {
  return postJson('/api/Wish', wish)
}