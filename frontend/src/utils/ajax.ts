import 'isomorphic-fetch'
import { first, values } from 'lodash-es'
import { normalize } from 'normalizr'
import { publish, Topic } from './pubSub'

export const acceptJsonHeader = {
  Accept: 'application/json',
  'Accept-Charset': 'utf-8',
}

const contentTypeJsonHeader = {
  'Content-Type': 'application/json',
}

export const jsonHeaders = {
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

export async function tryAjax<T>(func: () => Promise<Response>, schema?: any): Promise<T> {
  publish(Topic.AjaxStart)
  try {
    const response = await func()
    return await handleResponse(schema)(response)
  } finally {
    publish(Topic.AjaxStop)
  }
}
