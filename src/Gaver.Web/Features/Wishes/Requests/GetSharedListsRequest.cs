using Gaver.Web.Contracts;
using Gaver.Web.Features.Wishes.Models;
using MediatR;

namespace Gaver.Web.Features.Wishes.Requests
{
    public class GetSharedListsRequest : IRequest<SharedListsModel>, IAuthenticatedRequest
    {
        public int UserId { get; set; }
    }
}