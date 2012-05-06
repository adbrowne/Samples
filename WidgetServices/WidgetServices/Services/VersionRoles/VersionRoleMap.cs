namespace WidgetServices.Services.VersionRoles
{
    using FluentNHibernate.Mapping;

    public sealed class VersionRoleMap : ClassMap<VersionRole>
    {
        public VersionRoleMap()
        {
            this.Id(x => x.VersionRoleId);
            this.Map(x => x.VersionId);
            this.Map(x => x.PersonId);
            this.Map(x => x.Role);
        }
    }
}