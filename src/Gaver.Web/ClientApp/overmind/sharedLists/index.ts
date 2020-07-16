import * as actions from './actions'
import { state } from './state'
import * as effects from './effects'
import { useOvermind } from '..'

export default { actions, state, effects }

export const useFriends = () => {
  const {
    state: {
      friends: { users },
    },
  } = useOvermind()
  return users
}
