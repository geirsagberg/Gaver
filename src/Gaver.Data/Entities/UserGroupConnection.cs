namespace Gaver.Data.Entities
{
    public class UserGroupConnection
    {
        public int UserId { get; set; }
        public int UserGroupId { get; set; }
        public User? User { get; set; }
        public UserGroup? UserGroup { get; set; }
    }
}
