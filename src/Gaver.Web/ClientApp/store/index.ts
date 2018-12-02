import * as data from './data'
import { ReducersMapObject } from 'redux'
import { WithRouting } from '~/utils/reduxUtils'

export interface AppState {
  data: data.DataState
}

export const reducers: ReducersMapObject<AppState> = {
  data: data.reducer
}

export type AppStateWithRouting = AppState & WithRouting
