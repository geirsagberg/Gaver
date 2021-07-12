import { InvitationStatusDto, UserDto } from '~/types/data'
import { tryOrNotify } from '~/utils'
import { getJson, postJson } from '~/utils/ajax'
import { Context } from '..'
import { RouteCallbackArgs } from '../routing/effects'

export const checkInvitationStatus = (
  { state: { invitations } }: Context,
  token: string
) =>
  tryOrNotify(async () => {
    const status = await getJson<InvitationStatusDto>(
      `/api/invitations/${token}/status`
    )
    invitations.status = status
    invitations.token = token
  })

export const handleInvitation = (
  { actions }: Context,
  { params: { token } }: RouteCallbackArgs
) => {
  actions.routing.setCurrentPage('acceptInvitation')
  actions.invitations.checkInvitationStatus(token)
}

export const acceptInvitation = ({
  state: { invitations, friends },
  effects: {
    routing: { showSharedList },
  },
}: Context) =>
  tryOrNotify(async () => {
    const friend = await postJson<UserDto>(
      `/api/invitations/${invitations.token}/accept`
    )
    friends.users[friend.id] = friend
    showSharedList(friend.wishListId)
  })
