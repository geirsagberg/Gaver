import React from 'react'
import Loading from './components/Loading'
import { useAppState } from './overmind'
const LoginPage = React.lazy(() => import('./pages/Login'))
const MyListPage = React.lazy(() => import('./pages/MyList'))
const NotFoundPage = React.lazy(() => import('./pages/NotFound'))
const SharedListPage = React.lazy(() => import('./pages/SharedList'))
const UserGroupsPage = React.lazy(() => import('./pages/UserGroups'))

const AcceptInvitationPage = React.lazy(
  () => import('./pages/AcceptInvitation')
)

export const Content = () => {
  const {
    routing: { currentPage },
  } = useAppState()
  switch (currentPage) {
    case 'myList':
      return <MyListPage />
    case 'start':
      return <LoginPage />
    case 'notFound':
      return <NotFoundPage />
    case 'acceptInvitation':
      return <AcceptInvitationPage />
    case 'sharedList':
      return <SharedListPage />
    case 'userGroups':
      return <UserGroupsPage />
  }
  return <Loading />
}
