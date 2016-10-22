import { Schema, arrayOf } from 'normalizr'

export const wish = new Schema('wishes')

export const wishes = arrayOf(wish)

export const wishList = new Schema('wishLists')
wishList.define({
  wishes
})

export const user = new Schema('users')

export const users = arrayOf(user)

wish.define({
  boughtByUser: user
})

export const message = new Schema('messages')

message.define({
  user
})

export const messages = arrayOf(message)

export const chat = new Schema('chat')

chat.define({
  messages
})