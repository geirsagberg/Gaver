import 'isomorphic-fetch'
import { normalize } from 'normalizr'
import { publish, Topic } from './pubSub'
import AuthService from './AuthService'

const acceptJsonHeader = {
  Accept: 'application/json',
  'Accept-Charset': 'utf-8'
}

const contentTypeJsonHeader = {
  'Content-Type': 'application/json'
}

const jsonHeaders = {
  ...acceptJsonHeader,
  ...contentTypeJsonHeader
}

const handleResponse = schema => async response => {
  var contentType = response.headers.get('content-type')
  if (contentType && contentType.indexOf('json') !== -1) {
    const data = await response.json()
    if (!response.ok) {
      const message = Array.isArray(data)
        ? data.map(d => d.message).join()
        : data.message || data.error
        ? data.error.message
        : undefined
      throw new Error(message)
    }
    return schema ? normalize(data, schema) : data
  } else {
    if (!response.ok) {
      throw new Error('Something went wrong')
    }
  }
}

async function tryAjax<T>(func: () => Promise<Response>, schema): Promise<T> {
  publish(Topic.AjaxStart)
  try {
    const response = await func()
    return await handleResponse(schema)(response)
  } finally {
    publish(Topic.AjaxStop)
  }
}

const getAuthHeader = () => ({
  Authorization: 'Bearer ' + AuthService.loadAccessToken()
})

const getCredentials = (includeCredentials: boolean) => (includeCredentials ? 'include' : 'omit')

export const getJson = <T = any>(url, schema?, includeCredentials = true) =>
  tryAjax<T>(
    () =>
      fetch(url, {
        credentials: getCredentials(includeCredentials),
        headers: {
          ...acceptJsonHeader,
          ...getAuthHeader()
        }
      }),
    schema
  )

const createJsonMethod = (method: string) => <T = any>(url, data?, schema?, includeCredentials = true) =>
  tryAjax<T>(
    () =>
      fetch(url, {
        method,
        credentials: getCredentials(includeCredentials),
        headers: {
          ...jsonHeaders,
          ...getAuthHeader()
        },
        body: JSON.stringify(data)
      }),
    schema
  )

export const postJson = createJsonMethod('POST')

export const putJson = createJsonMethod('PUT')

export const patchJson = createJsonMethod('PATCH')

export const deleteJson = createJsonMethod('DELETE')
