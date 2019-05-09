import pageJs from 'page'

export type RouteCallbackArgs = {
  params: any
  next: () => any
}

export type RouteCallback = (args: RouteCallbackArgs) => void

export function route(route: string, ...actions: RouteCallback[]) {
  const callbacks = actions.map(action => ({ params }, next) => action({ params, next }))
  pageJs(route, ...callbacks)
}
export const start = pageJs.start
export const redirect = pageJs.redirect
export const exit = pageJs.exit

export const showStartPage = () => pageJs.show('/')
export const showMyList = () => pageJs.show('/myList')
export const showSharedList = (wishListId: number) => pageJs.show(`/list/${wishListId}`)
