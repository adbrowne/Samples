using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WidgetCreatorMvc3
{
    using System.Collections;

    using Autofac;
    using Autofac.Integration.Mvc;

    using FluentValidation;
    using FluentValidation.Mvc;
    using FluentValidation.Results;

    using WidgetCreatorMvc.Service;

    using WidgetCreatorMvc3.Service.DTO;

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

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterType<WidgetService>().SingleInstance();
            builder.RegisterModelBinders(typeof(MvcApplication).Assembly);
            builder.RegisterModelBinderProvider();
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            FluentValidationModelValidatorProvider.Configure(
                c => c.ValidatorFactory = new MyValidatorFactory() 
                );
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }

    public class MyValidatorFactory : IValidatorFactory
    {
        public IValidator<T> GetValidator<T>()
        {
            return (IValidator<T>)GetValidator(typeof(T));
        }

        public IValidator GetValidator(Type type)
        {
            if (type == typeof(WidgetTitleUpdate)) return new WidgetTitleValidator();
            return null;
        }
    }

    public class WidgetTitleValidator : AbstractValidator<WidgetTitleUpdate>
    {
        public WidgetTitleValidator()
        {
            RuleFor(x => x.NewTitle).NotEmpty();
        }
    }
}