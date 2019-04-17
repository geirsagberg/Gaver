import { AppBar, Icon, IconButton, Menu, MenuItem, Toolbar, Tooltip, Typography } from '@material-ui/core'
import React, { FC, useState } from 'react'
import { hot } from 'react-hot-loader/root'
import Expander from './components/Expander'
import { useOvermind } from './overmind'
import AcceptInvitationPage from './pages/AcceptInvitation'
import LoginPage from './pages/Login'
import MyListPage from './pages/MyList'
import NotFoundPage from './pages/NotFound'
import SharedListPage from './pages/SharedList'
import { ShareListDialog } from './ShareListDialog'
import { createStylesHook } from './utils/materialUtils'

export const useStyles = createStylesHook(theme => ({
  root: {
    height: '100%',
    position: 'relative'
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
  }
}))

const Content: FC = () => {
  const {
    state: {
      routing: { currentPage },
      auth: { isLoggedIn }
    }
  } = useOvermind()

  switch (currentPage) {
    case 'start':
      return isLoggedIn ? <MyListPage /> : <LoginPage />
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
      auth: { isLoggedIn }
    },
    actions: {
      myList: { startSharingList }
    }
  } = useOvermind()

  switch (currentPage) {
    case 'start':
      return isLoggedIn ? (
        <IconButton color="inherit" onClick={startSharingList}>
          <Icon>share</Icon>
        </IconButton>
      ) : null
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
  const classes = useStyles()
  const {
    state: {
      auth: { isLoggedIn }
    }
  } = useOvermind()

  return (
    <div className={classes.root}>
      {isLoggedIn && (
        <AppBar>
          <Toolbar>
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
      <ShareListDialog />
      <div id="portal-overlay" style={{ position: 'relative', zIndex: 1100 }} />
    </div>
  )
}

export default hot(Layout)
