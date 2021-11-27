namespace Gaver.Data.Entities
{
    public class UserFriendConnection
    {
        public int UserId { get; init; }
        public int FriendId { get; init; }
        public User? User { get; set; }
        public User? Friend { get; set; }
    }
}
