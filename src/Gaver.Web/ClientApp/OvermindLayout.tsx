import { AppBar, Avatar, ButtonBase, Menu, MenuItem, Toolbar, Typography } from '@material-ui/core'
import { makeStyles } from '@material-ui/styles'
import React, { FC, useState } from 'react'
import { hot } from 'react-hot-loader'
import Expander from './components/Expander'
import { useOvermind } from './overmind'
import LoginPage from './pages/Login'
import MyListPage from './pages/MyList'

const useStyles = makeStyles({
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

type Props = {}

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
  }
  return null
}

const Layout: FC<Props> = () => {
  const classes = useStyles()
  const {
    state: { auth },
    actions: {
      auth: { logOut }
    }
  } = useOvermind()
  const [menuAnchorEl, setMenuAnchorEl] = useState<HTMLElement>(null)

  const showProfileMenu = (event: React.MouseEvent<HTMLElement>) => setMenuAnchorEl(event.currentTarget)

  const hideProfileMenu = () => setMenuAnchorEl(null)

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
              <ButtonBase onClick={showProfileMenu}>
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
              <Menu anchorEl={menuAnchorEl} open={!!menuAnchorEl} onClose={hideProfileMenu}>
                <MenuItem onClick={logOut}>Logg ut</MenuItem>
              </Menu>
            </>
          )}
        </Toolbar>
      </AppBar>
      <div className={classes.content}>
        <Content />
      </div>
      <div id="portal-overlay" style={{ position: 'relative', zIndex: 1100 }} />
    </div>
  )
}

export default hot(module)(Layout)
