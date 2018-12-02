import { Wish } from '~/types/data'
import produce from 'immer'

export type WishListData = Dictionary<WishListState>

export interface WishListState {
  wishes: Dictionary<Wish>
}

const initialState: WishListData = {}

export const reducer = produce((draft, action) => {}, initialState)
