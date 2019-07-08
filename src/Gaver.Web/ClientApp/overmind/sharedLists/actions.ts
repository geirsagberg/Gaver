import { some } from 'lodash-es'
import { SharedListModel, UserModel } from '~/types/data'
import { tryOrNotify } from '~/utils'
import { getJson, putJson } from '~/utils/ajax'
import { normalizeArrays } from '~/utils/normalize'
import { Action } from '..'
import { RouteCallbackArgs } from '../routing/effects'

export const onUpdateUsers: Action<Dictionary<UserModel>> = ({ state }, users) => {
  state.sharedLists.users = {
    ...state.sharedLists.users,
    users
  }
}

export const handleSharedList: Action<RouteCallbackArgs> = async (
  {
    actions: {
      routing: { setCurrentPage, setCurrentSharedList },
      sharedLists: { loadSharedList, onUpdateUsers },
      chat: { clearMessages, loadMessages }
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
      onUpdateUsers
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
    const result = await getJson<SharedListModel>('/api/SharedLists/' + listId)
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
