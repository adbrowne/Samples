namespace WidgetCreatorMvc.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using WidgetCreatorMvc3.Service.Core;
    using WidgetCreatorMvc3.Service.DTO;
    using WidgetCreatorMvc3.Service.Domain;

    using Price = WidgetCreatorMvc3.Service.Domain.Price;

    public class WidgetService
    {
        public WidgetService()
        {
            widgets.Add(new Widget(new WidgetId(Guid.NewGuid()), "Big Widget", new Price(1000M, Currency.AUD)));
            widgets.Add(new Widget(new WidgetId(Guid.NewGuid()), "Small Widget", new Price(499M, Currency.AUD)));
        }

        private readonly List<Widget> widgets = new List<Widget>();
        public IEnumerable<WidgetSummary> AllWidgets()
        {
            return widgets.Select(x => new WidgetSummary { Id = x.Id, Title = x.Title });
        }

        public WidgetDetail GetWidget(WidgetId id)
        {
            var widget = widgets.Single(x => x.Id.Id == id.Id);
            return new WidgetDetail
                {
                    Id = widget.Id,
                    Title = widget.Title,
                    Price = new WidgetCreatorMvc3.Service.DTO.Price
                    {
                        Amount = widget.Price.Amount,
                        Currency = widget.Price.Currency
                    }
                };
        }

        public void UpdateWidgetTitle(WidgetTitleUpdate update)
        {
            var widget = widgets.Single(x => x.Id.Id == update.Id.Id);
            widget.UpdateTitle(update.NewTitle);
        }
    }
}