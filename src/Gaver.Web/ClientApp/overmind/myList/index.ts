import { state } from './state'
import * as actions from './actions'
import * as effects from './effects'
import { useOvermind } from '..'

export { state, actions, effects }

export const useMyList = () => {
  const { state, actions, effects } = useOvermind()
  return {
    state: state.myList,
    actions: actions.myList,
    effects: effects.myList
  }
}
