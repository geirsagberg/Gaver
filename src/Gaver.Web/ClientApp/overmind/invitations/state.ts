import { InvitationStatusDto } from '~/types/data'

export interface InvitationsState {
  token?: string
  status?: InvitationStatusDto
}

const state: InvitationsState = {}

export default state
