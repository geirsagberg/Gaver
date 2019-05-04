import { OnInitialize } from '..'
import * as actions from './actions'
import * as effects from './effects'
import { state } from './state'

const onInitialize: OnInitialize = ({
  actions: {
    routing: { setCurrentPage, handleStart, setCurrentSharedList },
    auth: { handleAuthentication, requireLogin },
    invitations: { handleInvitation },
    sharedLists: { handleSharedList },
    myList: { handleMyList }
  },
  effects: {
    routing: { route, start, exit }
  }
}) => {
  route('/', handleStart)
  route('/mylist', requireLogin, handleMyList)
  route('/callback', handleAuthentication)
  route('/invitations/:token', requireLogin, handleInvitation)
  route('/list/:listId', requireLogin, handleSharedList)
  route('*', () => setCurrentPage('notFound'))
  exit('/list/:listId', (_, next) => {
    setCurrentSharedList(null)
    next()
  })
  start()
}

export default { actions, state, effects, onInitialize }
