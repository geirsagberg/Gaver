import Lock from 'auth0-lock-passwordless'

var lock = new Lock('q57tZFsUo6359RyFzmzB0VYrmCeLVrBi', 'sagberg.eu.auth0.com')

export function initialize () {
  var hash = lock.parseHash(window.location.hash)

  if (hash && hash.error) {
    window.alert('There was an error: ' + hash.error + '\n' + hash.error_description)
  } else if (hash && hash.id_token) {
    // use id_token for retrieving profile.
    window.localStorage.setItem('id_token', hash.id_token)
    // retrieve profile
    lock.getProfile(hash.id_token, function (err, profile) {
      if (err) {
        // handle err
        window.console.log(err)
      } else {
        // use user profile
        window.console.log(profile)
      }
    })
  }
}

export function showLogin () {
  lock.magiclink()
}
