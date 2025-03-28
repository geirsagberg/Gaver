using System.Text.Json.Serialization;
using Gaver.Web.Contracts;
using HybridModelBinding;
using MediatR;

namespace Gaver.Web.Features.UserGroups;

public class DeleteUserGroupRequest : IRequest, IUserGroupRequest {
    [JsonIgnore] public int UserId { get; set; }

    [HybridBindProperty(Source.Route)]
    [JsonIgnore]
    public int UserGroupId { get; set; }
}
