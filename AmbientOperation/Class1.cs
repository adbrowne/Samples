namespace AmbientOperation
{
    using System;

    using NUnit.Framework;
    using Autofac;
    [TestFixture]
    public class Class1
    {
        public static OperationType OperationRun;

        public static IContainer Container;

        [SetUp]
        public void SetUp()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<ServiceImpl>().As<IService>().InstancePerLifetimeScope().Named<IService>("implementation");
            builder.RegisterType<ServiceWrapper>().As<IService>();
            builder.RegisterType<LowLevelComponentNeedingOperation>().As<ILowLevelComponentNeedingOperation>().InstancePerLifetimeScope();
            Container = builder.Build();
        }

        [Test]
        public void ReadDataPassesOperationDown()
        {
            var serviceInstance = GetServiceInstance();

            serviceInstance.ReadData();

            Assert.That(OperationRun,Is.EqualTo(OperationType.Read));
        }

        [Test]
        public void WriteDataPassesOperationDown()
        {
            var serviceInstance = GetServiceInstance();

            serviceInstance.WriteData();

            Assert.That(OperationRun,Is.EqualTo(OperationType.Write));
        }

        private IService GetServiceInstance()
        {
            return Container.Resolve<IService>();
        }
    }

    class ServiceWrapper : IService
    {
        public void ReadData()
        {
            this.WrapCall(s => s.ReadData(), OperationType.Read);
        }

        private void WrapCall(Action<IService> serviceAction, OperationType operationType)
        {
            IContainer container = GetContainer();
            using (var innerContainer = container.BeginLifetimeScope(b => this.SetupLifetime(b, operationType)))
            {
                var inner = innerContainer.ResolveNamed<IService>("implementation");
                serviceAction(inner);
            }
        }

        private static IContainer GetContainer()
        {
            return Class1.Container;
        }

        private void SetupLifetime(ContainerBuilder containerBuilder, OperationType operationType)
        {
            containerBuilder.Register(x => operationType);
        }

        public void WriteData()
        {
            this.WrapCall(s => s.ReadData(), OperationType.Write);
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

    internal interface IService
    {
        void ReadData();

        void WriteData();
    }
}
