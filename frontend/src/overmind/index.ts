import { map, size } from 'lodash-es'
import { derived, IContext } from 'overmind'
import {
  createActionsHook,
  createEffectsHook,
  createStateHook,
} from 'overmind-react'
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

export type Context = IContext<typeof config>
export type RootState = Context['state']

type SharedState = {
  currentSharedList: SharedList | null
  currentSharedOrderedWishes: SharedWish[] | null
  currentSharedListOwner: User | null
}

const state: SharedState = {
  currentSharedList: derived((state: RootState) =>
    state.routing.currentSharedListId &&
    state.sharedLists.wishLists[state.routing.currentSharedListId]
      ? state.sharedLists.wishLists[state.routing.currentSharedListId]
      : null
  ),
  currentSharedOrderedWishes: derived((state: RootState) =>
    state.currentSharedList && state.currentSharedList.wishesOrder
      ? map(
          state.currentSharedList.wishesOrder,
          (id) => state.currentSharedList!.wishes[id]
        )
      : state.currentSharedList && size(state.currentSharedList.wishes) === 0
      ? []
      : null
  ),
  currentSharedListOwner: derived((state: RootState) =>
    state.currentSharedList
      ? state.sharedLists.users[state.currentSharedList.ownerUserId]
      : null
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

export const useAppState = createStateHook<Context>()
export const useActions = createActionsHook<Context>()
export const useEffects = createEffectsHook<Context>()
export const useOvermind = () => {
  const state = useAppState()
  const actions = useActions()
  const effects = useEffects()
  return { state, actions, effects }
}

function createNamespaceHook<T extends keyof Context['state']>(
  key: T
): () => {
  state: Context['state'][T]
  actions: T extends keyof Context['actions'] ? Context['actions'][T] : never
  effects: T extends keyof Context['effects'] ? Context['effects'][T] : never
} {
  return () => {
    const state = useAppState()
    const actions = useActions()
    const effects = useEffects()
    return {
      state: state[key],
      actions:
        key in actions
          ? actions[key as keyof Context['actions']]
          : (undefined as any),
      effects:
        key in effects
          ? effects[key as keyof Context['effects']]
          : (undefined as any),
    }
  }
}

export const useMyList = createNamespaceHook('myList')
export const useChat = createNamespaceHook('chat')

export function useNamespace<T extends keyof Context['state']>(
  key: T
): {
  state: Context['state'][T]
  actions: T extends keyof Context['actions'] ? Context['actions'][T] : never
  effects: T extends keyof Context['effects'] ? Context['effects'][T] : never
} {
  const state = useAppState()
  const actions = useActions()
  const effects = useEffects()

  return {
    state: state[key],
    actions:
      key in actions
        ? actions[key as keyof Context['actions']]
        : (undefined as any),
    effects:
      key in effects
        ? effects[key as keyof Context['effects']]
        : (undefined as any),
  }
}
