import { keyBy, without } from 'lodash-es'
import { DeleteWishResponse, MyListModel, WishModel } from '~/types/data'
import { tryOrNotify } from '~/utils'
import { deleteJson, getJson, postJson, putJson } from '~/utils/ajax'
import { showError, showSuccess } from '~/utils/notifications'
import { Action } from '..'
import { getEmptyWish } from './state'

export const handleMyList: Action = async ({
  actions: {
    routing: { setCurrentPage },
    myList: { loadWishes }
  }
}) => {
  setCurrentPage('myList')
  await loadWishes()
}

export const addWish: Action = ({ state: { myList } }) =>
  tryOrNotify(async () => {
    const wish = await postJson<WishModel>('/api/WishLists', myList.newWish)
    myList.wishes[wish.id] = wish
    myList.isAddingWish = false
    myList.wishesOrder.push(wish.id)
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
    // if (confirm('Er du sikker på at du vil slette dette ønsket?')) {
    const response = await deleteJson<DeleteWishResponse>(`/api/WishLists/${myList.id}/${wishId}`)
    myList.wishesOrder = response.wishesOrder
    delete myList.wishes[wishId]
    // }
  })

export const cancelEditingWish: Action = ({ state: { myList } }) => {
  myList.editingWish = null
}

export const updateEditingWish: Action<{ field: keyof WishModel; value }> = (
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
    const result = await putJson<WishModel>(`/api/WishLists/${myList.id}/${wish.id}/title`, { title: wish.title })
    myList.wishes[wish.id] = result
    myList.editingWish = null
  })

export const setNewWishTitle: Action<string> = ({ state }, title) => {
  state.myList.newWish.title = title
}

export const loadWishes: Action = ({ state }) =>
  tryOrNotify(async () => {
    const model = await getJson<MyListModel>('/api/WishLists')
    const { myList } = state
    myList.wishes = keyBy(model.wishes, w => w.id)
    myList.id = model.id
    myList.wishesOrder = model.wishesOrder
    myList.wishesLoaded = true
  })

export const startSharingList: Action = ({ state: { myList } }) => {
  myList.isSharingList = true
  myList.shareEmails = []
}

export const toggleDeleting: Action = ({ state: { myList } }) => {
  myList.isDeleting = !myList.isDeleting
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
    await postJson('/api/WishLists/Share', { emails: myList.shareEmails })
    showSuccess('Ønskeliste delt')
    myList.isSharingList = false
  })

interface WishOrderChangedParams {
  oldIndex: number
  newIndex: number
  wishId: number
}
export const wishOrderChanged: Action<WishOrderChangedParams> = async ({ state: { myList } }, payload) => {
  if (payload.newIndex === payload.oldIndex) {
    return
  }
  var originalOrder = myList.wishesOrder.slice()
  myList.wishesOrder.splice(payload.oldIndex, 1)
  myList.wishesOrder.splice(payload.newIndex, 0, payload.wishId)

  try {
    await postJson('/api/WishLists/Order', { wishesOrder: myList.wishesOrder })
  } catch (error) {
    myList.wishesOrder = originalOrder
    showError(error)
  }
}
