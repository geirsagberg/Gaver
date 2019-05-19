import { WishModel } from '~/types/data'
import { Derive } from '..'

export type MyListState = {
  id?: number
  editingWish?: WishModel
  shareEmails: string[]
  isSharingList: boolean
  wishes: Dictionary<WishModel>
  newWish?: WishModel
  wishesLoaded?: boolean
  wishesOrder: number[]
  orderedWishes: Derive<MyListState, WishModel[]>
  isDeleting?: boolean
}

export const getEmptyWish = () => ({
  title: ''
})

export const state: MyListState = {
  shareEmails: [],
  wishes: {},
  isSharingList: false,
  wishesOrder: [],
  orderedWishes: state => state.wishesOrder.map(i => state.wishes[i])
}
