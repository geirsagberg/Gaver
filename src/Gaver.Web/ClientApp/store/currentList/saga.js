import { takeLatest, takeEvery, eventChannel } from 'redux-saga'
import { call, put, fork, take } from 'redux-saga/effects'
import * as Api from './api'
import * as actions from './actions'
import { showPrompt } from 'utils/dialogs'
import $ from 'jquery'
import Immutable from 'seamless-immutable'

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

function * shareList () {
  while (true) {
    yield take(actions.SHARE_LIST)
    const emails = yield call(showPrompt, {
      message: 'Skriv inn epostadressen til de du vil dele listen med',
      placeholder: 'eksempel@epost.com, ...'
    })
    if (emails !== null) {
      // TODO: Validation
      const emailList = emails.split(',').map(email => email.trim())
      yield call(Api.shareList, {
        emails: emailList
      })
    }
  }
}

function createSignalrChannel (connection) {
  const client = connection.listHub
  return eventChannel(emit => {
    client.updateUsers = users => emit(users)

    // unsubscribe
    return () => {
      delete client.updateUsers
    }
  })
}

function doStuff (users) {
  console.log(users)
}

function * initializeListUpdates () {
  $.connection.hub.logging = process.env.NODE_ENV === 'development'
  const listHub = $.connection.listHub
  const signalrChannel = yield call(createSignalrChannel, $.connection)
  signalrChannel.take(doStuff)
  yield call(() => $.connection.hub.start())
  const users = yield call(listHub.server.subscribe)
  yield put(actions.setUsers(Immutable(users)))
}

export default function rootSaga () {
  return [
    takeLatest(actions.LOAD_DATA, fetchWishData),
    takeEvery(actions.ADD_WISH, addWish),
    takeLatest(actions.DELETE_WISH, deleteWish),
    takeEvery(actions.INITIALIZE_LIST_UPDATES, initializeListUpdates),
    fork(shareList)
  ]
}
