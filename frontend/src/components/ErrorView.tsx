import React, { FC } from 'react'
import { colors, Typography, Icon, Button, makeStyles } from '@material-ui/core'

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
  content: {
    margin: '1rem',
  },
})

interface Props {
  onBackClicked?: () => any
}

const ErrorView: FC<Props> = ({ children, onBackClicked }) => {
  const classes = useStyles({})
  return (
    <div className={classes.root}>
      <Typography variant="h1">Oisann!</Typography>
      <Icon className={classes.icon}>sentiment_very_dissatisfied</Icon>
      <Typography className={classes.content}>{children}</Typography>
      <Button href="/" color="primary" variant="contained" onClick={onBackClicked}>
        Tilbake
      </Button>
    </div>
  )
}

export default ErrorView
