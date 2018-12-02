import * as data from './data'
import { ReducersMapObject } from 'redux'

export interface AppState {
  data: data.DataState
}

export const reducers: ReducersMapObject<AppState> = {
  data: data.reducer
}
