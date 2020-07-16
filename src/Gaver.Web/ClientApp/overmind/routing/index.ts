import { OnInitialize } from '..'
import * as actions from './actions'
import * as effects from './effects'
import { state } from './state'

const onInitialize: OnInitialize = ({
  actions: {
    routing: { setCurrentPage, handleStart, setCurrentSharedList },
    auth: { handleAuthentication, redirectIfNotLoggedIn, checkSession },
    invitations: { handleInvitation },
    sharedLists: { handleSharedList },
    myList: { handleMyList },
    userGroups: { handleUserGroups },
  },
  effects: {
    routing: { route, start, exit, enter },
  },
}) => {
  enter(async (_, next) => {
    await checkSession()
    next()
  })
  route('/', handleStart)
  route('/mylist', redirectIfNotLoggedIn, handleMyList)
  route('/callback', handleAuthentication)
  route('/invitations/:token', redirectIfNotLoggedIn, handleInvitation)
  route('/list/:listId', redirectIfNotLoggedIn, handleSharedList)
  route('/userGroups', redirectIfNotLoggedIn, handleUserGroups)
  route('*', () => setCurrentPage('notFound'))
  exit('/list/:listId', async (_, next) => {
    await setCurrentSharedList(null)
    next()
  })
  start()
}

export default { actions, state, effects, onInitialize }
