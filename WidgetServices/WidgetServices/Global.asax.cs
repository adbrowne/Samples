﻿using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace WidgetServices
{
    using Autofac;
    using Autofac.Integration.Mvc;

    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;

    using NHibernate;
    using NHibernate.Tool.hbm2ddl;

    using WidgetServices.Services.People;
    using WidgetServices.Services.Widget;

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

            builder.RegisterType<WidgetService>().As<IWidgetService>();
            builder.RegisterType<PersonService>().As<IPersonService>();
            builder.Register(c =>
                {
                    var session = c.Resolve<ISessionFactory>().OpenSession();
                    session.BeginTransaction();
                    return session;
                }).InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            var container = builder.Build();
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
}