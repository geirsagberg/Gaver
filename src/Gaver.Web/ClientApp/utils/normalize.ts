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
  const result =
    Array.isArray(obj) && !!obj[0]?.[iteratee]
      ? normalizeArrays(keyBy(obj, iteratee))
      : obj !== null && typeof obj === 'object'
      ? (mapValues((obj as unknown) as object, (value, key) => {
          return normalizeArrays(!includes(ignoreProps, key) && Array.isArray(value) ? keyBy(value, iteratee) : value)
        }) as Normalized<T>)
      : obj
  return result as T extends object ? Normalized<T> : T
}
