import { Derive } from '..'
import { SharedList } from '../sharedLists/state'

export type Page = 'start' | 'authCallback' | 'notFound' | 'acceptInvitation' | 'sharedList' | 'myList'

export type RoutingState = {
  currentPage: Page
  currentSharedListId?: number
  currentSharedList?: Derive<RoutingState, SharedList>
}

export const state: RoutingState = {
  currentPage: 'start',
  currentSharedList: (state, rootState) => rootState.sharedLists.wishLists[state.currentSharedListId]
}
