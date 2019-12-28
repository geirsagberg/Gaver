import { UserDto } from '~/types/data'

type User = UserDto

export interface UsersState {
  users: Dictionary<User>
}

export const state: UsersState = {
  users: {}
}
