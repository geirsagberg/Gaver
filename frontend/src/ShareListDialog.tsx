import {
  Autocomplete,
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
  TextField,
} from '@mui/material'
import { useState } from 'react'
import { useActions, useAppState } from './overmind'
import { isEmailValid } from './utils/validation'

export const ShareListDialog = () => {
  const {
    myList: { isSharingList, shareEmails },
    app: { isSavingOrLoading },
  } = useAppState()
  const {
    myList: { cancelSharingList, emailsChanged, shareList },
  } = useActions()
  const [emailInput, setEmailInput] = useState('')
  const addAndShareList = () => {
    shareList()
    setEmailInput('')
  }
  const cancel = () => {
    cancelSharingList()
    setEmailInput('')
  }
  return (
    <Dialog
      fullWidth
      PaperProps={{
        sx: {
          overflow: 'visible',
        },
      }}
      open={isSharingList}
      onClose={cancel}>
      <DialogTitle>Del din ønskeliste</DialogTitle>
      <DialogContent sx={{ overflow: 'visible' }}>
        <DialogContentText>Legg inn e-postadressene til de du vil dele listen med</DialogContentText>
        <Autocomplete
          value={shareEmails}
          multiple
          renderInput={(params) => <TextField {...params} type="email" autoFocus placeholder="abc@example.com, ..." />}
          options={[] as string[]}
          freeSolo
          onChange={(_, emails) => {
            emailsChanged(emails)
          }}
          onInputChange={(_, value) => {
            setEmailInput(value)
          }}
          inputValue={emailInput}
          onBlur={() => {
            if (emailInput) {
              emailsChanged(shareEmails.concat(emailInput))
              setEmailInput('')
            }
          }}
        />
      </DialogContent>
      <DialogActions>
        <Button
          disabled={
            isSavingOrLoading || (!!emailInput && !isEmailValid(emailInput)) || (!shareEmails.length && !emailInput)
          }
          variant="contained"
          color="primary"
          onClick={addAndShareList}>
          Del liste
        </Button>
        <Button disabled={isSavingOrLoading} onClick={cancel}>
          Avbryt
        </Button>
      </DialogActions>
    </Dialog>
  )
}
