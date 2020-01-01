import { CreateUserGroupRequest, UpdateUserGroupRequest, UserGroupsDto, UserGroupDto } from '~/types/data'
import { getJson, patchJson, postJson, deleteJson } from '~/utils/ajax'
import { normalizeArrays } from '~/utils/normalize'
import { UserGroup } from './state'

export const getUserGroups = async (): Promise<Dictionary<UserGroup>> => {
  const userGroupsDto = await getJson<UserGroupsDto>('/api/userGroups')
  return normalizeArrays(userGroupsDto.userGroups)
}

export const postUserGroup = (request: CreateUserGroupRequest) => postJson<UserGroupDto>('/api/userGroups', request)

export const patchUserGroup = (userGroupId: number, request: Partial<UpdateUserGroupRequest>) =>
  patchJson('/api/userGroups/' + userGroupId, request)

export const deleteUserGroup = (userGroupId: number) => deleteJson('/api/userGroups/' + userGroupId)
