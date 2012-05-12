namespace WidgetServices.Services.Version
{
    using System;
    using System.Linq;

    using NHibernate;
    using NHibernate.Criterion;

    using WidgetServices.Services.WidgetDetails;

    public class VersionService : IVersionService
    {
        private readonly ISession _session;

        public VersionService(UnitOfWork unitOfWork)
        {
            _session = unitOfWork.Session;
        }

        public WidgetVersion GetCurrentVersion(Guid widgetId)
        {
            var versions = _session.CreateCriteria<WidgetVersion>().Add(Restrictions.Eq("WidgetId", widgetId)).List<WidgetVersion>();
            return versions.OrderBy(x => x.VersionNumber).First();
        }

        public void WidgetCreated(WidgetCreatedEvent @event)
        {
            _session.Save(new WidgetVersion { WidgetId = @event.WidgetId });
        }
    }
}