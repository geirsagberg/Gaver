import { createStore, applyMiddleware, compose, combineReducers } from 'redux'
import thunkModule from 'redux-thunk'
import { routerReducer } from 'react-router-redux'
import * as Store from './store'
export default function configureStore (initialState) {
  // Build middleware. These are functions that can process the actions before they reach the store.
  const thunk = thunkModule
  const windowIfDefined = typeof window === 'undefined' ? null : window
  const devToolsExtension = windowIfDefined && windowIfDefined.devToolsExtension // If devTools is installed, connect to it
  const createStoreWithMiddleware = compose(applyMiddleware(thunk), devToolsExtension ? devToolsExtension() : f => f)(createStore)
  // Combine all reducers and instantiate the app-wide store instance
  const allReducers = buildRootReducer(Store.reducers)
  const store = createStoreWithMiddleware(allReducers, initialState)
  // Enable Webpack hot module replacement for reducers
  if (module.hot) {
    module.hot.accept('./store', () => {
      const nextRootReducer = require('./store')
      store.replaceReducer(buildRootReducer(nextRootReducer.reducers))
    })
  }
  return store
}
function buildRootReducer (allReducers) {
  return combineReducers(Object.assign({}, allReducers, { routing: routerReducer }))
}
