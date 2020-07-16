import { AppBar, Icon, IconButton, Toolbar, Typography } from '@material-ui/core'
import { makeStyles } from '@material-ui/core/styles'
import React, { FC } from 'react'
import { hot } from 'react-hot-loader/root'
import { Actions } from './Actions'
import Expander from './components/Expander'
import { Content } from './Content'
import FeedbackDialog from './FeedbackDialog'
import { useOvermind } from './overmind'
import { ShareListDialog } from './ShareListDialog'
import { MainMenu } from './MainMenu'

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

const Layout: FC = () => {
  const classes = useStyles({})
  const {
    state: {
      auth: { isLoggedIn },
      app: { title },
    },
    actions: {
      app: { showMenu },
    },
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
      <MainMenu />
      <ShareListDialog />
      <FeedbackDialog />
      <div id="portal-overlay" style={{ position: 'relative', zIndex: 1100 }} />
    </div>
  )
}

export default hot(Layout)
