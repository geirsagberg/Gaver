import {
  AppBar,
  Avatar,
  ButtonBase,
  Menu,
  MenuItem,
  Toolbar,
  Typography,
  createStyles,
  WithStyles,
  withStyles
} from '@material-ui/core'
import React, { Component } from 'react'
import { hot } from 'react-hot-loader'
import { connect } from 'react-redux'
import MyList from '~/pages/MyList'
import LoginPage from '~/pages/Login'
import { RouteType } from '~/routing'
import { AppStateWithRouting } from '~/store'
import Expander from './components/Expander'
import Loading from './components/Loading'
import { checkSession, logout } from './store/auth/thunks'
import { createMapDispatchToProps } from './utils/reduxUtils'

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
  },
  profileImage: {
    width: '2rem',
    height: '2rem',
    borderRadius: '50%',
    backgroundSize: 'cover',
    backgroundRepeat: 'no-repeat',
    backgroundPosition: 'center center'
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

type Props = ReturnType<typeof mapStateToProps> & ReturnType<typeof mapDispatchToProps> & WithStyles<typeof styles>

type State = {
  menuAnchorEl?: HTMLElement
}

class Layout extends Component<Props, State> {
  state: State = {}
  componentDidMount() {
    this.props.checkSession()
  }
  getContent() {
    if (this.props.auth.isLoggingIn) {
      return <Loading />
    }
    switch (this.props.routing.type) {
      case RouteType.FrontPage:
        return this.props.auth.isLoggedIn ? <MyList /> : <LoginPage />
    }
  }

  showProfileMenu = (event: React.MouseEvent<HTMLElement>) =>
    this.setState({
      menuAnchorEl: event.currentTarget
    })

  hideProfileMenu = () =>
    this.setState({
      menuAnchorEl: null
    })

  render() {
    const { auth, classes } = this.props

    const Content = this.getContent()
    return (
      <div className={classes.root}>
        <AppBar>
          <Toolbar>
            <Typography variant="h6" color="inherit">
              Gaver
            </Typography>
            <Expander />
            {auth.isLoggedIn && (
              <>
                <ButtonBase onClick={this.showProfileMenu}>
                  {auth.user.pictureUrl ? (
                    <Avatar src={auth.user.pictureUrl} />
                  ) : (
                    <Avatar>
                      {auth.user.name
                        .split(' ')
                        .map(s => (s.length > 0 ? s[0] : ''))
                        .join('')
                        .substr(0, 2)}
                    </Avatar>
                  )}
                </ButtonBase>
                <Menu
                  anchorEl={this.state.menuAnchorEl}
                  open={!!this.state.menuAnchorEl}
                  onClose={this.hideProfileMenu}>
                  <MenuItem onClick={this.props.logout}>Logg ut</MenuItem>
                </Menu>
              </>
            )}
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
