import { Button, Dialog, DialogActions, DialogContent, DialogTitle, Icon, TextField } from '@material-ui/core'
import React, { FC } from 'react'
import Expander from '~/components/Expander'
import { useOvermind } from '~/overmind'
import { selectIsSavingOrLoading } from '~/overmind/app/selectors'
import { Wish } from '~/overmind/myList/state'
import { createStylesHook } from '~/utils/materialUtils'
import AppSettings from '~/utils/appSettings'
import { commonStyles } from '~/theme'

type Props = {
  wish: Wish
  onSave: () => void
  onCancel: () => void
  onDelete?: (wishId: number) => void
  updateWish: (update: Partial<Wish>) => void
}

const useStyles = createStylesHook({
  actions: commonStyles.dialogActions,
  leftIcon: {
    marginRight: '0.5rem'
  }
})

const WishDetailsDialog: FC<Props> = ({ wish, onCancel, onSave, updateWish, onDelete }) => {
  const { state } = useOvermind()
  const classes = useStyles({})
  const isSavingOrLoading = selectIsSavingOrLoading(state)
  return wish ? (
    <Dialog fullWidth open={true} onClose={onCancel}>
      <form
        onSubmit={e => {
          e.preventDefault()
          onSave()
        }}>
        <DialogTitle>Hva ønsker du deg?</DialogTitle>
        <DialogContent>
          <TextField
            label="Beskrivelse"
            onChange={event => updateWish({ title: event.target.value })}
            autoFocus
            fullWidth
            required
            InputLabelProps={{ required: false }}
            value={wish.title}
            margin="dense"
            disabled={isSavingOrLoading}
          />
          <TextField
            label="Link (valgfritt)"
            margin="dense"
            fullWidth
            type="url"
            onChange={event => updateWish({ url: event.target.value })}
            value={wish.url || ''}
            disabled={isSavingOrLoading}
          />
          {AppSettings.features.wishOptions && (
            <Button disabled={isSavingOrLoading} variant="contained" color="inherit">
              Legg til alternativ
            </Button>
          )}
        </DialogContent>
        <DialogActions className={classes.actions}>
          {onDelete && (
            <Button onClick={() => onDelete(wish.id)} disabled={isSavingOrLoading}>
              <Icon className={classes.leftIcon}>delete</Icon>
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
