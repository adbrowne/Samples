namespace WidgetServices.Mvc.Widget
{
    using System.Collections.Generic;

    using WidgetServices.Services.WidgetDetails;

    public class WidgetListViewModel
    {
        public IEnumerable<WidgetDetail> Widgets { get; set; }
    }
}