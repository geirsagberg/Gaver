// Separate types from API vs store
// Store per entity type?
// No, MyList is separate
//

export interface UserModel {
  id?: number
  name?: string
  wishListId?: number
  pictureUrl?: string
}

export interface SharedWishModel extends WishModel {
  boughtByUserId?: number
}

export interface WishListModel {
  id: number
  wishes: WishModel[]
  wishesOrder: number[]
}

export interface SharedListModel extends WishListModel {
  ownerUserId: number
  users: UserModel[]
  wishes: SharedWishModel[]
}

export interface WishModel {
  id?: number
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

export interface MyListModel extends WishListModel {}

export interface SharedListsModel {
  invitations: Invitation[]
}

export interface InvitationTokenStatus {
  ok: boolean
  error: string
  owner: string
  pictureUrl: string
}

export interface DeleteWishResponse {
  wishesOrder: number[]
}
