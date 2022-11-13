import { Box, Paper, Typography } from '@mui/material'
import Color from 'color'
import { DateTime } from 'luxon'
import { useAppState } from '~/overmind'
import { ChatMessageDto } from '~/types/data'
import PictureAvatar from './PictureAvatar'

const ChatMessage = ({ message }: { message: ChatMessageDto }) => {
  const {
    auth: { user },
  } = useAppState()
  const currentUserId = user?.id

  return (
    <Box
      key={message.id}
      sx={(theme) => ({
        display: 'flex',
        alignItems: 'center',
        margin: '0.75rem 1rem',
        ...(currentUserId === message.user.id
          ? {
              flexDirection: 'row-reverse',
              '& $messageContent': {
                background: new Color(theme.palette.primary.dark).darken(0.5).hex(),
                marginLeft: 0,
                marginRight: '1rem',
              },
            }
          : {}),
      })}>
      <PictureAvatar user={message.user} title={message.user.name} />
      <Paper
        sx={{
          marginLeft: '1rem',
          display: 'flex',
          flexDirection: 'column',
          width: '100%',
          padding: '0.5rem',
        }}>
        <Typography
          variant="caption"
          sx={{
            alignSelf: 'flex-end',
          }}
          color="textSecondary">
          {DateTime.fromISO(message.created).setLocale(navigator.language).toLocaleString(DateTime.DATETIME_SHORT)}
        </Typography>
        <Typography>{message.text}</Typography>
      </Paper>
    </Box>
  )
}

export default ChatMessage
