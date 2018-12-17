import { HubConnectionBuilder } from '@aspnet/signalr'
import { AccessStatus } from '~/enums'
import { normalize } from 'normalizr'
import { push, replace } from 'react-router-redux'
import * as schemas from '~/schemas'
import { loadMessages } from '~/store/chat'
import { tryOrNotify } from '~/utils'
import { loadIdToken } from '~/utils/auth_old'
import { showError } from '~/utils/notifications'
import * as Api from './api'
import { dataLoaded, setBoughtSuccess, setAuthorized, setUsers, clearState } from '.'

export const loadSharedList = listId => async dispatch =>
  tryOrNotify(async () => {
    const data = await Api.loadSharedList(listId)
    dispatch(dataLoaded(data))
  })

export const setBought = ({ listId, wishId, isBought }) => async (dispatch, getState) =>
  tryOrNotify(async () => {
    await Api.setBought({ listId, wishId, isBought })
    dispatch(setBoughtSuccess({ wishId, isBought, userId: getState().user.id }))
  })

let listHub

export const subscribeList = (listId, token) => async dispatch =>
  tryOrNotify(async () => {
    if (token) {
      try {
        await Api.registerToken(listId, token)
      } catch (error) {
        showError(error)
        dispatch(replace('/'))
        return
      }
    }
    const accessStatus = await Api.checkSharedListAccess(listId)
    switch (accessStatus) {
      case AccessStatus.NotInvited:
        showError('Du er ikke invitert til denne listen')
        // TODO: Egen side for å be om tilgang
        dispatch(replace('/'))
        return
      case AccessStatus.Owner:
        dispatch(replace('/'))
        return
      case AccessStatus.Invited:
        dispatch(setAuthorized())
        break
    }

    dispatch(loadSharedList(listId))
    const idToken = loadIdToken()
    listHub = new HubConnectionBuilder().withUrl(`${document.location.origin}/listHub?id_token=${idToken}`)

    const updateUsers = data => {
      dispatch(setUsers(normalize(data.currentUsers, schemas.users)))
    }

    listHub.on('updateUsers', updateUsers)
    listHub.on('refresh', () => {
      dispatch(loadSharedList(listId))
      dispatch(loadMessages(listId))
    })
    await listHub.start()
    const users = await listHub.invoke('subscribe', listId)
    updateUsers(users)
  })

export const unsubscribeList = listId => async dispatch =>
  tryOrNotify(async () => {
    dispatch(clearState())
    if (listHub) {
      await listHub.invoke('unsubscribe', listId)
      await listHub.stop()
      listHub = null
    }
  })

export const showMyList = () => dispatch => {
  dispatch(push('/'))
}
