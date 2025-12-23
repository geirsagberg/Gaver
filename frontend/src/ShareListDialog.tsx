import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
  TextField,
} from '@mui/material'
import { useActions, useAppState } from './overmind'

export const ShareListDialog = () => {
  const {
    myList: { isSharingList, shareUrl },
    app: { isSavingOrLoading },
  } = useAppState()
  const {
    myList: { cancelSharingList, createShareLink, copyShareLink },
  } = useActions()

  return (
    <Dialog open={isSharingList} onClose={cancelSharingList}>
      <DialogTitle>Del din ønskeliste</DialogTitle>
      <DialogContent>
        {!shareUrl ? (
          <DialogContentText>
            Klikk på knappen nedenfor for å opprette en delingslenke.
          </DialogContentText>
        ) : (
          <>
            <DialogContentText>
              Lenken er klar! Klikk på "Kopier lenke" for å kopiere den til utklippstavlen.
            </DialogContentText>
            <TextField
              fullWidth
              value={shareUrl}
              margin="normal"
              InputProps={{
                readOnly: true,
              }}
              onClick={(e) => {
                const target = e.target as HTMLInputElement
                target.select()
              }}
            />
          </>
        )}
      </DialogContent>
      <DialogActions>
        {!shareUrl ? (
          <>
            <Button
              disabled={isSavingOrLoading}
              variant="contained"
              color="primary"
              onClick={createShareLink}>
              Opprett lenke
            </Button>
            <Button disabled={isSavingOrLoading} onClick={cancelSharingList}>
              Avbryt
            </Button>
          </>
        ) : (
          <>
            <Button
              variant="contained"
              color="primary"
              onClick={copyShareLink}>
              Kopier lenke
            </Button>
            <Button onClick={cancelSharingList}>
              Lukk
            </Button>
          </>
        )}
      </DialogActions>
    </Dialog>
  )
}
