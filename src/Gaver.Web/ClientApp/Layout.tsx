import {
  AppBar,
  Divider,
  Icon,
  IconButton,
  Link,
  List,
  ListItem,
  ListItemIcon,
  ListItemText,
  SwipeableDrawer,
  Toolbar,
  Typography
} from '@material-ui/core'
import { makeStyles, MuiThemeProvider } from '@material-ui/core/styles'
import classNames from 'classnames'
import { map, some } from 'lodash-es'
import React, { FC } from 'react'
import { hot } from 'react-hot-loader/root'
import { Actions } from './Actions'
import Expander from './components/Expander'
import { Content } from './Content'
import FeedbackDialog from './FeedbackDialog'
import { useOvermind } from './overmind'
import { ShareListDialog } from './ShareListDialog'
import { darkTheme } from './theme'

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
    width: 256,
    display: 'flex',
    flexDirection: 'column',
    height: '100%'
  },
  current: {}
}))

const Layout: FC = () => {
  const classes = useStyles({})
  const {
    state: {
      auth: { isLoggedIn },
      app: { isMenuShowing, title },
      routing: { currentPage, currentSharedListId },
      invitations: { sharedLists }
    },
    actions: {
      app: { showMenu, hideMenu, showFeedback },
      auth: { logOut }
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
              {title ? title : 'Gaver'}
            </Typography>
            <Expander />
            <Actions />
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
          <List className={classes.menu}>
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
            <Divider />
            <Expander />
            <ListItem
              button
              onClick={() => {
                hideMenu()
                showFeedback()
              }}
              color="inherit">
              <ListItemIcon>
                <Icon>feedback</Icon>
              </ListItemIcon>
              <ListItemText primary="Gi tilbakemelding" />
            </ListItem>
            <ListItem button onClick={logOut}>
              <ListItemIcon>
                <Icon>logout</Icon>
              </ListItemIcon>
              <ListItemText primary="Logg ut" />
            </ListItem>
            <ListItem button href="/dist/licenses.txt" component={Link} target="_blank" color="inherit">
              <ListItemIcon>
                <Icon>copyright</Icon>
              </ListItemIcon>
              <ListItemText primary="Softwarelisenser" />
            </ListItem>
          </List>
        </SwipeableDrawer>
      </MuiThemeProvider>

      <ShareListDialog />
      <FeedbackDialog />
      <div id="portal-overlay" style={{ position: 'relative', zIndex: 1100 }} />
    </div>
  )
}

export default hot(Layout)
