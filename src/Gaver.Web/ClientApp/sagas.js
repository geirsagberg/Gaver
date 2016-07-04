import { takeLatest, takeEvery } from 'redux-saga'
import { call, put, fork, take } from 'redux-saga/effects'
import * as Api from './listApi'
import { fetchDataSuccess, wishAdded, LOAD_DATA, ADD_WISH, WISH_ADDED } from './store/currentList'

function * fetchWishData () {
  try {
    const data = yield call(Api.fetchWishData)
    yield put(fetchDataSuccess(data))
  } catch (error) {
    yield put({
      type: 'FETCH_FAILED',
      error
    })
  }
}

function * addWish (action) {
  try {
    const wish = yield call(Api.addWish, action.wish)
    yield put(wishAdded(wish))
  } catch (error) {
    yield put({
      type: 'FETCH_FAILED',
      error
    })
  }
}

export function * watchFetchData () {
  yield * takeLatest(LOAD_DATA, fetchWishData)
}

export function * addWishSaga () {
  yield * takeEvery(ADD_WISH, addWish)
}

export default function * rootSaga () {
  yield [
    fork(addWishSaga),
    fork(watchFetchData)
  ]
}
