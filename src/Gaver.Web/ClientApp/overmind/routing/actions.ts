import { Action } from '..'
import { Page } from './state'

export const setCurrentPage: Action<Page> = ({ state }, page) => {
  state.routing.currentPage = page
}

export const handleStart: Action = async ({
  state: { auth },
  actions: {
    routing: { setCurrentPage },
    auth: { checkSession }
  },
  effects: {
    routing: { redirect }
  }
}) => {
  await checkSession()
  if (auth.isLoggedIn) {
    redirect('/mylist')
  } else if (!auth.isLoggingIn) {
    setCurrentPage('start')
  }
}
