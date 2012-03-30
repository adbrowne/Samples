using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using WebIntegration = Autofac.Integration.Web;
using AutofacContrib.NHibernate.Bytecode;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Mapping;
using NHibernate;
using NHibernate.ByteCode.Castle;
using NHibernate.Type;

namespace ConstructorInjectionTest
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication, WebIntegration.IContainerProviderAccessor
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            ISessionFactory sessionFactory = null;

            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterType<NameFormatter>().As<INameFormatter>();
            builder.Register(context => sessionFactory).As<ISessionFactory>();
            builder.Register(context => context.Resolve<ISessionFactory>().OpenSession()).As<ISession>().InstancePerHttpRequest();
            builder.RegisterType<Foo>().Named<Foo>(typeof(Foo).FullName).As<Foo>();
            var container = builder.Build();
            
            // Once you're done registering things, set the container
            // provider up with your registrations.
            _containerProvider = new WebIntegration.ContainerProvider(container);
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            
            NHibernate.Cfg.Environment.BytecodeProvider = 
                new AutofacBytecodeProvider(
               new WebIntegration.ContainerProviderContainer(_containerProvider)
                   , new ProxyFactoryFactory(), new DefaultCollectionTypeFactory());

            sessionFactory = CreateSessionFactory();

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        private static ISessionFactory CreateSessionFactory()
        {
            var configuration = Fluently.Configure()
                .Database(
                    MsSqlConfiguration.MsSql2008
                        .ConnectionString(c => c.FromConnectionStringWithKey("MyDb"))
                )
                .Mappings(m => m.FluentMappings.Add<FooMap>())
                .BuildConfiguration();

            configuration.SetProperty("hbm2ddl.auto", "update");
            return configuration.BuildSessionFactory();
        }

        // Provider that holds the application container.
        static WebIntegration.IContainerProvider _containerProvider;

        // Instance property that will be used by Autofac HttpModules
        // to resolve and inject dependencies.
        public WebIntegration.IContainerProvider ContainerProvider
        {
            get { return _containerProvider; }
        }
    }

    class FooMap : ClassMap<Foo>
    {
        public FooMap()
        {
            Proxy<IFoo>();
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Name);
        }
    }

    class Foo : IFoo
    {
        private readonly INameFormatter nameFormatter;

        public Foo(INameFormatter nameFormatter)
        {
            this.nameFormatter = nameFormatter;
        }

        public virtual int Id { get; set; }
        public virtual string Name { get; set; }

        public virtual string GetFormattedName()
        {
            return nameFormatter.Format(Name);
        }
    }

    interface IFoo
    {
    }

    internal interface INameFormatter
    {
        string Format(string name);
    }

    class NameFormatter : INameFormatter
    {
        public string Format(string name)
        {
            return "Formatted{" + name + "}";
        }
    }
}