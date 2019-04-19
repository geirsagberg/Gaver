export type Page = 'start' | 'authCallback' | 'notFound' | 'acceptInvitation' | 'sharedList' | 'myList'

export interface RoutingState {
  currentPage: Page
}

export const state: RoutingState = {
  currentPage: 'start'
}
