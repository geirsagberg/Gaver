import { SharedWishDto, UserDto, SharedListDto } from '~/types/data'
import { Normalized } from '~/utils/normalize'

export type SharedWish = Normalized<SharedWishDto>

export type User = UserDto

export type SharedList = Omit<Normalized<SharedListDto>, 'users'>

export interface SharedListState {
  wishLists: Dictionary<SharedList>
  users: Dictionary<User>
}

export const state: SharedListState = {
  wishLists: {},
  users: {},
}
