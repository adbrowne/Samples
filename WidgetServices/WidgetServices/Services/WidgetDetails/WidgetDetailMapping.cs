﻿namespace WidgetServices.Services.WidgetDetails
{
    using FluentNHibernate.Mapping;

    public sealed class WidgetDetailMapping : ClassMap<WidgetDetail>
    {
        public WidgetDetailMapping()
        {
            this.Id(x => x.WidgetId).GeneratedBy.Assigned();
            this.Map(x => x.Title);
        } 
    }
}