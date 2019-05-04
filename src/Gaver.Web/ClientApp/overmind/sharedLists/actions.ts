import { some } from 'lodash-es'
import { SharedListModel } from '~/types/data'
import { tryOrNotify } from '~/utils'
import { getJson, putJson } from '~/utils/ajax'
import { normalizeArrays } from '~/utils/normalize'
import { Action } from '..'
import { RouteCallbackArgs } from '../routing/effects'

export const handleSharedList: Action<RouteCallbackArgs> = async (
  {
    actions: {
      routing: { setCurrentPage, setCurrentSharedList },
      sharedLists: { loadSharedList },
      auth: { checkSession }
    },
    state: { auth, invitations },
    effects: {
      routing: { redirect }
    }
  },
  args
) => {
  const listId = +args.params['listId']
  await checkSession()
  if (listId === auth.user.wishListId) {
    redirect('/')
  } else if (!some(invitations.sharedLists, list => list.wishListId === listId)) {
    redirect('/notfound')
  } else {
    setCurrentPage('sharedList')
    setCurrentSharedList(listId)
    await loadSharedList(listId)
  }
}

export const loadSharedList: Action<number> = ({ state: { sharedLists } }, listId) =>
  tryOrNotify(async () => {
    const result = await getJson<SharedListModel>('/api/WishLists/' + listId)
    const normalized = normalizeArrays(result)
    const { users, ...sharedList } = normalized
    sharedLists.wishLists[sharedList.id] = sharedList
    sharedLists.users = { ...sharedLists.users, ...users }
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
    await putJson(`/api/WishLists/${listId}/${wishId}/Bought`, { isBought })
    currentSharedList.wishes[wishId].boughtByUserId = isBought ? user.id : null
  })
