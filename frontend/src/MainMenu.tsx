import { Divider, Icon, Link, List, ListItem, ListItemIcon, ListItemText, SwipeableDrawer } from '@mui/material'
import { ThemeProvider } from '@mui/material/styles'
import { map, some } from 'lodash-es'
import Expander from './components/Expander'
import { useActions, useAppState, useEffects } from './overmind'
import { darkTheme } from './theme'
import { useFeatures } from './utils/appSettings'

const SharedListsMenuItem = () => {
  const {
    routing: { currentSharedListId },
    friends: { users },
  } = useAppState()
  const {
    app: { hideMenu },
  } = useActions()
  const {
    routing: { showSharedList },
  } = useEffects()
  return some(users) ? (
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
  ) : null
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
    auth: { logOut },
  } = useActions()
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
    app: { hideMenu, showFeedback },
  } = useActions()

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
    routing: { currentPage },
  } = useAppState()
  const {
    app: { hideMenu },
  } = useActions()
  const {
    routing: { showMyList },
  } = useEffects()
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
    routing: { currentPage },
  } = useAppState()
  const {
    app: { hideMenu },
  } = useActions()
  const {
    routing: { showUserGroups },
  } = useEffects()
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

export const MainMenu = () => {
  const {
    app: { isMenuShowing },
  } = useAppState()
  const {
    app: { showMenu, hideMenu },
  } = useActions()

  const features = useFeatures()

  return (
    <ThemeProvider theme={darkTheme}>
      <SwipeableDrawer
        SwipeAreaProps={{
          sx: {
            marginTop: 56,
          },
        }}
        open={isMenuShowing}
        onOpen={showMenu}
        onClose={hideMenu}
        PaperProps={{
          sx: (theme) => ({
            background: theme.palette.primary.dark,
          }),
        }}>
        <List
          sx={{
            width: 256,
            display: 'flex',
            flexDirection: 'column',
            height: '100%',
          }}>
          <MyListMenuItem />
          {features?.userGroups ? <MyGroupsMenuItem /> : null}
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
