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
import { selectIsSavingOrLoading } from '~/overmind/app/selectors'
import { WishModel } from '~/types/data'

type Props = {
  wish: WishModel
  onSave: () => void
  onCancel: () => void
  updateWish: (update: Partial<WishModel>) => void
}

const WishDetailsDialog: FC<Props> = ({ wish, onCancel, onSave, updateWish }) => {
  const { state } = useOvermind()
  const isSavingOrLoading = selectIsSavingOrLoading(state)
  return wish ? (
    <Dialog fullWidth open={true} onClose={onCancel}>
      <form
        onSubmit={e => {
          e.preventDefault()
          onSave()
        }}>
        <DialogTitle>Nytt ønske</DialogTitle>
        <DialogContent>
          <DialogContentText>Hva ønsker du deg?</DialogContentText>
          <TextField
            onChange={event => updateWish({ title: event.target.value })}
            autoFocus
            fullWidth
            required
            value={wish.title}
            margin="dense"
          />
          <TextField
            label="Link (valgfritt)"
            margin="dense"
            fullWidth
            type="url"
            onChange={event => updateWish({ url: event.target.value })}
            value={wish.url || ''}
          />
        </DialogContent>
        <DialogActions>
          <Button type="submit" disabled={isSavingOrLoading} variant="contained" color="primary">
            Lagre
          </Button>
          <Button onClick={onCancel}>Avbryt</Button>
        </DialogActions>
      </form>
    </Dialog>
  ) : null
}

export default WishDetailsDialog
