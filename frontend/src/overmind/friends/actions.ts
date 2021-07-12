import { tryOrNotify } from '~/utils'
import { normalizeArrays } from '~/utils/normalize'
import { Context } from '..'

export const loadFriends = ({
  state: { friends },
  effects: {
    friends: { getFriends },
  },
}: Context) =>
  tryOrNotify(async () => {
    const dtos = await getFriends()
    const users = normalizeArrays(dtos)
    friends.users = users
  })
