import { Action } from '..'

export const showStartPage: Action = ({ state }) => {
  state.routing.currentPage = 'start'
}
