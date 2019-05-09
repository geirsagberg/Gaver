import { WishModel } from '~/types/data'
import { createAction } from '~/redux/reduxUtils'

export const wishesLoaded = (wishes: Dictionary<WishModel>, wishesOrder: number[]) =>
  createAction('wishesLoaded', { wishes, wishesOrder })
export const wishAdded = (wish: WishModel) => createAction('wishAdded', wish)
export const startAddingWish = () => createAction('startAddingWish')
export const cancelAddingWish = () => createAction('cancelAddingWish')
export const startEditingWish = (wishId: number) => createAction('startEditingWish', wishId)
export const cancelEditingWish = () => createAction('cancelEditingWish')
export const updateEditingWish = <K extends keyof WishModel>(field: K, value: WishModel[K]) =>
  createAction('updateEditingWish', { field, value })
export const setNewWishTitle = (title: string) => createAction('setNewWishTitle', title)
export const editingWishSaved = (wish: WishModel) => createAction('editingWishSaved', wish)
