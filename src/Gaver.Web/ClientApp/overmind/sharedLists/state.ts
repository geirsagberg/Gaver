import { SharedWishModel, UserModel } from '~/types/data'

export interface SharedList {
  id: number
  ownerUserId: number
  wishes: Dictionary<SharedWishModel>
}

export interface SharedListState {
  wishLists: Dictionary<SharedList>
  users: Dictionary<UserModel>
}

export const state: SharedListState = {
  wishLists: {},
  users: {}
}