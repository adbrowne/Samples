namespace WidgetServices.Services.WidgetDetails
{
    using System;

    using SchoolBus;
    using SchoolBus.InProcess;

    public class WidgetReminderEvent
    {
        
    }

    public class WidgetCreatedEvent : IMessage
    {
        public WidgetCreatedEvent(Guid widgetId)
        {
            this.WidgetId = widgetId;
        }

        public Guid WidgetId { get; private set; }
    }
}