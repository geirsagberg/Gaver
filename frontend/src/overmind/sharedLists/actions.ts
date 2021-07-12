import { some } from 'lodash-es'
import { SharedListDto } from '~/types/data'
import { tryOrNotify } from '~/utils'
import { getJson, putJson } from '~/utils/ajax'
import { normalizeArrays } from '~/utils/normalize'
import { Context } from '..'
import { RouteCallbackArgs } from '../routing/effects'
import { User } from './state'

export const onUpdateUsers = ({ state }: Context, users: Dictionary<User>) => {
  state.sharedLists.users = {
    ...state.sharedLists.users,
    ...users,
  }
}

export const handleSharedList = async (
  {
    actions,
    state: { auth, friends },
    effects: {
      routing: { redirect },
      sharedLists: { subscribeList },
    },
  }: Context,
  args: RouteCallbackArgs
) => {
  const {
    routing: { setCurrentPage, setCurrentSharedList },
    chat: { clearMessages, loadMessages, onMessageAdded },
    sharedLists: { loadSharedList, onUpdateUsers },
  } = actions
  const listId = +args.params['listId']
  if (listId === auth.user?.wishListId) {
    redirect('/')
  } else if (!some(friends.users, (u) => u.wishListId === listId)) {
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
      onMessageAdded,
    })
  }
}

export const loadSharedList = (
  {
    state: {
      sharedLists,
      auth: { user },
    },
  }: Context,
  listId: number
) =>
  tryOrNotify(async () => {
    if (user) {
      const result = await getJson<SharedListDto>('/api/SharedLists/' + listId)
      const normalized = normalizeArrays(result, ['wishesOrder'])
      const { users, ...sharedList } = normalized
      sharedLists.wishLists[sharedList.id] = sharedList
      sharedLists.users = { ...sharedLists.users, ...users, [user.id]: user }
    }
  })

export const setBought = (
  {
    state: {
      auth: { user },
      currentSharedList,
    },
  }: Context,
  { wishId, isBought }: { wishId: number; isBought: boolean }
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
    currentSharedList.wishes[wishId].boughtByUserId = isBought
      ? user.id
      : undefined
  })
