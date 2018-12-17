import { applyMiddleware, combineReducers, compose, createStore, ReducersMapObject, Reducer } from 'redux'
import { connectRoutes, LocationState } from 'redux-first-router'
import thunk from 'redux-thunk'
import { isDevelopment } from '~/utils'
import { routesMap, RouteKeys } from './routing'

export default function configureStore(reducers: ReducersMapObject) {
  const windowIfDefined = typeof window === 'undefined' ? null : window
  const devToolsExtension = isDevelopment && windowIfDefined && windowIfDefined['__REDUX_DEVTOOLS_EXTENSION__']

  const router = connectRoutes(routesMap, {
    location: 'routing'
  })

  const rootReducer = buildRootReducer(reducers, router.reducer)
  const middlewares = applyMiddleware(router.middleware, thunk)
  const enhancers = compose(
    router.enhancer,
    middlewares,
    devToolsExtension ? devToolsExtension() : f => f
  )
  const store = createStore(rootReducer, enhancers)
  const updateStore = (reducers: ReducersMapObject) => store.replaceReducer(buildRootReducer(reducers, router.reducer))
  return { store, updateStore }
}

const buildRootReducer = (reducers: ReducersMapObject, routing: Reducer<LocationState<RouteKeys>>) =>
  combineReducers({
    ...reducers,
    routing
  })
