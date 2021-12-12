import { InvitationStatusDto, UserDto } from '~/types/data'
import { tryOrNotify } from '~/utils'
import { getJson, postJson } from '~/utils/ajax'
import { showError } from '~/utils/notifications'
import { Context } from '..'
import { RouteCallbackArgs } from '../routing/effects'

export const handleInvitation = async (
  { actions, state, effects }: Context,
  { params: { token } }: RouteCallbackArgs
) => {
  try {
    const status = await getJson<InvitationStatusDto>(`/api/invitations/${token}/status`)
    const existingFriend = state.friends.users[status.ownerId]
    if (existingFriend) {
      effects.routing.showSharedList(existingFriend.wishListId)
    } else {
      state.invitations.status = status
      state.invitations.token = token
      actions.routing.setCurrentPage('acceptInvitation')
    }
  } catch (error) {
    showError(error as Error)
    effects.routing.showMyList()
  }
}

export const acceptInvitation = ({
  state: { invitations, friends },
  effects: {
    routing: { showSharedList },
  },
}: Context) =>
  tryOrNotify(async () => {
    const friend = await postJson<UserDto>(`/api/invitations/${invitations.token}/accept`)
    friends.users[friend.id] = friend
    showSharedList(friend.wishListId)
  })
