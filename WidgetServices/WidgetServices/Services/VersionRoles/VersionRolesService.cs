namespace WidgetServices.Services.VersionRoles
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using NHibernate;
    using NHibernate.Criterion;

    public class VersionRolesService : IVersionRolesService
    {
        private readonly ISession _session;

        public VersionRolesService(UnitOfWork unitOfWork)
        {
            _session = unitOfWork.Session;
        }
        public IEnumerable<VersionRole> GetRoles(Guid versionId)
        {
            return _session.CreateCriteria<VersionRole>()
               .Add(Restrictions.Eq("VersionId", versionId))
               .List<VersionRole>();
        }

        public void SetApprovers(Guid versionId, IEnumerable<Guid> approvers)
        {
            Debug.Assert(approvers != null, "approvers != null");

            var currentApprovers = _session.CreateCriteria<VersionRole>()
               .Add(Restrictions.Eq("VersionId", versionId))
               .Add(Restrictions.Eq("Role", VersionRoleType.Approver))
               .List<VersionRole>();

            // Remove people no longer selected
            foreach (var currentApprover in currentApprovers)
            {
                if (!approvers.Contains(currentApprover.PersonId))
                {
                    _session.Delete(currentApprover);
                }
            }

            // Add people missing
            foreach (var personId in approvers)
            {
               if(!currentApprovers.Any(x => x.PersonId == personId))
               {
                   _session.Save(
                       new VersionRole
                           {
                               PersonId = personId, Role = VersionRoleType.Approver, VersionId = versionId
                           });
               }
            }
        }
    }
}