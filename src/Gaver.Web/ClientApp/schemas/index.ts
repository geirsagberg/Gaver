import { schema } from 'normalizr'

export const wish = new schema.Entity('wishes')
export const wishes = [wish]

export const invitation = new schema.Entity(
  'invitations',
  {},
  {
    idAttribute: 'wishListId'
  }
)
export const invitations = [invitation]

export const wishList = new schema.Entity('wishLists')
wishList.define({
  wishes,
  invitations
})

export const user = new schema.Entity('users')

export const users = [user]

wish.define({
  boughtByUser: user
})

export const message = new schema.Entity('messages')

message.define({
  user
})

export const messages = [message]

export const chat = new schema.Entity('chat')

chat.define({
  messages
})
