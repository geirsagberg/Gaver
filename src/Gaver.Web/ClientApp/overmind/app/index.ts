import { derived } from 'overmind'
import { subscribe, Topic } from '~/utils/pubSub'
import { OnInitialize } from '..'
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

const onInitialize: OnInitialize = ({
  actions: {
    app: { incrementAjaxCounter, decrementAjaxCounter },
  },
}) => {
  subscribe(Topic.AjaxStart, incrementAjaxCounter)
  subscribe(Topic.AjaxStop, decrementAjaxCounter)
}

export default { actions, state, onInitialize }
