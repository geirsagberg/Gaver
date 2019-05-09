import * as actions from './actions'
import * as effects from './effects'
import { state } from './state'
import { OnInitialize } from 'overmind'

const onInitialize: OnInitialize = ({ actions: { auth } }) => {
  return auth.checkSession()
}

export default { actions, effects, state, onInitialize }
