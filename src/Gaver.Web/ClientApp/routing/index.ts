import { RoutesMap } from 'redux-first-router'

export enum RouteType {
  FrontPage,
  WishList
}

export const routesMap: RoutesMap = {
  [RouteType.FrontPage]: {
    path: '/'
  },
  [RouteType.WishList]: {
    path: '/list/:listId'
  }
}
