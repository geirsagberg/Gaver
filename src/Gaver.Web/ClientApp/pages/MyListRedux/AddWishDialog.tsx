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
import { ReduxState } from '~/redux/store'
import { createMapDispatchToProps } from '~/redux/reduxUtils'
import { cancelAddingWish, setNewWishTitle } from '~/redux/reducers/myList/actions'
import { addWish } from '~/redux/reducers/myList/thunks'
import { connect } from 'react-redux'

const mapStateToProps = (state: ReduxState) => ({
  newWish: state.myList.newWish,
  isAddingWish: state.myList.isAddingWish,
  isSavingOrLoading: state.app.isSavingOrLoading
})

const mapDispatchToProps = createMapDispatchToProps({
  cancelAddingWish,
  setNewWishTitle,
  addWish
})

type Props = ReturnType<typeof mapStateToProps> & ReturnType<typeof mapDispatchToProps>

const AddWishDialog: FC<Props> = ({
  newWish,
  isAddingWish,
  setNewWishTitle,
  isSavingOrLoading,
  cancelAddingWish,
  addWish
}) => {
  return newWish ? (
    <Dialog fullWidth open={isAddingWish} onClose={cancelAddingWish}>
      <DialogTitle>Nytt ønske</DialogTitle>
      <DialogContent>
        <DialogContentText>Hva ønsker du deg?</DialogContentText>
        <TextField
          onChange={event => setNewWishTitle(event.currentTarget.value)}
          autoFocus
          fullWidth
          onKeyPress={e => newWish.title && e.key === 'Enter' && addWish()}
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

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(AddWishDialog)
