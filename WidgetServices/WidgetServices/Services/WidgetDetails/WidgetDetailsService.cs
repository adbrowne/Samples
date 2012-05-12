namespace WidgetServices.Services.WidgetDetails
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using NHibernate;

    using SchoolBus;

    using WidgetServices.Messages;

    public class WidgetDetailsService : IWidgetDetailsService
    {
        private readonly IBus _bus;

        private readonly ISession _session;

        public WidgetDetailsService(UnitOfWork unitOfWork, IBus bus)
        {
            _bus = bus;
            _session = unitOfWork.Session;
        }

        public WidgetDetail GetWidgetDetails(Guid id)
        {
            return _session.Get<WidgetDetail>(id);
        }

        public IEnumerable<WidgetDetail> GetWidgets()
        {
            return _session.CreateCriteria<WidgetDetail>().List<WidgetDetail>().ToList();
        }

        public void Execute(UpdateWidgetCommand command)
        {
            _session.Update(command);
        }

        public void Execute(CreateWidgetCommand command)
        {
            _session.SaveOrUpdate(command);
            _session.Flush();
            _bus.Publish(new WidgetCreatedEvent(command.WidgetId));
        }
    }
}