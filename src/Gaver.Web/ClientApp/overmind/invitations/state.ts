import { InvitationDto, InvitationStatusDto } from '~/types/data'

export interface InvitationsState {
  token?: string
  status?: InvitationStatusDto
  sharedLists: InvitationDto[]
}

const state: InvitationsState = {
  sharedLists: []
}

export default state
