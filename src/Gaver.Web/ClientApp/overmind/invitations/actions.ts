import { AcceptInvitationResponse, InvitationTokenStatus } from '~/types/data'
import { tryOrNotify } from '~/utils'
import { getJson, postJson } from '~/utils/ajax'
import { Action } from 'overmind'
import { RouteCallbackArgs } from '../routing/effects'

export const checkInvitationStatus: Action<string> = ({ state: { invitations } }, token) =>
  tryOrNotify(async () => {
    const status = await getJson<InvitationTokenStatus>(`/api/invitations/${token}/status`)
    invitations.status = status
    invitations.token = token
  })

export const handleInvitation: Action<RouteCallbackArgs> = ({ actions }, { params: { token } }) => {
  actions.routing.setCurrentPage('acceptInvitation')
  actions.invitations.checkInvitationStatus(token)
}

export const acceptInvitation: Action = ({
  state: {
    invitations: { token }
  },
  effects: {
    routing: { showSharedList }
  }
}) =>
  tryOrNotify(async () => {
    const { wishListId } = await postJson<AcceptInvitationResponse>(`/api/invitations/${token}/accept`)
    showSharedList(wishListId)
  })
