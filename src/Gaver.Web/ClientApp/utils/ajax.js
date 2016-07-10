import 'isomorphic-fetch'
import { fromJS } from 'immutable'

const headers = {
  'Accept': 'application/json',
  'Content-Type': 'application/json'
}

const handleResponse = response => {
  var contentType = response.headers.get('content-type')
  if (contentType && contentType.indexOf('application/json') !== -1) {
    return response.json().then(fromJS)
  } else {
    return Promise.resolve()
  }
}

export function getJson (url) {
  return fetch(url).then(handleResponse)
}

export function postJson (url, data) {
  return fetch(url, {
    method: 'POST',
    headers,
    body: JSON.stringify(data)
  }).then(handleResponse)
}

export function deleteJson (url, data) {
  return fetch(url, {
    method: 'DELETE',
    headers,
    body: JSON.stringify(data)
  }).then(handleResponse)
}
