using Gaver.Web.Contracts;
using MediatR;
using System.Text.Json.Serialization;

namespace Gaver.Web.Features.MyList
{
    public class GetMyListRequest : IRequest<MyListDto>, IAuthenticatedRequest
    {
        [JsonIgnore]
        public int UserId { get; set; }
    }
}
