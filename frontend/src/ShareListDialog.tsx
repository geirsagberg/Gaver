import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
} from '@mui/material'
import { useActions, useAppState } from './overmind'

export const ShareListDialog = () => {
  const {
    myList: { isSharingList },
    app: { isSavingOrLoading },
  } = useAppState()
  const {
    myList: { cancelSharingList, copyShareLink },
  } = useActions()

  return (
    <Dialog open={isSharingList} onClose={cancelSharingList}>
      <DialogTitle>Del din ønskeliste</DialogTitle>
      <DialogContent>
        <DialogContentText>
          Klikk på knappen nedenfor for å kopiere en delingslenke til utklippstavlen.
        </DialogContentText>
      </DialogContent>
      <DialogActions>
        <Button
          disabled={isSavingOrLoading}
          variant="contained"
          color="primary"
          onClick={copyShareLink}>
          Kopier lenke
        </Button>
        <Button disabled={isSavingOrLoading} onClick={cancelSharingList}>
          Avbryt
        </Button>
      </DialogActions>
    </Dialog>
  )
}
