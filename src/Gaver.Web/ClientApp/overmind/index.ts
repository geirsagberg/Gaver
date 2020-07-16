import { map, size } from 'lodash-es'
import { derived, IAction, IConfig, IContext, IOnInitialize, IOperator } from 'overmind'
import { createHook } from 'overmind-react'
import { merge, namespaced } from 'overmind/config'
import api from './api'
import app from './app'
import auth from './auth'
import chat from './chat'
import friends from './friends'
import invitations from './invitations'
import myList from './myList'
import routing from './routing'
import sharedLists from './sharedLists'
import { SharedList, SharedWish, User } from './sharedLists/state'
import userGroups from './userGroups'

export interface Config extends IConfig<typeof config> {}
export interface OnInitialize extends IOnInitialize<Config> {}
export interface Action<Input = void, ReturnValue = void | Promise<void> | Promise<boolean>>
  extends IAction<Config, Input, ReturnValue> {}
export interface Operator<Input = void, Output = Input> extends IOperator<Config, Input, Output> {}
export interface Context extends IContext<typeof config> {}
export interface AsyncAction<Input = void> extends Action<Input, Promise<boolean>> {}

export type ConfigState = Config['state']
export type RootState = Context['state']

type SharedState = {
  currentSharedList: SharedList
  currentSharedOrderedWishes: SharedWish[]
  currentSharedListOwner: User
}

const state: SharedState = {
  currentSharedList: derived((state: RootState) =>
    state.routing.currentSharedListId && state.sharedLists.wishLists[state.routing.currentSharedListId]
      ? state.sharedLists.wishLists[state.routing.currentSharedListId]
      : null
  ),
  currentSharedOrderedWishes: derived((state: RootState) =>
    state.currentSharedList && state.currentSharedList.wishesOrder
      ? map(state.currentSharedList.wishesOrder, (id) => state.currentSharedList.wishes[id])
      : state.currentSharedList && size(state.currentSharedList.wishes) === 0
      ? []
      : null
  ),
  currentSharedListOwner: derived((state: RootState) =>
    state.currentSharedList ? state.sharedLists.users[state.currentSharedList.ownerUserId] : null
  ),
}

export const config = merge(
  namespaced({
    auth,
    routing,
    myList,
    app,
    invitations,
    sharedLists,
    api,
    chat,
    userGroups,
    friends,
  }),
  {
    state,
  }
)

export const useOvermind = createHook<Config>()
