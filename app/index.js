import Lock from 'auth0-lock-passwordless'

var lock = new Lock('q57tZFsUo6359RyFzmzB0VYrmCeLVrBi', 'sagberg.eu.auth0.com')

lock.magiclink({
  callbackURL: 'http://localhost:3000/',
  authParams: {
    scope: 'openid email'
  }
})