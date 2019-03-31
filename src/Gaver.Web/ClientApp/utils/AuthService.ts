import auth0 from 'auth0-js'
import AppSettings from '~/utils/appSettings'

const AccessTokenKey = 'access_token'
const ExpiresAtKey = 'expires_at'

const auth = new auth0.WebAuth({
  domain: AppSettings.domain,
  clientID: AppSettings.clientId,
  redirectUri: location.origin + '/callback',
  responseType: 'token',
  scope: 'openid email profile',
  audience: AppSettings.audience
})

export default class AuthService {
  static login() {
    const returnUrl = location.pathname + location.search + location.hash
    auth.authorize({ state: returnUrl, prompt: 'none' })
  }

  static logout() {
    localStorage.removeItem(AccessTokenKey)
    localStorage.removeItem(ExpiresAtKey)
  }

  static handleAuthentication(
    callback: ({ returnUrl, error }: { returnUrl?: string; error?: auth0.Auth0ParseHashError }) => void
  ) {
    auth.parseHash((error, { accessToken, expiresIn, state } = {}) => {
      if (accessToken && expiresIn) {
        const expiresAt = JSON.stringify(expiresIn * 1000 + new Date().getTime())
        localStorage.setItem(AccessTokenKey, accessToken)
        localStorage.setItem(ExpiresAtKey, expiresAt)
        const returnUrl = state || '/'
        callback({ returnUrl })
      } else if (error) {
        if (error.error === 'login_required') {
          auth.authorize({ state: error.state })
        } else {
          callback({ error })
        }
      }
    })
  }

  static saveAccessToken(accessToken: string, expiresIn: number) {
    const expiresAt = JSON.stringify(expiresIn * 1000 + new Date().getTime())
    localStorage.setItem(AccessTokenKey, accessToken)
    localStorage.setItem(ExpiresAtKey, expiresAt)
  }

  static loadAccessToken() {
    return localStorage.getItem(AccessTokenKey)
  }

  static isAuthenticated() {
    const epxiresAt = JSON.parse(localStorage.getItem(ExpiresAtKey))
    return new Date().getTime() < epxiresAt
  }
}
