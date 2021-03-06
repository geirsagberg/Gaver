﻿using System.Collections.Generic;

namespace Gaver.Web.Features.UserGroups
{
    public class UserGroupDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public List<int> UserIds { get; set; } = new List<int>();
        public int CreatedByUserId { get; set; }
    }
}
