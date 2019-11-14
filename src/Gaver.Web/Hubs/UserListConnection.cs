namespace Gaver.Web.Hubs
{
    public class UserListConnection
    {
        public int UserId { get; set; }
        public int ListId { get; set; }
        public string ConnectionId { get; set; } = "";
    }
}
