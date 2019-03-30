import { OnInitialize } from '..'
import * as actions from './actions'
import * as effects from './effects'
import { state } from './state'

const onInitialize: OnInitialize = ({
  actions: {
    routing: { setCurrentPage },
    auth: { handleAuthentication, handleInvitation }
  },
  effects: {
    routing: { route, start }
  }
}) => {
  route('/', () => setCurrentPage('start'))
  route('/callback', handleAuthentication)
  route('/invitation/:token', handleInvitation)
  route('*', () => setCurrentPage('notFound'))
  start()
}

export { actions, state, effects, onInitialize }
