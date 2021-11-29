namespace Gaver.Data.Entities;

public class UserGroupConnection
{
    public int UserId { get; init; }
    public int UserGroupId { get; init; }
    public User? User { get; set; }
    public UserGroup? UserGroup { get; set; }
}