import { isTokenExpired } from './jwtHelper'

const URL_AFTER_LOGIN = 'urlAfterLogin'
const ID_TOKEN = 'id_token'
const ACCESS_TOKEN = 'access_token'

export const loadIdToken = () => {
  const token = localStorage.getItem(ID_TOKEN)
  return !!token && !isTokenExpired(token) ? token : null
}

export const loadAccessToken = () => localStorage.getItem(ACCESS_TOKEN)
export const saveIdToken = (token) => localStorage.setItem(ID_TOKEN, token)
export const saveAccessToken = (token) => localStorage.setItem(ACCESS_TOKEN, token)

export const clearTokens = () => {
  localStorage.removeItem(ID_TOKEN)
  localStorage.removeItem(ACCESS_TOKEN)
}

export const saveUrlAfterLogin = (url) => localStorage.setItem(URL_AFTER_LOGIN, url)
export const loadUrlAfterLogin = () => localStorage.getItem(URL_AFTER_LOGIN)
export const clearUrlAfterLogin = () => localStorage.removeItem(URL_AFTER_LOGIN)
