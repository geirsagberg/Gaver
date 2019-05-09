import { keyBy } from 'lodash-es'
import { Thunk } from '~/redux/reduxUtils'
import { MyListModel, WishModel } from '~/types/data'
import { tryOrNotify } from '~/utils'
import { getJson, postJson, putJson } from '~/utils/ajax'
import { editingWishSaved, wishAdded, wishesLoaded } from './actions'

export const loadWishes = (): Thunk => dispatch =>
  tryOrNotify(async () => {
    const model = await getJson<MyListModel>('/api/WishLists')
    const wishes = keyBy(model.wishes, w => w.id)
    dispatch(wishesLoaded(wishes, model.wishesOrder))
  })

export const addWish = (): Thunk => (dispatch, getState) =>
  tryOrNotify(async () => {
    const { myList } = getState()
    const wish = await postJson<WishModel>('/api/WishLists', myList.newWish)
    dispatch(wishAdded(wish))
  })

export const saveEditingWish = (): Thunk => (dispatch, getState) =>
  tryOrNotify(async () => {
    const {
      myList: { editingWish }
    } = getState()
    const wish = await putJson<WishModel>(`/api/MyList/${editingWish.id}`, { title: editingWish.title })
    dispatch(editingWishSaved(wish))
  })
