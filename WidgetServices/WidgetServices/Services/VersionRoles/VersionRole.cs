namespace WidgetServices.Services.VersionRoles
{
    using System;

    public class VersionRole
    {
        public virtual Guid VersionRoleId { get; set; }
        public virtual Guid VersionId { get; set; }
        public virtual Guid PersonId { get; set; }
        public virtual VersionRoleType Role { get; set; }
    }
}