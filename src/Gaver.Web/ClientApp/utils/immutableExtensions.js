import _get from 'lodash/get'
import _map from 'lodash/map'

export function deepMerge(input) {
  return this.merge(input, { deep: true })
}

export function getIn(path, fallback) {
  if (Array.isArray(path)) {
    path = path.join('.')
  } else if (typeof path !== 'string') {
    throw new Error('path must be string or array')
  }
  return _get(this, path, fallback)
}

export function map(func) {
  return _map(this, func)
}