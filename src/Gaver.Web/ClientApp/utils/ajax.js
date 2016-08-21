import 'isomorphic-fetch'
import Immutable from 'seamless-immutable'
import { normalize, Schema } from 'normalizr'
import Promise from 'bluebird'

const headers = {
  'Accept': 'application/json',
  'Content-Type': 'application/json'
}

const handleResponse = schema => response => {
  var contentType = response.headers.get('content-type')
  if (contentType && contentType.indexOf('application/json') !== -1) {
    return Promise.resolve(response.json()).then(data => {
      if (!response.ok) {
        const message = Array.isArray(data)
          ? data.map(d => d.message).join()
          : data.message
        throw new Error(message)
      }
      return Immutable(normalize(data, schema))
    })
  } else {
    return response.ok ? Promise.resolve() : Promise.reject('Something went wrong')
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
