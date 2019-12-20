import { Normalized } from '~/utils/normalize'
import { UserGroupDto } from '~/types/data'

export type UserGroup = Normalized<UserGroupDto>
export type NewUserGroup = {
  name: string
}

export interface UserGroupState {
  newGroup?: NewUserGroup
  userGroups: Dictionary<UserGroup>
}

export const state: UserGroupState = {
  userGroups: {}
}
