import type { Context } from '..'
import { Page } from './state'

export const setCurrentPage = ({ state }: Context, page: Page) => {
  state.routing.currentPage = page
}

export const setCurrentSharedList = (
  { state }: Context,
  listId: number | undefined
) => {
  state.routing.currentSharedListId = listId
}

export const handleStart = async ({
  state: { auth },
  actions,
  effects: {
    routing: { redirect },
  },
}: Context) => {
  if (auth.isLoggedIn) {
    redirect('/mylist')
  } else if (!auth.isLoggingIn) {
    actions.routing.setCurrentPage('start')
  }
}

export const onInitializeOvermind = ({
  actions,
  effects: {
    routing: { route, start, exit, enter },
  },
}: Context) => {
  const {
    auth: { checkSession, redirectIfNotLoggedIn, handleAuthentication },
    myList: { handleMyList },
    routing: { handleStart, setCurrentSharedList, setCurrentPage },
    invitations: { handleInvitation },
    sharedLists: { handleSharedList },
    userGroups: { handleUserGroups },
  } = actions
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
    setCurrentSharedList(undefined)
    next()
  })
  start()
}
