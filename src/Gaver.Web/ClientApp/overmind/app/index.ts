import * as actions from './actions'
import { OnInitialize, Derive } from '..'
import { subscribe, Topic } from '~/utils/pubSub'

type AppState = {
  ajaxCounter: number
  isSavingOrLoading: Derive<AppState, boolean>
}

const state: AppState = {
  ajaxCounter: 0,
  isSavingOrLoading: state => state.ajaxCounter > 0
}

const onInitialize: OnInitialize = ({
  actions: {
    app: { incrementAjaxCounter, decrementAjaxCounter }
  }
}) => {
  subscribe(Topic.AjaxStart, incrementAjaxCounter)
  subscribe(Topic.AjaxStop, decrementAjaxCounter)
}

export default { actions, state, onInitialize }