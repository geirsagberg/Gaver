import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
  FormControlLabel,
  Switch,
  TextField
} from '@material-ui/core'
import React, { FC, useState } from 'react'
import { useOvermind } from './overmind'

const FeedbackDialog: FC = () => {
  const [message, setMessage] = useState('')
  const [anonymous, setAnonymous] = useState(false)
  const {
    actions: {
      app: { cancelFeedback, sendFeedback }
    },
    state: {
      app: { feedback, isSavingOrLoading }
    }
  } = useOvermind()
  const cancel = () => {
    cancelFeedback()
    setMessage('')
  }
  return (
    <Dialog fullWidth open={!!feedback} onClose={cancel}>
      <DialogTitle>Gi tilbakemelding</DialogTitle>
      <DialogContent>
        <DialogContentText>
          Hva liker du med appen? Hva kan bli bedre? Skjedde det en feil eller noe du ikke forventet?
        </DialogContentText>
        <TextField
          autoFocus
          placeholder="Skriv her..."
          fullWidth
          multiline
          value={message}
          onChange={e => setMessage(e.target.value)}
        />
        <FormControlLabel
          disabled={isSavingOrLoading}
          style={{ margin: '1rem 0' }}
          control={<Switch checked={anonymous} onChange={e => setAnonymous(e.target.checked)} />}
          label="Send inn anonymt"
        />
        <DialogActions>
          <Button
            disabled={isSavingOrLoading || !message.length}
            variant="contained"
            color="primary"
            onClick={async () => {
              if (await sendFeedback({ message, anonymous })) {
                setMessage('')
              }
            }}>
            Send inn
          </Button>
          <Button disabled={isSavingOrLoading} onClick={cancel}>
            Avbryt
          </Button>
        </DialogActions>
      </DialogContent>
    </Dialog>
  )
}

export default FeedbackDialog
