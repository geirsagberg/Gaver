using Gaver.Web.Contracts;
using HybridModelBinding;
using MediatR;
using System.Text.Json.Serialization;

namespace Gaver.Web.Features.UserGroups
{
    public class DeleteUserGroupRequest : IRequest, IUserGroupRequest
    {
        [JsonIgnore]
        public int UserId { get; set; }

        [HybridBindProperty(Source.Route)]
        [JsonIgnore]
        public int UserGroupId { get; set; }
    }
}
