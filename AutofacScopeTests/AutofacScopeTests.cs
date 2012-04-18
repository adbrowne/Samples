namespace AutofacScopeTests
{
    using System;

    using Autofac;
    using Autofac.Core.Registration;

    using NUnit.Framework;

    [TestFixture]
    public class AutofacScopeTests
    {
        interface IService
        {

        }

        private class ImplA : IService
        {
        }

        private class ImplB : IService
        {
        }

        [Test]
        public void ProcessScopesResolveIndepedently()
        {
            var container = (new ContainerBuilder()).Build();

            var lifeTimeA = container.BeginLifetimeScope(b => b.RegisterType<ImplA>().As<IService>());
            var lifeTimeB = container.BeginLifetimeScope(b => b.RegisterType<ImplB>().As<IService>());

            var serviceA = lifeTimeA.Resolve<IService>();
            var serviceB = lifeTimeB.Resolve<IService>();

            Assert.That(serviceA, Is.TypeOf<ImplA>());
            Assert.That(serviceB, Is.TypeOf<ImplB>());
        }
    }

    [TestFixture]
    public class NameTests
    {
        private class Component
        {
        }

        [Test]
        public void RegisteringWithNameDeniesResolutionWithoutName()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.Register(c => new Component()).Named<Component>("myname");

            var container = containerBuilder.Build();

            Assert.Throws<ComponentNotRegisteredException>(() => container.Resolve<Component>());
        }
    }
}