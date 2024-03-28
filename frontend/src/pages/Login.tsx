import { Button, colors, Typography } from '@mui/material'
import classNames from 'classnames'
import { Center } from '~/components'
import Loading from '~/components/Loading'
import { useActions, useAppState } from '~/overmind'

const LoginPage = () => {
  const {
    auth: { isLoggingIn },
  } = useAppState()
  const {
    auth: { logIn },
  } = useActions()

  return isLoggingIn ? (
    <Loading />
  ) : (
    <Center>
      <Typography variant="h1">Gaver</Typography>
      <Typography variant="subtitle1">Lag og del din ønskeliste</Typography>
      <Button
        color="primary"
        variant="contained"
        sx={{
          margin: '1rem 0 2rem',
        }}
        onClick={logIn}>
        Logg inn
      </Button>
      <span
        style={{
          color: colors.amber[600],
          fontSize: 80,
        }}
        className={classNames('icon-gift')}
      />
    </Center>
  )
}

export default LoginPage
