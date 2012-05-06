namespace WidgetServices.Services.Widget
{
    using FluentNHibernate.Mapping;

    public sealed class WidgetDetailsMapping : ClassMap<WidgetDetails>
    {
        public WidgetDetailsMapping()
        {
            this.Id(x => x.ApprovalId).GeneratedBy.Assigned();
            this.Map(x => x.Title);
        } 
    }
}