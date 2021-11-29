using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Gaver.Web.Contracts;
using MediatR;
using System.Text.Json.Serialization;

namespace Gaver.Web.Features.UserGroups;

public class CreateUserGroupRequest : IRequest<UserGroupDto>, IAuthenticatedRequest
{
    [JsonIgnore]
    public int UserId { get; set; }

    [MaxLength(40)]
    [MinLength(1)]
    [Required]
    public string Name { get; set; } = "";

    public List<int> UserIds { get; set; } = new();
}