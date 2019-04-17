import { without } from 'lodash-es'
import * as schemas from '~/schemas'
import { MyListModel, Wish } from '~/types/data'
import { tryOrNotify } from '~/utils'
import { getJson, postJson, putJson, deleteJson } from '~/utils/ajax'
import { showError, showSuccess } from '~/utils/notifications'
import { Action } from '..'
import { getEmptyWish } from './state'

export const addWish: Action = ({ state: { myList } }) =>
  tryOrNotify(async () => {
    const wish = await postJson<Wish>('/api/WishList', myList.newWish)
    myList.wishes[wish.id] = wish
    myList.isAddingWish = false
  })

export const startAddingWish: Action = ({ state: { myList } }) => {
  myList.newWish = getEmptyWish()
  myList.isAddingWish = true
}

export const cancelAddingWish: Action = ({ state: { myList } }) => {
  myList.newWish = getEmptyWish()
  myList.isAddingWish = false
}

export const startEditingWish: Action<number> = ({ state: { myList } }, wishId) => {
  const wish = myList.wishes[wishId]
  myList.editingWish = { ...wish }
}

export const confirmDeleteWish: Action<number> = ({ state: { myList } }, wishId) =>
  tryOrNotify(async () => {
    if (confirm('Er du sikker på at du vil slette dette ønsket?')) {
      await deleteJson(`/api/WishList/${myList.id}/${wishId}`)
      delete myList.wishes[wishId]
    }
  })

export const cancelEditingWish: Action = ({ state: { myList } }) => {
  myList.editingWish = null
}

export const updateEditingWish: Action<{ field: keyof Wish; value }> = (
  {
    state: {
      myList: { editingWish }
    }
  },
  { field, value }
) => {
  editingWish[field] = value
}

export const saveEditingWish: Action = ({ state: { myList } }) =>
  tryOrNotify(async () => {
    const wish = myList.editingWish
    const result = await putJson<Wish>(`/api/WishList/${myList.id}/${wish.id}/title`, { title: wish.title })
    myList.wishes[wish.id] = result
    myList.editingWish = null
  })

export const setNewWishTitle: Action<string> = ({ state }, title) => {
  state.myList.newWish.title = title
}

export const loadWishes: Action = async ({ state: { myList } }) => {
  const {
    result,
    entities: { wishes = {} }
  } = await getJson<MyListModel>('/api/WishList', schemas.wishList)
  myList.wishes = wishes
  myList.id = result
}

export const startSharingList: Action = ({ state: { myList } }) => {
  myList.isSharingList = true
  myList.shareEmails = []
}

export const cancelSharingList: Action = ({ state: { myList } }) => {
  myList.isSharingList = false
  myList.shareEmails = []
}

const emailRegex = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/

export const emailAdded: Action<string> = ({ state: { myList } }, email) => {
  if (emailRegex.test(email)) {
    myList.shareEmails.push(email)
  } else {
    showError('Ugyldig e-postadresse')
  }
}

export const emailDeleted: Action<string> = ({ state: { myList } }, email) => {
  myList.shareEmails = without(myList.shareEmails, email)
}

export const shareList: Action = ({ state: { myList } }) =>
  tryOrNotify(async () => {
    await postJson('/api/WishList/Share', { emails: myList.shareEmails })
    showSuccess('Ønskeliste delt')
    myList.isSharingList = false
  })
