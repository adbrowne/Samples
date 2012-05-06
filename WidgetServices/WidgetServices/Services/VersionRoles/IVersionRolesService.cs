namespace WidgetServices.Services.VersionRoles
{
    using System;
    using System.Collections.Generic;

    public interface IVersionRolesService
    {
        IEnumerable<VersionRole> GetRoles(Guid versionId);

        void SetApprovers(Guid versionId, IEnumerable<Guid> approvers);
    }
}