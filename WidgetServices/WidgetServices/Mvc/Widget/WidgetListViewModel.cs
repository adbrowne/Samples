namespace WidgetServices.Mvc.Widget
{
    using System.Collections.Generic;

    using WidgetServices.Services.Approval;

    public class WidgetListViewModel
    {
        public IEnumerable<WidgetDetails> Widgets { get; set; }
    }
}