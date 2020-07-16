import { Paper, Typography, makeStyles } from '@material-ui/core'
import React from 'react'
import PictureAvatar from './PictureAvatar'
import { DateTime } from 'luxon'
import { ChatMessageDto } from '~/types/data'
import { useOvermind } from '~/overmind'
import classNames from 'classnames'
import Color from 'color'

const useStyles = makeStyles((theme) => ({
  message: {
    display: 'flex',
    alignItems: 'center',
    margin: '0.75rem 1rem',
  },
  messageUser: {},
  messageText: {},
  messageContent: {
    marginLeft: '1rem',
    display: 'flex',
    flexDirection: 'column',
    width: '100%',
    padding: '0.5rem',
  },
  ownMessage: {
    flexDirection: 'row-reverse',
    '& $messageContent': {
      background: new Color(theme.palette.primary.dark).darken(0.5).hex(),
      marginLeft: 0,
      marginRight: '1rem',
    },
  },
  timestamp: {
    alignSelf: 'flex-end',
  },
  avatar: {},
}))

const ChatMessage = ({ message }: { message: ChatMessageDto }) => {
  const {
    state: {
      auth: { user },
    },
  } = useOvermind()
  const currentUserId = user.id
  const classes = useStyles({ currentUserId })

  return (
    <div
      key={message.id}
      className={classNames(classes.message, {
        [classes.ownMessage]: currentUserId === message.user.id,
      })}>
      <PictureAvatar className={classes.avatar} user={message.user} title={message.user.name} />
      <Paper className={classNames(classes.messageContent)}>
        <Typography variant="caption" className={classes.timestamp} color="textSecondary">
          {DateTime.fromISO(message.created).setLocale(navigator.language).toLocaleString(DateTime.DATETIME_SHORT)}
        </Typography>
        <Typography className={classes.messageText}>{message.text}</Typography>
      </Paper>
    </div>
  )
}

export default ChatMessage
