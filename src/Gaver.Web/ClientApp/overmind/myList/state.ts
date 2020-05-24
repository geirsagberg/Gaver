import { WishDto } from '~/types/data'
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
  orderedWishes: Wish[]
  isDeleting?: boolean
}

export const getEmptyWish = (): Wish => ({
  title: '',
})

export const state: MyListState = {
  shareEmails: [],
  wishes: {},
  isSharingList: false,
  wishesOrder: [],
  get orderedWishes() {
    return this.wishesOrder.map((i) => this.wishes[i])
  },
}
