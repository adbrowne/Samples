namespace WidgetServices.Services.Approval
{
    using FluentNHibernate.Mapping;

    public sealed class ApprovalDetailsMapping : ClassMap<WidgetDetails>
    {
        public ApprovalDetailsMapping()
        {
            Id(x => x.ApprovalId).GeneratedBy.Assigned();
            Map(x => x.Title);
        } 
    }
}