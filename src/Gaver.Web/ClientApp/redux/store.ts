import { configureStore } from 'redux-starter-kit'

import rootReducer from './reducers'

const store = configureStore({
  reducer: rootReducer
})

export default store

export type ReduxState = ReturnType<typeof store.getState>
