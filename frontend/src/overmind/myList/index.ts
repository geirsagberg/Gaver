import { useActions, useAppState } from '..'
import * as actions from './actions'
import { state } from './state'

export const useMyList = () => {
  const state = useAppState()
  const actions = useActions()
  return {
    state: state.myList,
    actions: actions.myList,
  }
}

export default { state, actions }
