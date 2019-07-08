import { makeStyles, SwipeableDrawer, TextField, Typography } from '@material-ui/core'
import { MuiThemeProvider } from '@material-ui/core/styles'
import React, { useState } from 'react'
import { useOvermind } from '~/overmind'
import { darkTheme } from '~/theme'
import ChatMessage from './ChatMessage'
import SimpleBar from 'simplebar-react'

const useStyles = makeStyles(theme => ({
  menuPaper: {
    // background: theme.palette.primary.dark
  },
  chatRoot: {
    width: `calc(100vw - 4rem)`,
    maxWidth: 512,
    display: 'flex',
    flexDirection: 'column',
    justifyContent: 'space-between',
    height: '100vh'
  },
  chatHeader: {
    textAlign: 'center',
    padding: '1rem',
    [theme.breakpoints.down('xs')]: {
      padding: '0.75rem 1rem'
    }
  },
  messages: {
    flex: 1,
    background: theme.palette.background.default,
    height: `calc(100vh - 8rem)`
  },
  inputForm: {},
  input: {
    padding: '0.5rem'
  }
}))

const ChatView = () => {
  const {
    state: {
      chat: { visible, messages }
    },
    actions: {
      chat: { showChat, hideChat, addMessage }
    },
    effects: {
      chat: { scrollChat }
    }
  } = useOvermind()
  const classes = useStyles({})
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
          {messages.map(m => (
            <ChatMessage message={m} key={m.id} />
          ))}
        </SimpleBar>
        <form
          className={classes.inputForm}
          onSubmit={async e => {
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
            onChange={e => setMessage(e.target.value)}
          />
        </form>
      </div>
    </SwipeableDrawer>
  )
}

export default () => (
  <MuiThemeProvider theme={darkTheme}>
    <ChatView />
  </MuiThemeProvider>
)
