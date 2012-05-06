namespace WidgetServices.Services.WidgetDetails
{
    using System;

    using WidgetServices.Messaging;

    public class WidgetCreatedEvent : IMessage
    {
        public WidgetCreatedEvent(Guid widgetId)
        {
            this.WidgetId = widgetId;
        }

        public Guid WidgetId { get; private set; }
    }
}