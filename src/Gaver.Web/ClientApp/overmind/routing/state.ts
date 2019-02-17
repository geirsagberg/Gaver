export type Page = 'start' | 'myList' | 'authCallback'

export type PageMap = { [page in Page]: string }

export interface RoutingState {
  currentPage: Page
}

export const state: RoutingState = {
  currentPage: 'start'
}
