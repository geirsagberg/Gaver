import { SwipeableDrawer, TextField, Typography } from '@mui/material';
import makeStyles from '@mui/styles/makeStyles';
import { ThemeProvider, Theme, StyledEngineProvider } from '@mui/material/styles';
import React, { useState } from 'react'
import SimpleBar from 'simplebar-react'
import { useNamespace } from '~/overmind'
import { darkTheme } from '~/theme'
import ChatMessage from './ChatMessage'


declare module '@mui/styles/defaultTheme' {
  // eslint-disable-next-line @typescript-eslint/no-empty-interface
  interface DefaultTheme extends Theme {}
}


const useStyles = makeStyles((theme) => ({
  menuPaper: {
    // background: theme.palette.primary.dark
  },
  chatRoot: {
    width: `calc(100vw - 4rem)`,
    maxWidth: 512,
    display: 'flex',
    flexDirection: 'column',
    justifyContent: 'space-between',
    height: '100vh',
  },
  chatHeader: {
    textAlign: 'center',
    padding: '1rem',
    [theme.breakpoints.down('sm')]: {
      padding: '0.75rem 1rem',
    },
  },
  messages: {
    flex: 1,
    background: theme.palette.background.default,
    height: `calc(100vh - 8rem)`,
  },
  inputForm: {},
  input: {
    padding: '0.5rem',
  },
}))

const ChatView = () => {
  const {
    state: { messages, visible },
    actions: { showChat, hideChat, addMessage },
    effects: { scrollChat },
  } = useNamespace('chat')
  const classes = useStyles()
  const [message, setMessage] = useState('')

  return (
    <SwipeableDrawer
      anchor="right"
      open={visible}
      onOpen={showChat}
      onClose={hideChat}
      classes={{ paper: classes.menuPaper }}>
      <div className={classes.chatRoot}>
        <Typography variant="h6" className={classes.chatHeader}>
          Chat
        </Typography>
        <SimpleBar className={classes.messages} id="chatMessages">
          {messages.map((m) => (
            <ChatMessage message={m} key={m.id} />
          ))}
        </SimpleBar>
        <form
          className={classes.inputForm}
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
            className={classes.input}
            value={message}
            onChange={(e) => setMessage(e.target.value)}
          />
        </form>
      </div>
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
