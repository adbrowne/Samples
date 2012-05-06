namespace WidgetServices.Services.WidgetDetails
{
    using System;
    using System.Collections.Generic;

    public interface IWidgetDetailsService
    {
        void SetWidgetDetails(WidgetDetail widgetDetail);

        WidgetDetail GetWidgetDetails(Guid id);

        IEnumerable<WidgetDetail> GetWidgets();

        void CreateWidget(WidgetDetail widgetDetail);
    }
}