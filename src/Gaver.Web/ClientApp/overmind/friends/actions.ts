import { tryOrNotify } from '~/utils'
import { normalizeArrays } from '~/utils/normalize'
import { AsyncAction } from '..'

export const loadFriends: AsyncAction = ({
  state: { friends },
  effects: {
    friends: { getFriends }
  }
}) =>
  tryOrNotify(async () => {
    const dtos = await getFriends()
    const users = normalizeArrays(dtos)
    friends.users = users
  })
