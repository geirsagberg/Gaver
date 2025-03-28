using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Gaver.Web.Contracts;
using HybridModelBinding;
using MediatR;

namespace Gaver.Web.Features.UserGroups;

public class UpdateUserGroupRequest : IRequest, IUserGroupRequest {
    [MinLength(1)] [MaxLength(40)] public string? Name { get; set; }

    [MinLength(1)] public List<int>? UserIds { get; set; }

    [JsonIgnore] public int UserId { get; set; }

    [HybridBindProperty(Source.Route)]
    [JsonIgnore]
    public int UserGroupId { get; set; }
}
