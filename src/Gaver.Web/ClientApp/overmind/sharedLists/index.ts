import * as actions from './actions'
import { state } from './state'
import * as effects from './effects'
import { useOvermind } from '..'

export default { actions, state, effects }

export const useUsers = () => {
  const {
    state: {
      sharedLists: { users }
    }
  } = useOvermind()
  return users
}
