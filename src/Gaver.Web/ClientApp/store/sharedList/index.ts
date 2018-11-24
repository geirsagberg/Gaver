import produce from 'immer'
import { merge } from 'lodash-es'
import { showMyList, unsubscribeList, subscribeList } from './thunks'
import { setBought, loadSharedList } from './api'
import { Wish, SharedWishModel, UserModel } from '~/types/data'

const namespace = 'gaver/sharedList/'

const DATA_LOADED = namespace + 'DATA_LOADED'
const SET_USERS = namespace + 'SET_USERS'
const SET_BOUGHT_SUCCESS = namespace + 'SET_BOUGHT_SUCCESS'
const CLEAR_STATE = namespace + 'CLEAR_STATE'
const SET_AUTHORIZED = namespace + 'SET_AUTHORIZED'

export interface SharedListState {
  id?: number
  wishes?: Dictionary<SharedWishModel>
  owner?: string
  users?: Dictionary<UserModel>
  currentUsers: number[]
  isAuthorized?: boolean
}

export function dataLoaded(data) {
  return {
    type: DATA_LOADED,
    data
  }
}

export function setBoughtSuccess({ wishId, isBought, userId }) {
  return {
    type: SET_BOUGHT_SUCCESS,
    wishId,
    isBought,
    userId
  }
}

export function setAuthorized() {
  return {
    type: SET_AUTHORIZED
  }
}

const initialState: SharedListState = {
  currentUsers: []
}

const reducer = produce((draft: SharedListState = initialState, action) => {
  switch (action.type) {
    case DATA_LOADED: {
      const wishListId = action.data.result
      draft.wishes = action.data.entities.wishes
      draft.users = merge(draft.users, action.data.entities.users, { deep: true })
      draft.owner = action.data.entities[wishListId].owner
      draft.id = wishListId
      return
    }
    case SET_BOUGHT_SUCCESS:
      draft.wishes[action.wishId].boughtByUser = action.isBought ? action.userId : null
      return
    case SET_USERS:
      draft.users = merge(draft.users, action.data.entities.users, { deep: true })
      draft.currentUsers = action.data.result
      return
    case SET_AUTHORIZED:
      draft.isAuthorized = true
      return
    case CLEAR_STATE:
      return initialState
  }
  return draft
})

export default reducer

export function setUsers(data) {
  return {
    type: SET_USERS,
    data
  }
}

export function clearState() {
  return {
    type: CLEAR_STATE
  }
}

export const actionCreators = {
  showMyList,
  setUsers,
  unsubscribeList,
  subscribeList,
  setBought,
  loadSharedList
}
