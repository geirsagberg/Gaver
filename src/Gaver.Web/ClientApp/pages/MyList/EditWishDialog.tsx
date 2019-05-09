import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
  TextField
} from '@material-ui/core'
import React, { FC } from 'react'
import { useOvermind } from '~/overmind'

const EditWishDialog: FC = () => {
  const {
    state: {
      myList: { editingWish },
      app: { isSavingOrLoading }
    },
    actions: {
      myList: { cancelEditingWish, updateEditingWish, saveEditingWish }
    }
  } = useOvermind()
  return editingWish ? (
    <Dialog fullWidth open={!!editingWish} onClose={cancelEditingWish}>
      <DialogTitle>Endre ønske</DialogTitle>
      <DialogContent>
        <DialogContentText>Hva ønsker du deg?</DialogContentText>
        <TextField
          onChange={event => updateEditingWish({ title: event.target.value })}
          autoFocus
          fullWidth
          onKeyPress={e => e.key === 'Enter' && saveEditingWish()}
          value={editingWish.title}
        />
      </DialogContent>
      <DialogActions>
        <Button
          disabled={isSavingOrLoading || !editingWish.title}
          variant="contained"
          color="primary"
          onClick={saveEditingWish}>
          Lagre
        </Button>
        <Button onClick={cancelEditingWish}>Avbryt</Button>
      </DialogActions>
    </Dialog>
  ) : null
}

export default EditWishDialog
