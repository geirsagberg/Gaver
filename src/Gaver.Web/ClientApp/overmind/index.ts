import { IAction, IConfig, IDerive, IOnInitialize, IOperator, IState, Overmind } from 'overmind'
import { createHook } from 'overmind-react'
import { namespaced } from 'overmind/config'
import * as auth from './auth'
import * as routing from './routing'
import * as myList from './myList'

export interface Config extends IConfig<typeof config> {}

export interface OnInitialize extends IOnInitialize<Config> {}

export interface Action<Input = void> extends IAction<Config, Input> {}

export interface Operator<Input = void, Output = Input> extends IOperator<Config, Input, Output> {}

export interface Derive<Parent extends IState, Output> extends IDerive<Config, Parent, Output> {}

const config = namespaced({
  auth,
  routing,
  myList
})

const overmind = new Overmind(config)

export const useOvermind = createHook(overmind)
