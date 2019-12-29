import { showSuccess } from '~/utils/notifications'
import { NavContext } from '.'
import { Action } from '../'

export const incrementAjaxCounter: Action = ({ state: { app } }) => {
  app.ajaxCounter += 1
}

export const decrementAjaxCounter: Action = ({ state: { app } }) => {
  app.ajaxCounter -= 1
}

export const showMenu: Action = ({ state: { app } }) => {
  app.isMenuShowing = true
}

export const hideMenu: Action = ({ state: { app } }) => {
  app.isMenuShowing = false
}

export const showFeedback: Action = ({ state: { app } }) => {
  app.feedback = true
}

export const cancelFeedback: Action = ({ state: { app } }) => {
  app.feedback = false
}

export const setNavContext: Action<NavContext> = ({ state: { app } }, navContext) => {
  app.title = navContext.title
}

export interface FeedbackDto {
  message: string
  anonymous: boolean
}

export const sendFeedback: Action<FeedbackDto> = async (
  {
    state: { app },
    effects: {
      api: { sendFeedback }
    }
  },
  feedback
) => {
  await sendFeedback(feedback)
  showSuccess('Takk for tilbakemeldingen!')
  app.feedback = false
  return true
}
