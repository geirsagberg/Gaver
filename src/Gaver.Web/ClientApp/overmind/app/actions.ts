import { orderBy } from 'lodash-es'
import { SharedListsModel } from '~/types/data'
import { tryOrNotify } from '~/utils'
import { getJson } from '~/utils/ajax'
import { Action } from '../'

export const incrementAjaxCounter: Action = ({ state: { app } }) => {
  app.ajaxCounter += 1
}

export const decrementAjaxCounter: Action = ({ state: { app } }) => {
  app.ajaxCounter -= 1
}

export const showMenu: Action = ({ state: { app } }) => {
  app.isMenuShowing = true
}

export const hideMenu: Action = ({ state: { app } }) => {
  app.isMenuShowing = false
}

export const loadSharedLists: Action = ({ state }) =>
  tryOrNotify(async () => {
    const model = await getJson<SharedListsModel>('/api/SharedLists')
    state.invitations.sharedLists = orderBy(model.invitations, i => i.wishListUserName)
  })
