import { RoutesMap } from 'redux-first-router'

export enum RouteType {
  FrontPage = 'ROUTE:FRONT_PAGE',
  WishList = 'ROUTE:WISH_LIST'
}

export const routesMap: RoutesMap = {
  [RouteType.FrontPage]: {
    path: '/'
  },
  [RouteType.WishList]: {
    path: '/list/:listId'
  }
}
