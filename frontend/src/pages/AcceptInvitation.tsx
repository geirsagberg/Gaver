import { Avatar, Button, Typography } from '@mui/material'
import React, { FC } from 'react'
import { Center } from '~/components'
import Loading from '~/components/Loading'
import { useActions, useAppState } from '~/overmind'

const AcceptInvitationPage: FC = () => {
  const { invitations } = useAppState()
  const {
    invitations: { acceptInvitation },
  } = useActions()
  return (
    <Center>
      {invitations.status ? (
        <>
          <Typography variant="h3">{invitations.status.owner}s ønskeliste</Typography>
          <Avatar
            sx={{ margin: '1rem', width: '6rem', height: '6rem' }}
            alt={invitations.status.owner}
            src={invitations.status.pictureUrl}
          />
          <Button color="primary" variant="contained" onClick={acceptInvitation}>
            Godta invitasjon
          </Button>
        </>
      ) : (
        <Loading />
      )}
    </Center>
  )
}

export default AcceptInvitationPage
