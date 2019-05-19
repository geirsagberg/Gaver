using System.Security.Claims;
using Gaver.Web.Contracts;
using Gaver.Web.Models;
using MediatR;
using Newtonsoft.Json;

namespace Gaver.Web.Features.Wishes.Requests
{
    public class GetMyListRequest : IRequest<MyListModel>, IAuthenticatedRequest
    {
        [JsonIgnore]
        public int UserId { get; set; }
    }
}
