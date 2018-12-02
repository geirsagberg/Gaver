import { ActionCreatorsMapObject, Dispatch, bindActionCreators } from 'redux'
import { ThunkDispatch } from 'redux-thunk'

export type ActionsUnion<A extends ActionCreatorsMapObject> =
  | ReturnType<A[keyof A]>
  | {
      type: 'ANY_ACTION'
    }

export interface Action<T extends string> {
  type: T
}

export interface ActionWithPayload<T extends string, P> extends Action<T> {
  type: T
  payload: P
}

export function createAction<T extends string>(type: T): Action<T>
export function createAction<T extends string, P>(type: T, payload: P): ActionWithPayload<T, P>
export function createAction<T extends string, P> (type: T, payload?: P) {
  return payload === undefined ? { type } : { type, payload }
}

export function bindThunkActionCreators<M extends ActionCreatorsMapObject> (map: M, dispatch: Dispatch) {
  return bindActionCreators<M, { [P in keyof M]: UnwrapThunk<M[P]> }>(map, dispatch)
}

export function createMapDispatchToProps<M extends ActionCreatorsMapObject> (actionCreators: M) {
  return function (dispatch: Dispatch) {
    return bindThunkActionCreators(actionCreators, dispatch)
  }
}

// Helpers
type IsValidArg<T> = T extends object ? (keyof T extends never ? false : true) : true

type UnwrapThunk<T extends Function> = T extends (
  a: infer A,
  b: infer B,
  c: infer C,
  d: infer D,
  e: infer E,
  f: infer F,
  g: infer G,
  h: infer H,
  i: infer I,
  j: infer J
) => (dispatch: ThunkDispatch<any, any, any>, getState?, extraArgument?) => infer R
  ? (IsValidArg<J> extends true
      ? (a: A, b: B, c: C, d: D, e: E, f: F, g: G, h: H, i: I, j: J) => R
      : IsValidArg<I> extends true
      ? (a: A, b: B, c: C, d: D, e: E, f: F, g: G, h: H, i: I) => R
      : IsValidArg<H> extends true
      ? (a: A, b: B, c: C, d: D, e: E, f: F, g: G, h: H) => R
      : IsValidArg<G> extends true
      ? (a: A, b: B, c: C, d: D, e: E, f: F, g: G) => R
      : IsValidArg<F> extends true
      ? (a: A, b: B, c: C, d: D, e: E, f: F) => R
      : IsValidArg<E> extends true
      ? (a: A, b: B, c: C, d: D, e: E) => R
      : IsValidArg<D> extends true
      ? (a: A, b: B, c: C, d: D) => R
      : IsValidArg<C> extends true
      ? (a: A, b: B, c: C) => R
      : IsValidArg<B> extends true
      ? (a: A, b: B) => R
      : IsValidArg<A> extends true
      ? (a: A) => R
      : () => R)
  : T
