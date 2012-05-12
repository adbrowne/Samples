using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using EasyNetQ;
using SchoolBus.EasyNetQ;

namespace WidgetServices
{
    using Autofac;
    using Autofac.Integration.Mvc;

    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;

    using NHibernate;
    using NHibernate.Tool.hbm2ddl;

    using SchoolBus;
    using SchoolBus.InProcess;

    using WidgetServices.Messages;
    using WidgetServices.Services.People;
    using WidgetServices.Services.Version;
    using WidgetServices.Services.VersionRoles;
    using WidgetServices.Services.WidgetDetails;

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            routes.MapRoute(
                name: "IdDirect",
                url: "{controller}/{id}",
                defaults: new { controller = "Home", action = "Index" },
                constraints: new { id = "^[A-Z0-9]{8}-[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{12}$" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            BundleTable.Bundles.RegisterTemplateBundles();

            HibernatingRhinos.Profiler.Appender.NHibernate.NHibernateProfiler.Initialize();

            var builder = new ContainerBuilder();
            builder.Register(x =>
                Fluently.Configure()
                    .Database(MsSqlConfiguration.MsSql2008.ConnectionString(d => d.FromConnectionStringWithKey("WidgetServicesDB")))
                    .Mappings(AddMappings)
                    .ExposeConfiguration(UpdateSchema)
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
            builder.Register(c => RabbitHutch.CreateBus("host=localhost"));
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            var container = builder.Build();
            var bus = container.Resolve<Bus>();
            var rBus = container.Resolve<IBus>();
            rBus.Subscribe<WidgetCreatedEvent>(Guid.NewGuid().ToString(),
                x =>
                {
                    var theEvent = x;
                    using (var childContainer = container.BeginLifetimeScope())
                    {
                        var versionService = childContainer.Resolve<IVersionService>();
                        versionService.WidgetCreated(theEvent);
                    }
                });

            rBus.Respond<CreateWidgetCommand, bool>(command =>
                                                        {
                                                            using (var child = container.BeginLifetimeScope())
                                                            {
                                                                var service = child.Resolve<WidgetDetailsService>();
                                                                service.Execute(command);
                                                                return true;
                                                            }
                                                        });

            bus.ForCommand<UpdateWidgetCommand>().Execute<WidgetDetailsService>();
            bus.ForCommand<CreateWidgetCommand>().Execute<WidgetDetailsService>();
            bus.ForCommand<SetRoleUsersCommand>().Execute<VersionRolesService>();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        private void UpdateSchema(NHibernate.Cfg.Configuration nHibernateConfiguration)
        {
            var schemaUpdate = new SchemaUpdate(nHibernateConfiguration);
            schemaUpdate.Execute(false, true);
        }

        private void AddMappings(MappingConfiguration mappingConfiguration)
        {
            mappingConfiguration.FluentMappings.AddFromAssemblyOf<MvcApplication>();
        }
    }
    public class NoDebugLogger : IEasyNetQLogger
    {
        public void DebugWrite(string format, params object[] args)
        {

        }

        public void InfoWrite(string format, params object[] args)
        {
            Console.WriteLine(format, args);
        }

        public void ErrorWrite(string format, params object[] args)
        {
            Console.WriteLine(format, args);
        }

        public void ErrorWrite(Exception exception)
        {
            Console.WriteLine(exception);
        }
    }

}