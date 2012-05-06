namespace WidgetServices.Services.Version
{
    using System;

    public class WidgetVersion
    {
        public virtual Guid WidgetId { get; set; }
        public virtual Guid VersionId { get; set; }
        public virtual int VersionNumber { get; set; }
    }
}