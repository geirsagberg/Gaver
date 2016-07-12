import { createStore, applyMiddleware, compose, combineReducers } from 'redux'
import { routerReducer } from 'react-router-redux'
import * as Store from './store'
import createSagaMiddleware from 'redux-saga'
import Immutable from 'seamless-immutable'

export default function configureStore (initialState) {
  // Build middleware. These are functions that can process the actions before they reach the store.
  const sagaMiddleware = createSagaMiddleware()
  // const thunk = thunkModule
  const windowIfDefined = typeof window === 'undefined' ? null : window
  const devToolsExtension = windowIfDefined && windowIfDefined.devToolsExtension // If devTools is installed, connect to it
  const createStoreWithMiddleware = compose(applyMiddleware(sagaMiddleware), devToolsExtension ? devToolsExtension() : f => f)(createStore)
  // Combine all reducers and instantiate the app-wide store instance
  const allReducers = buildRootReducer(Store.reducers)
  const immutableInitialState = Immutable(initialState)
  const store = createStoreWithMiddleware(allReducers, immutableInitialState)

  sagaMiddleware.run(Store.rootSaga)

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
  return combineReducers({
    ...allReducers,
    routing: routerReducer
  })
}
