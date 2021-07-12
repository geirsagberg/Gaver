import { derived } from 'overmind'
import * as actions from './actions'

export interface NavContext {
  title: string
}

type AppState = {
  ajaxCounter: number
  isSavingOrLoading: boolean
  isMenuShowing: boolean
  feedback?: boolean
  title?: string
}

const state: AppState = {
  ajaxCounter: 0,
  isSavingOrLoading: derived((state: AppState) => state.ajaxCounter > 0),
  isMenuShowing: false,
}

export default { actions, state }
