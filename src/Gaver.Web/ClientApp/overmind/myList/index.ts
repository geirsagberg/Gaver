import { useOvermind } from '..'
import * as actions from './actions'
import { state } from './state'

export const useMyList = () => {
  const { state, actions } = useOvermind()
  return {
    state: state.myList,
    actions: actions.myList
  }
}

export default { state, actions }
