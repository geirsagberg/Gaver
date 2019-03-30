import { Wish } from '~/types/data'

export interface MyListState {
  id?: number
  editingWish?: Wish
  shareEmails: string[]
  isSharingList: boolean
  wishes: Dictionary<Wish>
  isAddingWish: boolean
  newWish?: Wish
}

export const getEmptyWish = () => ({
  title: ''
})

export const state: MyListState = {
  shareEmails: [],
  wishes: {},
  isAddingWish: false,
  isSharingList: false
}
