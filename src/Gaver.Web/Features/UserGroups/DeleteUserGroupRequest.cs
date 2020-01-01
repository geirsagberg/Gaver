using Gaver.Web.Contracts;
using MediatR;
using Newtonsoft.Json;

namespace Gaver.Web.Features.UserGroups
{
    public class DeleteUserGroupRequest : IRequest, IUserGroupRequest
    {
        [JsonIgnore]
        public int UserId { get; set; }

        [JsonIgnore]
        public int UserGroupId { get; set; }
    }
}
