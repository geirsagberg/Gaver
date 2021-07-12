import 'isomorphic-fetch'
import { first, values } from 'lodash-es'
import { normalize } from 'normalizr'
import AuthService from './AuthService'
import { publish, Topic } from './pubSub'

const acceptJsonHeader = {
  Accept: 'application/json',
  'Accept-Charset': 'utf-8',
}

const contentTypeJsonHeader = {
  'Content-Type': 'application/json',
}

const jsonHeaders = {
  ...acceptJsonHeader,
  ...contentTypeJsonHeader,
}

const handleResponse = (schema: any) => async (response: any) => {
  const contentType = response.headers.get('content-type')
  if (contentType && contentType.indexOf('json') !== -1) {
    const data = await response.json()
    if (!response.ok) {
      const message = Array.isArray(data)
        ? data.map((d) => d.message).join()
        : data.message ||
          (data.error ? data.error.message : undefined) ||
          (typeof data.errors === 'object' ? first(values(data.errors)) : '')
      throw new Error(message)
    }
    return schema ? normalize(data, schema) : data
  } else {
    if (!response.ok) {
      throw new Error('Something went wrong')
    }
  }
}

async function tryAjax<T>(
  func: () => Promise<Response>,
  schema: any
): Promise<T> {
  publish(Topic.AjaxStart)
  try {
    const response = await func()
    return await handleResponse(schema)(response)
  } finally {
    publish(Topic.AjaxStop)
  }
}

const getAuthHeader = () => ({
  Authorization: 'Bearer ' + AuthService.loadAccessToken(),
})

const getCredentials = (includeCredentials: boolean) =>
  includeCredentials ? 'include' : 'omit'

export const getJson = <T = any>(
  url: string,
  schema?: any,
  includeCredentials = true
) =>
  tryAjax<T>(
    () =>
      fetch(url, {
        credentials: getCredentials(includeCredentials),
        headers: {
          ...acceptJsonHeader,
          ...getAuthHeader(),
        },
      }),
    schema
  )

const createJsonMethod =
  (method: string) =>
  <T = any>(url: string, data?: any, schema?: any, includeCredentials = true) =>
    tryAjax<T>(
      () =>
        fetch(url, {
          method,
          credentials: getCredentials(includeCredentials),
          headers: {
            ...jsonHeaders,
            ...getAuthHeader(),
          },
          body: JSON.stringify(data),
        }),
      schema
    )

export const postJson = createJsonMethod('POST')

export const putJson = createJsonMethod('PUT')

export const patchJson = createJsonMethod('PATCH')

export const deleteJson = createJsonMethod('DELETE')
