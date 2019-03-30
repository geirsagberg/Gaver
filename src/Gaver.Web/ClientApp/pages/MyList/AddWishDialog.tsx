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

const AddWishDialog: FC = () => {
  const {
    state: {
      myList: { isAddingWish, newWish },
      app: { isSavingOrLoading }
    },
    actions: {
      myList: { addWish, cancelAddingWish, setNewWishTitle }
    }
  } = useOvermind()
  return newWish ? (
    <Dialog fullWidth open={isAddingWish} onClose={cancelAddingWish}>
      <DialogTitle>Nytt ønske</DialogTitle>
      <DialogContent>
        <DialogContentText>Hva ønsker du deg?</DialogContentText>
        <TextField
          onChange={event => setNewWishTitle(event.currentTarget.value)}
          autoFocus
          fullWidth
          onKeyPress={e => e.key === 'Enter' && addWish()}
          value={newWish.title}
        />
      </DialogContent>
      <DialogActions>
        <Button disabled={isSavingOrLoading || !newWish.title} variant="contained" color="primary" onClick={addWish}>
          Lagre
        </Button>
        <Button onClick={cancelAddingWish}>Avbryt</Button>
      </DialogActions>
    </Dialog>
  ) : null
}

export default AddWishDialog
