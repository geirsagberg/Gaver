import {
  AppBar,
  createStyles,
  Toolbar,
  Typography,
  withStyles,
  WithStyles,
  Button,
  ButtonBase
} from '@material-ui/core'
import React, { Component } from 'react'
import { hot } from 'react-hot-loader'
import { connect } from 'react-redux'
import MyList from '~/pages/MyList'
import StartPage from '~/pages/Start'
import { RouteType } from '~/routing'
import { AppStateWithRouting } from '~/store'
import { checkSession, logout } from './store/auth/thunks'
import { createMapDispatchToProps } from './utils/reduxUtils'
import Loading from './components/Loading'
import Expander from './components/Expander'

const styles = createStyles({
  root: {
    height: '100%'
  },
  content: {
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'center',
    height: '100%',
    paddingTop: '4rem'
  }
})

const mapStateToProps = (state: AppStateWithRouting) => ({
  routing: state.routing,
  auth: state.auth
})

const mapDispatchToProps = createMapDispatchToProps({
  checkSession,
  logout
})

type Props = WithStyles<typeof styles> & ReturnType<typeof mapStateToProps> & ReturnType<typeof mapDispatchToProps>

class Layout extends Component<Props> {
  componentDidMount() {
    console.log('componentDidMount')
    this.props.checkSession()
  }
  getContent() {
    if (this.props.auth.isLoggingIn) {
      return <Loading />
    }
    switch (this.props.routing.type) {
      case RouteType.FrontPage:
        return this.props.auth.isLoggedIn ? <MyList /> : <StartPage />
    }
  }
  render() {
    const { classes, auth } = this.props

    const Content = this.getContent()
    return (
      <div className={classes.root}>
        <AppBar>
          <Toolbar>
            <Typography variant="h6" color="inherit">
              Gaver
            </Typography>
            <Expander />
            {auth.isLoggedIn && <ButtonBase onClick={this.props.logout}>{auth.user.name}</ButtonBase>}
          </Toolbar>
        </AppBar>
        <div className={classes.content}>{Content}</div>
      </div>
    )
  }
}

export default hot(module)(
  connect(
    mapStateToProps,
    mapDispatchToProps
  )(withStyles(styles)(Layout))
)
