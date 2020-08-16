import { Divider, Icon, Link, List, ListItem, ListItemIcon, ListItemText, SwipeableDrawer } from '@material-ui/core'
import { ThemeProvider, makeStyles } from '@material-ui/core/styles'
import { map, some } from 'lodash-es'
import React, { FC } from 'react'
import Expander from './components/Expander'
import { useOvermind } from './overmind'
import { darkTheme } from './theme'
import AppSettings from './utils/appSettings'
import { css } from './css'

const useStyles = makeStyles((theme) => ({
  menuPaper: {
    background: theme.palette.primary.dark,
  },
  menu: {
    width: 256,
    display: 'flex',
    flexDirection: 'column',
    height: '100%',
  },
}))

const SharedListsMenuItem: FC = () => {
  const {
    state: {
      routing: { currentSharedListId },
      friends: { users },
    },
    actions: {
      app: { hideMenu },
    },
    effects: {
      routing: { showSharedList },
    },
  } = useOvermind()
  return (
    some(users) && (
      <>
        <ListItem>
          <ListItemText secondary="Delte lister" />
        </ListItem>
        <Divider />
        {map(users, (user) => (
          <ListItem
            key={user.wishListId}
            button
            selected={currentSharedListId === user.wishListId}
            onClick={() => {
              showSharedList(user.wishListId)
              hideMenu()
            }}>
            <ListItemText primary={user.name} />
          </ListItem>
        ))}
      </>
    )
  )
}

const LicensesMenuItem = () => (
  <ListItem button href="/dist/licenses.txt" component={Link} target="_blank" color="inherit">
    <ListItemIcon>
      <Icon>copyright</Icon>
    </ListItemIcon>
    <ListItemText primary="Softwarelisenser" />
  </ListItem>
)

const LogOutMenuItem = () => {
  const {
    actions: {
      auth: { logOut },
    },
  } = useOvermind()
  return (
    <ListItem button onClick={logOut}>
      <ListItemIcon>
        <Icon>logout</Icon>
      </ListItemIcon>
      <ListItemText primary="Logg ut" />
    </ListItem>
  )
}

const FeedbackMenuItem = () => {
  const {
    state: {},
    actions: {
      app: { hideMenu, showFeedback },
    },
    effects: {},
  } = useOvermind()

  return (
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
  )
}

const MyListMenuItem = () => {
  const {
    state: {
      routing: { currentPage },
    },
    actions: {
      app: { hideMenu },
    },
    effects: {
      routing: { showMyList },
    },
  } = useOvermind()
  return (
    <ListItem
      button
      onClick={() => {
        showMyList()
        hideMenu()
      }}
      selected={currentPage === 'myList'}>
      <ListItemIcon>
        <Icon>home</Icon>
      </ListItemIcon>
      <ListItemText primary="Min liste" />
    </ListItem>
  )
}

export const MyGroupsMenuItem = () => {
  const {
    state: {
      routing: { currentPage },
    },
    actions: {
      app: { hideMenu },
    },
    effects: {
      routing: { showUserGroups },
    },
  } = useOvermind()
  return (
    <ListItem
      button
      onClick={() => {
        showUserGroups()
        hideMenu()
      }}
      selected={currentPage === 'userGroups'}>
      <ListItemIcon>
        <Icon>group</Icon>
      </ListItemIcon>
      <ListItemText primary="Mine grupper" />
    </ListItem>
  )
}

export const MainMenu: FC = () => {
  const classes = useStyles({})
  const {
    state: {
      app: { isMenuShowing },
    },
    actions: {
      app: { showMenu, hideMenu },
    },
    effects: {},
  } = useOvermind()
  return (
    <ThemeProvider theme={darkTheme}>
      <SwipeableDrawer
        SwipeAreaProps={{ className: css({ marginTop: '56px' }) }}
        open={isMenuShowing}
        onOpen={showMenu}
        onClose={hideMenu}
        classes={{ paper: classes.menuPaper }}>
        <List className={classes.menu}>
          <MyListMenuItem />
          {AppSettings.features.userGroups ? <MyGroupsMenuItem /> : null}
          <SharedListsMenuItem />
          <Divider />
          <Expander />
          <FeedbackMenuItem />
          <LogOutMenuItem />
          <LicensesMenuItem />
        </List>
      </SwipeableDrawer>
    </ThemeProvider>
  )
}
