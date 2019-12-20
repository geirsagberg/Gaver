import { CreateUserGroupRequest, UpdateUserGroupRequest, UserGroupsDto } from '~/types/data'
import { getJson, patchJson, postJson } from '~/utils/ajax'
import { normalizeArrays } from '~/utils/normalize'
import { UserGroup } from './state'

export const getUserGroups = async (): Promise<Dictionary<UserGroup>> => {
  const userGroupsDto = await getJson<UserGroupsDto>('/api/userGroups')
  return normalizeArrays(userGroupsDto.userGroups)
}

export const postUserGroup = (request: CreateUserGroupRequest) => postJson('/api/userGroups', request)

export const patchUserGroup = (userGroupId: number, request: Partial<UpdateUserGroupRequest>) =>
  patchJson('/api/userGroups/' + userGroupId, request)
