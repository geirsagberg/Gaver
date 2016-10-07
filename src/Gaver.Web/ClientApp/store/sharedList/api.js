import { getJson, postJson } from 'utils/ajax'
import * as schemas from 'schemas'

export function loadSharedList (listId) {
  return getJson('/api/WishList/' + listId, schemas.wishList)
}