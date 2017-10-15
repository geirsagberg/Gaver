import 'isomorphic-fetch'
import Immutable from 'seamless-immutable'
import { normalize } from 'normalizr'
import Promise from 'bluebird'
import PubSub from 'pubsub-js'
import * as topics from 'constants/topics'
import { loadIdToken } from 'utils/auth'

const jsonHeaders = {
  Accept: 'application/json',
  'Content-Type': 'application/json'
}

const handleResponse = (schema) => (response) => {
  var contentType = response.headers.get('content-type')
  if (contentType && contentType.indexOf('application/json') !== -1) {
    return Promise.try(() => response.json()).then((data) => {
      if (!response.ok) {
        const message = Array.isArray(data) ? data.map((d) => d.message).join() : data.message
        throw new Error(message)
      }
      return Immutable(schema ? normalize(data, schema) : data)
    })
  } else {
    return response.ok ? Promise.resolve() : Promise.reject(new Error('Something went wrong'))
  }
}

const handleError = (error) => {
  console.error(error)
  throw new Error('Could not reach server')
}

const getAuthHeader = () => ({
  Authorization: 'Bearer ' + loadIdToken()
})

const getAllHeaders = () => ({
  ...jsonHeaders,
  ...getAuthHeader()
})

export function getJson (url, schema) {
  PubSub.publish(topics.AJAX_START)
  return Promise.try(() =>
    fetch(url, {
      credentials: 'include',
      headers: getAuthHeader()
    })
  )
    .then(handleResponse(schema), handleError)
    .finally(() => PubSub.publish(topics.AJAX_STOP))
}

export function postJson (url, data, schema) {
  PubSub.publish(topics.AJAX_START)
  return Promise.try(() =>
    fetch(url, {
      method: 'POST',
      headers: getAllHeaders(),
      credentials: 'include',
      body: JSON.stringify(data)
    })
  )
    .then(handleResponse(schema), handleError)
    .finally(() => PubSub.publish(topics.AJAX_STOP))
}

export function putJson (url, data, schema) {
  PubSub.publish(topics.AJAX_START)
  return Promise.try(() =>
    fetch(url, {
      method: 'PUT',
      headers: getAllHeaders(),
      credentials: 'include',
      body: JSON.stringify(data)
    })
  )
    .then(handleResponse(schema), handleError)
    .finally(() => PubSub.publish(topics.AJAX_STOP))
}

export function deleteJson (url, data, schema) {
  PubSub.publish(topics.AJAX_START)
  return Promise.try(() =>
    fetch(url, {
      method: 'DELETE',
      headers: getAllHeaders(),
      credentials: 'include',
      body: JSON.stringify(data)
    })
  )
    .then(handleResponse(schema), handleError)
    .finally(() => PubSub.publish(topics.AJAX_STOP))
}
