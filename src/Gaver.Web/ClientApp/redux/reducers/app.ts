import { createReducer } from '../reduxUtils'

interface AppState {
  isSavingOrLoading: boolean
}

const initialState: AppState = {
  isSavingOrLoading: false
}

export default createReducer<AppState, any>(initialState, () => {})
