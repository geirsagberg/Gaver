import * as Api from './api'
import { showPrompt } from '~/utils/dialogs'
import { showSuccess } from '~/utils/notifications'
import { tryOrNotify } from '~/utils'
import { wishUpdated, myListLoaded, wishAdded, wishDeleted } from '.'

export const loadMyList = () => dispatch =>
  tryOrNotify(async () => {
    const data = await Api.fetchWishData()
    dispatch(myListLoaded(data))
  })

export const addWish = ({ listId, title }) => dispatch =>
  tryOrNotify(async () => {
    const wish = await Api.addWish({ listId, title })
    dispatch(wishAdded(wish))
  })

export const deleteWish = ({ listId, wishId }) => dispatch =>
  tryOrNotify(async () => {
    await Api.deleteWish({ listId, wishId })
    dispatch(wishDeleted(wishId))
  })

export const shareList = listId => async dispatch => {
  const input = await showPrompt({
    title: 'Skriv inn epostadressen til de du vil dele listen med',
    placeholder: 'eksempel@epost.com, ...'
  })
  if (input === null) {
    return
  }
  // TODO: Validation
  const emails = input.split(',').map(email => email.trim())
  tryOrNotify(async () => {
    await Api.shareList({
      listId,
      emails
    })
    showSuccess('Ønskeliste delt!')
  })
}

export const editUrl = ({ listId, wishId }) => async (dispatch, getState) => {
  const url = await showPrompt({
    title: 'Legg inn en lenke til gaven',
    placeholder: 'http://eksempel.no',
    value: getState().myList.wishes[wishId].url
  })
  if (url === null) {
    return
  }
  // TODO: Validation
  tryOrNotify(async () => {
    const wish = await Api.setUrl({
      listId,
      wishId,
      url
    })
    dispatch(wishUpdated(wish))
  })
}

export const editDescription = ({ listId, wishId }) => async (dispatch, getState) => {
  const description = await showPrompt({
    title: 'Legg inn beskrivelse og/eller pris',
    value: getState().myList.wishes[wishId].description
  })
  if (description === null) {
    return
  }
  tryOrNotify(async () => {
    const wish = await Api.setDescription({
      listId,
      wishId,
      description
    })
    dispatch(wishUpdated(wish))
  })
}
