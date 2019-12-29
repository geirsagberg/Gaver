import { Action } from '..'
import { tryOrNotify } from '~/utils'
import { normalizeArrays } from '~/utils/normalize'

export const loadFriends: Action = ({
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
