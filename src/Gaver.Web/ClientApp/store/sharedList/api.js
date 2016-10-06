import { getJson, postJson } from 'utils/ajax'
import * as schemas from 'schemas'

export function loadSharedList (listId) {
  return getJson('/Api/WishList/' + listId, schemas.wishList)
}