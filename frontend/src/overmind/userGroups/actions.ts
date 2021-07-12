import { tryOrNotify } from '~/utils'
import { showConfirm, showSuccess } from '~/utils/notifications'
import { Context } from '..'
import { NewUserGroup, UserGroup } from './state'

export const handleUserGroups = async ({ actions }: Context) => {
  actions.routing.setCurrentPage('userGroups')
  await actions.userGroups.loadUserGroups()
}

export const startAddingGroup = ({ state: { userGroups } }: Context) => {
  userGroups.newGroup = { name: '', userIds: [] }
}

export const cancelAddingGroup = ({ state: { userGroups } }: Context) => {
  delete userGroups.newGroup
}

export const startEditingGroup = (
  { state: { userGroups } }: Context,
  groupId: number
) => {
  userGroups.editingGroup = { ...userGroups.userGroups[groupId] }
}

export const cancelEditingGroup = ({ state: { userGroups } }: Context) => {
  delete userGroups.editingGroup
}

export const updateNewGroup = (
  { state: { userGroups } }: Context,
  update: Partial<UserGroup>
) => {
  userGroups.newGroup = {
    ...userGroups.newGroup,
    ...update,
  } as NewUserGroup
}

export const updateEditingGroup = (
  { state: { userGroups } }: Context,
  update: Partial<UserGroup>
) => {
  userGroups.editingGroup = {
    ...userGroups.editingGroup,
    ...update,
  } as UserGroup
}

export const loadUserGroups = ({
  effects: {
    userGroups: { getUserGroups },
  },
  state: { userGroups },
}: Context) =>
  tryOrNotify(async () => {
    const userGroupsData = await getUserGroups()
    userGroups.userGroups = userGroupsData
  })

export const createUserGroup = ({
  effects: {
    userGroups: { postUserGroup },
  },
  state: { userGroups },
}: Context) =>
  tryOrNotify(async () => {
    if (userGroups.newGroup) {
      const userGroup = await postUserGroup(userGroups.newGroup)
      userGroups.userGroups[userGroup.id] = userGroup
      delete userGroups.newGroup
      showSuccess('Gruppe opprettet')
    }
  })

export const updateUserGroup = ({
  effects: {
    userGroups: { patchUserGroup },
  },
  state: { userGroups },
}: Context) =>
  tryOrNotify(async () => {
    if (userGroups.editingGroup) {
      const userGroup = { ...userGroups.editingGroup }
      await patchUserGroup(userGroup.id, userGroup)
      userGroups.userGroups[userGroup.id] = userGroup
      delete userGroups.editingGroup
      showSuccess('Gruppe oppdatert')
    }
  })

export const deleteEditingGroup = ({
  effects: {
    userGroups: { deleteUserGroup },
  },
  state: { userGroups },
}: Context) =>
  tryOrNotify(async () => {
    if (userGroups.editingGroup) {
      const {
        editingGroup: { id, name },
      } = userGroups
      if (
        await showConfirm(`Er du sikker på at du vil slette gruppen "${name}"?`)
      ) {
        await deleteUserGroup(id)
        delete userGroups.userGroups[id]
        delete userGroups.editingGroup
        showSuccess('Gruppe slettet')
      }
    }
  })

export const setUserGroupName = (
  {
    effects: {
      userGroups: { patchUserGroup },
    },
    state: {
      routing: { currentUserGroupId },
    },
  }: Context,
  name: string
) =>
  tryOrNotify(async () => {
    if (currentUserGroupId) await patchUserGroup(currentUserGroupId, { name })
  })
