import * as Api from './api'
import { showPrompt } from 'utils/dialogs'
import { showSuccess, showError } from 'utils/notifications'
import { isDevelopment } from 'utils'
import { normalize } from 'normalizr'
import $ from 'jquery'
import Immutable from 'seamless-immutable'
import { compose } from 'redux'
import { wishes } from 'schemas'

const actionNamespace = 'gaver/currentList/'

export const ADD_WISH = actionNamespace + 'ADD_WISH'
export const LOAD_DATA = actionNamespace + 'LOAD_DATA'
export const DATA_LOADED = actionNamespace + 'DATA_LOADED'
export const WISH_ADDED = actionNamespace + 'WISH_ADDED'
export const DELETE_WISH = actionNamespace + 'DELETE_WISH'
export const WISH_DELETED = actionNamespace + 'WISH_DELETED'
export const SHARE_LIST = actionNamespace + 'SHARE_LIST'
export const INITIALIZE_LIST_UPDATES = actionNamespace + 'INITIALIZE_LIST_UPDATES'
export const SET_USERS = actionNamespace + 'SET_COUNT'

export const loadData = () => async dispatch => {
  try {
    const data = await Api.fetchWishData()
    dispatch(fetchDataSuccess(data))
  } catch (error) {
    showError(error)
  }
}

export const addWish = title => async dispatch => {
  try {
    const wish = await Api.addWish(title)
    dispatch(wishAdded(wish))
  } catch (error) {
    showError(error)
  }
}

export const deleteWish = id => async dispatch => {
  try {
    await Api.deleteWish(id)
    dispatch(wishDeleted(id))
  } catch (error) {
    showError(error)
  }
}

export const shareList = () => async dispatch => {
  const input = await showPrompt({
    message: 'Skriv inn epostadressen til de du vil dele listen med',
    placeholder: 'eksempel@epost.com, ...'
  })
  if (input !== null) {
    // TODO: Validation
    const emails = input.split(',').map(email => email.trim())
    try {
      await Api.shareList({
        emails
      })
      showSuccess('Ønskeliste delt!')
    } catch (error) {
      showError(error)
    }
  }
}

const createCaller = dispatch =>
  (action, schema) =>
      compose(dispatch, action, Immutable, data => schema ? normalize(data, schema) : data)

export const initializeListUpdates = () => async dispatch => {
  $.connection.hub.logging = isDevelopment
  const { client } = $.connection.listHub
  const call = createCaller(dispatch)
  const functions = {
    updateUsers: call(setUsers),
    refresh: call(fetchDataSuccess, wishes)
  }
  $.extend(client, functions)
}

export function wishAdded(wish) {
  return {
    type: WISH_ADDED,
    data: wish
  }
}

export function fetchDataSuccess(data) {
  return {
    type: DATA_LOADED,
    data
  }
}

export function wishDeleted(id) {
  return {
    type: WISH_DELETED,
    id
  }
}

export function setUsers(users) {
  return {
    type: SET_USERS,
    users
  }
}