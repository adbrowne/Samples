namespace WidgetCreatorMvc.Models.Widget
{
    using System.Collections.Generic;
    using System.Linq;

    using WidgetCreatorMvc.Controllers;

    public class WidgetListViewModel
    {
        private readonly IEnumerable<WidgetSummary> widgets;

        public class WidgetListViewModelItem
        {
            public string Id { get; set; }
            public string Title { get; set; }
        }

        public WidgetListViewModel(IEnumerable<WidgetSummary> widgets)
        {
            this.widgets = widgets;
        }

        public IEnumerable<WidgetListViewModelItem> Items
        {
            get
            {
                return this.widgets.Select(x => new WidgetListViewModelItem { Id = x.Id.ToString(), Title = x.Title });
            }
        }
    }
}