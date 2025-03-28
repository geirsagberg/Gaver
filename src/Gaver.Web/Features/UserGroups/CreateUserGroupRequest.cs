using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Gaver.Web.Contracts;
using MediatR;

namespace Gaver.Web.Features.UserGroups;

public class CreateUserGroupRequest : IRequest<UserGroupDto>, IAuthenticatedRequest {
    [MaxLength(40)]
    [MinLength(1)]
    [Required]
    public string Name { get; set; } = "";

    public List<int> UserIds { get; set; } = [];

    [JsonIgnore] public int UserId { get; set; }
}
