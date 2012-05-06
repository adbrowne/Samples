namespace WidgetServices.Services.Version
{
    using System;

    class VersionService : IVersionService
    {
        public WidgetVersion GetCurrentVersion(Guid widgetId)
        {
            return new WidgetVersion { WidgetId = widgetId, VersionId = Constants.FirstVersionId };
        }
    }
}