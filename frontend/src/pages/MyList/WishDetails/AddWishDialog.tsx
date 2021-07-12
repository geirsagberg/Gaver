import React, { FC } from 'react'
import { useActions, useAppState } from '~/overmind'
import WishDetailsDialog from './WishDetailsDialog'

const AddWishDialog: FC = () => {
  const {
    myList: { newWish },
  } = useAppState()
  const {
    myList: { addWish, cancelAddingWish, updateNewWish },
  } = useActions()
  return newWish ? (
    <WishDetailsDialog
      wish={newWish}
      onCancel={cancelAddingWish}
      onSave={addWish}
      updateWish={updateNewWish}
    />
  ) : null
}

export default AddWishDialog
