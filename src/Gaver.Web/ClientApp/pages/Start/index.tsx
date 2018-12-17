import { Button, colors, createStyles, Typography, WithStyles, withStyles } from '@material-ui/core'
import classNames from 'classnames'
import React, { Component } from 'react'
import { connect } from 'react-redux'
import { login } from '~/store/auth/thunks'
import { createMapDispatchToProps } from '~/utils/reduxUtils'

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

const mapDispatchToProps = createMapDispatchToProps({
  login
})

type Props = WithStyles<typeof styles> & ReturnType<typeof mapDispatchToProps>

class StartPage extends Component<Props> {
  logIn = () => this.props.login()

  render() {
    const { classes } = this.props
    return (
      <div className={classes.root}>
        <Typography variant="h1">Gaver</Typography>
        <Button color="primary" variant="contained" className={classes.loginButton} onClick={this.logIn}>
          Logg inn
        </Button>
        <span className={classNames('icon-gift', classes.icon)} />
      </div>
    )
  }
}

export default connect(
  null,
  mapDispatchToProps
)(withStyles(styles)(StartPage))
