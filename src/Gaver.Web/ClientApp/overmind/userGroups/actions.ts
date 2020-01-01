import { tryOrNotify } from '~/utils'
import { showConfirm, showSuccess } from '~/utils/notifications'
import { Action, AsyncAction } from '..'
import { RouteCallbackArgs } from '../routing/effects'
import { NewUserGroup, UserGroup } from './state'

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
  userGroups.newGroup = { name: '', userIds: [] }
}

export const cancelAddingGroup: Action = ({ state: { userGroups } }) => {
  delete userGroups.newGroup
}

export const startEditingGroup: Action<number> = ({ state: { userGroups } }, groupId) => {
  userGroups.editingGroup = { ...userGroups.userGroups[groupId] }
}

export const cancelEditingGroup: Action = ({ state: { userGroups } }) => {
  delete userGroups.editingGroup
}

export const updateNewGroup: Action<Partial<NewUserGroup>> = ({ state: { userGroups } }, update) => {
  userGroups.newGroup = {
    ...userGroups.newGroup,
    ...update
  }
}

export const updateEditingGroup: Action<Partial<UserGroup>> = ({ state: { userGroups } }, update) => {
  userGroups.editingGroup = {
    ...userGroups.editingGroup,
    ...update
  }
}

export const loadUserGroups: AsyncAction = ({
  effects: {
    userGroups: { getUserGroups }
  },
  state: { userGroups }
}) =>
  tryOrNotify(async () => {
    const userGroupsData = await getUserGroups()
    userGroups.userGroups = userGroupsData
  })

export const createUserGroup: Action = ({
  effects: {
    userGroups: { postUserGroup }
  },
  state: { userGroups }
}) =>
  tryOrNotify(async () => {
    const userGroup = await postUserGroup(userGroups.newGroup)
    userGroups.userGroups[userGroup.id] = userGroup
    delete userGroups.newGroup
    showSuccess('Gruppe opprettet')
  })

export const updateUserGroup: Action = ({
  effects: {
    userGroups: { patchUserGroup }
  },
  state: { userGroups }
}) =>
  tryOrNotify(async () => {
    const userGroup = { ...userGroups.editingGroup }
    await patchUserGroup(userGroup.id, userGroup)
    userGroups.userGroups[userGroup.id] = userGroup
    delete userGroups.editingGroup
    showSuccess('Gruppe oppdatert')
  })

export const deleteEditingGroup: Action = ({
  effects: {
    userGroups: { deleteUserGroup }
  },
  state: { userGroups }
}) =>
  tryOrNotify(async () => {
    const {
      editingGroup: { id, name }
    } = userGroups
    if (await showConfirm(`Er du sikker på at du vil slette gruppen "${name}"?`)) {
      await deleteUserGroup(id)
      delete userGroups.userGroups[id]
      delete userGroups.editingGroup
      showSuccess('Gruppe slettet')
    }
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
