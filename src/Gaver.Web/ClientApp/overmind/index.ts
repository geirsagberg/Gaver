import { IConfig, Overmind } from 'overmind'
import { createHook } from 'overmind-react'
import { merge, namespaced } from 'overmind/config'
import app from './app'
import auth from './auth'
import invitations from './invitations'
import myList from './myList'
import routing from './routing'

declare module 'overmind' {
  interface Config extends IConfig<typeof config> {}
}

// export interface Config extends IConfig<typeof config> {}
// export interface OnInitialize extends IOnInitialize<Config> {}
// export interface Action<Input = void> extends IAction<Config, Input> {}
// export interface Operator<Input = void, Output = Input> extends IOperator<Config, Input, Output> {}
// export interface Derive<Parent extends IState, Output> extends IDerive<Config, Parent, Output> {}

export type State = typeof config.state

type SharedState = {
  status: string
}

const state: SharedState = {
  status: ''
}

const config = merge(
  namespaced({
    auth,
    routing,
    myList,
    app,
    invitations
  }),
  {
    state
  }
)

const overmind = new Overmind(config)
export const useOvermind = createHook(overmind)
