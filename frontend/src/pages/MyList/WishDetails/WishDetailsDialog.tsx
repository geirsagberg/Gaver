import { Button, Dialog, DialogActions, DialogContent, DialogTitle, Icon, TextField } from '@mui/material'
import { FC } from 'react'
import Expander from '~/components/Expander'
import { useAppState } from '~/overmind'
import { selectIsSavingOrLoading } from '~/overmind/app/selectors'
import { Wish } from '~/overmind/myList/state'
import { dialogActions } from '~/theme'
import { useFeatures } from '~/utils/appSettings'

type Props = {
  wish: Wish
  onSave: () => void
  onCancel: () => void
  onDelete?: (wishId: number) => void
  updateWish: (update: Partial<Wish>) => void
}

const WishDetailsDialog: FC<Props> = ({ wish, onCancel, onSave, updateWish, onDelete }) => {
  const state = useAppState()
  const features = useFeatures()
  const isSavingOrLoading = selectIsSavingOrLoading(state)
  return wish ? (
    <Dialog fullWidth open={true} onClose={onCancel}>
      <form
        onSubmit={(e) => {
          e.preventDefault()
          onSave()
        }}>
        <DialogTitle>Hva ønsker du deg?</DialogTitle>
        <DialogContent>
          <TextField
            label="Beskrivelse"
            onChange={(event) => updateWish({ title: event.target.value })}
            autoFocus
            fullWidth
            required
            InputLabelProps={{ required: false }}
            value={wish.title}
            margin="dense"
            disabled={isSavingOrLoading}
            inputProps={{ maxLength: 255 }}
          />
          <TextField
            label="Link (valgfritt)"
            margin="dense"
            fullWidth
            type="url"
            onChange={(event) => updateWish({ url: event.target.value })}
            value={wish.url || ''}
            disabled={isSavingOrLoading}
            inputProps={{ maxLength: 255 }}
          />
          {features?.wishOptions && (
            <Button disabled={isSavingOrLoading} variant="contained" color="inherit">
              Legg til alternativ
            </Button>
          )}
        </DialogContent>
        <DialogActions sx={dialogActions}>
          {onDelete && (
            <Button onClick={() => wish.id && onDelete(wish.id)} disabled={isSavingOrLoading}>
              <Icon sx={{ marginRight: '0.5rem' }}>delete</Icon>
              Slett
            </Button>
          )}
          <Expander />
          <Button type="submit" disabled={isSavingOrLoading} variant="contained" color="primary">
            Lagre
          </Button>
          <Button onClick={onCancel} disabled={isSavingOrLoading}>
            Avbryt
          </Button>
        </DialogActions>
      </form>
    </Dialog>
  ) : null
}

export default WishDetailsDialog
