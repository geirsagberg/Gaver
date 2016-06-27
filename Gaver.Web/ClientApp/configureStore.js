import { createStore, applyMiddleware, compose, combineReducers } from 'redux';
import * as thunkModule from 'redux-thunk';
import { routerReducer } from 'react-router-redux';
import * as Store from './store';
import { typedToPlain } from 'redux-typed';
export default function configureStore(initialState) {
    // Build middleware. These are functions that can process the actions before they reach the store.
    const thunk = thunkModule.default; // Workaround for TypeScript not importing thunk module as expected
    const windowIfDefined = typeof window === 'undefined' ? null : window;
    const devToolsExtension = windowIfDefined && windowIfDefined.devToolsExtension; // If devTools is installed, connect to it
    const createStoreWithMiddleware = compose(applyMiddleware(thunk, typedToPlain), devToolsExtension ? devToolsExtension() : f => f)(createStore);
    // Combine all reducers and instantiate the app-wide store instance
    const allReducers = buildRootReducer(Store.reducers);
    const store = createStoreWithMiddleware(allReducers, initialState);
    // Enable Webpack hot module replacement for reducers
    if (module.hot) {
        module.hot.accept('./store', () => {
            const nextRootReducer = require('./store');
            store.replaceReducer(buildRootReducer(nextRootReducer.reducers));
        });
    }
    return store;
}
function buildRootReducer(allReducers) {
    return combineReducers(Object.assign({}, allReducers, { routing: routerReducer }));
}
//# sourceMappingURL=configureStore.js.map