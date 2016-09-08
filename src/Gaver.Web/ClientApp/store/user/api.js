import { postJson } from 'utils/ajax'
import * as schemas from 'schemas'

export function logIn (name) {
  return postJson('/api/User/Login', { name }, schemas.user)
}
