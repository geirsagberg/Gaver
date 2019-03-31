import { RouteCallbackArgs } from '../routing/effects'
import { Action } from '..'

export const handleSharedList: Action<RouteCallbackArgs> = ({
  actions: {
    routing: { setCurrentPage }
  }
}) => {
  setCurrentPage('sharedList')
}
