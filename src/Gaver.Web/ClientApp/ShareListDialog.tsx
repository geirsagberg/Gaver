import { Button, Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle } from '@material-ui/core'
import { makeStyles } from '@material-ui/styles'
import ChipInput from 'material-ui-chip-input'
import React, { FC, useState } from 'react'
import { KeyCodes } from '~/types'
import { useOvermind } from './overmind'
import { isEmailValid } from './utils/validation'

export const useStyles = makeStyles({
  overflowDialog: {
    overflow: 'visible'
  }
})

export const ShareListDialog: FC = () => {
  const classes = useStyles({})
  const {
    actions: {
      myList: { cancelSharingList, emailAdded, emailDeleted, shareList }
    },
    state: {
      myList: { isSharingList, shareEmails },
      app: { isSavingOrLoading }
    }
  } = useOvermind()
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
    <Dialog fullWidth classes={{ paper: classes.overflowDialog }} open={isSharingList} onClose={cancel}>
      <DialogTitle>Del din ønskeliste</DialogTitle>
      <DialogContent className={classes.overflowDialog}>
        <DialogContentText>Legg inn e-postadressene til de du vil dele listen med</DialogContentText>
        <ChipInput
          fullWidth
          placeholder="abc@example.com, ..."
          classes={{}}
          value={shareEmails}
          onAdd={emailAdded}
          InputProps={{ type: 'email', autoFocus: true }}
          onDelete={emailDeleted}
          onKeyPress={e => e.key === 'Enter' && addAndShareList()}
          onUpdateInput={e => setEmailInput(e.target.value)}
          blurBehavior="add"
          required
          newChipKeyCodes={[KeyCodes.Enter, KeyCodes.Tab, KeyCodes.Comma]}
        />
      </DialogContent>
      <DialogActions>
        <Button
          disabled={
            isSavingOrLoading || ((!!emailInput && !isEmailValid(emailInput)) || (!shareEmails.length && !emailInput))
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
