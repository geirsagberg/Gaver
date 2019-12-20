export type Page = 'start' | 'authCallback' | 'notFound' | 'acceptInvitation' | 'sharedList' | 'myList' | 'userGroups'

export type RoutingState = {
  currentPage?: Page
  currentSharedListId?: number
  currentUserGroupId?: number
}

export const state: RoutingState = {}
