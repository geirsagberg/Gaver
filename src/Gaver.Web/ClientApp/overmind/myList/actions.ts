import { keyBy } from 'lodash-es'
import { MyListModel, WishModel } from '~/types/data'
import { tryOrNotify } from '~/utils'
import { getJson, postJson, putJson } from '~/utils/ajax'
import { getEmptyWish } from './state'
import { Action } from 'overmind'

export const loadWishes: Action = ({ state }) =>
  tryOrNotify(async () => {
    const model = await getJson<MyListModel>('/api/MyList')
    const { myList } = state
    myList.wishes = keyBy(model.wishes, w => w.id)
    myList.wishesOrder = model.wishesOrder
    myList.wishesLoaded = true
  })

export const addWish: Action = ({ state: { myList } }) =>
  tryOrNotify(async () => {
    const wish = await postJson<WishModel>('/api/MyList', myList.newWish)
    myList.wishes[wish.id] = wish
    myList.isAddingWish = false
    myList.wishesOrder.push(wish.id)
  })

export const saveEditingWish: Action = ({ state: { myList } }) =>
  tryOrNotify(async () => {
    const editingWish = myList.editingWish
    const wish = await putJson<WishModel>(`/api/MyList/${editingWish.id}/title`, { title: editingWish.title })
    myList.wishes[editingWish.id] = wish
    myList.editingWish = null
  })

export const startAddingWish: Action = ({ state: { myList } }) => {
  myList.newWish = getEmptyWish()
  myList.isAddingWish = true
}

export const cancelAddingWish: Action = ({ state: { myList } }) => {
  myList.isAddingWish = false
}

export const startEditingWish: Action<number> = ({ state: { myList } }, wishId) => {
  const wish = myList.wishes[wishId]
  myList.editingWish = { ...wish }
}

export const cancelEditingWish: Action = ({ state: { myList } }) => {
  myList.editingWish = null
}

export const updateEditingWish: Action<Partial<WishModel>> = ({ state: { myList } }, value) => {
  myList.editingWish = {
    ...myList.editingWish,
    ...value
  }
}

export const setNewWishTitle: Action<string> = ({ state }, title) => {
  state.myList.newWish.title = title
}
