import { takeLatest, takeEvery } from 'redux-saga'
import { call, put, fork, take } from 'redux-saga/effects'
import * as Api from './api'
import * as actions from './actions'

function * fetchWishData () {
  try {
    const data = yield call(Api.fetchWishData)
    yield put(actions.fetchDataSuccess(data))
  } catch (error) {
    yield put(actions.fetchFailed(error))
  }
}

function * addWish (action) {
  try {
    const data = yield call(Api.addWish, action.wish)
    yield put(actions.fetchDataSuccess(data))
  } catch (error) {
    yield put(actions.fetchFailed(error))
  }
}

function * deleteWish (action) {
  try {
    yield call(Api.deleteWish, action.id)
    yield put(actions.wishDeleted(action.id))
  } catch (error) {
    yield put(actions.fetchFailed(error))
  }
}

export function * fetchDataSaga () {
  yield * takeLatest(actions.LOAD_DATA, fetchWishData)
}

export function * addWishSaga () {
  yield * takeEvery(actions.ADD_WISH, addWish)
}

export function * deleteWishSaga () {
  yield * takeLatest(actions.DELETE_WISH, deleteWish)
}

export default function rootSaga () {
  return [
    fork(addWishSaga),
    fork(fetchDataSaga),
    fork(deleteWishSaga)
  ]
}
