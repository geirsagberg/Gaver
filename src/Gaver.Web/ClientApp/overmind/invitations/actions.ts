import { InvitationDto, InvitationStatusDto } from '~/types/data'
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

export const handleInvitation: Action<RouteCallbackArgs> = ({ actions }, { params: { token } }) => {
  actions.routing.setCurrentPage('acceptInvitation')
  actions.invitations.checkInvitationStatus(token)
}

export const acceptInvitation: Action = ({
  state: { invitations },
  effects: {
    routing: { showSharedList }
  }
}) =>
  tryOrNotify(async () => {
    const invitation = await postJson<InvitationDto>(`/api/invitations/${invitations.token}/accept`)
    invitations.sharedLists.push(invitation)
    showSharedList(invitation.wishListId)
  })
