import { isTokenExpired } from './jwtHelper'

const URL_AFTER_LOGIN = 'urlAfterLogin'
const ID_TOKEN = 'id_token'

export const loadToken = () => {
  const token = localStorage.getItem(ID_TOKEN)
  return !!token && !isTokenExpired(token)
    ? token
    : null
}

export const saveToken = token => localStorage.setItem(ID_TOKEN, token)

export const clearToken = () => localStorage.removeItem(ID_TOKEN)

export const saveUrlAfterLogin = url => localStorage.setItem(URL_AFTER_LOGIN, url)

export const loadUrlAfterLogin = () => localStorage.getItem(URL_AFTER_LOGIN)

export const clearUrlAfterLogin = () => localStorage.removeItem(URL_AFTER_LOGIN)