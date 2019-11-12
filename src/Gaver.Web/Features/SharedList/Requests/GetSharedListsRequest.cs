using Gaver.Web.Contracts;
using MediatR;

namespace Gaver.Web.Features.SharedList.Requests
{
    public class GetSharedListsRequest : IRequest<SharedListsDto>, IAuthenticatedRequest
    {
        public int UserId { get; set; }
    }
}