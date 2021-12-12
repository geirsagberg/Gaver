import { showSuccess } from '~/utils/notifications'
import { subscribe, Topic } from '~/utils/pubSub'
import { NavContext } from '.'
import { Context } from '../'

export const incrementAjaxCounter = ({ state: { app } }: Context) => {
  app.ajaxCounter += 1
}

export const decrementAjaxCounter = ({ state: { app } }: Context) => {
  app.ajaxCounter -= 1
}

export const showMenu = ({ state: { app } }: Context) => {
  app.isMenuShowing = true
}

export const hideMenu = ({ state: { app } }: Context) => {
  app.isMenuShowing = false
}

export const showFeedback = ({ state: { app } }: Context) => {
  app.feedback = true
}

export const cancelFeedback = ({ state: { app } }: Context) => {
  app.feedback = false
}

export const setNavContext = ({ state: { app } }: Context, navContext: NavContext) => {
  app.title = navContext.title
}

export interface FeedbackDto {
  message: string
  anonymous: boolean
}

export const sendFeedback = async (
  {
    state: { app },
    effects: {
      api: { sendFeedback },
    },
  }: Context,
  feedback: FeedbackDto
) => {
  await sendFeedback(feedback)
  showSuccess('Takk for tilbakemeldingen!')
  app.feedback = false
  return true
}

export const onInitializeOvermind = ({ actions }: Context) => {
  subscribe(Topic.AjaxStart, actions.app.incrementAjaxCounter)
  subscribe(Topic.AjaxStop, actions.app.decrementAjaxCounter)
}
