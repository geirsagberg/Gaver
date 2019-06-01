import { Invitation, InvitationTokenStatus } from '~/types/data'

export interface InvitationsState {
  token?: string
  status?: InvitationTokenStatus
  sharedLists: Invitation[]
}

const state: InvitationsState = {
  sharedLists: []
}

export default state
