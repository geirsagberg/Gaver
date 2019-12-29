import { UserDto } from '~/types/data'

type User = UserDto

export interface FriendsState {
  users: Dictionary<User>
}

export const state: FriendsState = {
  users: {}
}
