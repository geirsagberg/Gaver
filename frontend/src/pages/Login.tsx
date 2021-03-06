import { Button, colors, Typography } from '@material-ui/core'
import { makeStyles } from '@material-ui/styles'
import classNames from 'classnames'
import React, { FC } from 'react'
import Loading from '~/components/Loading'
import { useActions, useAppState } from '~/overmind'

const useStyles = makeStyles({
  root: {
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'center',
    alignSelf: 'center',
  },
  icon: {
    color: colors.amber[600],
    fontSize: 80,
  },
  loginButton: {
    margin: '1rem 0 2rem',
  },
})

const LoginPage: FC = () => {
  const classes = useStyles({})
  const {
    auth: { isLoggingIn },
  } = useAppState()
  const {
    auth: { logIn },
  } = useActions()

  return isLoggingIn ? (
    <Loading />
  ) : (
    <div className={classes.root}>
      <Typography variant="h1">Gaver</Typography>
      <Typography variant="subtitle1">Lag og del din ønskeliste</Typography>
      <Button
        color="primary"
        variant="contained"
        className={classes.loginButton}
        onClick={logIn}>
        Logg inn
      </Button>
      <span className={classNames('icon-gift', classes.icon)} />
    </div>
  )
}

export default LoginPage
