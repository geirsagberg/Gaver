import { WishDto } from '~/types/data'
import { Derive } from '..'
import { Normalized } from '~/utils/normalize'

export type Wish = Partial<Normalized<WishDto>>

export type MyListState = {
  id?: number
  editingWish?: Wish
  shareEmails: string[]
  isSharingList: boolean
  wishes: Dictionary<Wish>
  newWish?: Wish
  wishesLoaded?: boolean
  wishesOrder: number[]
  orderedWishes: Derive<MyListState, Wish[]>
  isDeleting?: boolean
}

export const getEmptyWish = (): Wish => ({
  title: ''
})

export const state: MyListState = {
  shareEmails: [],
  wishes: {},
  isSharingList: false,
  wishesOrder: [],
  orderedWishes: state => state.wishesOrder.map(i => state.wishes[i])
}
