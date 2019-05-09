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
import { connect } from 'react-redux'
import { cancelEditingWish, updateEditingWish } from '~/redux/reducers/myList/actions'
import { saveEditingWish } from '~/redux/reducers/myList/thunks'
import { createMapDispatchToProps } from '~/redux/reduxUtils'
import { ReduxState } from '~/redux/store'

const mapStateToProps = (state: ReduxState) => ({
  editingWish: state.myList.editingWish,
  isSavingOrLoading: state.app.isSavingOrLoading
})

const mapDispatchToProps = createMapDispatchToProps({
  cancelEditingWish,
  updateEditingWish,
  saveEditingWish
})

type Props = ReturnType<typeof mapStateToProps> & ReturnType<typeof mapDispatchToProps>

const EditWishDialog: FC<Props> = ({
  editingWish,
  isSavingOrLoading,
  cancelEditingWish,
  updateEditingWish,
  saveEditingWish
}) => {
  return editingWish ? (
    <Dialog fullWidth open={!!editingWish} onClose={cancelEditingWish}>
      <DialogTitle>Endre ønske</DialogTitle>
      <DialogContent>
        <DialogContentText>Hva ønsker du deg?</DialogContentText>
        <TextField
          onChange={event => updateEditingWish('title', event.target.value)}
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

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(EditWishDialog)
