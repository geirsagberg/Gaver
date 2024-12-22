import { acceptJsonHeader, jsonHeaders, tryAjax } from './ajax'
import AuthService from './AuthService'

export const getJson = <T = any>(url: string, schema?: any, includeCredentials = true) =>
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

export const getAuthHeader = () => ({
  Authorization: 'Bearer ' + AuthService.loadAccessToken(),
})

export const getCredentials = (includeCredentials: boolean) => (includeCredentials ? 'include' : 'omit')
