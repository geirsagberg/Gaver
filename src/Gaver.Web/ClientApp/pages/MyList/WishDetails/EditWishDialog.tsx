import React, { FC } from 'react'
import { useOvermind } from '~/overmind'
import WishDetailsDialog from './WishDetailsDialog'

const AddWishDialog: FC = () => {
  const {
    state: {
      myList: { editingWish }
    },
    actions: {
      myList: { cancelEditingWish, updateEditingWish, saveEditingWish, deleteEditingWish }
    }
  } = useOvermind()
  return (
    <WishDetailsDialog wish={editingWish} onCancel={cancelEditingWish} onSave={saveEditingWish} updateWish={updateEditingWish} onDelete={deleteEditingWish} />
  )
}

export default AddWishDialog
