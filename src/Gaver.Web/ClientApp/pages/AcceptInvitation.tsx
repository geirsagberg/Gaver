import { Button, Typography, Avatar, makeStyles } from '@material-ui/core'
import React, { FC } from 'react'
import ErrorView from '~/components/ErrorView'
import Loading from '~/components/Loading'
import { useOvermind } from '~/overmind'
import { commonStyles } from '~/theme'

const useStyles = makeStyles({
  root: {
    ...commonStyles.centerContent,
  },
  avatar: {
    margin: '1rem',
    width: '6rem',
    height: '6rem',
  },
})

const AcceptInvitationPage: FC = () => {
  const classes = useStyles({})
  const {
    state: { invitations },
    actions: {
      invitations: { acceptInvitation },
    },
  } = useOvermind()
  return (
    <div className={classes.root}>
      {invitations.status ? (
        invitations.status.ok ? (
          <>
            <Typography variant="h3">{invitations.status.owner}s ønskeliste</Typography>
            <Avatar alt={invitations.status.owner} src={invitations.status.pictureUrl} className={classes.avatar} />
            <Button color="primary" variant="contained" onClick={acceptInvitation}>
              Godta invitasjon
            </Button>
          </>
        ) : (
          <ErrorView>{invitations.status.error}</ErrorView>
        )
      ) : (
        <Loading />
      )}
    </div>
  )
}

export default AcceptInvitationPage
