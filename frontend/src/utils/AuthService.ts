import auth0 from 'auth0-js'
import { loadSettings } from './appSettings'

const AccessTokenKey = 'access_token'
const ExpiresAtKey = 'expires_at'

export default class AuthService {
  static #auth?: auth0.WebAuth

  static async getAuth(): Promise<auth0.WebAuth> {
    if (this.#auth) {
      return this.#auth
    }
    const appSettings = await loadSettings()
    if (appSettings) {
      return (this.#auth = new auth0.WebAuth({
        domain: appSettings.domain,
        clientID: appSettings.clientId,
        redirectUri: location.origin + '/callback',
        responseType: 'token',
        scope: 'openid email profile',
        audience: appSettings.audience,
      }))
    }
    throw new Error('Could not load auth settings')
  }

  static async login() {
    const returnUrl = location.pathname.includes('callback')
      ? '/'
      : location.pathname + location.search + location.hash
    const auth = await this.getAuth()
    auth.authorize({ state: returnUrl, prompt: 'none' })
  }

  static async logout() {
    const auth = await this.getAuth()
    localStorage.removeItem(AccessTokenKey)
    localStorage.removeItem(ExpiresAtKey)
    auth.logout({ returnTo: location.origin })
  }

  static async handleAuthentication(
    callback: ({
      returnUrl,
      error,
    }: {
      returnUrl?: string
      error?: auth0.Auth0ParseHashError
    }) => void
  ) {
    const auth = await this.getAuth()
    auth.parseHash((error, hash) => {
      if (hash && hash.accessToken && hash.expiresIn) {
        const { accessToken, expiresIn, state } = hash
        this.saveAccessToken(accessToken, expiresIn)
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
    // TODO: Validate and refresh token
    return localStorage.getItem(AccessTokenKey) || ''
  }

  static isAuthenticated() {
    const epxiresAt = JSON.parse(localStorage.getItem(ExpiresAtKey) || '0')
    return new Date().getTime() < epxiresAt
  }
}
