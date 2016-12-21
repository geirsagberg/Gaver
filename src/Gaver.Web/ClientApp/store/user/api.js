import { postJson, getJson } from 'utils/ajax'
import * as schemas from 'schemas'

export function logIn (name) {
  return postJson('/api/User/Login', { name })
}

export function logOut () {
  return postJson('/api/User/Logout')
}

export function loadUserInfo () {
  return getJson('/api/User')
}