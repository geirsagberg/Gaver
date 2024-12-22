import { CurrentUserDto } from '~/types/data'
import { getJson, postJson } from '~/utils/jsonFetchers'
import { FeedbackDto } from '../overmind/app/actions'

export const getUserInfo = () => {
  return getJson<CurrentUserDto>('/api/user')
}

export const sendFeedback = (feedback: FeedbackDto) => {
  return postJson('/api/feedback', feedback)
}
