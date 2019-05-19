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
import { WishModel } from '~/types/data'
import { selectIsSavingOrLoading } from '~/overmind/app/selectors'

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
      <DialogTitle>Nytt ønske</DialogTitle>
      <DialogContent>
        <DialogContentText>Hva ønsker du deg?</DialogContentText>
        <TextField
          onChange={event => updateWish({ title: event.target.value })}
          autoFocus
          fullWidth
          onKeyPress={e => wish.title && e.key === 'Enter' && onSave()}
          value={wish.title}
        />
      </DialogContent>
      <DialogActions>
        <Button disabled={isSavingOrLoading || !wish.title} variant="contained" color="primary" onClick={onSave}>
          Lagre
        </Button>
        <Button onClick={onCancel}>Avbryt</Button>
      </DialogActions>
    </Dialog>
  ) : null
}

export default WishDetailsDialog
