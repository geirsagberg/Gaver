import { AppBar, Icon, IconButton, Menu, MenuItem, Toolbar, Typography } from '@material-ui/core'
import { makeStyles } from '@material-ui/styles'
import React, { FC, useState } from 'react'
import { hot } from 'react-hot-loader/root'
import Expander from './components/Expander'
import { useOvermind } from './overmind'
import LoginPage from './pages/Login'
import MyListPage from './pages/MyList'
import NotFoundPage from './pages/NotFound'
import { ShareListDialog } from './ShareListDialog'

export const useStyles = makeStyles({
  root: {
    height: '100%'
  },
  content: {
    display: 'flex',
    alignItems: 'flex-start',
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
      <IconButton color="inherit" onClick={showProfileMenu}>
        <Icon>account_circle</Icon>
      </IconButton>
      <Menu anchorEl={menuAnchorEl} open={!!menuAnchorEl} onClose={hideProfileMenu}>
        <MenuItem onClick={logOut}>Logg ut</MenuItem>
      </Menu>
    </>
  ) : null
}

const Layout: FC = () => {
  const classes = useStyles()
  const {
    actions: {
      myList: { startSharingList }
    }
  } = useOvermind()

  return (
    <div className={classes.root}>
      <AppBar>
        <Toolbar>
          <Typography variant="h6" color="inherit" style={{ marginRight: '1rem' }}>
            Gaver
          </Typography>
          <Expander />
          <IconButton color="inherit" onClick={startSharingList}>
            <Icon>share</Icon>
          </IconButton>
          <LoggedInAvatar />
        </Toolbar>
      </AppBar>
      <div className={classes.content}>
        <Content />
      </div>
      <ShareListDialog />
      <div id="portal-overlay" style={{ position: 'relative', zIndex: 1100 }} />
    </div>
  )
}

export default hot(Layout)
