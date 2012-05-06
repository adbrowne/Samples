namespace WidgetServices.Services.Version
{
    using FluentNHibernate.Mapping;

    public sealed class WidgetVersionMap : ClassMap<WidgetVersion>
    {
        public WidgetVersionMap()
        {
            this.Id(x => x.VersionId);
            this.Map(x => x.WidgetId);
        }
    }
}