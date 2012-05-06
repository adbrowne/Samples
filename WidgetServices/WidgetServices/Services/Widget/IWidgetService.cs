namespace WidgetServices.Services.Widget
{
    using System;
    using System.Collections.Generic;

    public interface IWidgetService
    {
        void SetWidgetDetails(WidgetDetails widgetDetails);

        WidgetDetails GetWidgetDetails(Guid id);

        IEnumerable<WidgetDetails> GetWidgets();
    }
}