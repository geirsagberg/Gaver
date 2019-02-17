import * as actions from './actions'
import * as effects from './effects'
import { state } from './state'
import { OnInitialize } from '..'

const onInitialize: OnInitialize = ({
  actions: {
    routing: { showStartPage },
    auth: { handleAuthentication }
  },
  effects: {
    routing: { route, start }
  }
}) => {
  route('/', showStartPage)
  route('/callback', handleAuthentication)
  start()
}

export { actions, state, effects, onInitialize }
