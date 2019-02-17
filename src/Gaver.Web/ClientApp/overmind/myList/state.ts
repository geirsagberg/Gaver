import { Wish } from '~/types/data'

export interface MyListState {
  wishes: Dictionary<Wish>
  isAddingWish: boolean
  newWish: Wish
}

export const getEmptyWish = () => ({
  title: ''
})

export const state: MyListState = {
  wishes: {},
  isAddingWish: false,
  newWish: getEmptyWish()
}
