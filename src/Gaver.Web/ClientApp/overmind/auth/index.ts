import * as actions from './actions'
import { state } from './state'
import { OnInitialize } from '..'

const onInitialize: OnInitialize = ({ actions: { auth } }) => {
  return auth.checkSession()
}

export default { actions, state, onInitialize }
