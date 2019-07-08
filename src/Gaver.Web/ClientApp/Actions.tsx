import { Icon, IconButton } from '@material-ui/core'
import React, { FC } from 'react'
import { useOvermind } from './overmind'

export const Actions: FC = () => {
  const {
    state: {
      routing: { currentPage },
      myList: { isDeleting }
    },
    actions: {
      myList: { startSharingList, toggleDeleting },
      chat: { toggleChat }
    }
  } = useOvermind()
  switch (currentPage) {
    case 'myList':
      return (
        <>
          <IconButton color="inherit" onClick={toggleDeleting}>
            <Icon>{isDeleting ? 'close' : 'delete'}</Icon>
          </IconButton>
          <IconButton color="inherit" onClick={startSharingList}>
            <Icon>share</Icon>
          </IconButton>
        </>
      )
    case 'sharedList':
      return (
        <>
          <IconButton color="inherit" onClick={toggleChat}>
            <Icon>chat</Icon>
          </IconButton>
        </>
      )
  }
  return null
}
