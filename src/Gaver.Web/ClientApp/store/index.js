import * as currentList from './currentList'
import { fork } from 'redux-saga/effects'

// Whenever an action is dispatched, Redux will update each top-level application state property using
// the reducer with the matching name. It's important that the names match exactly, and that the reducer
// acts on the corresponding ApplicationState property type.
export const reducers = {
  currentList: currentList.reducer
}

export function * rootSaga () {
  yield [
    fork(currentList.saga)
  ]
}