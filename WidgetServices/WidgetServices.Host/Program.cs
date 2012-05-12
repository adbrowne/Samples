using System;
using System.Threading;
using Autofac;
using EasyNetQ;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using SchoolBus.EasyNetQ;
using SchoolBus.InProcess;
using WidgetServices.Services.People;
using WidgetServices.Services.Version;
using WidgetServices.Services.VersionRoles;
using WidgetServices.Services.WidgetDetails;
using IBus = SchoolBus.IBus;

namespace WidgetServices.Host
{
    public class Program
    {
        public static void Main()
        {
            var builder = new ContainerBuilder();
            
            builder.Register(c => RabbitHutch.CreateBus("host=localhost"));
            builder.Register(x =>
                Fluently.Configure()
                    .Database(MsSqlConfiguration.MsSql2008.ConnectionString(d => d.FromConnectionStringWithKey("WidgetServicesDB")))
                    .Mappings(AddMappings)
                    .BuildSessionFactory())
                .As<ISessionFactory>()
                .SingleInstance();

            builder.RegisterType<WidgetDetailsService>().As<IWidgetDetailsService>();
            builder.RegisterType<PersonService>().As<IPersonService>();
            builder.RegisterType<VersionService>().As<IVersionService>();
            builder.RegisterType<VersionRolesService>().As<IVersionRolesService>();
            builder.RegisterModule<SchoolBusInProcessModule>();
            builder.Register(c =>
                {
                    var session = c.Resolve<ISessionFactory>().OpenSession();
                    session.BeginTransaction();
                    return session;
                }).InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterType<WidgetDetailsService>();
            builder.RegisterType<VersionRolesService>();
            builder.RegisterType<EasyNetQBus>().As<IBus>();
            var container = builder.Build();
            var rBus = container.Resolve<IBus>();
            rBus.Subscribe<WidgetCreatedEvent>(Guid.NewGuid().ToString(),
                x =>
                {
                    var theEvent = x;
                    using (var childContainer = container.BeginLifetimeScope())
                    {
                        var versionService = childContainer.Resolve<IVersionService>();
//                        versionService.WidgetCreated(theEvent);
                    }
                });

            Thread.Sleep(Timeout.Infinite);
        }
        
        private static void AddMappings(MappingConfiguration mappingConfiguration)
        {
            mappingConfiguration.FluentMappings.AddFromAssemblyOf<MvcApplication>();
        }
    }
}
