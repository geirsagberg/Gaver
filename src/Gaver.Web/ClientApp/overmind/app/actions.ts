import { Action } from '../'

export const incrementAjaxCounter: Action = ({ state: { app } }) => {
  app.ajaxCounter += 1
}

export const decrementAjaxCounter: Action = ({ state: { app } }) => {
  app.ajaxCounter -= 1
}
