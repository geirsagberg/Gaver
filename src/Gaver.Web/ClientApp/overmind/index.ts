import { IAction, IConfig, IDerive, IOnInitialize, IOperator, IState, Overmind } from 'overmind'
import { createHook } from 'overmind-react'
import { namespaced } from 'overmind/config'
import auth from './auth'
import routing from './routing'
import myList from './myList'
import app from './app'
import invitations from './invitations'
import sharedList from './sharedList'

export interface Config extends IConfig<typeof config> {}

export interface OnInitialize extends IOnInitialize<Config> {}

export interface Action<Input = void> extends IAction<Config, Input> {}

export interface Operator<Input = void, Output = Input> extends IOperator<Config, Input, Output> {}

export interface Derive<Parent extends IState, Output> extends IDerive<Config, Parent, Output> {}

const config = namespaced({
  auth,
  routing,
  myList,
  app,
  invitations,
  sharedList
})

const overmind = new Overmind(config)

export const useOvermind = createHook(overmind)
