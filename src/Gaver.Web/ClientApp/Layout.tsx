import {
  AppBar,
  Divider,
  Icon,
  IconButton,
  List,
  ListItem,
  ListItemIcon,
  ListItemText,
  Menu,
  MenuItem,
  SwipeableDrawer,
  Toolbar,
  Tooltip,
  Typography,
  Link
} from '@material-ui/core'
import classNames from 'classnames'
import { map, some } from 'lodash-es'
import React, { FC, useState } from 'react'
import Expander from './components/Expander'
import { useOvermind } from './overmind'
import AcceptInvitationPage from './pages/AcceptInvitation'
import LoginPage from './pages/Login'
import MyListPage from './pages/MyList'
import NotFoundPage from './pages/NotFound'
import SharedListPage from './pages/SharedList'
import { ShareListDialog } from './ShareListDialog'
import { darkTheme } from './theme'
import { hot } from 'react-hot-loader/root'
import { MuiThemeProvider, makeStyles } from '@material-ui/core/styles'

export const useStyles = makeStyles(theme => ({
  root: {
    height: '100%',
    position: 'relative',
    zIndex: 0
  },
  content: {
    display: 'flex',
    alignItems: 'flex-start',
    justifyContent: 'center',
    height: '100%',
    paddingTop: '4rem',
    [theme.breakpoints.down('xs')]: {
      paddingTop: '3.5rem'
    }
  },
  profileImage: {
    width: '2rem',
    height: '2rem',
    borderRadius: '50%',
    backgroundSize: 'cover',
    backgroundRepeat: 'no-repeat',
    backgroundPosition: 'center center'
  },
  menuIcon: {
    marginRight: '0.5rem'
  },
  toolbar: {
    padding: '0 0.5rem'
  },
  menuPaper: {
    background: theme.palette.primary.dark
  },
  menu: {
    width: 256
  },
  current: {}
}))

const Content: FC = () => {
  const {
    state: {
      routing: { currentPage }
    }
  } = useOvermind()

  switch (currentPage) {
    case 'myList':
      return <MyListPage />
    case 'start':
      return <LoginPage />
    case 'notFound':
      return <NotFoundPage />
    case 'acceptInvitation':
      return <AcceptInvitationPage />
    case 'sharedList':
      return <SharedListPage />
  }
  return null
}

const Actions: FC = () => {
  const {
    state: {
      routing: { currentPage },
      myList: { isDeleting }
    },
    actions: {
      myList: { startSharingList, toggleDeleting }
    }
  } = useOvermind()

  switch (currentPage) {
    case 'myList':
      return (
        <>
          <IconButton color="inherit" onClick={toggleDeleting}>
            <Icon>{isDeleting ? 'close' : 'delete'}</Icon>
          </IconButton>
          <IconButton color="inherit" onClick={startSharingList}>
            <Icon>share</Icon>
          </IconButton>
        </>
      )
  }
  return null
}

const LoggedInAvatar: FC = () => {
  const {
    state: { auth },
    actions: {
      auth: { logOut }
    }
  } = useOvermind()
  const [menuAnchorEl, setMenuAnchorEl] = useState<HTMLElement>(null)

  const showProfileMenu = (event: React.MouseEvent<HTMLElement>) => setMenuAnchorEl(event.currentTarget)

  const hideProfileMenu = () => setMenuAnchorEl(null)

  return auth.isLoggedIn ? (
    <>
      <Tooltip title={auth.user.name}>
        <IconButton color="inherit" onClick={showProfileMenu}>
          <Icon>account_circle</Icon>
        </IconButton>
      </Tooltip>

      <Menu anchorEl={menuAnchorEl} open={!!menuAnchorEl} onClose={hideProfileMenu}>
        <MenuItem onClick={logOut}>Logg ut</MenuItem>
      </Menu>
    </>
  ) : null
}

const Layout: FC = () => {
  const classes = useStyles({})
  const {
    state: {
      auth: { isLoggedIn },
      app: { isMenuShowing },
      routing: { currentPage, currentSharedListId },
      invitations: { sharedLists }
    },
    actions: {
      app: { showMenu, hideMenu }
    },
    effects: {
      routing: { showMyList, showSharedList }
    }
  } = useOvermind()

  return (
    <div className={classes.root}>
      {isLoggedIn && (
        <AppBar>
          <Toolbar disableGutters className={classes.toolbar}>
            <IconButton color="inherit" className={classes.menuIcon} onClick={showMenu}>
              <Icon>menu</Icon>
            </IconButton>
            <Typography variant="h6" color="inherit" style={{ marginRight: '1rem' }}>
              Gaver
            </Typography>
            <Expander />
            <Actions />
            <LoggedInAvatar />
          </Toolbar>
        </AppBar>
      )}
      <div className={classes.content}>
        <Content />
      </div>
      <MuiThemeProvider theme={darkTheme}>
        <SwipeableDrawer
          open={isMenuShowing}
          onOpen={showMenu}
          onClose={hideMenu}
          classes={{ paper: classes.menuPaper }}>
          <div className={classes.menu}>
            <List>
              <ListItem
                button
                onClick={() => {
                  showMyList()
                  hideMenu()
                }}
                selected={currentPage === 'myList'}
                className={classNames({ [classes.current]: currentPage === 'myList' })}>
                <ListItemIcon>
                  <Icon>home</Icon>
                </ListItemIcon>
                <ListItemText primary="Min liste" />
              </ListItem>
              {some(sharedLists) && (
                <>
                  <ListItem>
                    <ListItemText secondary="Delte lister" />
                  </ListItem>
                  <Divider />
                  {map(sharedLists, sharedList => (
                    <ListItem
                      key={sharedList.wishListId}
                      button
                      selected={currentSharedListId === sharedList.wishListId}
                      onClick={() => {
                        showSharedList(sharedList.wishListId)
                        hideMenu()
                      }}>
                      <ListItemText primary={sharedList.wishListUserName} />
                    </ListItem>
                  ))}
                </>
              )}
              <ListItem>
                <ListItemText primary="&nbsp;" />
              </ListItem>
              <Divider />
              <ListItem button href="/dist/licenses.txt" component={Link} target="_blank" color="inherit">
                <ListItemIcon>
                  <Icon>copyright</Icon>
                </ListItemIcon>
                <ListItemText primary="Softwarelisenser" />
              </ListItem>
            </List>
          </div>
        </SwipeableDrawer>
      </MuiThemeProvider>

      <ShareListDialog />
      <div id="portal-overlay" style={{ position: 'relative', zIndex: 1100 }} />
    </div>
  )
}

export default hot(Layout)
