import React, { FC } from 'react'
import { useOvermind } from '~/overmind'
import WishDetailsDialog from './WishDetailsDialog'

const AddWishDialog: FC = () => {
  const {
    state: {
      myList: { newWish },
    },
    actions: {
      myList: { addWish, cancelAddingWish, updateNewWish },
    },
  } = useOvermind()
  return <WishDetailsDialog wish={newWish} onCancel={cancelAddingWish} onSave={addWish} updateWish={updateNewWish} />
}

export default AddWishDialog
