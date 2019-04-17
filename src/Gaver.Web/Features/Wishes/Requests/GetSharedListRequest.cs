using System.Security.Claims;
using Gaver.Web.Contracts;
using Gaver.Web.CrossCutting;
using Gaver.Web.Features.Wishes.Models;
using MediatR;
using Newtonsoft.Json;

namespace Gaver.Web.Features.Wishes.Requests
{
    public class GetSharedListRequest : IRequest<SharedListModel>, ISharedListRequest
    {
        [JsonIgnore]
        public int UserId { get; set; }

        [JsonIgnore]
        public int WishListId { get; set; }
    }
}
