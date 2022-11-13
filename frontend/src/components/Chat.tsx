import { Box, SwipeableDrawer, TextField, Typography } from '@mui/material'
import { StyledEngineProvider, ThemeProvider } from '@mui/material/styles'
import React, { useState } from 'react'
import SimpleBar from 'simplebar-react'
import { useNamespace } from '~/overmind'
import theme, { darkTheme } from '~/theme'
import ChatMessage from './ChatMessage'

const ChatView = () => {
  const {
    state: { messages, visible },
    actions: { showChat, hideChat, addMessage },
    effects: { scrollChat },
  } = useNamespace('chat')
  const [message, setMessage] = useState('')

  return (
    <SwipeableDrawer anchor="right" open={visible} onOpen={showChat} onClose={hideChat}>
      <Box
        sx={{
          width: `calc(100vw - 4rem)`,
          maxWidth: 512,
          display: 'flex',
          flexDirection: 'column',
          justifyContent: 'space-between',
          height: '100vh',
        }}>
        <Typography
          variant="h6"
          sx={(theme) => ({
            textAlign: 'center',
            padding: '1rem',
            [theme.breakpoints.down('sm')]: {
              padding: '0.75rem 1rem',
            },
          })}>
          Chat
        </Typography>
        <SimpleBar
          style={{
            flex: 1,
            background: theme.palette.background.default,
            height: `calc(100vh - 8rem)`,
          }}
          id="chatMessages">
          {messages.map((m) => (
            <ChatMessage message={m} key={m.id} />
          ))}
        </SimpleBar>
        <form
          onSubmit={async (e) => {
            e.preventDefault()
            setMessage('')
            await addMessage(message)
            scrollChat()
          }}>
          <TextField
            placeholder="Skriv en melding..."
            inputProps={{ maxLength: 500 }}
            fullWidth
            sx={{ padding: '0.5rem' }}
            value={message}
            onChange={(e) => setMessage(e.target.value)}
          />
        </form>
      </Box>
    </SwipeableDrawer>
  )
}

export default () => (
  <StyledEngineProvider injectFirst>
    <ThemeProvider theme={darkTheme}>
      <ChatView />
    </ThemeProvider>
  </StyledEngineProvider>
)
