import { Schema, arrayOf } from 'normalizr'

export const wish = new Schema('wishes')
export const wishes = arrayOf(wish)

export const wishList = new Schema('wishLists')
wishList.define({
  wishes
})

export const user = new Schema('users')
