export interface FeatureFlags { wishOptions: boolean; userGroups: boolean; }
export interface CurrentUserDto { wishListId: number; id: number; name: string; pictureUrl: string; }
export interface GetOrCreateUserRequest { primaryIdentityId: string; }
export interface GetUserInfoRequest { userId: number; }
export interface UpdateUserInfoRequest { userId: number; }
export interface GetFriendsRequest { userId: number; }
export interface UserDto { id: number; name: string; pictureUrl: string; }
export interface CreateUserGroupRequest { name: string; }
export interface GetMyUserGroupsRequest { userId: number; }
export interface UpdateUserGroupRequest { name: string; userIds: number[]; }
export interface UserGroupDto { id: number; name: string; userIds: number[]; }
export interface UserGroupsDto { userGroups: UserGroupDto[]; }
export interface WishOptionDto { id: number; title: string; url: string; }
export interface SharedListDto { id: number; wishes: SharedWishDto[]; users: UserDto[]; ownerUserId: number; wishesOrder: number[]; canSeeMyList: boolean; }
export interface SharedWishDto { id: number; title: string; url: string; options: WishOptionDto[]; boughtByUserId?: number; }
export interface SharedListsDto { invitations: FriendDto[]; }
export interface FriendDto { userId: number; wishListId: number; userName: string; }
export interface CheckSharedListAccessRequest { wishListId: number; }
export interface GetSharedListsRequest { userId: number; }
export interface SetBoughtRequest { isBought: boolean; }
export interface AddWishOptionRequest { userId: number; wishId: number; title: string; url: string; }
export interface AddWishRequest { title: string; url: string; }
export interface DeleteWishRequest { wishId: number; userId: number; }
export interface DeleteWishResponse { wishesOrder: number[]; }
export interface InviteUserRequest { userId: number; inviteUserId: number; }
export interface MyListDto { id: number; title: string; wishes: WishDto[]; wishesOrder: number[]; }
export interface WishDto { id: number; title: string; url: string; options: WishOptionDto[]; }
export interface SetWishesOrderRequest { wishesOrder: number[]; }
export interface ShareListRequest { emails: string[]; }
export interface UpdateWishRequest { title: string; url: string; }
export interface AcceptInvitationRequest { token: string; userId: number; }
export interface GetInvitationStatusRequest { token: string; userId: number; }
export interface InvitationStatusDto { ok: boolean; error: string; owner: string; pictureUrl: string; }
export interface SubmitFeedbackRequest { userId: number; message: string; anonymous: boolean; }
export interface AddMessageRequest { text: string; }
export interface ChatDto { messages: ChatMessageDto[]; }
export interface ChatMessageDto { id: number; text: string; created: string; user: UserDto; }
