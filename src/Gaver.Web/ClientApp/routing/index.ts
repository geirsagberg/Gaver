import { RoutesMap, Options, redirect, RouteObject } from 'redux-first-router'
import { AppStateWithRouting } from '~/store'
import { handleAuthentication, login } from '~/store/auth/thunks'
import { getJson } from '~/utils/ajax'
import AuthService from '~/utils/AuthService'
import { selectIsLoggedIn } from '~/store/selectors'

export enum RouteType {
  FrontPage = 'ROUTE:FRONT_PAGE',
  WishList = 'ROUTE:WISH_LIST',
  AuthCallback = 'ROUTE:AUTH_CALLBACK'
}

export interface RouteKeys {
  requireLogin: boolean
}

export const routesMap: RoutesMap<RouteKeys, AppStateWithRouting> = {
  [RouteType.FrontPage]: {
    path: '/',
    requireLogin: false,
    thunk: async (dispatch, getState) => {
      if (selectIsLoggedIn(getState())) {
        const myList = await getJson('/api/WishList')
        console.log(myList)
      }
    }
  },
  [RouteType.AuthCallback]: {
    path: '/callback',
    thunk: (dispatch, getState) => {
      dispatch(handleAuthentication())
    },
    requireLogin: false
  },
  [RouteType.WishList]: {
    path: '/list/:listId',
    thunk: async (dispatch, getState) => {
      // const list = await getJson('/api/WishList/')
      // dispatch()
    },
    requireLogin: true
  }
}

export const routeActions = {
  showFrontPage: () => ({
    type: RouteType.FrontPage
  })
}

export const routingOptions: Options<RouteKeys, AppStateWithRouting> = {
  onBeforeChange: (dispatch, getState, { action }) => {
    const state = getState()
    const route = routesMap[action.type]
    if (typeof route === 'object' && route.requireLogin && !selectIsLoggedIn(state)) {
      AuthService.login()
    }
  }
}
