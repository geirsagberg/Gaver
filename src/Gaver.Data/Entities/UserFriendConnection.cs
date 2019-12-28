namespace Gaver.Data.Entities
{
    public class UserFriendConnection
    {
        public int UserId { get; set; }
        public int FriendId { get; set; }
        public User? User { get; set; }
        public User? Friend { get; set; }
    }
}
