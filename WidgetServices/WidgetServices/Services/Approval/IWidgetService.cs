namespace WidgetServices.Services.Approval
{
    using System;

    using NHibernate;

    public interface IWidgetService
    {
        void SetWidgetDetails(WidgetDetails widgetDetails);

        WidgetDetails GetWidgetDetails(Guid id);
    }

    public class WidgetDetails
    {
        public virtual Guid ApprovalId { get; set; }
        public virtual string Title { get; set; }
    }

    class WidgetService : IWidgetService
    {
        private readonly ISession _session;

        public WidgetService(UnitOfWork unitOfWork)
        {
            this._session = unitOfWork.Session;
        }

        public void SetWidgetDetails(WidgetDetails widgetDetails)
        {
            _session.SaveOrUpdate(widgetDetails);
        }

        public WidgetDetails GetWidgetDetails(Guid id)
        {
            return _session.Get<WidgetDetails>(id);
        }
    }
}