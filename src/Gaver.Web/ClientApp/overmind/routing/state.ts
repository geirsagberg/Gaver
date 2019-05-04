export type Page = 'start' | 'authCallback' | 'notFound' | 'acceptInvitation' | 'sharedList' | 'myList'

export type RoutingState = {
  currentPage: Page
  currentSharedListId?: number
}

export const state: RoutingState = {
  currentPage: 'start'
}
