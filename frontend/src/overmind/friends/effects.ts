import { UserDto } from '~/types/data'
import { getJson } from '~/utils/jsonFetchers'

export const getFriends = () => getJson<UserDto[]>('/api/friends')
