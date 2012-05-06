namespace WidgetServices.Services.Widget
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using NHibernate;

    class WidgetService : IWidgetService
    {
        private readonly ISession _session;

        public WidgetService(UnitOfWork unitOfWork)
        {
            this._session = unitOfWork.Session;
        }

        public void SetWidgetDetails(WidgetDetails widgetDetails)
        {
            this._session.SaveOrUpdate(widgetDetails);
        }

        public WidgetDetails GetWidgetDetails(Guid id)
        {
            return this._session.Get<WidgetDetails>(id);
        }

        public IEnumerable<WidgetDetails> GetWidgets()
        {
            return this._session.CreateCriteria<WidgetDetails>().List<WidgetDetails>().ToList();
        }
    }
}