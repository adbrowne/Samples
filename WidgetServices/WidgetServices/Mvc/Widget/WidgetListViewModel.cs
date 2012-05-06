namespace WidgetServices.Mvc.Widget
{
    using System.Collections.Generic;

    using WidgetServices.Services.Widget;

    public class WidgetListViewModel
    {
        public IEnumerable<WidgetDetails> Widgets { get; set; }
    }
}