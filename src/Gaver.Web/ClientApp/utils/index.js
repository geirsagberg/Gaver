export const isDevelopment = process.env.NODE_ENV === 'development'

// Bind arguments starting with argument number "n".
export function bindArgsFromN (fn, n, ...boundArgs) {
  return function (...args) {
    return fn(...args.slice(0, n - 1), ...boundArgs)
  }
}