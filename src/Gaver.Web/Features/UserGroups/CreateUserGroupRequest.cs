using System.ComponentModel.DataAnnotations;
using Gaver.Web.Contracts;
using MediatR;
using Newtonsoft.Json;

namespace Gaver.Web.Features.UserGroups
{
    public class CreateUserGroupRequest : IRequest<UserGroupDto>, IAuthenticatedRequest
    {
        [JsonIgnore]
        public int UserId { get; set; }

        [MaxLength(40)]
        [MinLength(1)]
        [Required]
        public string Name { get; set; } = "";
    }
}
