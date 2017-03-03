using Gaver.Web.Features.Wishes.Models;
using MediatR;

namespace Gaver.Web.Features.Wishes.Requests
{
    public class GetSharedListRequest : IRequest<SharedListModel>
    {
        public int ListId { get; set; }
        public int UserId { get; set; }
    }
}