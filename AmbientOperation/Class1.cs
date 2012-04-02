namespace AmbientOperation
{
    using System;
    using System.Linq;

    using Castle.DynamicProxy;

    using NUnit.Framework;
    using Autofac;
    [TestFixture]
    public class Class1
    {
        public static OperationType OperationRun;

        public static IContainer Container;

        public IService CreateServiceInterceptor(IComponentContext componentContext)
        {
            var generator = new ProxyGenerator();
            return generator.CreateInterfaceProxyWithoutTarget<IService>(new ServiceInterceptor());
        }

        [SetUp]
        public void SetUp()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ServiceImpl>().Named<IService>("implementation").InstancePerLifetimeScope();
            builder.Register(this.CreateServiceInterceptor);
            builder.RegisterType<LowLevelComponentNeedingOperation>().As<ILowLevelComponentNeedingOperation>().InstancePerLifetimeScope();
            Container = builder.Build();
        }

        [Test]
        public void ReadDataPassesOperationDown()
        {
            var serviceInstance = GetServiceInstance();

            serviceInstance.ReadData();

            Assert.That(OperationRun, Is.EqualTo(OperationType.Read));
        }

        [Test]
        public void WriteDataPassesOperationDown()
        {
            var serviceInstance = GetServiceInstance();

            serviceInstance.WriteData();

            Assert.That(OperationRun, Is.EqualTo(OperationType.Write));
        }

        private IService GetServiceInstance()
        {
            return Container.Resolve<IService>();
        }
    }

    public class ServiceInterceptor : IInterceptor
    {
        private static IContainer GetContainer()
        {
            return Class1.Container;
        }
        
        private void SetupLifetime(ContainerBuilder containerBuilder, OperationType operationType)
        {
            containerBuilder.Register(x => operationType);
        }

        public void Intercept(IInvocation invocation)
        {
            IContainer container = GetContainer();
            var operationNameAttribute =(OperationNameAttribute) invocation.Method.GetCustomAttributes(typeof(OperationNameAttribute), false).Single();
            using (var innerContainer = container.BeginLifetimeScope(b => this.SetupLifetime(b, operationNameAttribute.OperationType)))
            {
                var inner = innerContainer.ResolveNamed<IService>("implementation");
                invocation.Method.Invoke(inner, invocation.Arguments);
            }
        }
    }

    public interface ILowLevelComponentNeedingOperation
    {
        void DoSomething();
    }

    class LowLevelComponentNeedingOperation : ILowLevelComponentNeedingOperation
    {
        private readonly OperationType operationType;

        public LowLevelComponentNeedingOperation(OperationType operationType)
        {
            this.operationType = operationType;
        }

        public void DoSomething()
        {
            Class1.OperationRun = operationType;
        }
    }

    public class ServiceImpl : IService
    {
        private readonly ILowLevelComponentNeedingOperation lowLevelComponent;

        public ServiceImpl(ILowLevelComponentNeedingOperation lowLevelComponent)
        {
            this.lowLevelComponent = lowLevelComponent;
        }

        public void ReadData()
        {
            lowLevelComponent.DoSomething();
        }

        public void WriteData()
        {
            lowLevelComponent.DoSomething();
        }
    }

    public enum OperationType
    {
        Read,
        Write
    }

    public interface IService
    {
        [OperationName(OperationType.Read)]
        void ReadData();

        [OperationName(OperationType.Write)]
        void WriteData();
    }

    public class OperationNameAttribute : Attribute
    {
        public OperationType OperationType { get; set; }

        public OperationNameAttribute(OperationType operationType)
        {
            OperationType = operationType;
        }
    }
}
