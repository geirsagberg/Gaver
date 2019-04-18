import { Wish } from '~/types/data'
import { Derive } from '..'

export type MyListState = {
  id?: number
  editingWish?: Wish
  shareEmails: string[]
  isSharingList: boolean
  wishes: Dictionary<Wish>
  isAddingWish: boolean
  newWish?: Wish
  wishesLoaded?: boolean
  wishesOrder: number[]
  orderedWishes: Derive<MyListState, Wish[]>
  isDeleting?: boolean
}

export const getEmptyWish = () => ({
  title: ''
})

export const state: MyListState = {
  shareEmails: [],
  wishes: {},
  isAddingWish: false,
  isSharingList: false,
  wishesOrder: [],
  orderedWishes: state => state.wishesOrder.map(i => state.wishes[i])
}
