import 'isomorphic-fetch'

export function getJson (url) {
  return fetch(url).then(response => response.json())
}

export function postJson (url, data) {
  return fetch(url, {
    method: 'POST',
    headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(data)
  }).then(response => {
    var contentType = response.headers.get('content-type')
    if (contentType && contentType.indexOf('application/json') !== -1) {
      return response.json()
    } else {
      return Promise.resolve()
    }
  })
}
