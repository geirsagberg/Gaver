import { map } from 'lodash-es'
import { IAction, IConfig, IContext, IDerive, IOnInitialize, IOperator, IState } from 'overmind'
import { createHook } from 'overmind-react'
import { merge, namespaced } from 'overmind/config'
import api from './api'
import app from './app'
import auth from './auth'
import chat from './chat'
import invitations from './invitations'
import myList from './myList'
import routing from './routing'
import sharedLists from './sharedLists'
import userGroups from './userGroups'
import users from './users'
import { SharedList, SharedWish, User } from './sharedLists/state'

export interface Config extends IConfig<typeof config> {}
export interface OnInitialize extends IOnInitialize<Config> {}
export interface Action<Input = void, ReturnValue = void | Promise<void> | Promise<boolean>>
  extends IAction<Config, Input, ReturnValue> {}
export interface Operator<Input = void, Output = Input> extends IOperator<Config, Input, Output> {}
export interface Derive<Parent extends IState, Output> extends IDerive<Config, Parent, Output> {}
export interface Context extends IContext<typeof config> {}

export type ConfigState = Config['state']
export type ResolvedState = Context['state']

type SharedState = {
  currentSharedList: Derive<ConfigState, SharedList>
  currentSharedOrderedWishes: Derive<ConfigState, SharedWish[]>
  currentSharedListOwner: Derive<ConfigState, User>
}

const state: SharedState = {
  currentSharedList: state =>
    state.routing.currentSharedListId && state.sharedLists.wishLists[state.routing.currentSharedListId]
      ? state.sharedLists.wishLists[state.routing.currentSharedListId]
      : null,
  currentSharedOrderedWishes: state =>
    state.currentSharedList && state.currentSharedList.wishesOrder
      ? map(state.currentSharedList.wishesOrder, id => state.currentSharedList.wishes[id])
      : null,
  currentSharedListOwner: state =>
    state.currentSharedList ? state.sharedLists.users[state.currentSharedList.ownerUserId] : null
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
    users
  }),
  {
    state
  }
)

export const useOvermind = createHook<typeof config>()
