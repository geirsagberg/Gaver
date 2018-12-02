import React, { Component } from 'react'
import { WithStyles, createStyles, colors, Typography, Button, withStyles } from '@material-ui/core'
import classNames from 'classnames'

const styles = createStyles({
  root: {
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'center'
  },
  icon: {
    color: colors.amber[600],
    fontSize: 80
  },
  loginButton: {
    margin: '1rem 0 2rem'
  }
})

type Props = WithStyles<typeof styles>

class StartPage extends Component<Props> {
  render() {
    const { classes } = this.props
    return (
      <div className={classes.root}>
        <Typography variant="h1">Gaver</Typography>
        <Button color="primary" variant="contained" className={classes.loginButton}>
          Logg inn
        </Button>
        <span className={classNames('icon-gift', classes.icon)} />
      </div>
    )
  }
}

export default withStyles(styles)(StartPage)
