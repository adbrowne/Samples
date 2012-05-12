using System;
using System.Collections.Generic;
using System.Reflection;
using Autofac;
using NUnit.Framework;

namespace WidgetServices.Tests
{
    [TestFixture]
    public class ViewModelCompositionTests
    {
        [Test]
        public void CanGetViewModelFromId()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<ViewModelRegistry>();
            containerBuilder.RegisterType<WidgetViewModel>();
            containerBuilder.RegisterType<WidgetService>();
            containerBuilder.RegisterType<WidgetViewModelFromWidgetIdHandler>();
            var container = containerBuilder.Build();

            var viewModelRegistry = container.Resolve<ViewModelRegistry>();

            viewModelRegistry.Register<Guid, WidgetViewModel, WidgetViewModelFromWidgetIdHandler>();

            Guid input = Guid.NewGuid();
            var widgetModel = viewModelRegistry.GetModel<Guid, WidgetViewModel>(input);

            Assert.That(widgetModel.Title, Is.EqualTo("Test Value" + input));
        }
    }

    public class WidgetService
    {
        public string GetById(Guid id)
        {
            return "Test Value" + id;
        }
    }

    public class WidgetViewModelFromWidgetIdHandler
    {
        private readonly WidgetService _service;

        public WidgetViewModelFromWidgetIdHandler(WidgetService service)
        {
            _service = service;
        }

        public void Compose(Guid id, WidgetViewModel viewModel)
        {
            viewModel.Title = _service.GetById(id);
        }
    }

    public class WidgetViewModel
    {
        public string Title { get; set; }
    }

    public class ViewModelRegistry
    {
        private readonly ILifetimeScope _lifetimeScope;

        public ViewModelRegistry(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }

        private struct Mapping
        {
            public Type InputType;
            public Type ViewModelType;
        }

        readonly Dictionary<Mapping, Type> _composers = new Dictionary<Mapping, Type>();

        public void Register<TInput, TViewModel, TViewModelComposer>()
        {
            _composers.Add(new Mapping
                               {
                                   InputType = typeof(TInput),
                                   ViewModelType = typeof(TViewModel)
                               }, typeof(TViewModelComposer));
        }

        public TViewModel GetModel<TInput, TViewModel>(TInput input)
        {
            var modelComposerType = _composers[new Mapping
                                                   {
                                                       InputType = typeof(TInput),
                                                       ViewModelType = typeof(TViewModel)
                                                   }];

            var composer = _lifetimeScope.Resolve(modelComposerType);

            var viewModel = _lifetimeScope.Resolve<TViewModel>();

            modelComposerType.InvokeMember("Compose", BindingFlags.Default | BindingFlags.InvokeMethod, null, composer,
                                           new object[] { input, viewModel });

            return viewModel;
        }
    }
}