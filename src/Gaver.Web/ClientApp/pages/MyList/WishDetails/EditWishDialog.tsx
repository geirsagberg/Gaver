import React, { FC } from 'react'
import { useOvermind } from '~/overmind'
import WishDetailsDialog from './WishDetailsDialog'

const AddWishDialog: FC = () => {
  const {
    state: {
      myList: { editingWish }
    },
    actions: {
      myList: { cancelEditingWish, updateEditingWish, saveEditingWish }
    }
  } = useOvermind()
  return (
    <WishDetailsDialog
      wish={editingWish}
      onCancel={cancelEditingWish}
      onSave={saveEditingWish}
      updateWish={updateEditingWish}
    />
  )
}

export default AddWishDialog
