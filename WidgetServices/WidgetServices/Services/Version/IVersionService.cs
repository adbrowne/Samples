namespace WidgetServices.Services.Version
{
    using System;

    public interface IVersionService
    {
        WidgetVersion GetCurrentVersion(Guid widgetId);
    }
}