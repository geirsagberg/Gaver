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
import { useMyList } from '~/overmind/myList'

const EditWishDialog: FC = () => {
  const {
    state: { editingWish },
    actions: { cancelEditingWish, updateEditingWish, saveEditingWish }
  } = useMyList()
  const {
    state: {
      app: { isSavingOrLoading }
    }
  } = useOvermind()
  return editingWish ? (
    <Dialog fullWidth open={!!editingWish} onClose={cancelEditingWish}>
      <DialogTitle>Endre ønske</DialogTitle>
      <DialogContent>
        <DialogContentText>Hva ønsker du deg?</DialogContentText>
        <TextField
          onChange={event => updateEditingWish({ field: 'title', value: event.target.value })}
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
