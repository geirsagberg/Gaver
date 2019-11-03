using Gaver.Web.Contracts;
using Gaver.Web.Models;
using MediatR;
using Newtonsoft.Json;

namespace Gaver.Web.Features.MyList
{
    public class GetMyListRequest : IRequest<MyListModel>, IAuthenticatedRequest
    {
        [JsonIgnore]
        public int UserId { get; set; }
    }
}
