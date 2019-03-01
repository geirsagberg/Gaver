import {
  AppBar,
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
  Icon,
  IconButton,
  Menu,
  MenuItem,
  Toolbar,
  Typography
} from '@material-ui/core'
import { makeStyles } from '@material-ui/styles'
import ChipInput from 'material-ui-chip-input'
import React, { FC, useState } from 'react'
import { hot } from 'react-hot-loader'
import { KeyCodes } from '~/types'
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
  },
  overflowDialog: {
    overflow: 'visible'
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
      myList: { startSharingList, cancelSharingList, emailAdded, emailDeleted, shareList }
    },
    state: {
      myList: { isSharingList, shareEmails },
      app: { isSavingOrLoading }
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
      <Dialog fullWidth classes={{ paper: classes.overflowDialog }} open={isSharingList} onClose={cancelSharingList}>
        <DialogTitle>Del din ønskeliste</DialogTitle>
        <DialogContent className={classes.overflowDialog}>
          <DialogContentText>Legg inn e-postadressene til de du vil dele listen med</DialogContentText>
          <ChipInput
            fullWidth
            classes={{}}
            value={shareEmails}
            onAdd={emailAdded}
            InputProps={{ type: 'email' }}
            onDelete={emailDeleted}
            blurBehavior="add"
            required
            newChipKeyCodes={[KeyCodes.Enter, KeyCodes.Tab, KeyCodes.SemiColon, KeyCodes.Comma]}
          />
        </DialogContent>
        <DialogActions>
          <Button
            disabled={isSavingOrLoading || !shareEmails.length}
            variant="contained"
            color="primary"
            onClick={shareList}>
            Del liste
          </Button>
          <Button onClick={cancelSharingList}>Avbryt</Button>
        </DialogActions>
      </Dialog>
      <div id="portal-overlay" style={{ position: 'relative', zIndex: 1100 }} />
    </div>
  )
}

export default hot(module)(Layout)
