import { createStore, applyMiddleware, compose, combineReducers, StoreCreator } from 'redux'
import thunk from 'redux-thunk'
import * as Store from './store'
import Immutable from 'seamless-immutable'
import { initAuth } from './store/user'
import history from 'utils/history'
import { routerMiddleware as createRouterMiddleware, routerReducer } from 'react-router-redux'
import { isDevelopment } from 'utils'

function buildRootReducer(allReducers) {
  return combineReducers({
    ...allReducers,
    router: routerReducer
  })
}

export default function configureStore(initialState) {
  // Build middleware. These are functions that can process the actions before they reach the store.
  const windowIfDefined = typeof window === 'undefined' ? null : window
  const devToolsExtension = windowIfDefined && windowIfDefined['devToolsExtension'] // If devTools is installed, connect to it

  const routerMiddleware = createRouterMiddleware(history)

  const middleware = [thunk, routerMiddleware]

  if (isDevelopment) middleware.unshift(require('redux-immutable-state-invariant').default)

  const createStoreWithMiddleware: StoreCreator = compose<StoreCreator>(
    applyMiddleware(thunk, routerMiddleware),
    devToolsExtension ? devToolsExtension() : f => f
  )(createStore)
  // Combine all reducers and instantiate the app-wide store instance
  const rootReducer = buildRootReducer(Store.reducers)
  const immutableInitialState = Immutable(initialState)
  const store = createStoreWithMiddleware(rootReducer, immutableInitialState)
  // const store = createStore(rootReducer, immutableInitialState,)

  // Enable Webpack hot module replacement for reducers
  if (module.hot) {
    module.hot.accept('./store', () => {
      const nextRootReducer = require('./store')
      store.replaceReducer(buildRootReducer(nextRootReducer.reducers))
    })
  }
  store.dispatch(initAuth() as any)
  return store
}
