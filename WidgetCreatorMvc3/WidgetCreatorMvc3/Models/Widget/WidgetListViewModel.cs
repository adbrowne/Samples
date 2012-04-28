namespace WidgetCreatorMvc.Models.Widget
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using WidgetCreatorMvc3.Service.DTO;

    public class WidgetListViewModel
    {
        private readonly IEnumerable<WidgetSummary> widgets;

        public class WidgetListViewModelItem
        {
            public string Id { get; set; }

            private string title;

            public string Title
            {
                get
                {
                    if (String.IsNullOrEmpty(title)) return "[No Title]";
                    return this.title;
                }
                set
                {
                    this.title = value;
                }
            }
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