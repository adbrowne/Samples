namespace WidgetServices.Services.VersionRoles
{
    using System;
    using System.Collections.Generic;
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
            this.UpdateRole(versionId, approvers, VersionRoleType.Approver);
        }
        
        public void SetViewers(Guid versionId, IEnumerable<Guid> viewers)
        {
            this.UpdateRole(versionId, viewers, VersionRoleType.Viewer);
        }

        private void UpdateRole(Guid versionId, IEnumerable<Guid> newMembers, VersionRoleType versionRoleType)
        {
            var membersToAdd = newMembers ?? new List<Guid>();

            var currentApprovers =
                this._session.CreateCriteria<VersionRole>().Add(Restrictions.Eq("VersionId", versionId)).Add(
                    Restrictions.Eq("Role", versionRoleType)).List<VersionRole>();

            // Remove people no longer selected
            foreach (var currentApprover in currentApprovers)
            {
                if (!membersToAdd.Contains(currentApprover.PersonId))
                {
                    this._session.Delete(currentApprover);
                }
            }

            // Add people missing
            foreach (var personId in membersToAdd)
            {
                if (!currentApprovers.Any(x => x.PersonId == personId))
                {
                    this._session.Save(
                        new VersionRole { PersonId = personId, Role = versionRoleType, VersionId = versionId });
                }
            }
        }
    }
}