namespace WidgetCreatorMvc.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DTO;

    using WidgetCreatorMvc.Controllers;
    using WidgetCreatorMvc.Service.Domain;

    using Price = WidgetCreatorMvc.Service.Domain.Price;

    public class WidgetService
    {
        public WidgetService()
        {
            widgets.Add(new Widget(Guid.NewGuid(), "Big Widget", new Price(1000M, Currency.AUD)));
            widgets.Add(new Widget(Guid.NewGuid(), "Small Widget", new Price(499M, Currency.AUD)));
        }

        private readonly List<Widget> widgets = new List<Widget>();
        public IEnumerable<WidgetSummary> AllWidgets()
        {
            return widgets.Select(x => new WidgetSummary { Id = x.Id, Title = x.Title });
        }

        public WidgetDetail GetWidget(Guid id)
        {
            var widget = widgets.Single(x => x.Id == id);
            return new WidgetDetail
                {
                    Id = widget.Id,
                    Title = widget.Title,
                    Price = new DTO.Price
                    {
                        Amount = widget.Price.Amount,
                        Currency = widget.Price.Currency
                    }
                };
        }

        public void UpdateWidgetTitle(WidgetTitleUpdate update)
        {
            var widget = widgets.Single(x => x.Id == update.Id);
            widget.UpdateTitle(update.NewTitle);
        }
    }
}