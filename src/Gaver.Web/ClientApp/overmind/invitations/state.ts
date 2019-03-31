import { InvitationTokenStatus } from '~/types/data'

export interface InvitationsState {
  token?: string
  status?: InvitationTokenStatus
}

const state: InvitationsState = {}

export default state
