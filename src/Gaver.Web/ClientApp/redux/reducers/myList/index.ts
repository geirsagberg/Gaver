import { ActionsUnion, createReducer } from '~/redux/reduxUtils'
import { WishModel } from '~/types/data'

interface MyListState {
  id?: number
  editingWish?: WishModel
  shareEmails: string[]
  isSharingList: boolean
  wishes: Dictionary<WishModel>
  isAddingWish: boolean
  newWish?: WishModel
  wishesLoaded?: boolean
  wishesOrder: number[]
  isDeleting?: boolean
}

const initialState: MyListState = {
  shareEmails: [],
  wishes: {},
  isAddingWish: false,
  isSharingList: false,
  wishesOrder: []
}

type Action = ActionsUnion<typeof import('./actions')>

export default createReducer<MyListState, Action>(initialState, (state, action) => {
  switch (action.type) {
    case 'wishesLoaded': {
      const { wishes, wishesOrder } = action.payload
      state.wishes = wishes
      state.wishesOrder = wishesOrder
      state.wishesLoaded = true
      return
    }
    case 'wishAdded': {
      const wish = action.payload
      state.wishes[wish.id] = wish
      state.isAddingWish = false
      state.wishesOrder.push(wish.id)
      return
    }
    case 'startAddingWish': {
      state.isAddingWish = true
      state.newWish = { title: '' }
      return
    }
    case 'cancelAddingWish': {
      state.isAddingWish = false
      return
    }
    case 'startEditingWish': {
      const wish = state.wishes[action.payload]
      state.editingWish = { ...wish }
      return
    }
    case 'updateEditingWish': {
      const { field, value } = action.payload
      state.editingWish[field] = value
      return
    }
    case 'editingWishSaved': {
      const wish = action.payload
      state.wishes[wish.id] = wish
      state.editingWish = null
      return
    }
    case 'setNewWishTitle': {
      state.newWish.title = action.payload
      return
    }
  }
})
