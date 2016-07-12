import 'isomorphic-fetch'
import Immutable from 'seamless-immutable'
import { normalize, Schema } from 'normalizr'

const headers = {
  'Accept': 'application/json',
  'Content-Type': 'application/json'
}

const handleResponse = schema => response => {
  var contentType = response.headers.get('content-type')
  if (contentType && contentType.indexOf('application/json') !== -1) {
    return response.json().then(json => Immutable(normalize(json, schema)))
  } else {
    return Promise.resolve()
  }
}

export function getJson (url, schema = new Schema('item')) {
  return fetch(url).then(handleResponse(schema))
}

export function postJson (url, data, schema = new Schema('item')) {
  return fetch(url, {
    method: 'POST',
    headers,
    body: JSON.stringify(data)
  }).then(handleResponse(schema))
}

export function deleteJson (url, data, schema = new Schema('item')) {
  return fetch(url, {
    method: 'DELETE',
    headers,
    body: JSON.stringify(data)
  }).then(handleResponse(schema))
}
