import * as Api from './api'
import { showPrompt } from 'utils/dialogs'
import { showSuccess, showError } from 'utils/notifications'
import $ from 'jquery'

const actionNamespace = 'gaver/myList/'

export const ADD_WISH = actionNamespace + 'ADD_WISH'
export const LOAD_DATA = actionNamespace + 'LOAD_DATA'
export const DATA_LOADED = actionNamespace + 'DATA_LOADED'
export const WISH_ADDED = actionNamespace + 'WISH_ADDED'
export const DELETE_WISH = actionNamespace + 'DELETE_WISH'
export const WISH_DELETED = actionNamespace + 'WISH_DELETED'
export const SHARE_LIST = actionNamespace + 'SHARE_LIST'
export const INITIALIZE_LIST_UPDATES = actionNamespace + 'INITIALIZE_LIST_UPDATES'
export const WISH_UPDATED = actionNamespace + 'WISH_UPDATED'

export const loadMyList = () => async dispatch => {
  try {
    const data = await Api.fetchWishData()
    dispatch(fetchDataSuccess(data))
  } catch (error) {
    showError(error)
  }
}

export const addWish = ({listId, title}) => async dispatch => {
  try {
    const wish = await Api.addWish({listId, title})
    dispatch(wishAdded(wish))
  } catch (error) {
    showError(error)
  }
}

export const deleteWish = ({listId, wishId}) => async dispatch => {
  try {
    await Api.deleteWish({listId, wishId})
    dispatch(wishDeleted(wishId))
  } catch (error) {
    showError(error)
  }
}

export const shareList = listId => async dispatch => {
  const input = await showPrompt({
    title: 'Skriv inn epostadressen til de du vil dele listen med',
    placeholder: 'eksempel@epost.com, ...'
  })
  if (input !== null) {
    // TODO: Validation
    const emails = input.split(',').map(email => email.trim())
    try {
      await Api.shareList({
        listId,
        emails
      })
      showSuccess('Ønskeliste delt!')
    } catch (error) {
      showError(error)
    }
  }
}

export const editUrl = ({listId, wishId}) => async (dispatch, getState) => {
  const url = await showPrompt({
    title: 'Legg inn en lenke til gaven',
    placeholder: 'http://eksempel.no',
    value: getState().myList.wishes[wishId].url
  })
  if (url !== null) {
    // TODO: Validation
    try {
      const wish = await Api.setUrl({
        listId,
        wishId,
        url
      })
      dispatch(wishUpdated(wish))
    } catch (error) {
      showError(error)
    }
  }
}

export function wishUpdated(wish) {
  return {
    type: WISH_UPDATED,
    data: wish
  }
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