import { InvitationStatusDto, UserDto } from '~/types/data'
import { tryOrNotify } from '~/utils'
import { getJson, postJson } from '~/utils/ajax'
import { Action } from '..'
import { RouteCallbackArgs } from '../routing/effects'

export const checkInvitationStatus: Action<string> = ({ state: { invitations } }, token) =>
  tryOrNotify(async () => {
    const status = await getJson<InvitationStatusDto>(`/api/invitations/${token}/status`)
    invitations.status = status
    invitations.token = token
  })

export const handleInvitation: Action<RouteCallbackArgs> = (
  {
    actions: {
      routing: { setCurrentPage },
      invitations: { checkInvitationStatus }
    }
  },
  { params: { token } }
) => {
  setCurrentPage('acceptInvitation')
  checkInvitationStatus(token)
}

export const acceptInvitation: Action = ({
  state: { invitations, friends },
  effects: {
    routing: { showSharedList }
  }
}) =>
  tryOrNotify(async () => {
    const friend = await postJson<UserDto>(`/api/invitations/${invitations.token}/accept`)
    friends.users[friend.id] = friend
    showSharedList(friend.wishListId)
  })
