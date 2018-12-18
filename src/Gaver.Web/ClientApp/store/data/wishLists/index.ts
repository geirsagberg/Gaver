import { Wish } from '~/types/data'
import produce from 'immer'

export type WishListData = Dictionary<WishList>

export interface WishList {
  wishes: Dictionary<Wish>
}

const initialState: WishListData = {}

export const reducer = produce((draft: WishListData, action: any) => {
  return draft
}, initialState)
