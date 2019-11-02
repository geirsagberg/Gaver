import { keyBy, without, clone } from 'lodash-es'
import { DeleteWishResponse, MyListModel, WishModel } from '~/types/data'
import { tryOrNotify } from '~/utils'
import { deleteJson, getJson, patchJson, postJson } from '~/utils/ajax'
import { showError, showSuccess } from '~/utils/notifications'
import { Action } from '..'
import { getEmptyWish } from './state'
import { isEmailValid } from '~/utils/validation'

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
    const wish = await postJson<WishModel>('/api/MyList', myList.newWish)
    myList.wishes[wish.id] = wish
    myList.newWish = null
    myList.wishesOrder.push(wish.id)
  })

export const startAddingWish: Action = ({ state: { myList } }) => {
  myList.newWish = getEmptyWish()
}

export const cancelAddingWish: Action = ({ state: { myList } }) => {
  myList.newWish = null
}

export const startEditingWish: Action<number> = ({ state: { myList } }, wishId) => {
  const wish = myList.wishes[wishId]
  myList.editingWish = { ...wish }
}

export const deleteEditingWish: Action<number> = async (
  {
    state: { myList },
    actions: {
      myList: { confirmDeleteWish }
    }
  },
  wishId
) => {
  await confirmDeleteWish(wishId)
  myList.editingWish = null
}

export const confirmDeleteWish: Action<number> = ({ state: { myList } }, wishId) =>
  tryOrNotify(async () => {
    const response = await deleteJson<DeleteWishResponse>(`/api/MyList/${wishId}`)
    myList.wishesOrder = response.wishesOrder
    delete myList.wishes[wishId]
  })

export const cancelEditingWish: Action = ({ state: { myList } }) => {
  myList.editingWish = null
}

export const updateEditingWish: Action<Partial<WishModel>> = ({ state: { myList } }, update) => {
  myList.editingWish = {
    ...myList.editingWish,
    ...update
  }
}

export const saveEditingWish: Action = ({ state: { myList } }) =>
  tryOrNotify(async () => {
    const wish = myList.editingWish
    await patchJson<WishModel>(`/api/MyList/${wish.id}`, wish)
    myList.wishes[wish.id] = clone(wish)
    myList.editingWish = null
  })

export const updateNewWish: Action<Partial<WishModel>> = ({ state }, update) => {
  state.myList.newWish = {
    ...state.myList.newWish,
    ...update
  }
}

export const loadWishes: Action = ({ state }) =>
  tryOrNotify(async () => {
    const model = await getJson<MyListModel>('/api/MyList')
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

export const emailAdded: Action<string, boolean> = ({ state: { myList } }, email) => {
  if (isEmailValid(email)) {
    myList.shareEmails.push(email)
    return true
  } else {
    showError('Ugyldig e-postadresse')
    return false
  }
}

export const emailDeleted: Action<string> = ({ state: { myList } }, email) => {
  myList.shareEmails = without(myList.shareEmails, email)
}

export const shareList: Action = ({ state: { myList } }) =>
  tryOrNotify(async () => {
    await postJson('/api/MyList/Share', { emails: myList.shareEmails })
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
  const originalOrder = myList.wishesOrder.slice()
  myList.wishesOrder.splice(payload.oldIndex, 1)
  myList.wishesOrder.splice(payload.newIndex, 0, payload.wishId)

  try {
    await postJson('/api/MyList/Order', { wishesOrder: myList.wishesOrder })
  } catch (error) {
    myList.wishesOrder = originalOrder
    showError(error)
  }
}
