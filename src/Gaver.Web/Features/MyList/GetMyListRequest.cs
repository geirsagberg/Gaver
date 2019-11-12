using Gaver.Web.Contracts;
using MediatR;
using Newtonsoft.Json;

namespace Gaver.Web.Features.MyList
{
    public class GetMyListRequest : IRequest<MyListDto>, IAuthenticatedRequest
    {
        [JsonIgnore]
        public int UserId { get; set; }
    }
}
