import { UserModel } from '~/types/data'
import { getJson, postJson } from '~/utils/ajax'
import { FeedbackModel } from '../app/actions'

export const getUserInfo = () => {
  return getJson<UserModel>('/api/user')
}

export const sendFeedback = (feedback: FeedbackModel) => {
  return postJson('/api/feedback', feedback)
}
