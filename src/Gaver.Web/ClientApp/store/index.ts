import * as data from './data'
import * as auth from './auth'
import { ReducersMapObject } from 'redux'
import { WithRouting } from '~/utils/reduxUtils'
import { ThunkAction } from 'redux-thunk'

export type GaverThunk<R = any> = ThunkAction<R, AppStateWithRouting, any, any>

export interface AppState {
  data: data.DataState
  auth: auth.AuthState
}

export const reducers: ReducersMapObject<AppState> = {
  data: data.reducer,
  auth: auth.reducer
}

export type AppStateWithRouting = AppState & WithRouting
