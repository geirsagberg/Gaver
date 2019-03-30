import { Action } from '..'
import { Page } from './state'

export const setCurrentPage: Action<Page> = ({ state }, page) => {
  state.routing.currentPage = page
}
