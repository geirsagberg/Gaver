import { keyBy, mapValues, includes } from 'lodash-es'

export type Normalized<T> = T extends Array<infer A extends object ? infer A : never>
  ? Dictionary<Normalized<A>>
  : {
      [P in keyof T]: T[P] extends Array<infer A extends object ? infer A : never>
        ? Dictionary<Normalized<A>>
        : Normalized<T[P]>
    }

export function normalizeArrays<T>(
  obj: T,
  ignoreProps: string[] = null,
  iteratee = 'id'
): T extends object ? Normalized<T> : T {
  // If obj is nullish or not an object, return obj
  // If obj is an array of items with iteratee prop, convert to obj and normalize each item
  // If obj is any other array, normalize each item
  // Else, normalize each prop

  const result =
    obj == null || typeof obj !== 'object'
      ? obj
      : Array.isArray(obj) && (!obj.length || obj[0]?.hasOwnProperty(iteratee))
      ? normalizeArrays(keyBy(obj, iteratee))
      : Array.isArray(obj)
      ? obj.map(o => normalizeArrays(o))
      : (mapValues((obj as unknown) as object, (value, key) => {
          return normalizeArrays(
            !includes(ignoreProps, key) && Array.isArray(value) && obj[0]?.hasOwnProperty(iteratee)
              ? keyBy(value, iteratee)
              : value
          )
        }) as Normalized<T>)

  return result as T extends object ? Normalized<T> : T
}
