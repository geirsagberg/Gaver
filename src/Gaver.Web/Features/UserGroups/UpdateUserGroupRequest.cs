using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Gaver.Web.Contracts;
using MediatR;
using Newtonsoft.Json;

namespace Gaver.Web.Features.UserGroups
{
    public class UpdateUserGroupRequest : IRequest, IUserGroupRequest
    {
        [JsonIgnore]
        public int UserId { get; set; }

        [JsonIgnore]
        public int UserGroupId { get; set; }

        [MinLength(1)]
        [MaxLength(40)]
        public string? Name { get; set; }

        [MinLength(1)]
        public List<int>? UserIds { get; set; }
    }
}
