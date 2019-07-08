import React, { FC } from 'react'
import Loading from './components/Loading'
import { useOvermind } from './overmind'
import AcceptInvitationPage from './pages/AcceptInvitation'
import LoginPage from './pages/Login'
import MyListPage from './pages/MyList'
import NotFoundPage from './pages/NotFound'
import SharedListPage from './pages/SharedList'

export const Content: FC = () => {
  const {
    state: {
      routing: { currentPage }
    }
  } = useOvermind()
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
  }
  return <Loading />
}
