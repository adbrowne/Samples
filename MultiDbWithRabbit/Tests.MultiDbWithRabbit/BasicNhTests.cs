namespace Tests.MultiDbWithRabbit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using EasyNetQ;

    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;
    using FluentNHibernate.Mapping;

    using NHibernate;
    using NHibernate.Cfg;
    using NHibernate.Tool.hbm2ddl;

    using NUnit.Framework;

    [TestFixture]
    public class BasicNhTests
    {
        [Test]
        public void SaveValue()
        {
            string widgetName = "Widget-" + Guid.NewGuid();

            var widgetService = new WidgetService();
            var serviceBus = RabbitHutch.CreateBus();
            serviceBus.Subscribe<AddWidgetCommand>("WidgetService", widgetService.Handle);

            var clientBus = RabbitHutch.CreateBus();

            using (var publishChannel = clientBus.OpenPublishChannel())
            {
                publishChannel.Publish(new AddWidgetCommand { Name = widgetName });
            }

            Thread.Sleep(TimeSpan.FromMilliseconds(TestConstants.WaitTime));
            CollectionAssert.Contains(widgetService.WidgetNames(), widgetName);
        }
    }

    public class AddWidgetCommand
    {
        public string Name { get; set; }
    }

    public class WidgetService
    {
        public WidgetService()
        {
            var config = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008.ConnectionString(x => x.FromConnectionStringWithKey("widget")))
                .ExposeConfiguration(SetupDb)
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Widget>())
                .BuildConfiguration();
            this._sessionFactory = config.BuildSessionFactory();
        }

        private void SetupDb(Configuration obj)
        {
            var schemaExport = new SchemaExport(obj);
            schemaExport.Create(true, true);
        }

        private readonly ISessionFactory _sessionFactory;

        public IEnumerable<string> WidgetNames()
        {
            using (var session = _sessionFactory.OpenSession())
            {
                using (session.BeginTransaction())
                {
                    return session.CreateCriteria<Widget>().List<Widget>().Select(x => x.Name);
                }
            }
        }

        public void Handle(AddWidgetCommand command)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    var widget = new Widget { Name = command.Name };
                    session.Save(widget);
                    tx.Commit();
                }
            }
        }
    }

    public class Widget
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
    }

    public class WidgetMap : ClassMap<Widget>
    {
        public WidgetMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);
        }
    }
}