namespace WidgetServices.Tests
{
    using Autofac;

    using NUnit.Framework;

    using SchoolBus;
    using SchoolBus.InProcess;

    [TestFixture]
    public class CommandTests
    {
        [Test]
        public void CanSendCommand()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<SchoolBusInProcessModule>();
            containerBuilder.RegisterType<TestCommandHandler>();
            var container = containerBuilder.Build();
            var bus = container.Resolve<Bus>();
            bus.ForCommand<TestCommand>().Execute<TestCommandHandler>();
            var result = bus.Execute<int>(new TestCommand());
            Assert.That(result, Is.EqualTo(2));
        }
    }

    public class TestCommandHandler
    {
        private readonly IBus _bus;

        public TestCommandHandler(IBus bus)
        {
            _bus = bus;
        }

        public void Execute(TestCommand command)
        {
            _bus.Reply(2);
        }
    }

    public class TestCommand
    {
    }
}