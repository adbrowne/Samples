using System;
using System.Collections.Generic;
using WidgetServices.Services.VersionRoles;

namespace WidgetServices.Messages
{
    public class SetRoleUsersCommand
    {
        public VersionRoleType Role { get; set; }

        public IEnumerable<Guid> Users { get; set; }

        public Guid VersionId { get; set; }
    }
}