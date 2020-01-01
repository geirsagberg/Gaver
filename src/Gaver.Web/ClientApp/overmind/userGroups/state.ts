import { Normalized } from '~/utils/normalize'
import { UserGroupDto } from '~/types/data'

export type UserGroup = Normalized<UserGroupDto>

export type NewUserGroup = Omit<UserGroup, 'id' | 'createdByUserId'>

export interface UserGroupState {
  newGroup?: NewUserGroup
  userGroups: Dictionary<UserGroup>
  editingGroup?: UserGroup
}

export const state: UserGroupState = {
  userGroups: {}
}
