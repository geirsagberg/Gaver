import { OnInitialize } from 'overmind'
import * as actions from './actions'
import * as effects from './effects'
import { state } from './state'

const onInitialize: OnInitialize = ({
  actions: {
    routing: { setCurrentPage, handleStart, handleMyList },
    auth: { handleAuthentication, requireLogin },
    invitations: { handleInvitation }
  },
  effects: {
    routing: { route, start, exit }
  }
}) => {
  route('/', handleStart)
  route('/mylist', requireLogin, handleMyList)
  route('/callback', handleAuthentication)
  route('/invitations/:token', requireLogin, handleInvitation)
  route('*', () => setCurrentPage('notFound'))
  exit('/list/:listId', (_, next) => {
    next()
  })
  start()
}

export default { actions, state, effects, onInitialize }
