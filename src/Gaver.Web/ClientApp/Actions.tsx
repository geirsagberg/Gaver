import { Icon, IconButton } from '@material-ui/core'
import React, { FC } from 'react'
import { useOvermind } from './overmind'

// TODO: Move to NavContext
export const Actions: FC = () => {
  const {
    state: {
      routing: { currentPage },
      myList: { isDeleting },
      currentSharedList,
      currentSharedListOwner,
      app: { isSavingOrLoading }
    },
    actions: {
      myList: { startSharingList, toggleDeleting },
      sharedLists: { shareWithCurrentOwner },
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
          {currentSharedList?.canSeeMyList === false && (
            <IconButton
              color="inherit"
              onClick={shareWithCurrentOwner}
              title={`Del din liste med ${currentSharedListOwner.name}`}
              disabled={isSavingOrLoading}
            >
              <Icon>share</Icon>
            </IconButton>
          )}
          <IconButton color="inherit" onClick={toggleChat}>
            <Icon>chat</Icon>
          </IconButton>
        </>
      )
  }
  return null
}
