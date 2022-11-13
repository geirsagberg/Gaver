import { AppBar, Box, Icon, IconButton, Toolbar, Typography } from '@mui/material'
import { Suspense } from 'react'
import { Actions } from './Actions'
import Expander from './components/Expander'
import Loading from './components/Loading'
import { Content } from './Content'
import FeedbackDialog from './FeedbackDialog'
import { MainMenu } from './MainMenu'
import { useActions, useAppState } from './overmind'
import { ShareListDialog } from './ShareListDialog'

const Layout = () => {
  const {
    auth: { isLoggedIn },
    app: { title },
  } = useAppState()

  const {
    app: { showMenu },
  } = useActions()

  return (
    <Box
      sx={{
        height: '100%',
        position: 'relative',
        zIndex: 0,
      }}>
      {isLoggedIn && (
        <AppBar>
          <Toolbar
            disableGutters
            sx={{
              padding: '0 0.5rem',
            }}>
            <IconButton
              color="inherit"
              sx={{
                marginRight: '0.5rem',
              }}
              onClick={showMenu}
              size="large">
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
      <Box
        sx={(theme) => ({
          display: 'flex',
          alignItems: 'flex-start',
          justifyContent: 'center',
          height: '100%',
          paddingTop: '4rem',
          [theme.breakpoints.down('sm')]: {
            paddingTop: '3.5rem',
          },
        })}>
        <Suspense fallback={<Loading />}>
          <Content />
        </Suspense>
      </Box>
      <MainMenu />
      <ShareListDialog />
      <FeedbackDialog />
      <div id="portal-overlay" style={{ position: 'relative', zIndex: 1100 }} />
    </Box>
  )
}

export default Layout
