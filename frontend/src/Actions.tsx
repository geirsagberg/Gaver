import { Icon, IconButton } from '@mui/material'
import React, { FC } from 'react'
import { useOvermind } from './overmind'

// TODO: Move to NavContext
export const Actions: FC = () => {
  const {
    state: {
      routing: { currentPage },
      myList: { isDeleting },
    },
    actions: {
      myList: { startSharingList, toggleDeleting },
      chat: { toggleChat },
    },
  } = useOvermind()
  switch (currentPage) {
    case 'myList':
      return <>
        <IconButton color="inherit" onClick={toggleDeleting} size="large">
          <Icon>{isDeleting ? 'close' : 'delete'}</Icon>
        </IconButton>
        <IconButton color="inherit" onClick={startSharingList} size="large">
          <Icon>share</Icon>
        </IconButton>
      </>;
    case 'sharedList':
      return <>
        <IconButton color="inherit" onClick={toggleChat} size="large">
          <Icon>chat</Icon>
        </IconButton>
      </>;
  }
  return null
}
