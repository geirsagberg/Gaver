import { useActions, useAppState } from '~/overmind'
import WishDetailsDialog from './WishDetailsDialog'

const AddWishDialog = () => {
  const {
    myList: { editingWish },
  } = useAppState()
  const {
    myList: { cancelEditingWish, updateEditingWish, saveEditingWish, deleteEditingWish },
  } = useActions()
  return editingWish ? (
    <WishDetailsDialog
      wish={editingWish}
      onCancel={cancelEditingWish}
      onSave={saveEditingWish}
      updateWish={updateEditingWish}
      onDelete={deleteEditingWish}
    />
  ) : null
}

export default AddWishDialog
