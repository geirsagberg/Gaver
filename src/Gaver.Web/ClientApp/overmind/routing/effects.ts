import pageJs from 'page'
import { Action } from '..'
import { Page, PageMap } from './state'

const pageMap: PageMap = {
  start: '/',
  myList: '/myList',
  authCallback: '/callback',
  notFound: '/notFound'
}

export function route(route: string, action: Action) {
  pageJs(route, ({ params }) => action(params))
}
export const start = pageJs.start
export const show = (page: Page) => pageJs.show(pageMap[page])
export const redirect = pageJs.redirect
