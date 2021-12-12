using System.ComponentModel.DataAnnotations;
using Gaver.Web.Contracts;
using HybridModelBinding;
using MediatR;
using System.Text.Json.Serialization;

namespace Gaver.Web.Features.UserGroups;

public class UpdateUserGroupRequest : IRequest, IUserGroupRequest
{

    [JsonIgnore]
    public int UserId { get; set; }

    [HybridBindProperty(Source.Route)]
    [JsonIgnore]
    public int UserGroupId { get; set; }

    [MinLength(1)]
    [MaxLength(40)]
    public string? Name { get; set; }

    [MinLength(1)]
    public List<int>? UserIds { get; set; }
}