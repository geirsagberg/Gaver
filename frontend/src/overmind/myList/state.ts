import { WishDto } from '~/types/data'
import { Normalized } from '~/utils/normalize'

export type Wish = Normalized<WishDto>

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
  id: 0,
  title: '',
  options: {},
  url: '',
})

export const state: MyListState = {
  shareEmails: [],
  wishes: {},
  isSharingList: false,
  wishesOrder: [],
  get orderedWishes() {
    return this.wishesOrder.map((i) => this.wishes[i]).filter((i) => !!i)
  },
}
