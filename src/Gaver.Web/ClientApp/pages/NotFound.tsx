import React, { FC } from 'react'
import { makeStyles } from '@material-ui/styles'
import { Typography, colors, Icon, Button } from '@material-ui/core'

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
  }
})

const NotFoundPage: FC = () => {
  const classes = useStyles()
  return (
    <div className={classes.root}>
      <Typography variant="h1">Oisann!</Typography>
      <Icon className={classes.icon}>sentiment_very_dissatisfied</Icon>
      <Typography style={{ margin: `1rem` }}>Det ser ut til at siden du forsøkte å gå til, ikke finnes.</Typography>
      <Button href="/" color="primary" variant="contained">
        Tilbake
      </Button>
    </div>
  )
}

export default NotFoundPage
