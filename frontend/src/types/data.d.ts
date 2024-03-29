export interface FeatureFlags {
  wishOptions: boolean
  userGroups: boolean
}
export interface CurrentUserDto {
  id: number
  wishListId: number
  name: string
  pictureUrl: string
}
export interface GetFriendsRequest {
  userId: number
}
export interface GetOrCreateUserRequest {
  primaryIdentityId: string
}
export interface GetUserInfoRequest {
  userId: number
}
export interface UpdateUserInfoRequest {
  userId: number
}
export interface UserDto {
  id: number
  wishListId: number
  name: string
  pictureUrl: string
}
export interface CreateUserGroupRequest {
  name: string
  userIds: number[]
}
export interface GetMyUserGroupsRequest {
  userId: number
}
export interface UpdateUserGroupRequest {
  name: string
  userIds: number[]
}
export interface UserGroupDto {
  id: number
  name: string
  userIds: number[]
  createdByUserId: number
}
export interface UserGroupsDto {
  userGroups: UserGroupDto[]
}
export interface SharedListDto {
  id: number
  wishes: SharedWishDto[]
  users: UserDto[]
  ownerUserId: number
  wishesOrder: number[]
  canSeeMyList: boolean
}
export interface SharedWishDto {
  id: number
  title: string
  url: string
  options: WishOptionDto[]
  boughtByUserId?: number
}
export interface WishOptionDto {
  id: number
  title: string
  url: string
}
export interface SetBoughtRequest {
  isBought: boolean
}
export interface AddWishOptionRequest {
  title: string
  url: string
}
export interface AddWishRequest {
  title: string
  url: string
}
export interface DeleteWishResponse {
  wishesOrder: number[]
}
export interface ResetListRequest {
  keepWishes: number[]
}
export interface MyListDto {
  id: number
  title: string
  wishes: WishDto[]
  wishesOrder: number[]
}
export interface WishDto {
  id: number
  title: string
  url: string
  options: WishOptionDto[]
}
export interface SetWishesOrderRequest {
  wishesOrder: number[]
}
export interface ShareListRequest {
  emails: string[]
}
export interface UpdateWishRequest {
  title: string
  url: string
}
export interface InvitationStatusDto {
  owner: string
  pictureUrl: string
  ownerId: number
}
export interface SubmitFeedbackRequest {
  userId: number
  message: string
  anonymous: boolean
}
export interface AddMessageRequest {
  text: string
}
export interface ChatDto {
  messages: ChatMessageDto[]
}
export interface ChatMessageDto {
  id: number
  text: string
  created: string
  user: ChatUserDto
}
export interface ChatUserDto {
  id: number
  name: string
  pictureUrl: string
}
export interface AuthSettingsDto {
  clientId: string
  domain: string
  audience: string
}
