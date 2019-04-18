import { Button, Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle } from '@material-ui/core'
import { makeStyles } from '@material-ui/styles'
import ChipInput from 'material-ui-chip-input'
import React, { FC } from 'react'
import { KeyCodes } from '~/types'
import { useOvermind } from './overmind'

export const useStyles = makeStyles({
  overflowDialog: {
    overflow: 'visible'
  }
})

export const ShareListDialog: FC = () => {
  const classes = useStyles()
  const {
    actions: {
      myList: { cancelSharingList, emailAdded, emailDeleted, shareList }
    },
    state: {
      myList: { isSharingList, shareEmails },
      app: { isSavingOrLoading }
    }
  } = useOvermind()
  return (
    <Dialog fullWidth classes={{ paper: classes.overflowDialog }} open={isSharingList} onClose={cancelSharingList}>
      <DialogTitle>Del din ønskeliste</DialogTitle>
      <DialogContent className={classes.overflowDialog}>
        <DialogContentText>Legg inn e-postadressene til de du vil dele listen med</DialogContentText>
        <ChipInput
          fullWidth
          classes={{}}
          value={shareEmails}
          onAdd={emailAdded}
          InputProps={{ type: 'email', autoFocus: true }}
          onDelete={emailDeleted}
          onKeyPress={e => e.key === 'Enter' && shareList()}
          blurBehavior="add"
          required
          newChipKeyCodes={[KeyCodes.Enter, KeyCodes.Tab, KeyCodes.SemiColon, KeyCodes.Comma]}
        />
      </DialogContent>
      <DialogActions>
        <Button
          disabled={isSavingOrLoading || !shareEmails.length}
          variant="contained"
          color="primary"
          onClick={shareList}>
          Del liste
        </Button>
        <Button onClick={cancelSharingList}>Avbryt</Button>
      </DialogActions>
    </Dialog>
  )
}
