export interface UserModel {
  id?: number
  name?: string
  wishListId?: number
  pictureUrl?: string
}

export interface SharedWishModel extends Wish {
  boughtByUser?: UserModel
}

export interface WishListModel {
  id: number
  wishes: number[]
  invitations: number[]
  wishesOrder: number[]
}

export interface Wish {
  id?: number
  wishListId?: number
  title: string
  url?: string
  description?: string
}

export interface Invitation {
  wishListId: number
  wishListUserName: string
}

export interface ChatMessage {
  id: number
  text: string
  created: Date
  user: number
}

export interface MyListModel {
  result: number
  entities: {
    wishes: Dictionary<Wish>
    invitations: Dictionary<Invitation>
    users: Dictionary<UserModel>
    wishLists: Dictionary<WishListModel>
  }
}

export interface InvitationTokenStatus {
  ok: boolean
  error: string
  owner: string
  pictureUrl: string
}

export interface AcceptInvitationResponse {
  wishListId: number
}

export interface DeleteWishResponse {
  wishesOrder: number[]
}
