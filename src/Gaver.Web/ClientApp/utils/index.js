import { showError } from './notifications'

export const isDevelopment = process.env.NODE_ENV === 'development'

// Bind arguments starting with argument number "n".
export function bindArgsFromN (fn, n, ...boundArgs) {
  return function (...args) {
    return fn(...args.slice(0, n - 1), ...boundArgs)
  }
}

export async function tryOrNotify (func, finallyCallback) {
  try {
    await func()
    return true
  } catch (error) {
    showError(error)
    return false
  } finally {
    if (finallyCallback) {
      finallyCallback()
    }
  }
}

export function getQueryVariable (variable) {
  const query = window.location.search.substring(1)
  const vars = query.split('&')
  for (let i = 0; i < vars.length; i++) {
    const pair = vars[i].split('=')
    if (pair[0] === variable) {
      return pair[1]
    }
  }
  return false
}
