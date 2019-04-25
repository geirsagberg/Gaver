import { SharedListModel } from '~/types/data'
import { tryOrNotify } from '~/utils'
import { getJson } from '~/utils/ajax'
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
    state: { auth },
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
