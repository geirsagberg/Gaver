import { postJson, getJson } from 'utils/ajax'

export function logIn (name) {
  return postJson('/api/User/Login', { name })
}

export function logOut () {
  return postJson('/api/User/Logout')
}

export function loadUserInfo (accessToken) {
  return getJson('/api/User?accessToken=' + accessToken)
}