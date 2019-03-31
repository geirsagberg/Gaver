import React, { FC } from 'react'
import { colors, Typography, Icon, Button } from '@material-ui/core'
import { makeStyles } from '@material-ui/styles'

const useStyles = makeStyles({
  root: {
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'center',
    alignSelf: 'center'
  },
  icon: {
    color: colors.amber[600],
    fontSize: 80
  },
  content: {
    margin: '1rem'
  }
})

const ErrorView: FC = ({ children }) => {
  const classes = useStyles()
  return (
    <div className={classes.root}>
      <Typography variant="h1">Oisann!</Typography>
      <Icon className={classes.icon}>sentiment_very_dissatisfied</Icon>
      <Typography className={classes.content}>{children}</Typography>
      <Button href="/" color="primary" variant="contained">
        Tilbake
      </Button>
    </div>
  )
}

export default ErrorView
