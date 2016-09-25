import 'isomorphic-fetch'
import Immutable from 'seamless-immutable'
import { normalize } from 'normalizr'
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

      return Immutable(schema ? normalize(data, schema) : data)
    })
  } else {
    return response.ok ? Promise.resolve() : Promise.reject('Something went wrong')
  }
}

const handleError = () => {
  throw new Error('Could not reach server')
}

export function getJson (url, schema) {
  return fetch(url, {
    credentials: 'include'
  }).then(handleResponse(schema), handleError)
}

export function postJson (url, data, schema) {
  return fetch(url, {
    method: 'POST',
    headers,
    credentials: 'include',
    body: JSON.stringify(data)
  }).then(handleResponse(schema), handleError)
}

export function deleteJson (url, data, schema) {
  return fetch(url, {
    method: 'DELETE',
    headers,
    credentials: 'include',
    body: JSON.stringify(data)
  }).then(handleResponse(schema), handleError)
}
