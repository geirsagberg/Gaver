import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
  FormControlLabel,
  Switch,
  TextField,
} from '@mui/material'
import { useState } from 'react'
import { useActions, useAppState } from './overmind'

const FeedbackDialog = () => {
  const [message, setMessage] = useState('')
  const [anonymous, setAnonymous] = useState(false)
  const {
    app: { feedback, isSavingOrLoading },
  } = useAppState()
  const {
    app: { cancelFeedback, sendFeedback },
  } = useActions()
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
          onChange={(e) => setMessage(e.target.value)}
        />
        <FormControlLabel
          disabled={isSavingOrLoading}
          style={{ margin: '1rem 0' }}
          control={<Switch checked={anonymous} onChange={(e) => setAnonymous(e.target.checked)} />}
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
