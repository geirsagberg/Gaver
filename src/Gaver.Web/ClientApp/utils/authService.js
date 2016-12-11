import Auth0Lock from 'auth0-lock'
import { browserHistory } from 'react-router'
import { isTokenExpired } from './jwtHelper'
import { showError } from 'utils/notifications'
import EventEmitter from 'eventemitter3'

export default class AuthService extends EventEmitter {
  constructor({clientId, domain, redirectUrl}) {
    super()
    this.lock = new Auth0Lock(clientId, domain, {
      auth: {
        redirectUrl,
        responseType: 'token'
      }
    })
    this.lock.on('authenticated', this._doAuthentication.bind(this))
    // binds login functions to keep this context
    this.login = this.login.bind(this)
  }

  _doAuthentication(authResult) {
    this.emit('loadingStarted')
    this.lock.getProfile(authResult.idToken, (error, profile) => {
      this.emit('loadingStopped')
      if (error) {
        showError(error)
      } else {
        localStorage.setItem('id_token', authResult.idToken)
        localStorage.setItem('profile', JSON.stringify(profile))
        const urlAfterLogin = localStorage.getItem('urlAfterLogin') || '/'
        localStorage.removeItem('urlAfterLogin')
        browserHistory.replace(urlAfterLogin)
      }
    })
  }

  login() {
    this.lock.show()
  }

  loggedIn() {
    // Checks if there is a saved token and it's still valid
    const token = AuthService.getToken()
    return !!token && !isTokenExpired(token)
  }

  static getToken() {
    return localStorage.getItem('id_token')
  }

  logout() {
    localStorage.removeItem('id_token')
    localStorage.removeItem('profile')
    browserHistory.replace('/login')
  }

  setUrlAfterLogin(url) {
    localStorage.setItem('urlAfterLogin', url)
  }
}