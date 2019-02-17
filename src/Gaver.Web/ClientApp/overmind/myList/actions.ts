import { Wish, MyListModel } from '~/types/data'
import { tryOrNotify } from '~/utils'
import { postJson, getJson } from '~/utils/ajax'
import { Action } from '..'
import { getEmptyWish } from './state'
import * as schemas from '~/schemas'

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

export const setTitle: Action<string> = ({ state }, title) => {
  state.myList.newWish.title = title
}

export const loadWishes: Action = async ({ state: { myList } }) => {
  const {
    entities: { wishes }
  } = await getJson<MyListModel>('/api/WishList', schemas.wishList)
  myList.wishes = wishes
}
