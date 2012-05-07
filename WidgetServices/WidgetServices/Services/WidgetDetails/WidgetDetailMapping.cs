namespace WidgetServices.Services.WidgetDetails
{
    using FluentNHibernate.Mapping;

    using WidgetServices.Messages;

    public sealed class WidgetDetailMapping : ClassMap<WidgetDetail>
    {
        public WidgetDetailMapping()
        {
            this.Id(x => x.WidgetId).GeneratedBy.Assigned();
            this.Map(x => x.Title);
        } 
    }

    public sealed class CreateWidgetCommandMapping : SubclassMap<CreateWidgetCommand>
    {
        
    }
}