using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.WebApi;
using Autofac.Integration.Mvc;
using NHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NoteApi.Domain;
using NHibernate.Tool.hbm2ddl;
using FluentNHibernate.Automapping;

namespace NoteApi
{
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
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }

        protected void Application_Start()
        {
            HibernatingRhinos.Profiler.Appender.NHibernate.NHibernateProfiler.Initialize();

            var configuration = GlobalConfiguration.Configuration;

            var builder = new ContainerBuilder();
            builder.ConfigureWebApi(configuration);
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.Register(c => CreateSessionFactory()).As<ISessionFactory>().SingleInstance();
            builder.Register(c =>
                {
                    var sf = c.Resolve<ISessionFactory>();
                    var session = sf.OpenSession();
                    var export = new SchemaExport(SavedConfig);
                    export.Execute(true, true, false, session.Connection, null);
                    return session;
                }
                ).As<ISession>().SingleInstance();

            builder.RegisterType<UnitOfWork>().InstancePerHttpRequest();
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            var resolver = new AutofacWebApiDependencyResolver(container);
            configuration.ServiceResolver.SetResolver(resolver);

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            BundleTable.Bundles.RegisterTemplateBundles();
        }
        
        private ISessionFactory CreateSessionFactory()
        {
            var automapConfig = new NoteApiAutomapperConfiguration();
            return Fluently.Configure()
                .Mappings(m => 
                    {
                    m.AutoMappings.Add(
                        AutoMap.AssemblyOf<Image>(automapConfig));
                    m.FluentMappings
                        .AddFromAssembly(typeof(Image).Assembly);
                    }
                )
                .Database(SQLiteConfiguration.Standard.InMemory())
                .ExposeConfiguration(c => SavedConfig = c )
                .BuildSessionFactory();
        }
        
        private NHibernate.Cfg.Configuration SavedConfig;
    }

    public class NoteApiAutomapperConfiguration : DefaultAutomappingConfiguration
    {
        public override bool ShouldMap(Type type)
        {
            return type.Namespace == typeof(Image).Namespace;
        }
    }

    public class UnitOfWork : IDisposable
    {
        private ITransaction _trans;
        public UnitOfWork(ISession session)
        {
            _trans = session.BeginTransaction();
        }

        public void Dispose()
        {
            _trans.Commit();
        }
    }
}