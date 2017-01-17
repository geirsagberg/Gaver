import { showError } from './notifications'
import Promise from 'bluebird'

export const isDevelopment = process.env.NODE_ENV === 'development'

// Bind arguments starting with argument number "n".
export function bindArgsFromN(fn, n, ...boundArgs) {
  return function (...args) {
    return fn(...args.slice(0, n - 1), ...boundArgs)
  }
}

export async function tryOrNotify(func) {
  try {
    await func()
  } catch (error) {
    showError(error)
  }
  return Promise.resolve()
}

export function getQueryVariable(variable) {
  var query = window.location.search.substring(1)
  var vars = query.split('&')
  for (var i = 0; i < vars.length; i++) {
    var pair = vars[i].split('=')
    if (pair[0] === variable) { return pair[1] }
  }
  return false
}