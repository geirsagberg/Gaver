import { Action } from '..'
import { Wish } from '~/types/data'
import { tryOrNotify } from '~/utils'
import { postJson } from '~/utils/ajax'

export const addWish: Action<Wish> = ({ state }, newWish) =>
  tryOrNotify(async () => {
    const wish = await postJson<Wish>('/api/WishList', newWish)
    state.myList.wishes[wish.id] = wish
  })
