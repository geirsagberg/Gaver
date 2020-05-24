import * as actions from './actions'
import { OnInitialize } from '..'
import { subscribe, Topic } from '~/utils/pubSub'

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
  isSavingOrLoading: (((state) => state.ajaxCounter > 0) as unknown) as boolean,
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
