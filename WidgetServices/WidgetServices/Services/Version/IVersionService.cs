namespace WidgetServices.Services.Version
{
    using System;

    using WidgetServices.Services.WidgetDetails;

    public interface IVersionService
    {
        WidgetVersion GetCurrentVersion(Guid widgetId);

        void WidgetCreated(WidgetCreatedEvent @event);
    }
}