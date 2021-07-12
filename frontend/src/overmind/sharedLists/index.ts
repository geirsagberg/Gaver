import { useAppState } from '..'
import * as actions from './actions'
import * as effects from './effects'
import { state } from './state'

export default { actions, state, effects }

export const useFriends = () => {
  const {
    friends: { users },
  } = useAppState()
  return users
}
