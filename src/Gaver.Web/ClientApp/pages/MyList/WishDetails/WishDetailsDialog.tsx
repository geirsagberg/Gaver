import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
  TextField,
  Icon
} from '@material-ui/core'
import React, { FC } from 'react'
import { useOvermind } from '~/overmind'
import { selectIsSavingOrLoading } from '~/overmind/app/selectors'
import { WishModel } from '~/types/data'
import Expander from '~/components/Expander'
import { createStylesHook } from '~/utils/materialUtils'

type Props = {
  wish: WishModel
  onSave: () => void
  onCancel: () => void
  onDelete?: (wishId: number) => void
  updateWish: (update: Partial<WishModel>) => void
}

const useStyles = createStylesHook({
  actions: {
    margin: '1rem',
    '& > :first-child': {
      marginLeft: 0
    },
    '& > :last-child': {
      marginRight: 0
    }
  },
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
