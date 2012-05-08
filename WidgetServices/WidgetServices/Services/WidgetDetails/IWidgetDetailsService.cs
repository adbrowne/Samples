namespace WidgetServices.Services.WidgetDetails
{
    using System;
    using System.Collections.Generic;

    public interface IWidgetDetailsService
    {
        WidgetDetail GetWidgetDetails(Guid id);

        IEnumerable<WidgetDetail> GetWidgets();
    }
}