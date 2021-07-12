import {
  AppBar,
  Icon,
  IconButton,
  Toolbar,
  Typography,
} from '@material-ui/core'
import { makeStyles } from '@material-ui/core/styles'
import React, { Suspense } from 'react'
import { Actions } from './Actions'
import Expander from './components/Expander'
import Loading from './components/Loading'
import { Content } from './Content'
import FeedbackDialog from './FeedbackDialog'
import { MainMenu } from './MainMenu'
import { useActions, useAppState } from './overmind'
import { ShareListDialog } from './ShareListDialog'

const useStyles = makeStyles((theme) => ({
  root: {
    height: '100%',
    position: 'relative',
    zIndex: 0,
  },
  content: {
    display: 'flex',
    alignItems: 'flex-start',
    justifyContent: 'center',
    height: '100%',
    paddingTop: '4rem',
    [theme.breakpoints.down('xs')]: {
      paddingTop: '3.5rem',
    },
  },
  menuIcon: {
    marginRight: '0.5rem',
  },
  toolbar: {
    padding: '0 0.5rem',
  },
}))

const Layout = () => {
  const classes = useStyles({})
  const {
    auth: { isLoggedIn },
    app: { title },
  } = useAppState()

  const {
    app: { showMenu },
  } = useActions()

  return (
    <div className={classes.root}>
      {isLoggedIn && (
        <AppBar>
          <Toolbar disableGutters className={classes.toolbar}>
            <IconButton
              color="inherit"
              className={classes.menuIcon}
              onClick={showMenu}>
              <Icon>menu</Icon>
            </IconButton>
            <Typography
              variant="h6"
              color="inherit"
              style={{ marginRight: '1rem' }}>
              {title ? title : 'Gaver'}
            </Typography>
            <Expander />
            <Actions />
          </Toolbar>
        </AppBar>
      )}
      <div className={classes.content}>
        <Suspense fallback={<Loading />}>
          <Content />
        </Suspense>
      </div>
      <MainMenu />
      <ShareListDialog />
      <FeedbackDialog />
      <div id="portal-overlay" style={{ position: 'relative', zIndex: 1100 }} />
    </div>
  )
}

export default Layout
