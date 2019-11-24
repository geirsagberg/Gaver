import { some } from 'lodash-es'
import { SharedListDto } from '~/types/data'
import { tryOrNotify } from '~/utils'
import { getJson, putJson, postJson } from '~/utils/ajax'
import { normalizeArrays } from '~/utils/normalize'
import { Action } from '..'
import { RouteCallbackArgs } from '../routing/effects'
import { User } from './state'
import { showSuccess } from '~/utils/notifications'

export const onUpdateUsers: Action<Dictionary<User>> = ({ state }, users) => {
  state.sharedLists.users = {
    ...state.sharedLists.users,
    ...users
  }
}

export const handleSharedList: Action<RouteCallbackArgs> = async (
  {
    actions: {
      routing: { setCurrentPage, setCurrentSharedList },
      sharedLists: { loadSharedList, onUpdateUsers },
      chat: { clearMessages, loadMessages, onMessageAdded }
    },
    state: { auth, invitations },
    effects: {
      routing: { redirect },
      sharedLists: { subscribeList }
    }
  },
  args
) => {
  const listId = +args.params['listId']
  if (listId === auth.user.wishListId) {
    redirect('/')
  } else if (!some(invitations.sharedLists, list => list.wishListId === listId)) {
    redirect('/notfound')
  } else {
    setCurrentPage('sharedList')
    setCurrentSharedList(listId)
    clearMessages()
    await loadSharedList(listId)
    await loadMessages(listId)
    await subscribeList(listId, {
      onRefresh: () => loadSharedList(listId),
      onUpdateUsers,
      onMessageAdded
    })
  }
}

export const loadSharedList: Action<number> = (
  {
    state: {
      sharedLists,
      auth: { user }
    }
  },
  listId
) =>
  tryOrNotify(async () => {
    const result = await getJson<SharedListDto>('/api/SharedLists/' + listId)
    const normalized = normalizeArrays(result, ['wishesOrder'])
    const { users, ...sharedList } = normalized
    sharedLists.wishLists[sharedList.id] = sharedList
    sharedLists.users = { ...sharedLists.users, ...users, [user.id]: user }
  })

export const setBought: Action<{ wishId: number; isBought: boolean }> = (
  {
    state: {
      auth: { user },
      currentSharedList
    }
  },
  { wishId, isBought }
) =>
  tryOrNotify(async () => {
    if (!currentSharedList) {
      throw new Error('Ønskelisten er ikke lastet')
    }
    if (!user) {
      throw new Error('Du er ikke logget inn')
    }
    const listId = currentSharedList.id
    await putJson(`/api/SharedLists/${listId}/${wishId}/Bought`, { isBought })
    currentSharedList.wishes[wishId].boughtByUserId = isBought ? user.id : null
  })

export const shareWithCurrentOwner: Action = ({ state: { currentSharedList, currentSharedListOwner } }) =>
  tryOrNotify(async () => {
    if (!currentSharedList || !currentSharedListOwner) {
      throw new Error('Ønskelisten er ikke lastet')
    }
    if (confirm(`Du har ikke enda delt din liste med ${currentSharedListOwner.name}. Vil du gjøre det nå?`)) {
      await postJson(`/api/MyList/InviteUser`, { inviteUserId: currentSharedList.ownerUserId })
      currentSharedList.canSeeMyList = true
      showSuccess(`Din ønskeliste er nå delt med ${currentSharedListOwner.name}`)
    }
  })
