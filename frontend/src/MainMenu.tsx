import {
  Divider,
  Icon,
  Link,
  List,
  ListItem,
  ListItemButton,
  ListItemIcon,
  ListItemText,
  SwipeableDrawer,
} from '@mui/material'
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
        <ListItemButton
          key={user.wishListId}
          selected={currentSharedListId === user.wishListId}
          onClick={() => {
            showSharedList(user.wishListId)
            hideMenu()
          }}>
          <ListItemText primary={user.name} />
        </ListItemButton>
      ))}
    </>
  ) : null
}

const LicensesMenuItem = () => (
  <ListItemButton href="/dist/licenses.txt" component={Link} target="_blank" color="inherit">
    <ListItemIcon>
      <Icon>copyright</Icon>
    </ListItemIcon>
    <ListItemText primary="Softwarelisenser" />
  </ListItemButton>
)

const LogOutMenuItem = () => {
  const {
    auth: { logOut },
  } = useActions()
  return (
    <ListItemButton onClick={logOut}>
      <ListItemIcon>
        <Icon>logout</Icon>
      </ListItemIcon>
      <ListItemText primary="Logg ut" />
    </ListItemButton>
  )
}

const FeedbackMenuItem = () => {
  const {
    app: { hideMenu, showFeedback },
  } = useActions()

  return (
    <ListItemButton
      onClick={() => {
        hideMenu()
        showFeedback()
      }}
      color="inherit">
      <ListItemIcon>
        <Icon>feedback</Icon>
      </ListItemIcon>
      <ListItemText primary="Gi tilbakemelding" />
    </ListItemButton>
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
    <ListItemButton
      onClick={() => {
        showMyList()
        hideMenu()
      }}
      selected={currentPage === 'myList'}>
      <ListItemIcon>
        <Icon>home</Icon>
      </ListItemIcon>
      <ListItemText primary="Min liste" />
    </ListItemButton>
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
    <ListItemButton
      onClick={() => {
        showUserGroups()
        hideMenu()
      }}
      selected={currentPage === 'userGroups'}>
      <ListItemIcon>
        <Icon>group</Icon>
      </ListItemIcon>
      <ListItemText primary="Mine grupper" />
    </ListItemButton>
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
