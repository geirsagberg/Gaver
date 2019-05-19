import { UserModel } from '~/types/data'
import { getJson } from '~/utils/ajax'

export const getUserInfo = () => {
  return getJson<UserModel>('/api/user')
}
