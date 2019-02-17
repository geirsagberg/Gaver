import * as actions from './actions'
import * as effects from './effects'
import { state } from './state'
import { OnInitialize } from '..'

const onInitialize: OnInitialize = ({ actions: { auth } }) => {
  return auth.checkSession()
}

export { actions, effects, state, onInitialize }
