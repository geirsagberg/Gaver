import { getJson } from '~/utils/ajax'
import { UserDto } from '~/types/data'

export const getFriends = () => getJson<UserDto[]>('/api/friends')
