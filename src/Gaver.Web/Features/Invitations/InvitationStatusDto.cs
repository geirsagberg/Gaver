namespace Gaver.Web.Features.Invitations
{
    public class InvitationStatusDto
    {
        public bool Ok { get; set; }
        public string? Error { get; set; }
        public string? Owner { get; set; }
        public string? PictureUrl { get; set; }
    }
}
