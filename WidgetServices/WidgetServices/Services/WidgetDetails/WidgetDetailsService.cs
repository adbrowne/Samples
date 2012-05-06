namespace WidgetServices.Services.WidgetDetails
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using NHibernate;

    using WidgetServices.Messaging;

    class WidgetDetailsService : IWidgetDetailsService
    {
        private readonly IBus _bus;

        private readonly ISession _session;

        public WidgetDetailsService(UnitOfWork unitOfWork, IBus bus)
        {
            _bus = bus;
            this._session = unitOfWork.Session;
        }

        public void SetWidgetDetails(WidgetDetail widgetDetail)
        {
            this._session.SaveOrUpdate(widgetDetail);
        }

        public WidgetDetail GetWidgetDetails(Guid id)
        {
            return this._session.Get<WidgetDetail>(id);
        }

        public IEnumerable<WidgetDetail> GetWidgets()
        {
            return this._session.CreateCriteria<WidgetDetail>().List<WidgetDetail>().ToList();
        }

        public void CreateWidget(WidgetDetail widgetDetail)
        {
            SetWidgetDetails(widgetDetail);
            _bus.Publish(new WidgetCreatedEvent(widgetDetail.WidgetId));
        }
    }
}