import { OnInitialize } from '..'
import * as actions from './actions'
import * as effects from './effects'
import { state } from './state'

const onInitialize: OnInitialize = ({
  actions: {
    routing: { setCurrentPage },
    auth: { handleAuthentication, requireLogin },
    invitations: { handleInvitation },
    sharedList: { handleSharedList }
  },
  effects: {
    routing: { route, start }
  }
}) => {
  route('/', () => setCurrentPage('start'))
  route('/callback', handleAuthentication)
  route('/invitations/:token', requireLogin, handleInvitation)
  route('/list/:listId', requireLogin, handleSharedList)
  route('*', () => setCurrentPage('notFound'))
  start()
}

export default { actions, state, effects, onInitialize }
