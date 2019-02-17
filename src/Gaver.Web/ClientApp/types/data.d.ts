export interface UserModel {
  id?: number
  name?: string
  wishListId?: number
  pictureUrl?: string
}

export interface SharedWishModel extends Wish {
  boughtByUser?: UserModel
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
  }
}
