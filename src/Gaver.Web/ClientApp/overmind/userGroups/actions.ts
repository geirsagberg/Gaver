import { tryOrNotify } from '~/utils'
import { Action } from '..'
import { CreateUserGroupRequest } from '~/types/data'
import { showSuccess } from '~/utils/notifications'
import { RouteCallbackArgs } from '../routing/effects'

export const handleUserGroups: Action<RouteCallbackArgs> = async ({
  actions: {
    routing: { setCurrentPage },
    userGroups: { loadUserGroups }
  }
}) => {
  setCurrentPage('userGroups')
  await loadUserGroups()
}

export const startAddingGroup: Action = ({ state: { userGroups } }) => {
  userGroups.newGroup = { name: '' }
}

export const cancelAddingGroup: Action = ({ state: { userGroups } }) => {
  delete userGroups.newGroup
}

export const loadUserGroups: Action = ({
  effects: {
    userGroups: { getUserGroups }
  },
  state: { userGroups }
}) =>
  tryOrNotify(async () => {
    const userGroupsData = await getUserGroups()
    userGroups.userGroups = userGroupsData
  })

export const createUserGroup: Action<CreateUserGroupRequest> = (
  {
    effects: {
      userGroups: { postUserGroup }
    }
  },
  request
) =>
  tryOrNotify(async () => {
    await postUserGroup(request)
    showSuccess('Gruppe opprettet')
  })

export const setUserGroupName: Action<string> = (
  {
    effects: {
      userGroups: { patchUserGroup }
    },
    state: {
      routing: { currentUserGroupId }
    }
  },
  name
) =>
  tryOrNotify(async () => {
    await patchUserGroup(currentUserGroupId, { name })
  })
