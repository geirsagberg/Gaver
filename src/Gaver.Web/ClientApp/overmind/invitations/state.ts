import { FriendDto, InvitationStatusDto } from '~/types/data'

export interface InvitationsState {
  token?: string
  status?: InvitationStatusDto
  sharedLists: FriendDto[]
}

const state: InvitationsState = {
  sharedLists: []
}

export default state
